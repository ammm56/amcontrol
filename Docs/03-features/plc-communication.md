# PLC 通信模块实现说明

**文档编号**：FEAT-PLC-002  
**版本**：2.1.0  
**状态**：已实现  
**最后更新**：2026-04-23  
**维护人**：Am

---

## 1. 模块定位

PLC 通信模块当前已经覆盖以下完整能力：

- 站与点位配置管理；
- 协议插件加载与客户端创建；
- 后台扫描与运行态缓存；
- 运行时快照查询；
- 按配置点位或直接地址的手动读写；
- WinForms 运行页、调试页、配置页首版 UI。

在当前解决方案中，PLC 模块已经从“规划中”进入“可持续收口与扩展”的状态。

当前实现还补充了两层更明确的异常语义：

1. 站级连接离线统一前缀：`[PlcDisconnected]`
2. 协议层 socket 失败统一前缀：`[ProtocolSocketError]`

这样可以把“PLC 站不可用”和“底层协议通信异常”从运行日志中直接区分出来。

---

## 2. 代码结构

```text
AM.Model/
├── Entity/Plc/
│   ├── PlcStationConfigEntity.cs
│   └── PlcPointConfigEntity.cs
├── Plc/
│   ├── PlcConfig.cs
│   ├── PlcStationConfig.cs
│   └── PlcPointConfig.cs
├── Interfaces/Plc/
│   ├── IPlcClient.cs
│   ├── IPlcClientFactory.cs
│   ├── App/IPlcConfigAppService.cs
│   ├── Config/IPlcStationCrudService.cs
│   ├── Config/IPlcPointCrudService.cs
│   └── Runtime/
│       ├── IPlcRuntimeService.cs
│       └── IPlcScanWorker.cs
└── Runtime/
    ├── PlcRuntimeState.cs
    ├── PlcStationRuntimeSnapshot.cs
    └── PlcPointRuntimeSnapshot.cs

AM.DBService/Services/Plc/
├── Config/
│   ├── PlcStationCrudService.cs
│   └── PlcPointCrudService.cs
├── App/
│   ├── PlcConfigAppService.cs
│   └── PlcConfigSeedService.cs
├── Driver/
│   ├── PlcClientFactory.cs
│   ├── ProtocolPlcClient.cs
│   ├── NullPlcClient.cs
│   └── ProtocolAssemblyRegistry.cs
└── Runtime/
    ├── PlcScanWorker.cs
    ├── PlcRuntimeQueryService.cs
    ├── PlcStationScanRunner.cs
    └── PlcOperationService.cs

AM.PageModel/Plc/
├── PlcStatusPageModel.cs
├── PlcMonitorPageModel.cs
└── PlcDebugPageModel.cs

AM.PageModel/SysConfig/
└── PlcConfigManagementPageModel.cs

AMControlWinF/Views/
├── Plc/
│   ├── PlcStatusPage.cs
│   ├── PlcMonitorPage.cs
│   ├── PlcDebugPage.cs
│   ├── PlcStatusDetailControl.cs
│   ├── PlcStatusVirtualListControl.cs
│   ├── PlcPointVirtualListControl.cs
│   └── PlcPointDetailControl.cs
└── SysConfig/
    ├── PlcConfigManagementPage.cs
    ├── PlcStationEditDialog.cs
    └── PlcPointEditDialog.cs
```

---

## 3. 模型定义

### 3.1 站配置

`PlcStationConfig` / `PlcStationConfigEntity` 关注：

- `Name`
- `DisplayName`
- `ProtocolType`
- `ConnectionType`
- `IpAddress`
- `Port`
- `TimeoutMs`
- `ReconnectIntervalMs`
- `ScanIntervalMs`
- `IsEnabled`

### 3.2 点位配置

`PlcPointConfig` / `PlcPointConfigEntity` 关注：

- `PlcName`
- `Name`
- `DisplayName`
- `GroupName`
- `Address`
- `DataType`
- `Length`
- `AccessMode`
- `IsEnabled`

### 3.3 运行时快照

| 模型 | 说明 |
|------|------|
| `PlcStationRuntimeSnapshot` | 站级在线状态、错误信息、扫描统计、协议信息 |
| `PlcPointRuntimeSnapshot` | 点位值、质量、更新时间、错误信息 |
| `PlcRuntimeState` | 总运行态缓存与扫描服务状态 |

---

## 4. 协议库与接口

### 4.1 协议层接口

```csharp
public interface IProtocol
{
    M_Return<bool> Configure(M_ProtocolOptions options);
    M_Return<bool> Connect();
    M_Return<bool> Disconnect();
    M_Return<bool> Reconnect();
    M_Return<bool> IsConnected();
    M_Return<M_PointData> ReadPoint(M_PointReadRequest request);
    M_Return<M_PointData> WritePoint(M_PointWriteRequest request);
}
```

### 4.2 AM 侧客户端接口

```csharp
public interface IPlcClient
{
    Result Configure(M_ProtocolOptions options);
    Result Connect();
    Result Disconnect();
    Result Reconnect();
    Result<bool> IsConnected();
    Result<M_PointData> ReadPoint(M_PointReadRequest request);
    Result<M_PointData> WritePoint(M_PointWriteRequest request);
}
```

### 4.3 当前协议库

| 协议 | 实现 |
|------|------|
| Modbus TCP | `ProtocolLib.ModbusTcp.Protocol` |
| Siemens S7 TCP | `ProtocolLib.S7Tcp.Protocol` |

---

## 5. 配置与运行链路

### 5.1 配置重载

```text
PlcConfigAppService.ReloadFromDatabase()
  → 查询站配置与点位配置
  → 组装 PlcConfig
  → 写入 ConfigContext.Config.PlcConfig
  → 调用 PlcClientFactory 创建客户端
  → 注册到 MachineContext.Plcs
```

### 5.2 扫描链路

```text
PlcScanWorker
    → 为每个启用站创建或维护 PlcStationScanRunner
    → Runner 独立执行连接检查与扫描循环
    → 自动连接 / 重连
    → 按点位读取
    → 写入 RuntimeContext.Instance.Plc
    → NotifySnapshotChanged()
```

当前实现意图是把扫描责任拆成两层：

1. `PlcScanWorker` 负责顶层调度、启动、停止与站集合管理；
2. `PlcStationScanRunner` 负责单站的连接维护、离线判断、重连与点位扫描。

这样做的直接收益是：

1. 单站离线不会让其它站跟着阻塞；
2. 断线日志可以按站节流；
3. UI 与运行时查询可以继续消费其他在线站的快照。

### 5.3 查询链路

```text
PlcRuntimeQueryService.QueryAllStations()
PlcRuntimeQueryService.QueryAllPoints()
```

查询时会把：

- 配置元数据；
- 运行时快照；
- 默认空态；

统一合并成 UI 可直接消费的对象。

### 5.4 调试链路

```text
PlcOperationService
  ├── TestReadPoint(pointName)
  ├── WritePoint(pointName, value, confirmed)
  ├── TestReadAddress(plcName, address, dataType, length)
  └── WriteAddress(plcName, address, dataType, value, length, confirmed)
```

---

## 5.5 当前异常前缀语义

### 站级运行态异常

`PlcStationScanRunner` 在站连接失败、站掉线、重连等待等场景下，统一通过 `ReportDisconnectIfNeeded()` 输出带 `[PlcDisconnected]` 前缀的消息。

该类消息表示：

1. 当前 PLC 站不可用；
2. 问题发生在“站级连接 / 可达性”层；
3. 运行时通常会继续尝试重连，而不是立即终止整个扫描系统。

### 协议层异常

`ProtocolPlcClient` 在 `Connect`、`Disconnect`、`Reconnect`、`IsConnected`、`ReadPoint`、`WritePoint` 等调用中，若识别到 `SocketException` 或其内层异常，会统一输出带 `[ProtocolSocketError]` 前缀的失败消息。

该类消息表示：

1. 当前异常来自协议库或 socket 通信层；
2. 它比站级离线更接近底层网络实现；
3. 排查时应优先查看 `ProtocolLib`、网络参数、端口、防火墙与设备可达性。

---

## 6. 调用参数示例

### 6.1 客户端配置示例

```csharp
var options = new M_ProtocolOptions
{
    protocolType = "modbustcp",
    connectionType = "tcp",
    ip = "192.168.1.10",
    port = 502,
    stationNo = 1,
    timeoutMs = 1000,
    byteOrder = "ABCD",
    wordOrder = "HighLow",
    stringEncoding = "ASCII"
};
```

### 6.2 读取请求示例

```csharp
var request = new M_PointReadRequest
{
    address = "40001",
    dataType = "uint16",
    length = 1
};
```

### 6.3 写入请求示例

```csharp
var request = new M_PointWriteRequest
{
    address = "40010",
    dataType = "bool",
    value = true,
    length = 1
};
```

### 6.4 直接地址调试示例

```csharp
var service = new PlcOperationService();
var readResult = service.TestReadAddress("PlcMain", "40030", "float", 1);
var writeResult = service.WriteAddress("PlcMain", "40010", "bool", true, 1, true);
```

### 6.5 按配置点位调试示例

```csharp
var service = new PlcOperationService();
var readResult = service.TestReadPoint("MachineReady");
var writeResult = service.WritePoint("StartCmd", true, true);
```

---

## 7. 地址格式示例

| 协议 | 说明 | 示例 |
|------|------|------|
| Modbus TCP | 线圈 | `00001` |
| Modbus TCP | 保持寄存器 | `40001` |
| Modbus TCP | 寄存器高字节 | `40010.H` |
| Modbus TCP | 寄存器低字节 | `40010.L` |
| Modbus TCP | 寄存器位 | `40010.0`、`40010.1` |
| S7 TCP | DB 区 | `DB1.0`、`DB1.2` |
| S7 TCP | M 区双字 | `MD10` |

---

## 8. 当前 UI 页面映射

### 8.1 `PLC.Status`

- 页面定位：站级通讯状态页
- 数据来源：`PlcRuntimeQueryService.QueryAllStations()`
- 交互：搜索、刷新、单轮扫描、启动扫描、停止扫描
- 权限：监视对操作员开放，扫描控制仅工程师 / 管理员

### 8.2 `PLC.Monitor`

- 页面定位：点位监视页
- 数据来源：`PlcRuntimeQueryService.QueryAllPoints()`
- 交互：按 PLC、分组、关键字筛选，低频刷新
- 权限：操作员可见

### 8.3 `PLC.Debug`

- 页面定位：工程调试页
- 数据来源：`PlcDebugPageModel` + `PlcOperationService`
- 交互：按配置点位读写、按地址读写、结果历史记录
- 权限：工程师 / 管理员

### 8.4 `SysConfig.Plc`

- 页面定位：PLC 唯一配置管理入口
- 数据来源：站 / 点位 CRUD + `PlcConfigAppService`
- 交互：站点位 CRUD、配置重载、扫描控制
- 权限：工程师 / 管理员

---

## 9. 使用说明（开发与联调）

### 9.1 开发时建议顺序

1. 先在 `SysConfig.Plc` 完成站与点位配置；
2. 重载配置，确认客户端已注册到 `MachineContext.Plcs`；
3. 打开 `PLC.Status` 确认站在线与扫描状态；
4. 打开 `PLC.Monitor` 确认点位值和质量；
5. 打开 `PLC.Debug` 做读写联调；
6. 在 `RunLogPage` 或日志文件中核对调试记录。

### 9.2 页面联调建议

- 配置页改动后优先点击“重载配置”；
- 若站离线，先在 `PLC.Status` 查看错误；
- 调试写入前确认权限与风险确认勾选；
- 点位读写失败时先检查：协议、地址、类型、长度、站连接状态。

---

## 10. 当前实现结论

PLC 模块当前已经形成：

- 配置闭环；
- 协议闭环；
- 扫描闭环；
- 查询闭环；
- 调试闭环；
- WinForms 页面闭环。

后续工作应集中在页面交互收口、字段完整性补充、日志审计和使用手册完善，而不是重新设计协议架构。

---

## 相关文档

- [PLC 协议库与 AM 上层分层架构](../01-architecture/plc-protocol-integration-design.md)
- [WinForms 解决方案架构](../01-architecture/winf-solution-architecture.md)
- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)
