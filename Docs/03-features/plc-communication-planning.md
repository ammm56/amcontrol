# PLC 通信模块整体规划

**文档编号**：FEAT-001  
**版本**：1.0.0  
**状态**：草稿  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 文档目的

本文档用于统一规划 `ammm56/amcontrol` 解决方案中的 PLC 模块，覆盖以下内容：

- PLC 配置模型与数据库实体；
- 运行时状态缓存与全局上下文接入；
- 驱动接口、协议抽象与读写分层；
- 配置服务、运行服务、扫描服务的职责边界；
- `SysConfig.Plc`、`PLC.Status`、`PLC.Monitor`、`PLC.Register`、`PLC.Write` 页面落地范围；
- 与 `ConfigContext`、`MachineContext`、`RuntimeContext`、主界面状态栏、导航与权限体系的关联关系。

本文档定位于**功能模块整体规划文档**，后续如需进一步拆分数据库表结构、协议细节或页面说明，可分别补充到：

- `Docs/09-database-config/`
- `Docs/01-architecture/`
- `Docs/06-user-manual/`

---

## 2. 当前解决方案中的已有基础

结合当前代码和文档，PLC 模块已有以下基础约束：

1. **页面导航已预留**  
   `NavigationCatalog` 已定义：
   - `SysConfig.Plc`
   - `PLC.Status`
   - `PLC.Monitor`
   - `PLC.Register`
   - `PLC.Write`

2. **全局上下文已预留扩展位**  
   - `RuntimeContext` 已注释预留 `PlcRuntimeState`
   - `MachineContext` 已注释预留 `IPlcService`/PLC 服务入口
   - `ConfigContext` 当前尚未聚合 PLC 配置对象，需要新增

3. **统一架构约束已明确**  
   - `ConfigContext` 是全局配置唯一来源；
   - `MachineContext` 是全局设备对象唯一入口；
   - `RuntimeContext` 是全局运行时状态统一缓存；
   - 服务层统一使用 `Result/Result<T>`；
   - 页面刷新优先采用数据变化驱动，不在页面 `code-behind` 中引入轮询逻辑；
   - 文档与代码都需按现有层级结构统一落位，避免新建孤立目录和分散实现。

4. **主界面状态栏已有 PLC 汇总展示入口**  
   `MainWindow.xaml` 底部状态区已有 `PLC: 已连接` 占位文本，后续应改为来自 PLC 运行时汇总状态。

---

## 3. PLC 模块的业务目标

PLC 模块在本解决方案中的目标不是单一“读写寄存器”，而是统一承载以下能力：

### 3.1 配置能力

- 维护 PLC 厂商、型号、连接方式、通讯协议；
- 维护 TCP/串口等连接参数；
- 维护 PLC 站、地址块、逻辑点位映射；
- 维护不同数据类型的解析规则；
- 维护批量读写策略与安全写入权限；
- 支撑后续 MES、视觉、外设、工艺逻辑对 PLC 数据的复用。

### 3.2 运行能力

- 与一个或多个 PLC 建立连接；
- 按协议和地址分组执行批量读取；
- 将最新值写入统一 `RuntimeContext`；
- 提供写入接口给工程调试页或业务服务；
- 记录连接状态、扫描状态、最后错误、最近通讯时间；
- 为页面、报警、日志、上位业务提供统一查询入口。

### 3.3 页面能力

- `SysConfig.Plc`：PLC 配置维护页；
- `PLC.Status`：通讯链路、扫描状态、最近错误、统计信息；
- `PLC.Monitor`：按逻辑点位监视实时值；
- `PLC.Register`：按地址/区域浏览寄存器；
- `PLC.Write`：受限的工程写入调试页。

---

## 4. 建议的解决方案层次落位

PLC 模块应严格遵循当前解决方案分层，不单独拉出新体系。

### 4.1 `AM.Model`

建议新增目录：

```text
AM.Model/
  Entity/
    Plc/
  Plc/
    Enums/
  Interfaces/
    Plc/
      App/
      Config/
      Runtime/
  Runtime/
```

职责：

- 数据库实体；
- 运行时配置对象；
- 协议/区域/数据类型枚举；
- 驱动接口；
- 运行时状态快照；
- 配置聚合模型。

### 4.2 `AM.Core`

建议接入：

- `ConfigContext`：增加 PLC 配置聚合对象；
- `MachineContext`：增加 PLC 客户端/PLC 服务对象统一入口；
- `RuntimeContext`：增加 `PlcRuntimeState`；
- 后续如需长期后台工作单元管理，可在 `AM.Core` 中承载公共调度基类，但首版不强制。

### 4.3 `AM.DBService`

建议新增目录：

```text
AM.DBService/
  Services/
    Plc/
      App/
      Config/
      Runtime/
      Driver/
```

职责：

- 配置 CRUD；
- 配置热装载与上下文同步；
- PLC 驱动工厂与协议实现；
- 扫描服务与运行时查询服务；
- 工程写入服务。

### 4.4 `AM.ViewModel`

建议新增目录：

```text
AM.ViewModel/
  ViewModels/
    Config/
    Plc/
```

职责：

- `SysConfig.Plc` 配置页 ViewModel；
- 运行态监视、状态、寄存器、写入页 ViewModel；
- 将 PLC 配置与运行时状态以 WPF MVVM 方式组织给界面层。

### 4.5 `AMControlWPF`

建议新增目录：

```text
AMControlWPF/
  Views/
    Config/
    Plc/
```

职责：

- PLC 配置页；
- PLC 运行态相关页面；
- 与当前 `MainWindow` 导航、权限、状态栏集成。

---

## 5. PLC 核心配置维度

PLC 配置不能只停留在“IP + 地址”层面，至少要覆盖以下维度。

### 5.1 厂商（Vendor）

建议使用枚举 `PlcVendorType`，首版至少包含：

- `Unknown`
- `Siemens`
- `Mitsubishi`
- `Omron`
- `Keyence`
- `Panasonic`
- `Delta`
- `ModbusGeneric`
- `Custom`

### 5.2 型号（Model）

型号不建议枚举固化，建议使用字符串字段，例如：

- `S7-1200`
- `S7-1500`
- `FX5U`
- `Q03UDE`
- `KV-7500`

原因：

- 型号数量多且常新增；
- 不应为每种型号频繁修改枚举与界面；
- 型号主要用于显示与驱动参数选择，不适合硬编码成枚举长期维护。

### 5.3 连接方式（ConnectionType）

建议枚举 `PlcConnectionType`：

- `Tcp`
- `Serial`
- `Usb`
- `Virtual`

### 5.4 通讯协议（ProtocolType）

建议枚举 `PlcProtocolType`：

- `ModbusTcp`
- `ModbusRtu`
- `S7`
- `Mc3E`
- `Mc1E`
- `FinsTcp`
- `FinsUdp`
- `HostLink`
- `Custom`

### 5.5 地址区域（AreaType）

不同 PLC 地址体系不同，但系统应提供统一抽象，建议枚举 `PlcAreaType`：

- `Coil`
- `DiscreteInput`
- `InputRegister`
- `HoldingRegister`
- `X`
- `Y`
- `M`
- `D`
- `W`
- `R`
- `T`
- `C`
- `DB`
- `V`
- `Custom`

### 5.6 数据类型（DataType）

建议枚举 `PlcDataType`，首版至少支持：

- `Bit`
- `Bool`
- `Byte`
- `Short`
- `UShort`
- `Int`
- `UInt`
- `Float`
- `Double`
- `Long`
- `ULong`
- `String`
- `ByteArray`

### 5.7 访问模式（AccessMode）

建议枚举 `PlcAccessMode`：

- `ReadOnly`
- `ReadWrite`
- `WriteOnly`

### 5.8 读取模式（ReadMode）

建议枚举 `PlcReadMode`：

- `Single`
- `BatchByAddress`
- `BatchByDataType`
- `BatchByWordLength`
- `BatchByByteLength`

用于明确点位应如何归入批量读取块，而不是在运行时临时推断。

---

## 6. PLC 数据库实体规划

PLC 首轮建议至少有两张核心配置表；第三张“批量读取块配置”可首版预留、第二版落库。

## 6.1 PLC 站配置表

建议文件：
- `AM.Model/Entity/Plc/PlcStationConfigEntity.cs`

建议表名：
- `plc_station`

建议字段：

| 字段 | 说明 |
|------|------|
| `Id` | 主键 |
| `Name` | 内部唯一名称 |
| `DisplayName` | 界面显示名 |
| `Vendor` | 厂商枚举值或名称 |
| `Model` | PLC 型号 |
| `ConnectionType` | TCP/串口/USB/虚拟 |
| `ProtocolType` | Modbus/S7/MC/FINS 等 |
| `IpAddress` | TCP 地址 |
| `Port` | TCP 端口 |
| `ComPort` | 串口号 |
| `BaudRate` | 串口波特率 |
| `DataBits` | 串口数据位 |
| `Parity` | 串口校验 |
| `StopBits` | 串口停止位 |
| `StationNo` | 站号 |
| `NetworkNo` | 网络号 |
| `PcNo` | PLC PC 号 |
| `Rack` | S7 机架号 |
| `Slot` | S7 插槽号 |
| `TimeoutMs` | 通讯超时 |
| `ReconnectIntervalMs` | 重连周期 |
| `ScanIntervalMs` | 采样周期 |
| `IsEnabled` | 是否启用 |
| `SortOrder` | 排序 |
| `Description` | 说明 |
| `Remark` | 备注 |
| `CreateTime` | 创建时间 |
| `UpdateTime` | 更新时间 |

说明：

- TCP 与串口字段可共存，按 `ConnectionType` 判定实际使用；
- 协议特定字段（如 `Rack/Slot`、`NetworkNo/PcNo`）允许留空；
- 不建议拆成多张协议专属表，避免配置层过早碎片化。

## 6.2 PLC 点位配置表

建议文件：
- `AM.Model/Entity/Plc/PlcPointConfigEntity.cs`

建议表名：
- `plc_point`

建议字段：

| 字段 | 说明 |
|------|------|
| `Id` | 主键 |
| `PlcName` | 所属 PLC 站名称 |
| `Name` | 内部唯一点位名 |
| `DisplayName` | 显示名称 |
| `GroupName` | 分组名 |
| `AreaType` | 区域类型 |
| `Address` | 起始地址 |
| `BitIndex` | 位索引，可空 |
| `DataType` | 数据类型 |
| `StringLength` | 字符串长度 |
| `ArrayLength` | 数组或块长度 |
| `Scale` | 缩放系数 |
| `Offset` | 偏移 |
| `Unit` | 单位 |
| `AccessMode` | 读写权限 |
| `ReadMode` | 读取策略 |
| `BatchKey` | 批量读取分组键 |
| `IsEnabled` | 是否启用 |
| `SortOrder` | 排序 |
| `Description` | 说明 |
| `Remark` | 备注 |

说明：

- `Address` 不建议仅保存一个完整文本而无辅助字段，首版可保存文本，同时在运行时解析成结构化地址；
- `StringLength` 对 `String` 类型是必填；
- `ArrayLength` 对数组或批量块读取场景是必要字段；
- `BitIndex` 用于字地址内位访问；
- `BatchKey` 用于显式控制点位如何被归到批读块中。

## 6.3 PLC 批量读取块配置表（建议预留）

建议文件：
- `AM.Model/Entity/Plc/PlcReadBlockConfigEntity.cs`

建议表名：
- `plc_read_block`

建议字段：

| 字段 | 说明 |
|------|------|
| `Id` | 主键 |
| `PlcName` | 所属 PLC |
| `BlockName` | 读取块名称 |
| `AreaType` | 区域 |
| `StartAddress` | 起始地址 |
| `Length` | 读取长度 |
| `DataType` | 块数据类型或块解释方式 |
| `ReadMode` | 批量读取模式 |
| `IsEnabled` | 是否启用 |
| `SortOrder` | 排序 |
| `Description` | 说明 |

说明：

- 首版可不立即落库，但建议在模型层先设计；
- 用于解决“连续地址可批读但点位定义分散”的协议适配问题；
- 对大批量寄存器、高频采样和字符串/数组读取尤为重要。

---

## 7. PLC 运行时模型规划

PLC 运行时模型应按“站级状态 + 点位级状态 + 原始块读结果”三层组织。

## 7.1 PLC 配置聚合模型

建议文件：
- `AM.Model/Plc/PlcConfig.cs`
- `AM.Model/Plc/PlcStationConfig.cs`
- `AM.Model/Plc/PlcPointConfig.cs`
- `AM.Model/Plc/PlcReadBlockConfig.cs`

建议：

- `PlcConfig` 作为 `ConfigContext.Instance.Config.PlcConfig` 聚合对象；
- 所有 PLC 站、点位、批读块统一通过该聚合对象暴露；
- 数据库实体与运行时配置对象分离，避免 UI/服务直接依赖数据库实体。

## 7.2 点位运行时快照

建议文件：
- `AM.Model/Runtime/PlcPointRuntimeSnapshot.cs`

建议字段：

- `PlcName`
- `PointName`
- `DisplayName`
- `AddressText`
- `AreaType`
- `DataType`
- `ValueText`
- `RawValue`
- `Quality`
- `UpdateTime`
- `IsConnected`
- `HasError`
- `ErrorMessage`

说明：

- `ValueText` 供 UI 直接显示；
- `RawValue` 供服务和调试页做原始数据分析；
- `Quality` 不应只有成功/失败，建议支持 `Good`、`Stale`、`Disconnected`、`Error`。

## 7.3 PLC 站运行时快照

建议文件：
- `AM.Model/Runtime/PlcStationRuntimeSnapshot.cs`

建议字段：

- `PlcName`
- `DisplayName`
- `IsEnabled`
- `IsConnected`
- `IsScanRunning`
- `LastConnectTime`
- `LastScanTime`
- `LastError`
- `SuccessReadCount`
- `FailedReadCount`
- `AverageReadMs`
- `AverageWriteMs`
- `CurrentProtocol`
- `CurrentConnectionType`

## 7.4 PLC 全局运行时状态

建议文件：
- `AM.Model/Runtime/PlcRuntimeState.cs`

建议能力：

- 保存多个 PLC 站快照；
- 保存多个点位快照；
- 支持查询：
  - `TryGetStationSnapshot`
  - `TryGetPointSnapshot`
  - `GetPointsByPlc`
  - `GetPointsByGroup`
- 提供事件：
  - `SnapshotChanged`
  - `StationSnapshotChanged`
  - `PointSnapshotChanged`

说明：

- 必须接入 `RuntimeContext.Instance.Plc`；
- 设计风格应与 `MotionAxisRuntimeState`、`MotionIoRuntimeState` 保持一致；
- 不能让各页面各自维护 PLC 值缓存。

---

## 8. PLC 驱动接口与协议抽象规划

## 8.1 通用客户端接口

建议文件：
- `AM.Model/Interfaces/Plc/IPlcClient.cs`

建议职责：

- 建立连接、断开连接；
- 查询连接状态；
- 批量读取原始块；
- 单值写入；
- 批量写入；
- 获取当前驱动能力描述。

建议方法：

- `Result Connect()`
- `Result Disconnect()`
- `Result<bool> IsConnected()`
- `Result<PlcRawDataBlock> ReadBlock(...)`
- `Result Write(...)`
- `Result WriteBlock(...)`

## 8.2 客户端工厂

建议文件：
- `AM.Model/Interfaces/Plc/IPlcClientFactory.cs`
- `AM.DBService/Services/Plc/Driver/PlcClientFactory.cs`

职责：

- 按 `ProtocolType + ConnectionType + Vendor + Model` 创建具体协议客户端；
- 不在页面或 ViewModel 里直接 `new` 协议驱动；
- 后续支持逐步扩展多协议驱动实现。

## 8.3 原始读块模型

建议文件：
- `AM.Model/Plc/PlcRawDataBlock.cs`

用途：

- 承载一段原始地址块数据；
- 供上层解析为 `Bit/Int/Float/String/Array`；
- 解决字符串、数组、连续块与离散点位共存时的统一解析问题。

## 8.4 首轮驱动实现建议顺序

建议目录：

```text
AM.DBService/Services/Plc/Driver/
  ModbusTcpPlcClient.cs
  ModbusRtuPlcClient.cs
  S7PlcClient.cs
  Mc3EPlcClient.cs
```

建议实现顺序：

1. `ModbusTcp`
2. `ModbusRtu`
3. `S7`
4. `MC 3E`

理由：

- Modbus 通用性最高，适合作为首个贯通全链路的基线实现；
- S7/MC 可在首版结构稳定后继续扩展；
- 驱动层应独立于页面层和配置层，不让 UI 决定协议细节。

---

## 9. 配置服务与运行服务规划

建议目录：

```text
AM.DBService/Services/Plc/
  App/
  Config/
  Runtime/
  Driver/
```

## 9.1 配置服务

### 9.1.1 `PlcStationCrudService`

文件：
- `AM.DBService/Services/Plc/Config/PlcStationCrudService.cs`

职责：

- PLC 站配置增删改查；
- 校验名称唯一、协议与连接参数组合合法；
- 屏蔽数据库访问细节。

### 9.1.2 `PlcPointCrudService`

文件：
- `AM.DBService/Services/Plc/Config/PlcPointCrudService.cs`

职责：

- PLC 点位配置增删改查；
- 校验地址、数据类型、长度、读写权限是否合理；
- 校验 `StringLength`、`ArrayLength` 等数据类型相关字段。

### 9.1.3 `PlcReadBlockCrudService`

文件：
- `AM.DBService/Services/Plc/Config/PlcReadBlockCrudService.cs`

职责：

- 批量块配置维护；
- 校验地址块与长度的合理性；
- 校验与点位分组之间是否匹配。

### 9.1.4 `PlcConfigAppService`

文件：
- `AM.DBService/Services/Plc/App/PlcConfigAppService.cs`

职责：

- 聚合站配置、点位配置、读块配置；
- 从数据库装配出 `PlcConfig`；
- 写回 `ConfigContext.Instance.Config.PlcConfig`；
- 创建并刷新 `MachineContext` 中的 PLC 设备入口。

说明：

- 这个服务是 PLC 配置层的统一入口；
- 风格应与当前运动配置 AppService 保持一致；
- 不建议由页面各自调用多个 CRUD 服务拼装配置。

## 9.2 运行服务

### 9.2.1 `PlcRuntimeQueryService`

文件：
- `AM.DBService/Services/Plc/Runtime/PlcRuntimeQueryService.cs`

职责：

- 给页面查询站状态、点位值、最近错误；
- 面向 `PLC.Status`、`PLC.Monitor`、`PLC.Register` ViewModel。

### 9.2.2 `PlcOperationService`

文件：
- `AM.DBService/Services/Plc/Runtime/PlcOperationService.cs`

职责：

- 单点写入；
- 批量写入；
- 地址测试读取；
- 写入安全校验；
- 为 `PLC.Write` 页提供统一入口。

### 9.2.3 `PlcScanWorker`

文件：
- `AM.DBService/Services/Plc/Runtime/PlcScanWorker.cs`

职责：

- 后台执行 PLC 批量采样；
- 读取多个地址块；
- 解析值并写入 `RuntimeContext.Instance.Plc`；
- 维护扫描状态、最近时间、质量状态、最近错误。

说明：

- 该工作单元是 PLC 运行态的核心；
- 页面不应自己轮询 PLC；
- 成功结果默认不应频繁写日志和消息，避免高频扫描造成日志污染。

---

## 10. 全局上下文接入规划

## 10.1 `ConfigContext`

当前 `Config` 中尚无 PLC 聚合配置，应新增：

文件：
- `AM.Model/Common/Config.cs`
- `AM.Model/Plc/PlcConfig.cs`

建议新增属性：

- `[JsonIgnore] public PlcConfig PlcConfig { get; set; } = new PlcConfig();`

说明：

- PLC 配置与运动卡配置一致，建议运行时由数据库装配，不写入 `config.json`；
- 轻量运行选项（如是否自动启动 PLC 扫描）可后续单独放入 `Setting` 或 `Runtime` 配置中。

## 10.2 `MachineContext`

建议从当前预留注释改为真实属性：

- `public IDictionary<string, IPlcClient> Plcs { get; } = new Dictionary<string, IPlcClient>();`

说明：

- 即便首版只接一台 PLC，也建议使用字典结构；
- 避免后续从单实例属性再改成多实例集合；
- 上层业务通过 `MachineContext` 统一拿 PLC 客户端或 PLC 服务对象。

## 10.3 `RuntimeContext`

建议从预留注释改为真实属性：

- `public PlcRuntimeState Plc { get; } = new PlcRuntimeState();`

说明：

- PLC 值缓存必须统一进入 `RuntimeContext`；
- 后续视觉、扫码器、传感器也应遵循同样模式。

---

## 11. ViewModel 与页面文件规划

## 11.1 配置页

### `SysConfig.Plc`

建议文件：

- `AM.ViewModel/ViewModels/Config/PlcConfigViewModel.cs`
- `AMControlWPF/Views/Config/PlcConfigView.xaml`
- `AMControlWPF/Views/Config/PlcConfigView.xaml.cs`

职责：

- 左侧 PLC 站列表；
- 中间连接参数表单；
- 右侧点位表与块配置；
- 支持新增、编辑、删除、启停、测试连接、保存。

界面布局建议：

- 左：站列表
- 中：站参数编辑
- 右：点位/批读块配置和说明

## 11.2 运行态页面

### `PLC.Status`

建议文件：

- `AM.ViewModel/ViewModels/Plc/PlcStatusViewModel.cs`
- `AMControlWPF/Views/Plc/PlcStatusView.xaml`
- `AMControlWPF/Views/Plc/PlcStatusView.xaml.cs`

职责：

- 查看连接状态、扫描状态、最近错误、读写统计；
- 支持按 PLC 切换查看明细。

### `PLC.Monitor`

建议文件：

- `AM.ViewModel/ViewModels/Plc/PlcMonitorViewModel.cs`
- `AMControlWPF/Views/Plc/PlcMonitorView.xaml`
- `AMControlWPF/Views/Plc/PlcMonitorView.xaml.cs`

职责：

- 按逻辑点位显示实时值；
- 顶部支持 PLC 站筛选、分组筛选、关键字搜索；
- 右侧显示当前点位详情。

布局建议：

- 参考 `DI/DO` 监视页的稳定风格，不单独重造布局体系；
- 重点展示名称、当前值、质量状态、地址、数据类型。

### `PLC.Register`

建议文件：

- `AM.ViewModel/ViewModels/Plc/PlcRegisterViewModel.cs`
- `AMControlWPF/Views/Plc/PlcRegisterView.xaml`
- `AMControlWPF/Views/Plc/PlcRegisterView.xaml.cs`

职责：

- 表格方式分页查看寄存器；
- 支持地址范围、区域类型、数据类型筛选；
- 适合大数据量浏览。

### `PLC.Write`

建议文件：

- `AM.ViewModel/ViewModels/Plc/PlcWriteViewModel.cs`
- `AMControlWPF/Views/Plc/PlcWriteView.xaml`
- `AMControlWPF/Views/Plc/PlcWriteView.xaml.cs`

职责：

- 工程调试写入；
- 支持目标地址、类型、值输入；
- 支持高风险确认和写入结果反馈；
- 禁止操作员访问。

---

## 12. PLC 页面与主界面的关联关系

## 12.1 主界面导航

`NavigationCatalog` 已定义 PLC 页面键，后续只需在 `MainWindow.xaml.cs` 中将现有占位页替换为真实页面：

- `PLC.Monitor`
- `PLC.Register`
- `PLC.Status`
- `PLC.Write`
- `SysConfig.Plc`

## 12.2 主界面底部状态栏

`MainWindow.xaml` 底部已有：

- `PLC: 已连接`

后续建议：

- 改为由 PLC 运行态汇总文本驱动；
- 例如：`PLC: 2/3 已连接`、`PLC: 断开`、`PLC: 扫描异常`；
- 数据来源统一取自 `RuntimeContext.Instance.Plc` 或其查询服务。

## 12.3 权限体系

当前导航中 PLC 页权限已定义，建议保持：

| 页面 | 默认角色 |
|------|----------|
| `PLC.Monitor` | `Operator,Engineer,Am` |
| `PLC.Register` | `Engineer,Am` |
| `PLC.Status` | `Operator,Engineer,Am` |
| `PLC.Write` | `Engineer,Am` |
| `SysConfig.Plc` | `Engineer,Am` |

说明：

- 监视类页面允许操作员查看；
- 写入类和配置类页面应保留工程师及管理员权限；
- 页面级权限仍由数据库管理，不在页面内硬编码额外权限逻辑。

---

## 13. PLC 首轮必须提前考虑的细节

## 13.1 地址不能只存一个字符串

仅用 `Address = "D100"` 不够。至少要考虑：

- 地址区域；
- 起始地址；
- 位偏移；
- 读取长度；
- 数据类型；
- 批量归属。

首版可保留地址文本字段，但运行时必须解析成结构化信息。

## 13.2 字符串必须有长度

`String` 读写若无长度信息，无法确定读取范围与解析规则，因此：

- `StringLength` 为 `String` 类型的必要字段；
- 后续如接入多字节编码，还需考虑编码方式与字节序。

## 13.3 数组与批量块必须有长度

对于：

- `Int[10]`
- `Float[6]`
- `ByteArray[32]`

都必须有 `ArrayLength` 或块长度配置，不能依赖页面临时输入。

## 13.4 `Bit` 与 `Word` 不应简单混批

批量读取时应至少按下列维度分组：

- PLC 站；
- 协议；
- 区域；
- 数据类型或块解释方式；
- 地址连续性；
- 设备允许的最大长度。

## 13.5 写入页必须考虑安全边界

`PLC.Write` 首版即应考虑：

- 仅对 `ReadWrite` 地址开放；
- 对高风险写入做显式确认；
- 写入结果要有清晰的 `Result` 返回；
- 不能直接把底层驱动暴露给 ViewModel。

## 13.6 运行态质量不能只有“有值/无值”

建议点位质量至少支持：

- `Good`
- `Stale`
- `Disconnected`
- `Error`

这对监视页、寄存器页和状态页都很关键。

---

## 14. 第一批文件落地清单

PLC 第一批建议优先新建以下文件。

## 14.1 模型与接口

```text
AM.Model/
  Entity/Plc/
    PlcStationConfigEntity.cs
    PlcPointConfigEntity.cs
    PlcReadBlockConfigEntity.cs
  Plc/
    PlcConfig.cs
    PlcStationConfig.cs
    PlcPointConfig.cs
    PlcReadBlockConfig.cs
    PlcRawDataBlock.cs
    Enums/
      PlcVendorType.cs
      PlcProtocolType.cs
      PlcConnectionType.cs
      PlcAreaType.cs
      PlcDataType.cs
      PlcAccessMode.cs
      PlcReadMode.cs
      PlcPointQuality.cs
  Interfaces/
    Plc/
      IPlcClient.cs
      IPlcClientFactory.cs
      App/
        IPlcConfigAppService.cs
      Config/
        IPlcStationCrudService.cs
        IPlcPointCrudService.cs
        IPlcReadBlockCrudService.cs
      Runtime/
        IPlcRuntimeService.cs
        IPlcScanWorker.cs
  Runtime/
    PlcRuntimeState.cs
    PlcStationRuntimeSnapshot.cs
    PlcPointRuntimeSnapshot.cs
```

## 14.2 服务层

```text
AM.DBService/
  Services/Plc/
    App/
      PlcConfigAppService.cs
    Config/
      PlcStationCrudService.cs
      PlcPointCrudService.cs
      PlcReadBlockCrudService.cs
    Runtime/
      PlcRuntimeQueryService.cs
      PlcOperationService.cs
      PlcScanWorker.cs
    Driver/
      PlcClientFactory.cs
      ModbusTcpPlcClient.cs
      ModbusRtuPlcClient.cs
      S7PlcClient.cs
      Mc3EPlcClient.cs
```

## 14.3 页面与 ViewModel

```text
AM.ViewModel/
  ViewModels/Config/
    PlcConfigViewModel.cs
  ViewModels/Plc/
    PlcStatusViewModel.cs
    PlcMonitorViewModel.cs
    PlcRegisterViewModel.cs
    PlcWriteViewModel.cs

AMControlWPF/
  Views/Config/
    PlcConfigView.xaml
    PlcConfigView.xaml.cs
  Views/Plc/
    PlcStatusView.xaml
    PlcStatusView.xaml.cs
    PlcMonitorView.xaml
    PlcMonitorView.xaml.cs
    PlcRegisterView.xaml
    PlcRegisterView.xaml.cs
    PlcWriteView.xaml
    PlcWriteView.xaml.cs
```

---

## 15. 建议的实施顺序

建议按以下顺序推进，而不是直接先做页面：

### 第一步：基础模型与上下文接入

1. 建立 `AM.Model/Entity/Plc`、`AM.Model/Plc`、`AM.Model/Runtime`、`AM.Model/Interfaces/Plc`；
2. 新增 PLC 枚举、配置模型、运行时状态模型；
3. 将 `PlcConfig` 接入 `ConfigContext`；
4. 将 `PlcRuntimeState` 接入 `RuntimeContext`；
5. 将 PLC 客户端集合接入 `MachineContext`。

### 第二步：配置服务

6. 实现 `PlcStationCrudService`；
7. 实现 `PlcPointCrudService`；
8. 实现 `PlcConfigAppService`，完成数据库到上下文的装配。

### 第三步：驱动与扫描基线

9. 定义 `IPlcClient` 与 `PlcClientFactory`；
10. 首先实现 `ModbusTcp` 基线驱动；
11. 实现 `PlcScanWorker` 将值写入 `RuntimeContext.Instance.Plc`；
12. 实现 `PlcRuntimeQueryService`。

### 第四步：配置页面

13. 实现 `SysConfig.Plc`；
14. 先打通站配置与点位配置；
15. 补测试连接能力。

### 第五步：监视与调试页面

16. 实现 `PLC.Status`；
17. 实现 `PLC.Monitor`；
18. 实现 `PLC.Register`；
19. 最后实现 `PLC.Write`。

---

## 16. 后续文档拆分建议

当 PLC 第一批代码开始落地后，建议再拆分以下文档：

1. **数据库与字段说明**  
   放到 `Docs/09-database-config/`：
   - `plc-table-design.md`

2. **协议与驱动实现说明**  
   放到 `Docs/01-architecture/` 或 `Docs/08-third-party/`：
   - `plc-driver-abstraction.md`
   - `plc-protocol-support-matrix.md`

3. **页面使用与工程调试说明**  
   放到 `Docs/06-user-manual/`：
   - `plc-status-page.md`
   - `plc-write-debugging.md`

---

## 17. 本轮结论

结合当前解决方案结构，PLC 第一批不应从页面先做，而应从以下三件事先建立基础：

1. **模型完整**：站配置、点位配置、批量块配置、运行态快照、统一枚举；
2. **上下文贯通**：`ConfigContext`、`MachineContext`、`RuntimeContext` 三处全部接入 PLC；
3. **服务分层明确**：配置服务、驱动工厂、扫描服务、查询服务、写入服务各司其职。

在此基础上，再落地：

- `SysConfig.Plc`
- `PLC.Status`
- `PLC.Monitor`
- `PLC.Register`
- `PLC.Write`

才能保证 PLC 模块不会变成只够演示的局部实现，而是能够长期支撑整机通讯、监视、调试与业务复用的统一基础设施。
