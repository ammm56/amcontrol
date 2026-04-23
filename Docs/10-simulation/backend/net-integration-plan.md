# .NET 侧 3D 仿真接入历史规划归档说明

**文档编号**：SIM-BE-001  
**版本**：1.0.0  
**状态**：归档  
**最后更新**：2026-04-23  
**维护人**：Am

---

**状态说明**：本文件保留为 3D 仿真方向的历史规划与对照资料，当前仓库并未按本文完整落地。

**使用边界**：

1. 用于理解早期 WPF + 3D 仿真一体化方向的设计设想；
2. 用于后续恢复仿真方向时复用其中的消息、DTO、集成层思路；
3. 不用于判断当前活跃 WinForms 分支的既成事实；
4. 若与当前仓库状态冲突，应优先参考代码、WinForms 总览和当前开发进展。
5. 文中出现的“必须”“不允许”“统一”等措辞，应理解为当时方案内部的设计约束，而不是未来所有仿真方案或前后端架构的永久规则。

## 1. 目标与原则

本文档用于为 `ammm56/amcontrol` 项目规划一套与当时仓库架构一致的 3D 虚拟设备接入方案。目标是在**不破坏当时 WPF 工业控制主程序架构**的前提下，为 `VirtualMotionCardService` 增加一个可供 Vue3 + Three.js 外部 3D 仿真前端实时联动的 .NET 侧集成层。

本方案严格遵守当前项目既有约束：

- 运动控制对象统一通过 `MachineContext` 作为全局唯一入口访问，不允许各窗口、各服务维护私有运动控制副本。
- 全局配置统一通过 `ConfigContext` 读取与保存，不在各处维护私有长期缓存配置副本。
- 全项目统一使用单一 `Result/Result<T>` 返回模型。
- WPF ViewModel 属性继续采用手动字段 + 属性方式，不使用 `[ObservableProperty]` 等源生成器特性。
- 页面层改动必须复用现有 `View/UserControl` 基本框架与导航缓存模式。
- 状态语义必须区分 `PulsePosition`、`CommandPositionMm`、`EncoderPositionMm`，不可退化为单一毫米位置接口。
- 可复用样式下沉到统一资源字典，不在 `MainWindow.xaml` 中内联扩散。

---

## 2. 与当前仓库结构的关系

当前仓库已具备以下基础：

- `AM.MotionService/Virtual/VirtualMotionCardService.cs`
  - 已提供 `AxisPositionChanged`、`AxisStatusChanged`、`DOChanged`、`DIChanged` 事件。
  - 已提供 `InjectDI(short bit, bool value)` 供外部模拟器回写传感器。
  - 已提供 `GetAllAxisStates()`、`GetAllDoValues()`、`GetAllDiValues()` 供全量快照读取。
- `AMControlWPF`
  - 已采用缓存复用页面模式。
  - 各 `Motion*View` 页面均采用首次 `LoadAsync()`、后续复用的加载方式。
- 项目全局约束已明确要求 `MachineContext` / `ConfigContext` 为统一入口。

因此本次设计不应把 3D 仿真能力散落进多个窗口，也不应让前端直接耦合某个具体页面，而应在 .NET 侧增加一个**仿真集成层**，统一：

1. 从 `MachineContext` 取运行时对象；
2. 从 `ConfigContext` 与数据库取仿真配置；
3. 通过 WebSocket 向外输出实时状态；
4. 接收外部仿真端命令并回注入虚拟设备状态；
5. 给 WPF 提供统一页面与统一监控入口。

---

## 3. 推荐新增项目与放置位置

> 结论：建议**新增独立项目 `AM.SimulationService`**，而不是把所有 3D 接入逻辑继续堆进 `AM.MotionService` 或 `AMControlWPF`。

原因：

- `AM.MotionService` 应继续聚焦运动控制卡及其执行层逻辑；
- `AMControlWPF` 应聚焦页面展示层；
- 3D 集成涉及通信、消息协议、快照装配、绑定解析、运行时会话管理，职责独立；
- 后续如果扩展为远程仿真、培训演示、数字孪生调试平台，独立项目更易维护。

### 3.1 新增项目

建议新增：

- `AM.SimulationService`

建议在解决方案中与以下项目并列：

- `AM.Core`
- `AM.Model`
- `AM.MotionService`
- `AM.DBService`
- `AM.ViewModel`
- `AMControlWPF`

### 3.2 各项目内新增目录建议

#### `AM.Model`

新增目录：

- `Simulation/Enums`
- `Simulation/Dtos`
- `Simulation/Interfaces`
- `Simulation/Configs`

#### `AM.SimulationService`

新增目录：

- `Gateway`
- `Session`
- `Runtime`
- `Binding`
- `Snapshot`
- `Dispatcher`
- `Handlers`
- `Extensions`

#### `AM.ViewModel`

新增目录：

- `ViewModels/Simulation`

#### `AMControlWPF`

新增目录：

- `Views/Simulation`
- `Resources/Themes/Styles`（如需新增页面专用样式，仍统一放到已有样式集中处）

---

## 4. 推荐新增文件清单

以下是建议的首批文件结构。

### 4.1 `AM.Model`

#### `AM.Model/Simulation/Enums/SimulationMessageType.cs`
定义消息类型常量或枚举。

#### `AM.Model/Simulation/Enums/SimulationDeviceType.cs`
定义设备类型：Axis、Cylinder、Vacuum、Gripper、TowerLight、Door、Sensor、Scanner、Plc、Camera、RobotArm 等。

#### `AM.Model/Simulation/Dtos/SimulationEnvelopeDto.cs`
统一 WebSocket 消息包。

#### `AM.Model/Simulation/Dtos/SimulationAxisStateDto.cs`
单轴状态 DTO。

#### `AM.Model/Simulation/Dtos/SimulationAxisStatusDto.cs`
轴状态标志 DTO。

#### `AM.Model/Simulation/Dtos/SimulationIoPointDto.cs`
DI/DO 点位 DTO。

#### `AM.Model/Simulation/Dtos/SimulationMachineSnapshotDto.cs`
整机全量快照 DTO。

#### `AM.Model/Simulation/Dtos/SimulationDeviceBindingDto.cs`
设备绑定关系 DTO。

#### `AM.Model/Simulation/Dtos/SimulationLayoutNodeDto.cs`
3D 模型节点映射 DTO。

#### `AM.Model/Simulation/Dtos/SimulationAlarmDto.cs`
报警/消息展示 DTO（如后续接统一报警链路）。

#### `AM.Model/Simulation/Dtos/SimulationCommandDto.cs`
前端下发控制命令 DTO。

#### `AM.Model/Simulation/Configs/SimulationGatewayConfig.cs`
仿真网关配置。

#### `AM.Model/Simulation/Interfaces/ISimulationGatewayService.cs`
仿真网关服务接口。

#### `AM.Model/Simulation/Interfaces/ISimulationRuntimeService.cs`
仿真运行时服务接口。

#### `AM.Model/Simulation/Interfaces/ISimulationSnapshotService.cs`
全量快照构建接口。

#### `AM.Model/Simulation/Interfaces/ISimulationBindingService.cs`
设备绑定解析接口。

#### `AM.Model/Simulation/Interfaces/ISimulationCommandHandler.cs`
仿真命令处理器接口。

### 4.2 `AM.SimulationService`

#### `AM.SimulationService/Gateway/SimulationGatewayService.cs`
WebSocket 主服务，负责启动监听、接收连接、广播消息。

#### `AM.SimulationService/Session/SimulationClientSession.cs`
表示单个仿真客户端会话。

#### `AM.SimulationService/Runtime/SimulationRuntimeService.cs`
协调事件订阅、会话广播、状态推送、生命周期管理。

#### `AM.SimulationService/Snapshot/SimulationSnapshotService.cs`
构建全量设备快照。

#### `AM.SimulationService/Binding/SimulationBindingService.cs`
从数据库/配置中解析 3D 绑定配置。

#### `AM.SimulationService/Dispatcher/SimulationMessageDispatcher.cs`
负责把收到的前端消息路由到不同命令处理器。

#### `AM.SimulationService/Handlers/InjectDiCommandHandler.cs`
处理前端 DI 注入命令。

#### `AM.SimulationService/Handlers/RequestSnapshotCommandHandler.cs`
处理前端请求整机快照命令。

#### `AM.SimulationService/Handlers/FlowControlCommandHandler.cs`
处理前端流程播放/暂停等命令（首期可空实现或仅保留结构）。

#### `AM.SimulationService/Extensions/SimulationServiceCollectionExtensions.cs`
依赖注入注册扩展。

### 4.3 `AM.ViewModel`

#### `AM.ViewModel/ViewModels/Simulation/SimulationMonitorViewModel.cs`
仿真运行态监视页面 ViewModel。

#### `AM.ViewModel/ViewModels/Simulation/SimulationConfigViewModel.cs`
仿真配置页面 ViewModel。

### 4.4 `AMControlWPF`

#### `AMControlWPF/Views/Simulation/SimulationMonitorView.xaml`
仿真监控页。

#### `AMControlWPF/Views/Simulation/SimulationMonitorView.xaml.cs`
沿用现有首次加载模式。

#### `AMControlWPF/Views/Simulation/SimulationConfigView.xaml`
仿真配置页。

#### `AMControlWPF/Views/Simulation/SimulationConfigView.xaml.cs`
仿真配置页代码隐藏。

如需统一样式：

#### `AMControlWPF/Resources/Themes/Styles/SimulationStyle.xaml`
放置仿真页可复用样式，并在总样式入口中引用。

---

## 5. 核心职责划分

### 5.1 `VirtualMotionCardService` 继续负责

- 虚拟轴运动学仿真
- 虚拟 DI/DO 状态维护
- 运动与状态事件输出
- 传感器 DI 注入入口

### 5.2 `AM.SimulationService` 负责

- 把运行态对象转换成可外发的消息
- 统一维护 WebSocket 客户端连接
- 统一广播轴位置、轴状态、DI/DO 变化
- 接收前端仿真命令并调用正确的后端服务
- 提供全量快照构建
- 维护绑定配置解析与缓存刷新

### 5.3 `AMControlWPF` 负责

- 提供仿真状态查看与配置入口
- 显示网关启动状态、客户端连接状态、消息统计、异常信息
- 不直接维护底层通信细节

---

## 6. 接口定义建议

以下接口全部建议放在 `AM.Model/Simulation/Interfaces`。

### 6.1 `ISimulationGatewayService`

职责：管理 WebSocket 服务生命周期。

建议接口：

```csharp
public interface ISimulationGatewayService
{
    Result Start();
    Result Stop();
    Result Broadcast(SimulationEnvelopeDto message);
    Result<int> GetClientCount();
    bool IsRunning { get; }
}
```

### 6.2 `ISimulationRuntimeService`

职责：统一管理仿真事件订阅与运行态广播。

```csharp
public interface ISimulationRuntimeService
{
    Result Initialize();
    Result Start();
    Result Stop();
    Result PublishFullSnapshot();
    Result HandleClientCommand(SimulationEnvelopeDto envelope);
    bool IsStarted { get; }
}
```

### 6.3 `ISimulationSnapshotService`

职责：构建整机全量快照。

```csharp
public interface ISimulationSnapshotService
{
    Result<SimulationMachineSnapshotDto> BuildSnapshot();
}
```

### 6.4 `ISimulationBindingService`

职责：解析数据库与配置中的仿真绑定信息。

```csharp
public interface ISimulationBindingService
{
    Result<IReadOnlyList<SimulationDeviceBindingDto>> GetBindings();
    Result Reload();
}
```

### 6.5 `ISimulationCommandHandler`

职责：按消息类型处理命令。

```csharp
public interface ISimulationCommandHandler
{
    string MessageType { get; }
    Result Handle(SimulationEnvelopeDto envelope);
}
```

---

## 7. DTO 设计建议

### 7.1 `SimulationEnvelopeDto`

统一消息包，所有 WebSocket 消息均使用此结构。

```csharp
public class SimulationEnvelopeDto
{
    public string MessageId { get; set; }
    public string MessageType { get; set; }
    public DateTime Timestamp { get; set; }
    public object Payload { get; set; }
    public string Source { get; set; }
    public string ClientId { get; set; }
}
```

### 7.2 `SimulationAxisStateDto`

必须保留三套位置语义。

```csharp
public class SimulationAxisStateDto
{
    public short LogicalAxis { get; set; }
    public string AxisName { get; set; }
    public double PulseCommandPosition { get; set; }
    public double PulseEncoderPosition { get; set; }
    public double CommandPositionMm { get; set; }
    public double EncoderPositionMm { get; set; }
    public double CurrentVelocityPulsePerMs { get; set; }
    public bool IsMoving { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsAlarm { get; set; }
    public bool IsAtHome { get; set; }
}
```

### 7.3 `SimulationIoPointDto`

```csharp
public class SimulationIoPointDto
{
    public short Bit { get; set; }
    public string Name { get; set; }
    public string IoType { get; set; }
    public bool Value { get; set; }
    public string DeviceName { get; set; }
    public string Description { get; set; }
}
```

### 7.4 `SimulationDeviceBindingDto`

用于描述 3D 模型节点与后端对象绑定关系。

```csharp
public class SimulationDeviceBindingDto
{
    public string Name { get; set; }
    public string DeviceType { get; set; }
    public string ModelNodeKey { get; set; }
    public short? LogicalAxis { get; set; }
    public short? ForwardDoBit { get; set; }
    public short? BackwardDoBit { get; set; }
    public short? ForwardDiBit { get; set; }
    public short? BackwardDiBit { get; set; }
    public double StrokeMm { get; set; }
    public double RotateDeg { get; set; }
    public string ExtraJson { get; set; }
}
```

### 7.5 `SimulationMachineSnapshotDto`

```csharp
public class SimulationMachineSnapshotDto
{
    public string MachineName { get; set; }
    public DateTime Timestamp { get; set; }
    public List<SimulationAxisStateDto> Axes { get; set; }
    public List<SimulationIoPointDto> Dis { get; set; }
    public List<SimulationIoPointDto> Dos { get; set; }
    public List<SimulationDeviceBindingDto> Bindings { get; set; }
}
```

### 7.6 `SimulationGatewayConfig`

该配置建议由 `ConfigContext` 统一管理。

```csharp
public class SimulationGatewayConfig
{
    public bool Enabled { get; set; }
    public bool AutoStart { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string RoutePath { get; set; }
    public bool AllowInjectDi { get; set; }
    public bool BroadcastSnapshotOnConnect { get; set; }
}
```

---

## 8. WebSocket 消息协议设计

建议采用统一 Envelope + 业务载荷方式。

### 8.1 消息命名约定

建议全部使用小写点分格式：

- `sim.hello`
- `sim.snapshot.full`
- `axis.position`
- `axis.status`
- `io.di.changed`
- `io.do.changed`
- `io.di.inject`
- `sim.flow.play`
- `sim.flow.stop`
- `sim.error`
- `sim.heartbeat`

### 8.2 前后端消息方向

#### .NET -> 前端

- `sim.hello`
- `sim.snapshot.full`
- `axis.position`
- `axis.status`
- `io.di.changed`
- `io.do.changed`
- `sim.error`
- `sim.heartbeat`

#### 前端 -> .NET

- `sim.hello`
- `sim.snapshot.request`
- `io.di.inject`
- `sim.flow.play`
- `sim.flow.stop`
- `sim.ping`

### 8.3 典型消息示例

#### 连接欢迎

```json
{
  "messageId": "8db5d3f0-01",
  "messageType": "sim.hello",
  "timestamp": "2026-03-27T10:00:00",
  "source": "AMControlWPF",
  "clientId": "",
  "payload": {
    "machineName": "amcontrol-virtual-machine",
    "serverTime": "2026-03-27T10:00:00",
    "protocolVersion": "1.0"
  }
}
```

#### 全量快照

```json
{
  "messageId": "8db5d3f0-02",
  "messageType": "sim.snapshot.full",
  "timestamp": "2026-03-27T10:00:01",
  "source": "AMControlWPF",
  "clientId": "client-001",
  "payload": {
    "machineName": "amcontrol-virtual-machine",
    "timestamp": "2026-03-27T10:00:01",
    "axes": [
      {
        "logicalAxis": 1,
        "axisName": "XAxis",
        "pulseCommandPosition": 10000,
        "pulseEncoderPosition": 10000,
        "commandPositionMm": 50.0,
        "encoderPositionMm": 50.0,
        "currentVelocityPulsePerMs": 2.0,
        "isMoving": false,
        "isEnabled": true,
        "isAlarm": false,
        "isAtHome": true
      }
    ],
    "dis": [],
    "dos": [],
    "bindings": []
  }
}
```

#### 单轴位置更新

```json
{
  "messageId": "8db5d3f0-03",
  "messageType": "axis.position",
  "timestamp": "2026-03-27T10:00:02",
  "source": "AMControlWPF",
  "clientId": "",
  "payload": {
    "logicalAxis": 1,
    "pulseCommandPosition": 10500,
    "pulseEncoderPosition": 10500,
    "commandPositionMm": 52.5,
    "encoderPositionMm": 52.5
  }
}
```

#### DO 变化

```json
{
  "messageId": "8db5d3f0-04",
  "messageType": "io.do.changed",
  "timestamp": "2026-03-27T10:00:03",
  "source": "AMControlWPF",
  "clientId": "",
  "payload": {
    "bit": 101,
    "name": "ClampForward",
    "ioType": "DO",
    "value": true,
    "deviceName": "ClampCylinder1"
  }
}
```

#### 前端注入 DI

```json
{
  "messageId": "8db5d3f0-05",
  "messageType": "io.di.inject",
  "timestamp": "2026-03-27T10:00:04",
  "source": "ThreeSimulator",
  "clientId": "client-001",
  "payload": {
    "bit": 201,
    "value": true,
    "deviceName": "ClampCylinder1",
    "reason": "forward-arrived"
  }
}
```

### 8.4 消息处理原则

- 所有消息统一序列化为 JSON。
- 所有命令处理统一返回 `Result`。
- 前端注入命令必须经过 `ISimulationCommandHandler`，不允许页面直接调用后端对象。
- 协议必须允许未来增加 PLC、视觉、报警等消息类型。

---

## 9. 与 `MachineContext` 的集成方式

### 9.1 基本原则

仿真运行时服务只能通过 `MachineContext` 获取当前运行设备对象，不允许：

- 在网关服务中自己 new `VirtualMotionCardService`
- 在页面中维护私有虚拟卡副本
- 在命令处理器中缓存私有长期运动对象实例

### 9.2 推荐访问方式

建议由 `SimulationRuntimeService` 在 `Start()` 中：

1. 通过 `MachineContext` 获取当前运动控制管理对象；
2. 找到当前启用的虚拟卡实例或统一的运动服务入口；
3. 订阅其事件：
   - `AxisPositionChanged`
   - `AxisStatusChanged`
   - `DOChanged`
   - `DIChanged`
4. 在停止时解除订阅。

### 9.3 推荐封装

如果当前 `MachineContext` 还没有一个清晰的“获取当前虚拟卡”的统一接口，建议补一层**只读访问接口**，例如：

```csharp
public interface ICurrentMotionProvider
{
    Result<MotionCardBase> GetCurrentMotionCard();
    Result<VirtualMotionCardService> GetCurrentVirtualMotionCard();
}
```

此接口可以由 `MachineContext` 所在层提供实现，供 `AM.SimulationService` 调用。

### 9.4 运行流程

- 主程序启动完成基础设施初始化；
- `MachineContext` 完成设备初始化；
- 若 `ConfigContext` 中 `SimulationGatewayConfig.AutoStart == true`，则启动 `ISimulationRuntimeService.Start()`；
- `SimulationRuntimeService` 订阅虚拟卡事件并开启网关；
- 前端连接后自动下发 `sim.hello` 与可选全量快照。

---

## 10. 与 `ConfigContext` 的集成方式

### 10.1 基本原则

仿真配置统一由 `ConfigContext` 读取和保存，不允许：

- `SimulationGatewayService` 私有静态缓存长期持有配置副本；
- 页面 ViewModel 自己维护另一份端口、地址等配置来源；
- 前端地址、网关端口等配置散落到多个文件。

### 10.2 建议新增配置段

在 `config.json` 对应的配置模型中新增：

```json
{
  "SimulationGateway": {
    "Enabled": true,
    "AutoStart": true,
    "Host": "127.0.0.1",
    "Port": 18081,
    "RoutePath": "/ws/simulation",
    "AllowInjectDi": true,
    "BroadcastSnapshotOnConnect": true
  }
}
```

对应 `ConfigContext` 中新增强类型配置项：

- `SimulationGatewayConfig SimulationGateway { get; set; }`

### 10.3 数据库存储内容建议

本地 `config.json` 负责：

- 是否启用仿真网关
- 主机地址
- 端口
- 自动启动
- 连接策略

数据库负责：

- 轴与 3D 节点映射
- IO 与设备组件映射
- 气缸、夹爪、真空、灯塔、门禁、光栅等设备绑定参数
- 可选流程模板与演示配置

### 10.4 配置刷新原则

- 网关配置改动后，由页面触发统一保存逻辑写回 `ConfigContext`。
- 绑定配置改动后，由 `ISimulationBindingService.Reload()` 重新从数据库加载。
- 运行态服务不长期持有脱离 `ConfigContext` 的私有配置副本。

---

## 11. 快照构建逻辑设计

### 11.1 `SimulationSnapshotService` 输入来源

- `MachineContext`：当前运动控制对象
- `ConfigContext`：网关与显示级配置
- 数据库：设备绑定、节点映射、名称信息

### 11.2 快照构建内容

#### 轴数据

对每个逻辑轴构建：

- `LogicalAxis`
- `AxisName`
- `PulseCommandPosition`
- `PulseEncoderPosition`
- `CommandPositionMm`
- `EncoderPositionMm`
- `CurrentVelocityPulsePerMs`
- `IsMoving`
- `IsEnabled`
- `IsAlarm`
- `IsAtHome`

#### IO 数据

分别构建：

- DI 列表
- DO 列表

#### 绑定数据

从数据库中读取：

- 模型节点 key
- 逻辑轴
- 位号绑定
- 行程参数
- 扩展 JSON

### 11.3 首次连接与主动刷新

- 前端首次连接：如果开启 `BroadcastSnapshotOnConnect`，立即推送全量快照；
- WPF 页面可提供“强制刷新快照”按钮，调用 `PublishFullSnapshot()`；
- 前端也可发送 `sim.snapshot.request` 主动请求。

---

## 12. 事件桥接设计

`SimulationRuntimeService` 应负责桥接后端运行态事件到 WebSocket 消息。

### 12.1 订阅事件

若当前为虚拟卡，则订阅：

- `AxisPositionChanged`
- `AxisStatusChanged`
- `DOChanged`
- `DIChanged`

### 12.2 转换规则

#### `AxisPositionChanged`
转换为：
- `axis.position`

并补充毫米位置：
- `CommandPositionMm = pulse / K`
- `EncoderPositionMm = pulse / K`

#### `AxisStatusChanged`
转换为：
- `axis.status`

#### `DOChanged`
转换为：
- `io.do.changed`

#### `DIChanged`
转换为：
- `io.di.changed`

### 12.3 频率控制建议

首版可直接逐事件发送。后续如发现高频广播压力过大，可增加：

- 轴位置节流（如 20ms 或 50ms 一次）
- 同轴去重广播
- 客户端订阅粒度控制

但首版不要过早复杂化。

---

## 13. 前端命令处理设计

### 13.1 首期建议支持的命令

- `sim.snapshot.request`
- `io.di.inject`
- `sim.flow.play`（可先保留结构）
- `sim.flow.stop`（可先保留结构）
- `sim.ping`

### 13.2 `io.di.inject` 处理规则

建议由 `InjectDiCommandHandler`：

1. 检查 `ConfigContext.SimulationGateway.AllowInjectDi`；
2. 验证当前运动卡是否为 `VirtualMotionCardService`；
3. 解析 bit / value；
4. 调用 `VirtualMotionCardService.InjectDI(bit, value)`；
5. 返回 `Result`。

### 13.3 错误消息返回

对于非法命令、仿真未启用、不是虚拟卡、参数错误等情况，统一返回：

- `sim.error`

负载示例：

```json
{
  "code": "Simulation.NotVirtualCard",
  "message": "当前运行设备不是虚拟运动卡，不能注入 DI。"
}
```

---

## 14. WPF 页面规划

结合你当前主界面固定左侧一级/二级导航结构，推荐新增一级或二级导航组：

- 一级：`仿真`
- 二级：
  - `仿真监控`
  - `仿真配置`

### 14.1 `SimulationMonitorView`

用途：运行态监控。

建议显示：

- 网关运行状态
- 监听地址/端口
- 当前连接客户端数量
- 最近消息时间
- 最近异常
- 快照发送按钮
- DI 注入测试面板
- 轴状态摘要
- IO 状态摘要

交互模式应复用现有 `MotionMonitorView` / `MotionActuatorView` 风格：

- 页面首次加载执行 `LoadAsync()`
- 后续依赖运行态服务持续刷新
- 不在 `Unloaded` 中随意释放核心运行态对象

### 14.2 `SimulationConfigView`

用途：仿真配置维护。

建议显示：

- 网关启用开关
- 自动启动
- Host
- Port
- RoutePath
- 允许注入 DI
- 连接后自动发送快照
- 保存按钮
- 绑定配置刷新按钮
- 可跳转到数据库映射维护页面

### 14.3 页面样式要求

- 样式资源统一放入 `Resources/Themes/Styles/Style.xaml` 或拆分引用的 `SimulationStyle.xaml`
- 页面中通过静态资源调用
- 不在 `MainWindow.xaml` 内联大量样式

### 14.4 ViewModel 约束

ViewModel 继续使用手写属性：

- 私有字段
- 公共属性
- `SetProperty(ref field, value)` 风格

不使用 `[ObservableProperty]`。

---

## 15. WPF 导航接入建议

在 `MainWindow` 当前导航结构中新增仿真页面注册项时，应遵守现有页面缓存模式。

建议页面标识：

- `Simulation.Monitor`
- `Simulation.Config`

`MainWindow.xaml.cs` 页面工厂新增：

- `case "Simulation.Monitor": return new SimulationMonitorView();`
- `case "Simulation.Config": return new SimulationConfigView();`

如权限体系已接入，则按页面级权限控制是否显示导航项。

---

## 16. 命名空间建议

### 16.1 `AM.Model`

- `AM.Model.Simulation.Enums`
- `AM.Model.Simulation.Dtos`
- `AM.Model.Simulation.Interfaces`
- `AM.Model.Simulation.Configs`

### 16.2 `AM.SimulationService`

- `AM.SimulationService.Gateway`
- `AM.SimulationService.Runtime`
- `AM.SimulationService.Snapshot`
- `AM.SimulationService.Binding`
- `AM.SimulationService.Dispatcher`
- `AM.SimulationService.Handlers`
- `AM.SimulationService.Session`
- `AM.SimulationService.Extensions`

### 16.3 `AM.ViewModel`

- `AM.ViewModel.ViewModels.Simulation`

### 16.4 `AMControlWPF`

- `AMControlWPF.Views.Simulation`

---

## 17. 启动流程建议

### 17.1 程序启动时序

1. 初始化 `ConfigContext`
2. 初始化数据库连接
3. 初始化 `MachineContext`
4. 初始化运动卡及其他设备
5. 注册 `AM.SimulationService` 依赖
6. 若 `ConfigContext.SimulationGateway.Enabled && AutoStart` 为真，则启动 `ISimulationRuntimeService`
7. 打开主窗口

### 17.2 手动启动/停止

WPF 仿真监控页可提供：

- 启动网关
- 停止网关
- 重载绑定
- 发送全量快照

### 17.3 关闭流程

程序退出时：

- 先停止 `ISimulationRuntimeService`
- 再释放底层设备连接

防止关闭时仍有 WebSocket 客户端持有旧状态。

---

## 18. MVP 实施范围建议

首版只做最小闭环，不一开始覆盖整条产线。

### 18.1 MVP 范围

- WebSocket 网关
- 全量快照
- 轴位置/状态实时推送
- DI/DO 实时推送
- 前端注入 DI
- WPF 仿真监控页
- WPF 仿真配置页
- 基础绑定配置读取

### 18.2 暂不纳入 MVP

- PLC 寄存器级完整仿真
- 视觉图像级仿真
- 机械臂逆运动学
- 碰撞检测与物理引擎
- 流程编排编辑器
- 故障脚本系统

---

## 19. 风险点与规避策略

### 19.1 风险：状态源分裂

如果页面、网关、前端各自缓存一份运动对象，会导致急停、回零、位置显示不同步。

**规避：** 全部运行态从 `MachineContext` 获取。

### 19.2 风险：配置源分裂

如果端口、允许注入 DI 等配置散落在多个类，会导致保存后运行态不可见。

**规避：** 统一收口到 `ConfigContext`。

### 19.3 风险：位置语义混乱

只传一个毫米位置会导致前端无法区分规划位置与编码器位置。

**规避：** 强制同时保留 pulse / command mm / encoder mm 三套语义。

### 19.4 风险：通信逻辑侵入页面

如果页面直接操控 Socket，会导致后续难以维护。

**规避：** 页面只绑定 ViewModel，通信细节下沉到 `AM.SimulationService`。

### 19.5 风险：过早做全量高精度模型

可能拖慢整体开发进度。

**规避：** 先完成 .NET 侧通信闭环与简单模型联动，再逐步扩展。

---

## 20. 建议实施顺序

### 第一阶段：基础骨架

- 新增 `AM.SimulationService`
- 新增 `AM.Model.Simulation.*`
- 接入依赖注入
- 接入 `ConfigContext` 仿真配置

### 第二阶段：运行态打通

- `SimulationRuntimeService` 从 `MachineContext` 获取虚拟卡
- 订阅事件
- 构建 WebSocket 广播
- 支持快照请求

### 第三阶段：WPF 页面

- `SimulationMonitorView`
- `SimulationConfigView`
- 导航接入

### 第四阶段：绑定配置

- 数据库表或配置项设计
- `SimulationBindingService`
- 快照中加入绑定信息

### 第五阶段：与前端联调

- Vue3 + Three.js 首版联调
- XYZ + IO + 气缸闭环验证

---

## 21. 后续扩展方向

在 MVP 完成后，可逐步扩展：

- 气缸/夹爪/真空/灯塔专用状态 DTO
- PLC 信号仿真 DTO
- 视觉触发与结果注入 DTO
- 故障注入协议
- 运行轨迹录制与回放
- 自动流程演示命令
- 多客户端订阅与权限控制

---

## 22. 结论

按当前仓库结构，`.NET 侧 3D 仿真接入` 的最佳实现方式是：

1. **新增独立 `AM.SimulationService` 项目**，统一承载通信、快照、绑定、命令处理；
2. **在 `AM.Model` 中新增 `Simulation` 相关接口与 DTO**；
3. **严格通过 `MachineContext` 获取运行态设备，通过 `ConfigContext` 获取网关配置**；
4. **使用统一 `Result/Result<T>` 与统一 WebSocket Envelope 协议**；
5. **在 `AMControlWPF` 中新增仿真监控/配置页，并复用现有页面加载模式**；
6. **先完成 MVP 闭环，再逐步扩展整机级数字孪生能力**。

该方案与当前仓库已存在的 `VirtualMotionCardService` 设计方向高度一致，也最符合现阶段“允许较大重构，但优先保证架构与接口分层正确”的总体要求。