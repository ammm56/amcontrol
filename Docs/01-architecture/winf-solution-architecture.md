# AMControlWinF 解决方案架构

**文档编号**：ARCH-W001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 文档目标

本文档用于统一描述当前 `AMControlWinF` 分支的最新解决方案架构，覆盖：

- 分层职责；
- 上下文体系；
- 运行时链路；
- Motion / PLC / Alarm / Auth 模块边界；
- 服务、模型、接口与协议库；
- 已实现页面与后续扩展基线。

本文档以当前仓库中的**实际实现**为准，而非早期规划状态。

---

## 2. 整体架构视图

```text
AMControlWinF (WinForms + AntdUI)
  → MainWindow / UserControl Pages / Dialogs
  → AM.PageModel (导航、页面模型、主窗体模型)
  → AM.DBService (配置、运行时、调试、日志、报警持久化)
  → AM.Core (Context / Reporter / Alarm / MessageBus / ServiceBase)
  → AM.Model (实体、配置模型、运行时模型、接口、Result)
  → AM.MotionService (控制卡驱动与 MotionHub)
  → ProtocolLib (PLC 协议实现)
```

### 2.1 设计原则

1. **配置唯一来源**：`ConfigContext.Instance`
2. **设备唯一入口**：`MachineContext.Instance`
3. **运行态唯一缓存**：`RuntimeContext.Instance`
4. **结果统一**：全项目统一 `Result` / `Result<T>`
5. **导航与权限唯一来源**：`NavigationCatalog`
6. **页面只做页面职责**：页面不直接承载底层协议或数据库访问逻辑

---

## 3. 分层职责

| 层级 | 项目 | 主要职责 |
|------|------|----------|
| UI 壳层 | `AMControlWinF` | 主窗体、页面、对话框、子控件、AntdUI 布局 |
| 页面模型层 | `AM.PageModel` | 导航、页面模型、状态绑定、筛选分页与动作入口 |
| 启动层 | `AM.App` | 启动编排、组合根、初始化顺序 |
| 基础设施层 | `AM.Core` | Context、MessageBus、Reporter、AlarmManager、ServiceBase |
| 模型层 | `AM.Model` | Entity、接口、Config 模型、运行时快照、Result |
| 数据服务层 | `AM.DBService` | CRUD、配置重载、扫描、调试、日志、报警持久化 |
| 运动控制层 | `AM.MotionService` | 控制卡驱动、Hub 路由、工厂 |
| 协议层 | `ProtocolLib` | `IProtocol` 与具体 PLC 协议实现 |

---

## 4. 全局上下文体系

### 4.1 `ConfigContext`

保存：

- 本地 `config.json` 基础配置；
- 从数据库重载后的 Motion 卡、轴、IO、执行器、PLC 配置；
- 语言、主题、扫描参数等运行配置。

### 4.2 `SystemContext`

保存：

- `IAMLogger`
- `IMessageBus`
- `AlarmManager`
- `IErrorCatalog`
- `IAppReporter`
- `IRuntimeTaskManager`

### 4.3 `MachineContext`

保存：

- `MotionCards`
- `AxisMotionCards` / `DICards` / `DOCards`
- `MotionHub`
- `Cylinders` / `Vacuums` / `Grippers` / `StackLights`
- `Plcs`

### 4.4 `UserContext`

保存：

- 当前用户；
- 当前角色；
- 当前页面权限集合。

### 4.5 `RuntimeContext`

保存：

- `MotionIo`
- `MotionAxis`
- `Plc`

其中 `Plc` 即 `PlcRuntimeState`，保存 PLC 站 / 点位快照与扫描状态。

---

## 5. 应用启动顺序

当前 `AppBootstrap.Initialize()` 在 WinForms 主线中采用以下顺序：

```text
1. 读取 config.json
2. 初始化 Logger / MessageBus / AlarmManager / Reporter
3. 初始化 SystemContext
4. 初始化认证种子数据
5. 初始化 Motion 配置种子数据
6. 初始化 PLC 配置种子数据
7. ProtocolAssemblyRegistry.Reload()
8. MachineConfigReloadService.ReloadAndRebuild()
9. PlcConfigAppService.ReloadFromDatabase()
10. 初始化并连接控制卡
11. 注册 IoScanWorker / MotionAxisScanWorker / PlcScanWorker
12. 进入 Program while 主循环
```

该顺序已被当前代码验证可用，属于现阶段 WinForms 主线的稳定实现链路。若后续出现新的宿主程序、启动模型或跨平台重构，应在对应架构文档中定义新的启动顺序，而不是把本页视为所有未来实现的固定约束。

---

## 6. 导航与 UI 架构

### 6.1 主窗体壳层

`MainWindow` 只负责：

- 一级 / 二级导航构建；
- 页面缓存；
- 用户菜单；
- 语言 / 主题；
- 状态栏消息；
- 报警抽屉。

### 6.2 页面模式

每个业务页统一采用：

```text
UserControl Page
  ├── PageModel
  ├── Crud / Query / Operation Service
  └── RuntimeContext / ConfigContext / MachineContext
```

### 6.3 页面缓存约束

- `MainWindow` 使用 `_pageFactories` + `_pageCache`；
- 页面首次访问才创建；
- 切页只移出容器不销毁；
- 页面使用 `_isFirstLoad` + `VisibleChanged` 控制一次性初始化与低频刷新。

---

## 7. Motion 架构

### 7.1 关键接口

`IMotionCardService` 聚合以下能力：

- 连接：`Initialize` / `Connect` / `Disconnect`
- 轴控制：`Enable` / `Stop` / `Home` / `MoveAbsolute` / `MoveRelative` / `JogMove`
- IO：`SetDO` / `GetDI` / `GetDO`
- 参数：`SetVel` / `SetAcc` / `SetDec`
- 状态：`GetAxisStatus` / `IsMoving`

### 7.2 关键实现

| 类型 | 实现 |
|------|------|
| 公共基类 | `MotionCardBase` |
| 固高 | `GoogoMotionCardService` |
| 雷赛 | `LeisaiMotionCardService` |
| 虚拟卡 | `VirtualMotionCardService` |
| 路由入口 | `MotionServiceHub` |

### 7.3 运行时服务

| 服务 | 说明 |
|------|------|
| `IoScanWorker` | 周期扫描 DI / DO |
| `MotionAxisScanWorker` | 周期采样轴状态 |
| `MachineConfigReloadService` | DB → Context 重建 |

### 7.4 已实现 UI 页面

- `Motion.DI`
- `Motion.DO`
- `Motion.Monitor`
- `Motion.Axis`
- `Motion.Actuator`
- `MotionConfig.*` 五页

---

## 8. PLC 架构

### 8.1 配置模型

| 模型 | 说明 |
|------|------|
| `PlcConfig` | PLC 配置聚合 |
| `PlcStationConfig` | 站配置 |
| `PlcPointConfig` | 点位配置 |

### 8.2 服务链路

| 类型 | 实现 |
|------|------|
| 客户端工厂 | `PlcClientFactory` |
| 客户端门面 | `ProtocolPlcClient` |
| 占位客户端 | `NullPlcClient` |
| 协议注册 | `ProtocolAssemblyRegistry` |
| 配置应用服务 | `PlcConfigAppService` |
| CRUD | `PlcStationCrudService` / `PlcPointCrudService` |
| 运行时查询 | `PlcRuntimeQueryService` |
| 调试服务 | `PlcOperationService` |
| 扫描工作单元 | `PlcScanWorker` |

### 8.3 协议库

| 协议 | 实现 |
|------|------|
| Modbus TCP | `ProtocolLib.ModbusTcp.Protocol` |
| Siemens S7 TCP | `ProtocolLib.S7Tcp.Protocol` |

### 8.4 已实现 UI 页面

- `PLC.Status`
- `PLC.Monitor`
- `PLC.Debug`
- `SysConfig.Plc`

---

## 9. Auth / Alarm / Log 架构

### 9.1 Auth

- 用户、角色、页面权限、登录日志表已完成；
- 默认管理员：`am / am123`；
- 当前权限粒度为页面级；
- `NavigationCatalog` 是权限目录唯一来源。

### 9.2 Alarm

- `AlarmManager` 管理活动报警；
- `DevAlarmRecordService` 持久化历史报警；
- 启动时可恢复未清除报警；
- 主窗体报警抽屉与 `AlarmLog.Current` 页面复用同一内容控件。

### 9.3 RunLog

- 运行日志由 NLog 输出；
- `RunLogPage` 支持日志文件选择、等级过滤、关键字查询、分页；
- 运行时调试操作应通过消息总线与日志统一记录。

---

## 10. 已实现页面矩阵

| 模块 | 页面 |
|------|------|
| `System` | 用户管理、权限分配、登录日志 |
| `Motion` | DI、DO、多轴总览、轴控制、执行器控制 |
| `MotionConfig` | 控制卡、轴、IO、参数、执行器配置 |
| `PLC` | 通讯状态、点位监视、调试工具 |
| `SysConfig` | PLC 配置 |
| `AlarmLog` | 当前报警、报警历史、运行日志 |

---

## 11. 当前扩展原则

后续新增页面或功能，应遵守：

1. 先检查 `NavigationCatalog`、`MainWindow`、`PageModel`、对应 `Service` 是否已有相同模式；
2. 优先复用现有上下文、运行时缓存、消息与报警机制；
3. 不新增新的页面状态管理框架；
4. 配置生效走 reload / rebuild；
5. 高频后台状态通过缓存 + 低频 UI 采样，不直接高频推 UI；
6. 文档同步更新到 `Docs`。

---

## 12. 当前结论

`AMControlWinF` 当前已经具备：

- 可持续扩展的统一架构；
- 可继续落地页面的固定模板；
- Motion / PLC / Alarm / Auth 的核心基础；
- 文档、代码、导航、权限、运行态之间的一致性基线。

后续工作重点应放在**收口现有页面**与**补齐剩余业务页**，而不是改造整体架构。

---

## 相关文档

- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [导航系统与页面缓存](winf-navigation-system.md)
- [PLC 协议库与 AM 上层分层架构](plc-protocol-integration-design.md)
- [统一 UI 规范与开发模板](winf-ui-standards.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)