# AMControlWinF 开发进展

**文档编号**：REL-W001  
**版本**：2.0.0  
**状态**：有效  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 项目当前阶段

`AMControlWinF` 当前已完成主框架、核心上下文、权限体系、Motion / PLC / Alarm 后端链路与一批核心页面的首版实现，项目阶段已从“搭建基础设施”进入“基于现有架构持续收口页面与补齐业务模块”。

当前状态的核心判断：

- **后端基础设施已成型**：可以稳定支撑后续页面开发；
- **WinForms 壳层已成型**：导航、页面缓存、主题、语言、状态栏、报警抽屉链路已完成；
- **核心页面已铺开**：System / Motion / MotionConfig / PLC / AlarmLog 已有首版页面；
- **剩余工作重点**：统一 UI 规范、收口核心页面、补齐首页和远期业务页。

---

## 2. 已完成内容

### 2.1 基础框架与生命周期

| 功能 | 状态 | 说明 |
|------|------|------|
| 解决方案分层 | ✅ | `AMControlWinF / AM.PageModel / AM.App / AM.Core / AM.Model / AM.DBService / AM.MotionService / AM.Tools / ProtocolLib` |
| `Program.cs` 主循环 | ✅ | `while` 主循环 + 登录 / 主窗体生命周期 |
| `AppBootstrap` 启动编排 | ✅ | 配置加载、上下文初始化、种子、重载、硬件连接、后台任务注册 |
| 页面权限同步 | ✅ | `NavigationCatalog` → `SysPagePermission` |
| 登录 → 主窗体 → 切换用户 / 退出登录 | ✅ | `MainWindowExitReason` 已闭环 |

### 2.2 WinForms 壳层

| 功能 | 状态 | 说明 |
|------|------|------|
| 主窗体三段布局 | ✅ | 两级导航 + 工作区 + 状态栏 |
| 页面缓存机制 | ✅ | `_pageFactories` + `_pageCache` |
| 主题切换 | ✅ | `AppThemeHelper` + `TextureBackgroundControl` |
| 语言切换 | ✅ | 壳层双语、页面缓存重建 |
| 系统消息通知 | ✅ | MessageBus → 状态栏 + AntdUI.Message |
| 用户头像菜单 | ✅ | 切换用户 / 修改密码占位 / 退出登录 |
| 报警抽屉 | ✅ | 主界面可直接打开活动报警内容 |

### 2.3 Auth / System 页面

| 页面 | 状态 | 说明 |
|------|------|------|
| `System.User` | ✅ | `UserManagementPage` 完整 CRUD |
| `System.Permission` | ✅ | `UserPermissionPage` 已实现用户选择、模块切换、保存与恢复默认 |
| `System.LoginLog` | ✅ | `LoginLogPage` 已实现筛选、分页、统计 |
| 登录窗体 | ✅ | `LoginForm` 已完成 |

### 2.4 Motion 页面

| 页面 | 状态 | 说明 |
|------|------|------|
| `Motion.DI` | ✅ 首版 | 监视页、分页、卡选择、详情区已实现 |
| `Motion.DO` | ✅ 首版 | 与 DI 同型实现 |
| `Motion.Monitor` | ✅ 首版 | 多轴总览、分页、详情区已实现 |
| `Motion.Axis` | ✅ 首版 | 单轴控制、动作卡片、参数动作、详情区已实现 |
| `Motion.Actuator` | ✅ 首版 | 类型筛选、动作区、详情区已实现 |

### 2.5 MotionConfig 页面

| 页面 | 状态 | 说明 |
|------|------|------|
| `MotionConfig.Card` | ✅ 首版 | 控制卡配置页 + 热重载 |
| `MotionConfig.Axis` | ✅ 首版 | 轴拓扑管理页 |
| `MotionConfig.IoMap` | ✅ 首版 | IO 映射管理页 |
| `MotionConfig.AxisParam` | ✅ 首版 | 轴参数配置页 |
| `MotionConfig.Actuator` | ✅ 首版 | 执行器配置页 |

### 2.6 PLC 模块

| 模块 / 页面 | 状态 | 说明 |
|------|------|------|
| PLC 模型与实体 | ✅ | `PlcConfig` / `PlcStationConfig` / `PlcPointConfig` 与 DB 实体已完成 |
| PLC CRUD | ✅ | 站与点位 CRUD 已完成 |
| PLC 配置应用服务 | ✅ | `PlcConfigAppService.ReloadFromDatabase()` 已完成 |
| PLC 协议插件机制 | ✅ | `ProtocolAssemblyRegistry` + `ProtocolPlcClient` + `NullPlcClient` |
| PLC 扫描与运行时查询 | ✅ | `PlcScanWorker` + `PlcRuntimeQueryService` |
| PLC 调试服务 | ✅ | `PlcOperationService` |
| `PLC.Status` | ✅ 首版 | 站状态页已实现 |
| `PLC.Monitor` | ✅ 首版 | 点位监视页已实现 |
| `PLC.Debug` | ✅ 首版 | 调试页三行结构已落地 |
| `SysConfig.Plc` | ✅ 首版 | 站 / 点位配置与重载页已实现 |

### 2.7 Alarm / Log 模块

| 页面 | 状态 | 说明 |
|------|------|------|
| `AlarmLog.Current` | ✅ | 当前报警导航页 + 公共内容控件 |
| `AlarmLog.History` | ✅ 首版 | 报警历史筛选、统计、分页 |
| `AlarmLog.RunLog` | ✅ 首版 | NLog 日志文件查询、筛选、分页 |

---

## 3. 已固定的实现模式

### 3.1 页面模式

项目中已形成统一页面模式：

```text
MainWindow
  └─ UserControl Page
       ├─ PageModel
       ├─ RuntimeQuery / Crud / Operation Service
       └─ RuntimeContext / ConfigContext / MachineContext
```

### 3.2 生命周期模式

- 页面缓存复用；
- 首次加载用 `_isFirstLoad`；
- 页面可见时启动定时刷新；
- 页面不可见时停止刷新；
- 页面只在 `Disposed` 时释放 `Timer`；
- PageModel 不在切页时销毁。

### 3.3 布局模式

| 页面类型 | 统一布局模式 |
|----------|--------------|
| 配置页 | 顶部工具栏 + 列表/卡片主体 + 对话框编辑 |
| 监视页 | 顶部筛选 + 统计卡片 + 列表区 + 详情区 |
| 调试页 | 顶部工具栏 + 左操作区 + 右结果区 |
| 日志页 | 顶部筛选 + 统计卡片 + `Table` + 分页 |

---

## 4. 当前剩余问题

| 问题 | 状态 | 说明 |
|------|------|------|
| 文档与代码曾存在脱节 | 已同步处理中 | 当前文档已按最新实现重写 |
| 多数核心页面仍属首版 | 已知 | 需要持续收口交互细节与字段完整性 |
| 修改密码仍为占位 | 已知 | 后续补独立轻量弹窗 |
| 首页与远期业务页尚未实现 | 已知 | 仍为占位页 |

---

## 5. 下一步优先级

### 5.1 第一优先级：已实现核心页收口

1. `PLC.Debug`
2. `PLC.Monitor`
3. `PLC.Status`
4. `SysConfig.Plc`
5. `Motion.Axis`
6. `Motion.Monitor`
7. `Motion.Actuator`
8. `AlarmLog.Current`
9. `AlarmLog.RunLog`

### 5.2 第二优先级：首页

10. `Home.Overview`
11. `Home.SysStatus`

### 5.3 第三优先级：远期模块

12. `Peripheral.*`
13. `Vision.*`
14. `Production.Recipe`
15. `Production.Data`
16. `Production.Report`
17. 其余 `SysConfig.*`

---

## 6. 已实现页面清单（可直接开发与测试）

```text
Assembly
└── Wiring

System
├── User
├── Permission
└── LoginLog

Motion
├── DI
├── DO
├── Monitor
├── Axis
└── Actuator

MotionConfig
├── Card
├── Axis
├── IoMap
├── AxisParam
└── Actuator

PLC
├── Status
├── Monitor
└── Debug

SysConfig
└── Plc

AlarmLog
├── Current
├── History
└── RunLog
```

---

## 相关文档

- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [WinForms 解决方案架构](../01-architecture/winf-solution-architecture.md)
- [统一 UI 规范与开发模板](../01-architecture/winf-ui-standards.md)
- [页面开发模板与实施基线](../02-development/winf-page-development-template.md)
- [PLC 协议库与 AM 上层分层架构](../01-architecture/plc-protocol-integration-design.md)
- [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)

