# PLC 协议库与 AM 上层分层架构

**文档编号**：ARCH-PLC-001  
**版本**：3.0.0  
**状态**：已实现  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 文档目的

本文档描述当前仓库中 PLC 模块的**已落地实现**，覆盖：

- 分层边界与职责；
- 关键接口、模型和服务；
- 协议插件加载机制；
- 配置装配、后台扫描、运行时查询、手动读写链路；
- 调用参数示例与页面映射关系。

---

## 2. 当前设计结论

| 决策 | 当前实现 |
|------|----------|
| 极简点位模型 | `Address` 直接表达完整协议地址，不拆 AreaType / DB / BitIndex 等多层结构 |
| 统一类型表达 | `DataType` 使用字符串，如 `bool`、`uint16`、`float`、`string` |
| 统一长度字段 | `Length` 同时承载标量、字符串和数组长度 |
| AM 侧薄门面 | `IPlcClient` 仅做生命周期与结果转换，不承载协议算法 |
| 协议插件化 | `ProtocolAssemblyRegistry` 动态发现 `IProtocol` 实现 |
| 占位客户端兜底 | `NullPlcClient` 保证协议库缺失时系统可启动 |
| 运行态集中缓存 | `RuntimeContext.Instance.Plc` 统一保存站 / 点位扫描快照 |
| UI 低频采样 | 页面统一以低频定时器读取运行态，不直接高频事件驱动整页 |

---

## 3. 总体分层

```text
AMControlWinF / AM.PageModel
  → PlcStatusPage / PlcMonitorPage / PlcDebugPage / PlcConfigManagementPage
  → PlcStatusPageModel / PlcMonitorPageModel / PlcDebugPageModel / PlcConfigManagementPageModel
        |
        v
AM.DBService.Services.Plc
  → PlcConfigAppService
  → PlcStationCrudService / PlcPointCrudService
  → PlcRuntimeQueryService
  → PlcOperationService
  → PlcScanWorker
  → PlcClientFactory / ProtocolPlcClient / NullPlcClient / ProtocolAssemblyRegistry
        |
        v
AM.Model / AM.Core
  → PlcConfig / PlcStationConfig / PlcPointConfig
  → PlcRuntimeState / PlcStationRuntimeSnapshot / PlcPointRuntimeSnapshot
  → ConfigContext / MachineContext / RuntimeContext / SystemContext
        |
        v
ProtocolLib.CommonLib.Interface
  → IProtocol
        |
        +-- ProtocolLib.ModbusTcp.Protocol
        +-- ProtocolLib.S7Tcp.Protocol
```

### 3.1 分层职责

#### UI / PageModel 层负责

- 页面布局、交互、筛选、分页、选中项维护；
- 读取 `PlcRuntimeQueryService` 查询结果；
- 调用 `PlcOperationService`、`RuntimeTaskManager` 执行调试与扫描控制。

#### AM.DBService 层负责

- 站 / 点位 CRUD；
- DB → `ConfigContext` / `MachineContext.Plcs` 装配；
- 后台扫描调度与运行态写入；
- 统一读写入口与结果转换。

#### ProtocolLib 层负责

- 协议连接；
- 地址解析；
- 数据类型解释；
- 单点读写；
- 原始协议返回模型。

---

## 4. 接口定义

### 4.1 协议接口 `IProtocol`

协议库统一接口位于 `ProtocolLib.CommonLib.Interface.IProtocol`。

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

### 4.2 AM 侧客户端接口 `IPlcClient`

位于 `AM.Model.Interfaces.Plc.IPlcClient`。

```csharp
public interface IPlcClient
{
    string PlcName { get; }
    string ProtocolType { get; }
    string ConnectionType { get; }

    Result Configure(M_ProtocolOptions options);
    Result Connect();
    Result Disconnect();
    Result Reconnect();
    Result<bool> IsConnected();
    Result<M_PointData> ReadPoint(M_PointReadRequest request);
    Result<M_PointData> WritePoint(M_PointWriteRequest request);
}
```

### 4.3 配置与运行时接口

| 接口 | 说明 | 当前实现 |
|------|------|----------|
| `IPlcClientFactory` | 创建 PLC 客户端 | `PlcClientFactory` |
| `IPlcStationCrudService` | 站 CRUD | `PlcStationCrudService` |
| `IPlcPointCrudService` | 点位 CRUD | `PlcPointCrudService` |
| `IPlcConfigAppService` | DB → Context 装配 | `PlcConfigAppService` |
| `IPlcRuntimeService` | 运行时查询 | `PlcRuntimeQueryService` |
| `IPlcScanWorker` | 后台扫描工作单元 | `PlcScanWorker` |

---

## 5. 核心模型

### 5.1 配置模型

| 模型 | 说明 |
|------|------|
| `PlcConfig` | PLC 配置聚合对象 |
| `PlcStationConfig` | 站配置，描述协议、连接方式、端点、扫描参数 |
| `PlcPointConfig` | 点位配置，描述点位名称、分组、地址、类型、长度、访问模式 |

### 5.2 数据库实体

| 实体 | 表 | 说明 |
|------|----|------|
| `PlcStationConfigEntity` | `plc_station` | 站配置持久化 |
| `PlcPointConfigEntity` | `plc_point` | 点位配置持久化 |

### 5.3 运行时模型

| 模型 | 说明 |
|------|------|
| `PlcRuntimeState` | PLC 运行时缓存总入口 |
| `PlcStationRuntimeSnapshot` | 站级快照 |
| `PlcPointRuntimeSnapshot` | 点位级快照 |

### 5.4 协议模型

| 模型 | 说明 |
|------|------|
| `M_ProtocolOptions` | 协议连接配置 |
| `M_PointReadRequest` | 点位读取请求 |
| `M_PointWriteRequest` | 点位写入请求 |
| `M_PointData` | 读写结果数据 |
| `M_Return<T>` | 协议层统一返回模型 |

---

## 6. 协议参数定义与示例

### 6.1 `M_ProtocolOptions`

常用字段：

| 字段 | 说明 | 示例 |
|------|------|------|
| `protocolType` | 协议名 | `modbustcp` / `s7tcp` |
| `connectionType` | 连接方式 | `tcp` |
| `ip` | 设备 IP | `192.168.1.10` |
| `port` | 端口 | `502` / `102` |
| `stationNo` | 站号 | `1` |
| `rack` | S7 机架号 | `0` |
| `slot` | S7 槽号 | `1` |
| `timeoutMs` | 超时毫秒 | `1000` |
| `byteOrder` | 字节序 | `ABCD` / `DCBA` |
| `wordOrder` | 字序 | `HighLow` / `LowHigh` |
| `stringEncoding` | 字符串编码 | `ASCII` / `UTF-8` |

示例：

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

### 6.2 `M_PointReadRequest`

```csharp
var request = new M_PointReadRequest
{
    address = "40001",
    dataType = "uint16",
    length = 1
};
```

### 6.3 `M_PointWriteRequest`

```csharp
var request = new M_PointWriteRequest
{
    address = "40010",
    dataType = "bool",
    value = true,
    length = 1
};
```

### 6.4 地址格式示例

| 协议 | 含义 | 示例 |
|------|------|------|
| Modbus TCP | 线圈 | `00001` |
| Modbus TCP | 保持寄存器 | `40001` |
| Modbus TCP | 寄存器高字节 | `40010.H` |
| Modbus TCP | 寄存器低字节 | `40010.L` |
| Modbus TCP | 寄存器内 bit | `40010.0`、`40010.1` |
| S7 TCP | DB 区 | `DB1.0`、`DB1.2` |
| S7 TCP | M 区双字 | `MD10` |

---

## 7. 启动装配链路

`AppBootstrap.Initialize()` 中 PLC 相关链路如下：

```text
PlcConfigSeedService.EnsureSeedData()
  → ProtocolAssemblyRegistry.Reload()
  → PlcConfigAppService.ReloadFromDatabase()
  → RegisterRuntimeWorkerWithTolerance(new PlcScanWorker(), autoStart: true)
```

### 7.1 `PlcConfigAppService.ReloadFromDatabase()`

职责：

1. 读取 `plc_station` 与 `plc_point`；
2. 组装 `PlcConfig`；
3. 写回 `ConfigContext.Instance.Config.PlcConfig`；
4. 使用 `PlcClientFactory` 为每个启用站创建客户端；
5. 注册到 `MachineContext.Instance.Plcs`；
6. 供 `PlcScanWorker` 与 `PlcOperationService` 后续复用。

---

## 8. 后台扫描链路

### 8.1 `PlcScanWorker`

核心职责：

- 按站扫描；
- 自动重连；
- 调用协议客户端读取点位；
- 写入 `RuntimeContext.Instance.Plc`；
- 更新整体扫描时间与服务运行状态。

### 8.2 运行态缓存 `PlcRuntimeState`

当前 `PlcRuntimeState` 包含：

| 字段 / 事件 | 说明 |
|-------------|------|
| `IsScanServiceRunning` | 扫描服务运行状态 |
| `LastScanTime` | 最近一轮整体扫描时间 |
| `ScanIntervalMs` | 当前扫描周期 |
| `SetStationSnapshot(...)` | 写入站级快照 |
| `SetPointSnapshot(...)` | 写入点位快照 |
| `GetStationSnapshots()` | 查询全部站快照 |
| `GetPointSnapshots()` | 查询全部点位快照 |
| `GetPointsByPlc(plcName)` | 按 PLC 查询点位 |
| `GetPointsByGroup(plcName, groupName)` | 按分组查询点位 |
| `SnapshotChanged` | 整轮扫描完成事件 |
| `StationSnapshotChanged` | 单站快照变化事件 |
| `PointSnapshotChanged` | 单点快照变化事件 |

### 8.3 扫描逻辑摘要

```text
PlcScanWorker.ScanLoopAsync()
  → 遍历启用站
  → 获取 MachineContext.Plcs[plcName]
  → 确认连接 / 自动重连
  → 遍历站下启用点位
  → client.ReadPoint(request)
  → runtime.SetPointSnapshot(snapshot)
  → runtime.SetStationSnapshot(snapshot)
  → runtime.MarkScanTime(now)
  → runtime.NotifySnapshotChanged()
```

---

## 9. 运行时查询与调试链路

### 9.1 `PlcRuntimeQueryService`

职责：

- 查询全部站；
- 查询全部点位；
- 合并配置与运行态快照；
- 为 UI 提供稳定的显示对象。

### 9.2 `PlcOperationService`

职责：

- 按配置点位读取 / 写入；
- 按直接地址读取 / 写入；
- 写入后同步更新运行态；
- 统一返回 `Result` / `Result<M_PointData>`。

### 9.3 调用示例

#### 按配置点位读取

```csharp
var service = new PlcOperationService();
var result = service.TestReadPoint("LineReady");
```

#### 按直接地址读取

```csharp
var service = new PlcOperationService();
var result = service.TestReadAddress("PlcMain", "40001", "uint16", 1);
```

#### 按配置点位写入

```csharp
var service = new PlcOperationService();
var result = service.WritePoint("StartCmd", true, true);
```

#### 按直接地址写入

```csharp
var service = new PlcOperationService();
var result = service.WriteAddress("PlcMain", "40010", "bool", true, 1, true);
```

说明：最后一个 `true` 表示高风险写入确认已通过。

---

## 10. 协议插件机制

### 10.1 `ProtocolAssemblyRegistry`

职责：

1. 扫描 `Protocols/` 目录；
2. 加载 `ProtocolLib.*.dll`；
3. 反射发现 `IProtocol` 实现；
4. 以协议名注册到内部字典；
5. 提供 `TryResolve(protocolType)`。

### 10.2 当前已注册协议

| 协议名 | 实现类 |
|--------|--------|
| `modbustcp` | `ProtocolLib.ModbusTcp.Protocol` |
| `s7tcp` | `ProtocolLib.S7Tcp.Protocol` |

### 10.3 协议不可用时的降级

当协议 DLL 缺失或解析失败时，系统使用 `NullPlcClient`：

- 保持配置链路可完成；
- 页面仍可进入；
- 扫描与读写返回明确失败结果；
- 避免启动阶段直接崩溃。

---

## 11. 当前 UI 页面映射

| 页面 | 数据来源 | 主要服务 |
|------|----------|----------|
| `PLC.Status` | 全部站快照 | `PlcRuntimeQueryService`、`RuntimeTaskManager` |
| `PLC.Monitor` | 全部点位快照 | `PlcRuntimeQueryService` |
| `PLC.Debug` | 配置点位 + 直接地址调试 | `PlcOperationService` |
| `SysConfig.Plc` | 站与点位配置 | `PlcStationCrudService`、`PlcPointCrudService`、`PlcConfigAppService` |

当前 4 个页面均已在 WinForms 中具备首版实现。

---

## 12. 当前结论

PLC 模块当前已具备完整闭环：

- 配置可落库；
- 配置可重载到上下文；
- 客户端可按协议插件创建；
- 后台可自动扫描；
- 运行态可统一查询；
- 调试读写可直接调用；
- WinForms 页面已具备可继续收口的首版。

后续工作重点不再是重做架构，而是补齐字段、优化页面交互、完善手册与测试。

---

## 相关文档

- [PLC 通信模块实现说明](../03-features/plc-communication.md)
- [WinForms 解决方案架构](winf-solution-architecture.md)
- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)

