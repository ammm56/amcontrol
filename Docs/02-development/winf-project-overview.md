# AMControlWinF 项目总览

**文档编号**：DEV-W001  
**版本**：2.1.0  
**状态**：有效  
**最后更新**：2026-04-23  
**维护人**：Am

---

## 1. 项目定位

`AMControlWinF` 是 `ammm56/amcontrol` 仓库当前活跃的 WinForms 工业设备控制软件分支，目标运行环境为 `.NET Framework 4.6.1`，UI 基于 `AntdUI`，后端与 `AMControlWPF` 共享 `AM.Core`、`AM.Model`、`AM.DBService`、`AM.MotionService`、`AM.App`、`AM.Tools`、`AM.PageModel` 等项目。

当前阶段的重点已从“搭建壳层”进入“在既有架构上持续补齐页面与功能闭环”：

- 基础设施、上下文、启动链路、权限体系已经成型；
- Motion / PLC / Alarm / Auth 的后端服务已经可用；
- WinForms 壳层、导航、页面缓存与一批核心页面已经落地；
- 后续开发应严格基于现有结构推进，不再引入新的 UI 分层方案。

---

## 2. 技术栈

| 层级 | 技术 / 框架 | 说明 |
|------|------------|------|
| UI 框架 | AntdUI (WinForms) | 使用 `Panel`、`GridPanel`、`FlowPanel`、`Menu`、`Table`、`Popover` 等控件实现现代化 WinForms 页面 |
| 目标框架 | .NET Framework 4.6.1 | 当前分支运行框架 |
| 语言版本 | C# 7.3 | 页面实现与共享项目均需兼容 |
| 页面模型 | `AM.PageModel` | WinForms / WPF 共用的无 UI 依赖中间层 |
| ORM | SqlSugar | 支持 SQLite / Access / MySQL |
| 日志 | NLog | 统一日志输出与运行日志页面查询 |
| 配置 | `ConfigContext` + `config.json` + DB | 本地基础配置 + 数据库设备配置混合体系 |
| 消息 | `IMessageBus` / `MessageBus` | 主窗体状态栏与全局消息提示统一来源 |
| 报警 | `AlarmManager` + `DevAlarmRecordService` | 活动报警、历史报警、启动恢复 |
| 运行时任务 | `IRuntimeTaskManager` | 管理 IO / Axis / PLC 扫描工作单元 |

---

## 2.1 当前开发环境补充说明

当前仓库在开发环境上存在以下事实：

1. 解决方案采用“旧式 .NET Framework 项目 + SDK-style 项目”混合结构；
2. `ProtocolLib`、`AntdUI`、`AM.Tests` 属于 SDK-style 项目；
3. VS2019 若命中过新的 .NET SDK，可能在加载 SDK-style 项目时失败；
4. VS2019 兼容主要通过本地 `global.json` 约束 SDK 版本解决；
5. 对 `AntdUI.Window` 驱动的复杂 WinForms 页面，VS2019 设计器不作为主推荐环境。

详见：[VS2019 兼容说明](winf-vs2019-compatibility.md)。

---

## 3. 解决方案结构

```text
amcontrol_winform.sln
├── AMControlWinF/          WinForms UI 壳层、页面、对话框、子控件
├── AM.PageModel/           页面模型、中间层、导航、登录、主窗体模型
├── AM.App/                 启动编排与组合根（AppBootstrap）
├── AM.Core/                上下文、日志、消息、报警、Reporter、ServiceBase
├── AM.Model/               实体、接口、运行时模型、Result、Config 模型
├── AM.DBService/           Auth / Motion / Plc / Alarm / Runtime 数据与应用服务
├── AM.MotionService/       运动控制卡驱动实现、工厂与 Hub 路由
├── AM.Tools/               配置读写、日志适配、辅助工具
├── ProtocolLib/            PLC 协议库（Common / ModbusTcp / S7Tcp）
└── AM.Tests/               协议与关键模块测试
```

---

## 4. 当前分层职责

| 项目 | 职责 |
|------|------|
| `AMControlWinF` | 主窗体壳层、UserControl 页面、对话框、公共子控件、AntdUI 布局实现 |
| `AM.PageModel` | 页面模型、导航定义、权限过滤、页面状态绑定、轻量 `BindableBase` |
| `AM.App` | 应用启动顺序：配置加载、上下文初始化、种子数据、配置重载、硬件连接、后台任务注册 |
| `AM.Core` | 5 大上下文（Config/System/Machine/User/Runtime）、报警、消息总线、Reporter、ServiceBase |
| `AM.Model` | 数据库实体、设备配置模型、PLC 模型、Motion 模型、运行时快照、接口定义、统一 Result |
| `AM.DBService` | 配置 CRUD、配置重载、运行时查询、调试操作、后台扫描工作单元、日志/报警持久化 |
| `AM.MotionService` | 三种控制卡服务（Googo / Leisai / Virtual）、统一调度入口 `MotionServiceHub` |
| `ProtocolLib` | 协议接口 `IProtocol` 与具体协议实现，当前已落地 `ModbusTcp` 与 `S7Tcp` |

---

## 5. 全局上下文与运行主线

### 5.1 五大上下文

| 上下文 | 职责 |
|--------|------|
| `ConfigContext` | 保存 `config.json` 基础配置与 DB 重载后的 Motion / PLC / 执行器配置 |
| `SystemContext` | 保存日志、消息总线、报警管理、Reporter、错误目录、后台任务管理器 |
| `MachineContext` | 保存控制卡实例、轴/IO 路由、执行器索引、PLC 客户端实例 |
| `UserContext` | 保存当前登录用户、角色、页面权限 |
| `RuntimeContext` | 保存 Motion IO、Motion Axis、PLC 三类运行时缓存 |

### 5.2 启动主线

`Program.Main()` + `AppBootstrap.Initialize()` 当前已形成固定链路：

1. 读取 `config.json`；
2. 初始化日志、消息、报警、Reporter；
3. 初始化认证表与默认管理员；
4. 初始化 Motion 配置种子；
5. 初始化 PLC 配置种子；
6. `MachineConfigReloadService.ReloadAndRebuild()` 重建 Motion 上下文；
7. `PlcConfigAppService.ReloadFromDatabase()` 重建 PLC 客户端上下文；
8. 控制卡 `Initialize + Connect`；
9. 注册 `IoScanWorker`、`MotionAxisScanWorker`、`PlcScanWorker`；
10. 进入 `while` 主循环：登录 → `Application.Run(MainWindow)` → 按 `ExitReason` 继续或退出。

---

## 6. AMControlWinF 项目结构（最新）

```text
AMControlWinF/
├── Program.cs
├── MainWindow.cs / .Designer.cs / .resx
├── Tools/
│   ├── AppThemeHelper.cs
│   ├── PageDialogHelper.cs
│   └── ControlDisposeHelper.cs
├── Views/
│   ├── Auth/
│   │   └── LoginForm.cs / .Designer.cs
│   ├── Main/
│   │   ├── TextureBackgroundControl.cs
│   │   ├── UserAvatarMenuControl.cs
│   │   ├── UserAvatarPopoverCard.cs
│   │   └── ActiveAlarmDrawerControl.cs
│   ├── Am/
│   │   ├── UserManagementPage.cs
│   │   ├── UserPermissionPage.cs
│   │   ├── LoginLogPage.cs
│   │   └── 各类编辑/选择对话框
│   ├── Motion/
│   │   ├── DIMotionPage.cs
│   │   ├── DOMotionPage.cs
│   │   ├── MotionMonitorPage.cs
│   │   ├── MotionAxisPage.cs
│   │   ├── MotionActuatorPage.cs
│   │   └── 多个列表/详情/动作子控件
│   ├── MotionConfig/
│   │   ├── MotionCardManagementPage.cs
│   │   ├── MotionAxisManagementPage.cs
│   │   ├── MotionIoMapManagementPage.cs
│   │   ├── MotionAxisParamManagementPage.cs
│   │   └── ActuatorManagementPage.cs
│   ├── Plc/
│   │   ├── PlcStatusPage.cs
│   │   ├── PlcMonitorPage.cs
│   │   ├── PlcDebugPage.cs
│   │   └── 列表/详情子控件
│   ├── SysConfig/
│   │   ├── PlcConfigManagementPage.cs
│   │   ├── PlcStationEditDialog.cs
│   │   └── PlcPointEditDialog.cs
│   └── AlarmLog/
│       ├── CurrentAlarmPage.cs
│       ├── ActiveAlarmContentControl.cs
│       ├── AlarmHistoryPage.cs
│       └── RunLogPage.cs
└── Properties/
```

---

## 7. 导航与页面落地状态

### 7.1 导航定义来源

- 唯一来源：`AM.PageModel.Navigation.NavigationCatalog`
- 权限同步：`Program.SyncPagePermissions()` → `AuthSeedService.SyncPagePermissions(...)`
- 页面过滤：`MainWindowModel.LoadNavigation()` → `UserContext.Instance.HasPagePermission(page.PageKey)`

### 7.2 当前已注册并具备实现的页面

以下页面已在 `MainWindow.CreatePageFactories()` 中注册为真实页面而非占位：

| 模块 | 页面 |
|------|------|
| `Assembly` | `Assembly.Wiring` |
| `Motion` | `Motion.DI`、`Motion.DO`、`Motion.Monitor`、`Motion.Axis`、`Motion.Actuator` |
| `MotionConfig` | `MotionConfig.Card`、`MotionConfig.Axis`、`MotionConfig.IoMap`、`MotionConfig.AxisParam`、`MotionConfig.Actuator` |
| `PLC` | `PLC.Status`、`PLC.Monitor`、`PLC.Debug` |
| `SysConfig` | `SysConfig.Plc` |
| `AlarmLog` | `AlarmLog.Current`、`AlarmLog.History`、`AlarmLog.RunLog` |
| `System` | `System.User`、`System.Permission`、`System.LoginLog` |

### 7.3 当前仍为占位的页面

- `Home.*`
- `Production.*`
- `Vision.*`
- `Peripheral.*`
- `SysConfig.Camera / Sensor / Scanner / Mes / Runtime`

---

## 8. 统一页面实现模式

当前 WinForms 页面已形成统一实现基线：

### 8.1 页面分工

| 层级 | 职责 |
|------|------|
| `MainWindow` | 导航切换、页面缓存、状态栏、报警抽屉、主题/语言 |
| `UserControl` 页面 | 首次加载控制、事件接线、低频刷新、把模型结果绑定到控件 |
| `PageModel` | 查询、筛选、分页、选中项维护、权限态、动作执行入口 |
| `Service` | 配置加载、运行态读取、读写动作、重载、扫描控制 |
| `RuntimeContext` | 保存运行态缓存，供 UI 低频采样读取 |

### 8.2 生命周期约束

- 页面被 `MainWindow` 缓存复用；
- 首次加载统一使用 `_isFirstLoad`；
- 可见时启动 `Timer`，不可见时停止；
- 页面只在 `Disposed` 时释放 `Timer` 等资源；
- 页面不在切走时主动销毁 ViewModel / PageModel。

### 8.3 典型布局模板

| 页面类型 | 统一布局 |
|----------|----------|
| 配置管理页 | 顶部工具栏 + 主体列表/卡片 + 底部分页 / 编辑对话框 |
| 监视页 | 顶部筛选栏 + 统计卡片 + 列表区 + 详情区 |
| 调试页 | 顶部工具栏 + 左侧操作区 + 右侧结果区 |
| 日志审计页 | 顶部筛选栏 + 统计卡片 + 整页 `Table` + 分页 |

---

## 9. 关键模块最新状态

### 9.1 Auth / System

- `LoginForm` 已完成；
- `UserManagementPage`、`UserPermissionPage`、`LoginLogPage` 已完成基础闭环；
- `MainWindow` 已实现切换用户、退出登录、主题、语言、状态栏消息。

### 9.2 Motion

- Motion 拓扑 CRUD、参数 CRUD、执行器 CRUD 已全部具备；
- `IoScanWorker`、`MotionAxisScanWorker` 已写入 `RuntimeContext`；
- DI / DO / 多轴总览 / 单轴控制 / 执行器控制页面均已落地基础版；
- 运控配置 5 页已落地基础版。

### 9.3 PLC

- PLC 配置模型、实体、CRUD、上下文重载、客户端工厂、协议插件注册、运行时扫描、查询服务、调试操作服务均已落地；
- `ProtocolLib.ModbusTcp.Protocol` 与 `ProtocolLib.S7Tcp.Protocol` 已实现；
- PLC 扫描当前已重构为“顶层 Worker + 站级独立 Runner”模式，单站离线不再拖慢其他站；
- PLC 与后端异常日志已经补充统一前缀，便于直接从运行日志区分超时、不可达、站离线与协议层 socket 失败；
- `PLC.Status`、`PLC.Monitor`、`PLC.Debug`、`SysConfig.Plc` 已完成首版 UI；
- 当前优先工作为 PLC 页面收口与使用手册完善。

### 9.5 当前已知兼容边界

- `Debug|AnyCPU` 配置在部分旧式项目中仍实际输出 `x64`；
- 这对运行时主链是可接受的，但会放大 VS2019 WinForms 设计器的设计时加载风险；
- 当前推荐开发方式仍是：VS2022 用于 UI 设计器与主界面维护，VS2019 作为兼容构建 / 普通代码编辑环境。

### 9.4 Alarm / Log

- `AlarmManager` + `DevAlarmRecordService` + 启动恢复 已成型；
- 当前报警页、报警历史页、运行日志页已完成；
- 主窗体底部报警按钮与抽屉联动已具备。

---

## 10. 当前开发基线与约束

1. 不新增新的 UI 架构层；
2. 继续沿用 `UserControl + PageModel + Service + RuntimeContext` 模式；
3. 配置变更后通过 `ReloadAndRebuild()` / `ReloadFromDatabase()` 生效，不做局部脏补丁；
4. 高频后台变化不直接驱动整页 UI，页面统一低频刷新；
5. 成功/失败消息优先走消息总线，不在页面重复弹窗；
6. 页面权限、导航目录、默认角色始终以 `NavigationCatalog` 为准；
7. 新页面先接入导航、工厂与页面模型，再补子控件和手册。

---

## 11. 推荐后续推进顺序

### 第一优先级：收口已实现页面

1. `PLC.Debug`
2. `PLC.Monitor`
3. `PLC.Status`
4. `SysConfig.Plc`
5. `Motion.Axis`
6. `Motion.Monitor`
7. `Motion.Actuator`
8. `AlarmLog.Current`
9. `AlarmLog.RunLog`

### 第二优先级：补齐首页

10. `Home.Overview`
11. `Home.SysStatus`

### 第三优先级：补齐远期业务页

12. `Peripheral.*`
13. `Vision.*`
14. `Production.*`
15. `Engineer.*`
16. 其余 `SysConfig.*`

---

## 相关文档

- [WinForms 解决方案架构](../01-architecture/winf-solution-architecture.md)
- [导航系统与页面缓存](../01-architecture/winf-navigation-system.md)
- [统一 UI 规范与开发模板](../01-architecture/winf-ui-standards.md)
- [页面开发模板与实施基线](winf-page-development-template.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
- [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)
