# PLC 通信模块实现说明

**文档编号**：FEAT-PLC-002  
**版本**：1.0.0  
**状态**：后端已实现  
**最后更新**：2026-05-09  
**维护人**：Am

---

## 1. 模块定位

PLC 通信模块负责在 AMControlWinF 中连接、采样、查询与手动操作 PLC 设备数据，覆盖：

- 配置管理（站与点位的增删改查及运行时装配）
- 后台周期扫描（运行时状态缓存写入）
- 运行时快照查询（供 UI 页面消费）
- 手动读写操作（工程调试页使用）

---

## 2. 文件目录结构

```
AM.Model/
  Entity/Plc/
    PlcStationConfigEntity.cs    数据库实体（plc_station 表）
    PlcPointConfigEntity.cs      数据库实体（plc_point 表）
  Plc/
    PlcConfig.cs                 配置聚合对象
    PlcStationConfig.cs          站运行时配置
    PlcPointConfig.cs            点位运行时配置
  Interfaces/Plc/
    IPlcClient.cs                AM 侧客户端门面接口
    IPlcClientFactory.cs         客户端工厂接口
    App/
      IPlcConfigAppService.cs    配置应用服务接口
    Config/
      IPlcStationCrudService.cs  站 CRUD 接口
      IPlcPointCrudService.cs    点位 CRUD 接口
    Runtime/
      IPlcRuntimeService.cs      运行时服务接口
      IPlcScanWorker.cs          扫描工作单元接口
  Runtime/
    PlcStationRuntimeSnapshot.cs 站运行时快照
    PlcPointRuntimeSnapshot.cs   点位运行时快照

AM.DBService/Services/Plc/
  Config/
    PlcStationCrudService.cs
    PlcPointCrudService.cs
  Driver/
    ProtocolAssemblyRegistry.cs  协议 DLL 插件扫描与注册
    ProtocolPlcClient.cs         AM 侧门面客户端实现
    NullPlcClient.cs             占位客户端
    PlcClientFactory.cs          工厂实现
  App/
    PlcConfigAppService.cs       DB → Context 装配
    PlcConfigSeedService.cs      默认种子数据
  Runtime/
    PlcScanWorker.cs             后台扫描工作单元
    PlcRuntimeQueryService.cs    快照查询服务
    PlcOperationService.cs       手动读写服务

ProtocolLib/
  Common/CommonLib/
    Interface/IProtocol.cs       协议统一接口
    Model/
      M_ProtocolOptions.cs
      M_PointReadRequest.cs
      M_PointWriteRequest.cs
      M_PointData.cs
      M_Return.cs
  Modbus/ModbusTcp/Protocol.cs   Modbus TCP 实现
  Siemens/S7Tcp/Protocol.cs      Siemens S7 实现

AM.Tests/Protocols/
  ModbusTcpProtocolTests.cs      ModbusTcp 集成测试
```

---

## 3. 数据模型设计原则

当前 PLC 模块采用极简模型，以下设计原则已确定：

| 原则 | 说明 |
|------|------|
| Address 直接表达完整协议地址 | 不拆分 AreaType / BitIndex 等字段 |
| DataType 统一字符串名称 | bool/uint8/int8/uint16/int16/uint32/int32/uint64/int64/float/double/string，不使用枚举 |
| Length 统一长度 | 标量=1，字符串=字符长度，数组=元素个数；不拆分 StringLength / ArrayLength |
| 无块读写 | 当前版本仅按点位读写，无 BlockRead / BlockWrite |
| 无 PlcName 字段在点位请求中 | 点位配置本身已含 PlcName，读写请求通过配置查找路由 |

---

## 4. 关键类说明

### 4.1 ProtocolPlcClient

`AM.DBService.Services.Plc.Driver.ProtocolPlcClient` 是 AM 侧 PLC 客户端门面实现，核心逻辑：

```csharp
// Configure() 时按 protocolType 解析协议实现类
ProtocolAssemblyRegistry.TryResolve(options.protocolType, out Type implType);
_protocol = (IProtocol)Activator.CreateInstance(implType);
_protocol.Configure(options);

// 所有读写直接转发到协议库
public Result<M_PointData> ReadPoint(M_PointReadRequest request)
    => ToResult(_protocol.ReadPoint(request));
```

### 4.2 PlcScanWorker

关键设计点：

- **配置缓存引用级比较**：`object.ReferenceEquals(_cachedPlcConfig, plcConfig)` 避免每轮重建分组
- **按站控制扫描频率**：每个站独立维护 `LastScanTime`，按各站 `ScanIntervalMs` 决定是否扫描
- **自动重连**：每站独立维护 `NextReconnectTime`，避免高频重连冲击
- **站快照构建**：包含成功/失败读取计数、平均读取耗时、连接时间等统计信息

### 4.3 PlcRuntimeQueryService

快照合并策略（`MergeStationSnapshot` / `MergePointSnapshot`）：

- 以运行时快照为主体（IsConnected / 时间戳 / 统计信息）
- 配置侧补全 DisplayName / IsEnabled / CurrentProtocol 等元数据字段
- 若运行时快照不存在，返回基于配置构建的默认快照（IsConnected=false，Quality="Unknown"/"Stale"）

### 4.4 PlcConfigAppService.ReloadFromDatabase()

执行流程：

```
1. EnsureTables()                      // CodeFirst 建表
2. QueryAll()                          // 读取全部配置
3. ConfigContext.Config.PlcConfig = plcConfig  // 注入 ConfigContext
4. 遍历启用站：
   PlcClientFactory.Create(station)    // 创建 ProtocolPlcClient 或 NullPlcClient
   MachineContext.Plcs[name] = client  // 注册到 MachineContext
5. 遍历启用站：连接
   client.Connect()                    // 建立实际协议连接
```

---

## 5. 地址格式参考

| 协议 | 类型 | 地址示例 |
|------|------|----------|
| Modbus TCP | 线圈（bool） | `00001` |
| Modbus TCP | 保持寄存器（uint16） | `40001` |
| Modbus TCP | 保持寄存器（float） | `40030` |
| Modbus TCP | 字符串（20字节） | `40040` + length=20，或 `40040[20]` |
| Siemens S7 | DB 数据块 bool | `DB1.0` |
| Siemens S7 | DB 数据块 int16 | `DB1.2` |
| Siemens S7 | DB 数据块字符串 | `DB1.20` + length=20 |
| Siemens S7 | M 区 float | `MD10` |

---

## 6. 集成测试

`AM.Tests/Protocols/ModbusTcpProtocolTests.cs` 覆盖：

- 协议实例化（通过 `IProtocol` 接口操作，不直接依赖具体类）
- Configure + Connect + Disconnect
- 各基础数据类型读写（bool / uint16 / int16 / uint32 / int32 / float / double / string）
- 字符串固定长度方案（`[20]` 后缀）
- 重连测试

---

## 7. 下一步（UI 页面开发参考）

### 7.0 页面层级与导航归属

当前 WinForms 分支中，PLC 页面层级建议固定为以下结构：

- 一级导航 `PLC`
  - `PLC.Status`：通讯状态
  - `PLC.Monitor`：点位监视
  - `PLC.Register`：寄存器监视
  - `PLC.Write`：写入调试
- 一级导航 `SysConfig`
  - `SysConfig.Plc`：PLC 配置

页面归属原则：

- `PLC.*` 统一承载运行期页面，面向设备在线状态查看、点位监视、调试读取与调试写入；
- `SysConfig.Plc` 统一承载配置期页面，面向 PLC 站、点位、重载与扫描控制；
- 当前阶段不建议把 `SysConfig.Plc` 并入 `PLC` 一级导航，避免运行页与配置页混在同一工作区语义下。

当前页面层级树如下：

```text
PLC
├── PLC.Status      通讯状态
├── PLC.Monitor     点位监视
├── PLC.Register    寄存器监视
└── PLC.Write       写入调试

SysConfig
└── SysConfig.Plc   PLC 配置
```

### 7.1 PLC.Monitor 点位实时监视

- 数据来源：`PlcRuntimeQueryService.QueryAllPoints()`
- 刷新方式：以 ~500ms 低频采样刷新为主，不将每次缓存变化直接驱动整页刷新
- 页面定位：PLC 点位运行态总览页，面向操作员、工程师、管理员
- 建议布局：顶部工具栏 + 顶部统计卡 + 左侧点位列表 + 右侧点位详情
- 建议筛选：PLC 站筛选、分组筛选、关键字搜索（点位名/显示名/地址）
- 建议显示列：点位名 / 地址 / 数据类型 / 当前值 / Quality / 更新时间
- 建议详情字段：PLC 名称 / 分组 / Address / DataType / ValueText / RawValue / ErrorMessage

### 7.2 PLC.Register 寄存器读取调试

- 页面定位：工程调试读页面，不承担配置职责
- 数据来源：
  - 配置点位读取：`PlcOperationService.TestReadPoint(...)`
  - 原始地址读取：`PlcOperationService.TestReadAddress(...)`
- 建议布局：顶部模式切换区 + 左侧配置点位列表 + 右侧读取调试面板 + 底部最近读取结果
- 建议模式：
  - 按配置点位读取
  - 按直接地址读取
- 建议输入项：PLC 站、Address、DataType、Length

### 7.3 PLC.Write 手动写入

- 服务：
  - 配置点位写入：`PlcOperationService.WritePoint(...)`
  - 原始地址写入：`PlcOperationService.WriteAddress(...)`
- 权限：`Engineer / Am`
- 页面定位：高风险调试写页面，默认不对操作员开放
- 建议布局：顶部风险提示区 + 左侧可写点位列表 + 右侧写入调试面板 + 底部最近写入结果
- 建议交互：选择站 / 点位 → 输入类型与值 → 显式确认 → 写入 → 可选写后回读

### 7.4 SysConfig.Plc 配置管理

- 站列表：`IPlcStationCrudService.QueryAll()`，增删改保存后调用 `PlcConfigAppService.ReloadFromDatabase()`
- 点位列表：`IPlcPointCrudService.QueryByPlcName(plcName)`，关联所选站
- 重载配置：配置变更后调用 `ReloadFromDatabase()` 重建 MachineContext.Plcs

页面定位补充：

- `SysConfig.Plc` 为 PLC 唯一配置入口，放置在 `SysConfig` 一级导航下；
- 当前阶段建议保持单个二级页面，不再拆成 `Station` / `Point` / `Runtime` 多个子页面；
- 页面内部采用三段式布局：
  1. 站配置区
  2. 点位配置区
  3. 配置重载与扫描控制区

建议字段范围：

- 站配置：Name、DisplayName、ProtocolType、ConnectionType、IpAddress、Port、TimeoutMs、ReconnectIntervalMs、ScanIntervalMs、IsEnabled
- 点位配置：PlcName、Name、DisplayName、GroupName、Address、DataType、Length、AccessMode、IsEnabled

### 7.5 PLC.Status 通讯状态页

- 数据来源：`PlcRuntimeQueryService.QueryAllStations()`
- 页面定位：PLC 站级运行态总览页
- 建议布局：顶部工具栏 + 顶部统计卡 + 左侧站列表 + 右侧站详情
- 建议工具栏动作：刷新、单轮扫描、启动扫描、停止扫描、关键字搜索
- 建议显示内容：站名 / 协议 / 连接方式 / 在线状态 / 最近扫描时间 / 最近错误 / 平均读取耗时 / 成功失败计数

### 7.6 WinForms 页面文件规划

为后续 WinForms 实现，建议预留以下文件清单。

#### 7.6.1 PageModel

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

#### 7.6.2 WinForms 页面

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

#### 7.6.3 WinForms 子控件与对话框

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

---

## 相关文档

- [PLC 协议库架构设计](../01-architecture/plc-protocol-integration-design.md)
- [数据库表结构](../09-database-config/README.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
