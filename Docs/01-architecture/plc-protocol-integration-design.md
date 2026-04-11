# PLC 协议库与 AM 上层分层架构

**文档编号**：ARCH-PLC-001  
**版本**：2.0.0  
**状态**：已实现  
**最后更新**：2026-05-09  
**维护人**：Am

---

## 1. 文档目的

本文档描述 PLC 协议库与 AM 上层之间已落地的分层边界、类关系、数据流与关键设计决策。

---

## 2. 核心设计决策

| 决策 | 说明 |
|------|------|
| 协议行为全部收敛到 ProtocolLib | AM.DBService 不直接操作 ModbusTCP、SiemensS7 等底层对象 |
| IPlcClient 是 AM 侧薄门面 | 负责生命周期管理和结果转换，不承载协议实现 |
| 无 AreaType / BlockRead | 当前版本极简：Address 直接表示完整协议地址，不拆分区域字段；无块读写接口 |
| DataType 统一字符串表达 | bool/uint8/int8/uint16/int16/uint32/int32/uint64/int64/float/double/string，不使用枚举 |
| Length 统一长度字段 | 标量=1，字符串=字符长度，数组=元素个数 |
| 协议插件化加载 | ProtocolAssemblyRegistry 从 Protocols/ 目录扫描 DLL，反射发现 IProtocol 实现 |
| NullPlcClient 保障链路完整 | 协议库不可用时返回占位客户端，保证上下文装配和启动流程不中断 |

---

## 3. 总体分层结构

```
AMControlWinF / AM.DBService
  PlcConfigAppService          DB → ConfigContext + MachineContext 装配
  PlcScanWorker                100ms 后台循环 → RuntimeContext.Plc 更新
  PlcRuntimeQueryService       运行时快照查询（站 / 点位 / 全量）
  PlcOperationService          手动读写（按名称 or 直接地址）
  PlcClientFactory             创建 ProtocolPlcClient，BuildProtocolOptions 映射
  ProtocolPlcClient            AM 侧门面（反射实例化 IProtocol → 统一转发）
  NullPlcClient                占位客户端（协议不可用时保持链路完整）
        |
        v
ProtocolLib.CommonLib.Interface
  IProtocol                    统一协议接口（Configure/Connect/Disconnect/Reconnect/IsConnected/ReadPoint/WritePoint）
        |
        +---- ProtocolLib.ModbusTcp.Protocol    Modbus TCP 完整实现
        |
        +---- ProtocolLib.S7Tcp.Protocol        Siemens S7 TCP 完整实现
```

### 3.1 分层职责边界

#### ProtocolLib 负责

- 协议连接生命周期管理
- 单点读写（标量 / 字符串 / 数组）
- 地址解释（完整协议地址字符串，内部解析）
- 类型解释（string 类型名 → 协议对应类型）
- 协议结果模型返回（M_Return\<M_PointData\>）

#### AM.DBService / AM.Core / AM.Model 负责

- 配置装配（DB → PlcConfig → MachineContext.Plcs）
- 运行时调度（PlcScanWorker 周期扫描）
- 结果缓存（PlcRuntimeState → RuntimeContext.Instance.Plc）
- 页面与业务服务调用（PlcRuntimeQueryService / PlcOperationService）

---

## 4. 接口定义

### 4.1 IProtocol（ProtocolLib.CommonLib.Interface）

```csharp
public interface IProtocol
{
    M_Return<bool>        Configure(M_ProtocolOptions options);
    M_Return<bool>        Connect();
    M_Return<bool>        Disconnect();
    M_Return<bool>        Reconnect();
    M_Return<bool>        IsConnected();
    M_Return<M_PointData> ReadPoint(M_PointReadRequest request);
    M_Return<M_PointData> WritePoint(M_PointWriteRequest request);
}
```

### 4.2 IPlcClient（AM.Model.Interfaces.Plc）

```csharp
public interface IPlcClient
{
    string PlcName         { get; }
    string ProtocolType    { get; }
    string ConnectionType  { get; }

    Result               Configure(M_ProtocolOptions options);
    Result               Connect();
    Result               Disconnect();
    Result               Reconnect();
    Result<bool>         IsConnected();
    Result<M_PointData>  ReadPoint(M_PointReadRequest request);
    Result<M_PointData>  WritePoint(M_PointWriteRequest request);
}
```

---

## 5. 协议模型（ProtocolLib.CommonLib.Model）

### 5.1 M_ProtocolOptions — 协议连接配置

```csharp
public class M_ProtocolOptions
{
    public string  protocolType    { get; set; }  // 协议名，例如 modbustcp / s7tcp
    public string  connectionType  { get; set; }  // 连接方式，例如 tcp
    public string  ip              { get; set; }
    public int     port            { get; set; }
    public short?  stationNo       { get; set; }
    public short?  rack            { get; set; }
    public short?  slot            { get; set; }
    public int     timeoutMs       { get; set; }
    public string  byteOrder       { get; set; }
    public string  wordOrder       { get; set; }
    public string  stringEncoding  { get; set; }
}
```

### 5.2 M_PointReadRequest — 点位读取请求

```csharp
public class M_PointReadRequest
{
    public string address   { get; set; }  // 完整协议地址，例如 40001 / DB1.0 / 40040[20]
    public string dataType  { get; set; }  // 字符串类型名，例如 uint16 / string / bool
    public int    length    { get; set; }  // 标量=1，字符串=字符长度，数组=元素个数
}
```

### 5.3 M_PointWriteRequest — 点位写入请求

```csharp
public class M_PointWriteRequest
{
    public string address   { get; set; }
    public string dataType  { get; set; }
    public object value     { get; set; }  // 标量 / 字符串 / 数组均通过此字段传入
    public int    length    { get; set; }
}
```

### 5.4 M_PointData — 读写结果

```csharp
public class M_PointData
{
    public string  address    { get; set; }
    public string  dataType   { get; set; }
    public int     length     { get; set; }
    public string  value      { get; set; }   // 统一文本承载，数组由协议层序列化
    public byte[]  rawBuffer  { get; set; }   // 原始字节，需要底层结果时使用
    public string  quality    { get; set; }   // Good / Error / Disconnected
}
```

---

## 6. 数据流

### 6.1 启动装配流程

```
AppBootstrap.Initialize()
  → PlcConfigSeedService.EnsureSeedData()    // 初始化默认测试种子数据
  → ProtocolAssemblyRegistry.Reload()        // 扫描 Protocols/ 目录，反射注册协议实现
  → PlcConfigAppService.ReloadFromDatabase() // DB → PlcConfig → ConfigContext + MachineContext.Plcs
  → PlcScanWorker 注册到 RuntimeTaskManager  // autoStart=true，服务启动即开始扫描
```

### 6.2 后台扫描流程（PlcScanWorker，100ms）

```
ScanLoopAsync(CancellationToken)
  → RefreshScanCacheIfNeeded()               // 引用级比较，PlcConfig 对象未变则跳过重建
  → ScanStations(forceAll=false, now)
      foreach 启用站点：
        ShouldScanStation(station, now)       // 按各站 ScanIntervalMs 控制扫描频率
        GetClient(MachineContext.Plcs)        // 复用 MachineContext 中已建立的客户端
        TryEnsureClientConnected()            // 自动重连，NextReconnectTime 控制频率
        foreach 站下点位：
          client.ReadPoint(request)
          runtime.SetPointSnapshot(...)      // 写入 RuntimeContext.Plc 点位快照
        runtime.SetStationSnapshot(...)      // 写入 RuntimeContext.Plc 站快照
  → runtime.MarkScanTime(now)
  → runtime.NotifySnapshotChanged()          // 触发 UI 刷新事件（低频采样，~500ms）
```

### 6.3 点位手动写入流程（PlcOperationService）

```
PlcOperationService.WritePoint(pointName, value, confirmed)
  → FindPointConfig(pointName)               // 从 ConfigContext.PlcConfig.Points 查找
  → ValidatePointWrite(point, confirmed)     // 写保护校验
  → GetConnectedClient(point.PlcName)        // 从 MachineContext.Plcs 获取已连接客户端
  → client.WritePoint(M_PointWriteRequest)   // ProtocolPlcClient → IProtocol.WritePoint
  → UpdatePointRuntimeAfterWrite(...)        // 更新 RuntimeContext 写入后快照
```

### 6.4 运行时查询流程（PlcRuntimeQueryService）

```
QueryAllStations()
  → ConfigContext.PlcConfig.Stations         // 配置来源（保证顺序和完整性）
  → RuntimeContext.Plc.GetStationSnapshots() // 运行时快照来源
  → MergeStationSnapshot(config, runtime)    // 合并：配置侧补全 DisplayName/Protocol/Enabled
  → 返回 List<PlcStationRuntimeSnapshot>

QueryAllPoints()
  → ConfigContext.PlcConfig.Points
  → RuntimeContext.Plc.GetPointSnapshots()
  → MergePointSnapshot(config, runtime)
  → 返回 List<PlcPointRuntimeSnapshot>
```

---

## 7. 协议插件加载机制

`ProtocolAssemblyRegistry`（AM.DBService.Services.Plc.Driver）负责：

1. 启动时扫描 `AppDomain.CurrentDomain.BaseDirectory/Protocols/` 目录；
2. 加载匹配 `ProtocolLib.*.dll` 的程序集（排除 `CommonLib`）；
3. 反射枚举程序集中实现 `IProtocol` 的具体类型；
4. 按协议名（取 `static ProtocolName` 字段或类名，小写）注册到内部字典；
5. `ProtocolPlcClient.Configure()` 调用 `TryResolve(protocolType)` 解析对应类型，`Activator.CreateInstance` 创建实例。

已注册协议：

| 协议名 | DLL | 类 |
|--------|-----|----|
| `modbustcp` | ProtocolLib.ModbusTcp.dll | ProtocolLib.ModbusTcp.Protocol |
| `s7tcp` | ProtocolLib.S7Tcp.dll | ProtocolLib.S7Tcp.Protocol |

---

## 8. 运行时状态缓存（RuntimeContext.Plc）

`PlcRuntimeState`（AM.Model.Runtime）维护：

| 字段 | 说明 |
|------|------|
| `IsScanServiceRunning` | 扫描服务运行状态 |
| `ScanIntervalMs` | 当前扫描间隔 |
| `LastScanTime` | 最后一次整体扫描时间 |
| `StationSnapshots` | 站快照字典（ConcurrentDictionary，按 PlcName 索引） |
| `PointSnapshots` | 点位快照字典（ConcurrentDictionary，按 PointName 索引） |
| `SnapshotChanged` | 全局快照变化事件（UI 订阅，~500ms 低频采样） |
| `StationSnapshotChanged` | 站级快照变化事件（按 PlcName 分发） |

---

## 9. 已实现的服务层接口

| 接口 | 实现 | 说明 |
|------|------|------|
| `IPlcClient` | `ProtocolPlcClient`、`NullPlcClient` | AM 侧 PLC 客户端门面 |
| `IPlcClientFactory` | `PlcClientFactory` | 工厂，BuildProtocolOptions 映射后 Create |
| `IPlcStationCrudService` | `PlcStationCrudService` | 站 CRUD（plc_station 表） |
| `IPlcPointCrudService` | `PlcPointCrudService` | 点位 CRUD（plc_point 表） |
| `IPlcConfigAppService` | `PlcConfigAppService` | DB → Context 装配 |
| `IPlcScanWorker` | `PlcScanWorker` | 后台扫描工作单元 |
| `IPlcRuntimeService` | `PlcRuntimeQueryService` | 运行时快照查询 |
| —（无独立接口） | `PlcOperationService` | 手动读写 |

---

## 10. 待实现（UI 层）

后端链路全部贯通，以下均为 WinForms 页面待开发：

| 导航 | 页面 | 所需后端能力 |
|------|------|-------------|
| PLC > Monitor | 站状态总览 | `QueryAllStations()` |
| PLC > Register | 点位实时监视 | `QueryAllPoints()` + SnapshotChanged 事件 |
| PLC > Status | 连接状态详情 | `QueryStation(plcName)` |
| PLC > Write | 手动写入调试 | `PlcOperationService.WritePoint()` |
| SysConfig > Plc | 站 & 点位配置管理 | `IPlcStationCrudService` / `IPlcPointCrudService` / `PlcConfigAppService.ReloadFromDatabase()` |

---

## 相关文档

- [PLC 通信功能文档](../03-features/plc-communication.md)
- [数据库表结构](../09-database-config/README.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
