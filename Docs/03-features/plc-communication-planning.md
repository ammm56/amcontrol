# PLC 通信模块功能文档

**文档编号**：FEAT-PLC-001  
**版本**：2.0.0  
**状态**：后端已实现，UI 待开发  
**最后更新**：2026-05-09  
**维护人**：Am

---

## 1. 文档目的

本文档描述 `ammm56/amcontrol` 解决方案中 PLC 通信模块的**已实现**功能，包括：

- 数据模型与数据库实体
- 配置服务、驱动层、扫描服务、运行时查询与操作服务的职责边界
- 与 ConfigContext / MachineContext / RuntimeContext 全局上下文的集成
- 页面导航规划及当前实现状态

> **注意**：原规划文档中提及的 AreaType 枚举、BlockRead/BlockWrite、PlcVendorType / PlcConnectionType / PlcProtocolType / PlcDataType 枚举等方案**均未采用**。  
> 当前实现采用极简模型：Address 字符串直接表达完整协议地址，DataType 使用字符串名称，Length 统一表达长度。

---

## 2. 模块总览

PLC 模块当前实现状态：

| 层级 | 内容 | 状态 |
|------|------|------|
| 数据模型（AM.Model） | 实体、配置对象、快照、接口 | ✅ 完成 |
| 数据库服务（AM.DBService） | 配置 CRUD、驱动工厂、扫描、查询、操作 | ✅ 完成 |
| 协议库（ProtocolLib） | ModbusTcp、S7Tcp 完整协议实现 | ✅ 完成 |
| 启动集成（AM.App） | 种子、装配、注册后台工作单元 | ✅ 完成 |
| WinForms UI（AMControlWinF） | PLC 全部页面 | ❌ 待开发 |

---

## 3. 配置模型

### 3.1 PlcStationConfig（PLC 站运行时配置）

| 字段 | 类型 | 说明 |
|------|------|------|
| Name | string | 内部唯一名称，运行时索引键 |
| DisplayName | string | 显示名称 |
| Vendor | string | 厂商，例如 Siemens / Mitsubishi / ModbusGeneric |
| Model | string | 型号，例如 S7-1200 / FX5U |
| ConnectionType | string | 连接方式，例如 tcp / serial |
| ProtocolType | string | 通讯协议，例如 modbustcp / s7tcp |
| IpAddress | string | TCP IP 地址 |
| Port | int? | TCP 端口（默认按协议选取：ModbusTcp=502，S7=102） |
| ComPort | string | 串口号（串口连接时使用） |
| BaudRate / DataBits / Parity / StopBits | 串口参数 | 串口连接时使用 |
| StationNo / NetworkNo / PcNo / Rack / Slot | 协议参数 | PLC 协议寻址参数 |
| TimeoutMs | int | 读写超时（毫秒） |
| ReconnectIntervalMs | int | 自动重连间隔（毫秒） |
| ScanIntervalMs | int | 该站扫描间隔（毫秒） |
| IsEnabled | bool | 是否启用 |
| SortOrder | int | 显示排序 |

### 3.2 PlcPointConfig（PLC 点位运行时配置）

| 字段 | 类型 | 说明 |
|------|------|------|
| PlcName | string | 所属 PLC 站名称 |
| Name | string | 内部唯一点位名 |
| DisplayName | string | 显示名称 |
| GroupName | string | 分组名称（UI 分类用） |
| Address | string | 完整协议地址（ModbusTcp: 40001；S7: DB1.0；数组: 40040[20]） |
| DataType | string | 类型名：bool/uint8/int8/uint16/int16/uint32/int32/uint64/int64/float/double/string |
| Length | int | 标量=1，字符串=字符长度，数组=元素个数 |
| AccessMode | string | 访问模式，例如 read / write / readwrite |
| IsEnabled | bool | 是否启用 |
| SortOrder | int | 显示排序 |

### 3.3 PlcConfig（配置聚合对象）

挂入 `ConfigContext.Instance.Config.PlcConfig`：

```csharp
public class PlcConfig
{
    public List<PlcStationConfig> Stations { get; set; }
    public List<PlcPointConfig>   Points   { get; set; }
}
```

---

## 4. 数据库实体

| 表名 | 实体 | 说明 |
|------|------|------|
| `plc_station` | `PlcStationConfigEntity` | PLC 站配置（SqlSugar CodeFirst） |
| `plc_point` | `PlcPointConfigEntity` | PLC 点位配置 |

详见 [数据库表结构文档](../09-database-config/README.md)。

---

## 5. 服务层

### 5.1 配置 CRUD 服务

| 服务 | 表 | 说明 |
|------|----|------|
| `PlcStationCrudService` | plc_station | 站增删查改 + EnsureTables |
| `PlcPointCrudService` | plc_point | 点位增删查改 |

### 5.2 配置应用服务（PlcConfigAppService）

职责：

1. `EnsureTables()` — CodeFirst 创建 plc_station / plc_point 表
2. `QueryAll()` — 读取全部配置，组装为 `PlcConfig`
3. `ReloadFromDatabase()` — 从 DB 重载配置 → 写入 ConfigContext → 创建 IPlcClient → 注册到 MachineContext.Plcs

### 5.3 配置种子服务（PlcConfigSeedService）

启动时自动补入最小默认数据（一个 ModbusTcp 测试站 + 若干测试点位）。  
若表中已存在数据则跳过，不重复初始化。

### 5.4 驱动工厂（PlcClientFactory + ProtocolPlcClient）

```
PlcClientFactory.Create(stationConfig)
  → BuildProtocolOptions(stationConfig)        // 站配置 → M_ProtocolOptions
  → new ProtocolPlcClient(stationConfig)       // 创建门面客户端
  → client.Configure(options)                  // 门面内部：ProtocolAssemblyRegistry.TryResolve
                                               //           → Activator.CreateInstance(IProtocol)
                                               //           → protocol.Configure(options)
```

若 `TryResolve` 失败（协议 DLL 未加载），`PlcClientFactory` 返回 `NullPlcClient` 占位，保证启动链路不中断。

### 5.5 协议插件注册表（ProtocolAssemblyRegistry）

- 扫描 `BaseDirectory/Protocols/` 目录下的 `ProtocolLib.*.dll`
- 排除 CommonLib
- 反射发现实现 `IProtocol` 的类，取 `static ProtocolName` 字段（小写）为注册键
- 已注册：`modbustcp` → ProtocolLib.ModbusTcp.Protocol；`s7tcp` → ProtocolLib.S7Tcp.Protocol

### 5.6 后台扫描（PlcScanWorker）

- 继承 `IRuntimeWorker`，注册到 `RuntimeTaskManager`，`autoStart=true`
- 扫描循环周期：取各启用站 `ScanIntervalMs` 最小值（最低 20ms，默认 100ms）
- 配置缓存：引用级比较 `PlcConfig` 对象，配置未变则不重建分组
- 自动重连：按站维护 `NextReconnectTime`，连接失败后隔 `ReconnectIntervalMs` 才重试
- 扫描结果写入 `RuntimeContext.Instance.Plc`（点位快照 + 站快照），触发 `SnapshotChanged` 事件

### 5.7 运行时查询（PlcRuntimeQueryService）

| 方法 | 说明 |
|------|------|
| `Start()` | 启动扫描工作单元 |
| `Stop()` | 停止扫描工作单元 |
| `ScanOnce()` | 手动执行单轮扫描 |
| `QueryStation(plcName)` | 查询指定站快照（配置 + 运行时合并） |
| `QueryPoint(pointName)` | 查询指定点位快照 |
| `QueryAllStations()` | 查询全部站快照列表 |
| `QueryAllPoints()` | 查询全部点位快照列表 |

快照合并策略：以运行时快照为主，配置侧补全 DisplayName / IsEnabled / CurrentProtocol 等字段。

### 5.8 运行时操作（PlcOperationService）

| 方法 | 说明 |
|------|------|
| `WritePoint(pointName, value, confirmed)` | 按配置点位名称写入（含写保护校验） |
| `TestReadPoint(pointName)` | 按配置点位名称测试读取 |
| `WriteAddress(plcName, address, dataType, value, length)` | 调试页直接地址写入 |
| `TestReadAddress(plcName, address, dataType, length)` | 调试页直接地址读取 |

---

## 5.9 WinForms 页面层级规划

### 页面归属决策

PLC 相关页面在 WinForms 中分为两类：

1. 运行期页面：放在一级导航 `PLC`
2. 配置期页面：放在一级导航 `SysConfig`

具体归属如下：

| 一级导航 | 二级页面 | 说明 |
|----------|----------|------|
| `PLC` | `PLC.Status` | 站级通讯状态与扫描状态 |
| `PLC` | `PLC.Monitor` | 点位实时监视 |
| `PLC` | `PLC.Register` | 原始地址/点位读取调试 |
| `PLC` | `PLC.Write` | 原始地址/点位写入调试 |
| `SysConfig` | `SysConfig.Plc` | PLC 站/点位配置与重载 |

保留 `SysConfig.Plc` 在系统配置下的原因：

- PLC 站与点位本质上属于系统接入配置；
- 配置修改后需要执行 `PlcConfigAppService.ReloadFromDatabase()` 重建运行时上下文；
- 与 `Camera / Sensor / Scanner / Mes / Runtime` 同类页面职责一致；
- 可避免运行页与配置页混在同一一级导航下导致操作语义混乱。

### 页面层级树

```text
PLC
├── PLC.Status
├── PLC.Monitor
├── PLC.Register
└── PLC.Write

SysConfig
└── SysConfig.Plc
```

### 页面实现顺序建议

1. `SysConfig.Plc`
2. `PLC.Status`
3. `PLC.Monitor`
4. `PLC.Register`
5. `PLC.Write`

顺序理由：先完成配置闭环，再完成运行状态可视化，最后进入工程读写调试。

---

## 5.10 WinForms 页面结构规划

### `PLC.Status`

- 顶部工具栏：刷新、单轮扫描、启动扫描、停止扫描、搜索站名
- 顶部统计卡：总站数、在线数、离线数、扫描状态
- 主体区：左侧站列表，右侧站详情
- 服务来源：`PlcRuntimeQueryService.QueryAllStations()`

### `PLC.Monitor`

- 顶部工具栏：站筛选、分组筛选、关键字搜索
- 顶部统计卡：点位总数、Good 数、Error 数、Disconnected 数
- 主体区：左侧点位列表，右侧点位详情
- 服务来源：`PlcRuntimeQueryService.QueryAllPoints()`
- 刷新策略：~500ms 低频采样刷新，不直接对每次快照变化整页重绘

### `PLC.Register`

- 顶部模式区：配置点位读取 / 原始地址读取切换
- 左侧区：配置点位列表
- 右侧区：读取调试面板
- 底部区：最近读取结果
- 服务来源：`PlcOperationService.TestReadPoint(...)`、`PlcOperationService.TestReadAddress(...)`

### `PLC.Write`

- 顶部风险提示区：明确高风险写入
- 左侧区：可写点位列表
- 右侧区：写入调试面板
- 底部区：最近写入结果
- 服务来源：`PlcOperationService.WritePoint(...)`、`PlcOperationService.WriteAddress(...)`
- 权限：`Engineer / Am`

### `SysConfig.Plc`

- 第一段：PLC 站配置区
- 第二段：点位配置区
- 第三段：配置重载与扫描控制区
- 服务来源：`PlcStationCrudService`、`PlcPointCrudService`、`PlcConfigAppService`
- 当前阶段保持为单个二级页面，不再拆分 `Station` / `Point` / `Runtime` 多页结构

---

## 5.11 WinForms 文件清单规划

### PageModel 文件

```text
AM.PageModel/
├── Plc/
│   ├── PlcStatusPageModel.cs
│   ├── PlcMonitorPageModel.cs
│   ├── PlcRegisterPageModel.cs
│   ├── PlcWritePageModel.cs
│   ├── PlcStationViewItem.cs
│   ├── PlcPointViewItem.cs
│   ├── PlcRegisterReadResultItem.cs
│   └── PlcWriteResultItem.cs
└── SysConfig/
    ├── PlcConfigManagementPageModel.cs
    ├── PlcStationEditorModel.cs
    └── PlcPointEditorModel.cs
```

### WinForms 页面文件

```text
AMControlWinF/Views/
├── Plc/
│   ├── PlcStatusPage.cs
│   ├── PlcStatusPage.Designer.cs
│   ├── PlcMonitorPage.cs
│   ├── PlcMonitorPage.Designer.cs
│   ├── PlcRegisterPage.cs
│   ├── PlcRegisterPage.Designer.cs
│   ├── PlcWritePage.cs
│   └── PlcWritePage.Designer.cs
└── SysConfig/
    ├── PlcConfigManagementPage.cs
    └── PlcConfigManagementPage.Designer.cs
```

### WinForms 子控件与对话框文件

```text
AMControlWinF/Views/
├── Plc/
│   ├── PlcStationCardControl.cs
│   ├── PlcStationCardControl.Designer.cs
│   ├── PlcStationDetailControl.cs
│   ├── PlcStationDetailControl.Designer.cs
│   ├── PlcPointVirtualListControl.cs
│   ├── PlcPointVirtualListControl.Designer.cs
│   ├── PlcPointDetailControl.cs
│   ├── PlcPointDetailControl.Designer.cs
│   ├── PlcReadDebugPanelControl.cs
│   ├── PlcReadDebugPanelControl.Designer.cs
│   ├── PlcWriteDebugPanelControl.cs
│   └── PlcWriteDebugPanelControl.Designer.cs
└── SysConfig/
    ├── PlcStationListControl.cs
    ├── PlcStationListControl.Designer.cs
    ├── PlcPointListControl.cs
    ├── PlcPointListControl.Designer.cs
    ├── PlcConfigActionPanelControl.cs
    ├── PlcConfigActionPanelControl.Designer.cs
    ├── PlcStationEditDialog.cs
    ├── PlcStationEditDialog.Designer.cs
    ├── PlcPointEditDialog.cs
    ├── PlcPointEditDialog.Designer.cs
    ├── PlcStationDeleteConfirmDialog.cs
    ├── PlcStationDeleteConfirmDialog.Designer.cs
    ├── PlcPointDeleteConfirmDialog.cs
    └── PlcPointDeleteConfirmDialog.Designer.cs
```

以上文件清单用于后续代码落地前的结构预留与任务拆分，不代表当前已实现。

---

## 6. 运行时状态快照

### 6.1 PlcStationRuntimeSnapshot

| 字段 | 说明 |
|------|------|
| PlcName / DisplayName | 站标识 |
| IsEnabled | 是否启用 |
| IsConnected | 当前是否已连接 |
| IsScanRunning | 扫描服务运行状态 |
| LastConnectTime | 最近一次成功连接时间 |
| LastScanTime | 最近一次扫描时间 |
| LastError | 最近错误信息 |
| SuccessReadCount / FailedReadCount | 读取统计 |
| AverageReadMs | 平均读取耗时 |
| CurrentProtocol / CurrentConnectionType | 当前协议/连接方式 |

### 6.2 PlcPointRuntimeSnapshot

| 字段 | 说明 |
|------|------|
| PlcName / PointName / DisplayName / GroupName | 点位标识 |
| AddressText | 地址文本 |
| DataType | 数据类型 |
| ValueText | 显示值（字符串形式） |
| RawValue | 原始值文本 |
| Quality | 质量：Good / Disconnected / Error |
| UpdateTime | 最近更新时间 |
| IsConnected / HasError / ErrorMessage | 连接与错误状态 |

---

## 7. 与全局上下文的集成

| 上下文 | PLC 相关字段 |
|--------|------------|
| `ConfigContext.Config.PlcConfig` | 装配后的 PLC 站与点位配置（不写入 config.json，JsonIgnore） |
| `MachineContext.Plcs` | `ConcurrentDictionary<string, IPlcClient>`，按 PlcName 索引 |
| `RuntimeContext.Plc` | `PlcRuntimeState`，维护站/点位快照与扫描服务状态 |

---

## 8. 启动集成（AppBootstrap）

```
步骤 5.1  PlcConfigSeedService.EnsureSeedData()      初始化默认种子数据
步骤 5.2  ProtocolAssemblyRegistry.Reload()           扫描并注册协议 DLL
步骤 6.1  PlcConfigAppService.ReloadFromDatabase()    DB → 装配 MachineContext.Plcs
步骤 8.2  PlcScanWorker 注册到 RuntimeTaskManager    autoStart=true，随服务启动
```

---

## 9. 协议库（ProtocolLib）

| 协议 | 程序集 | 完整实现能力 |
|------|--------|-------------|
| Modbus TCP | ProtocolLib.ModbusTcp.dll | Connect/Disconnect/Reconnect/ReadPoint/WritePoint；标量/字符串/数组；含集成测试 |
| Siemens S7 TCP | ProtocolLib.S7Tcp.dll | Connect/Disconnect/Reconnect/ReadPoint/WritePoint；标量/字符串/数组 |

协议库均实现 `IProtocol`，通过 `ProtocolAssemblyRegistry` 插件式加载，不直接被 AM.Model 引用。

---

## 10. 页面规划与实现状态

| 导航路径 | 页面 | 后端能力 | UI 状态 |
|----------|------|----------|---------|
| PLC > Monitor | 站状态总览 | QueryAllStations() | ❌ 占位页 |
| PLC > Register | 点位实时监视 | QueryAllPoints() + SnapshotChanged | ❌ 占位页 |
| PLC > Status | 连接状态详情 | QueryStation(plcName) | ❌ 占位页 |
| PLC > Write | 手动写入调试 | PlcOperationService.WriteRaw() | ❌ 占位页 |
| SysConfig > Plc | 站 & 点位配置管理 | CRUD 服务 + ReloadFromDatabase() | ❌ 占位页 |

---

## 相关文档

- [PLC 协议库架构设计](../01-architecture/plc-protocol-integration-design.md)
- [数据库表结构](../09-database-config/README.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
