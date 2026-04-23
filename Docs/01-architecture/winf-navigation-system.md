# AMControlWinF 导航系统与页面缓存

**文档编号**：ARCH-W003  
**版本**：2.0.0  
**状态**：有效  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 设计目标

当前 WinForms 分支的导航系统当前采用以下目标与做法：

- 左侧两级导航固定可见；
- 导航目录与页面权限共用同一数据源；
- 页面按 `PageKey` 工厂创建并缓存复用；
- 页面切换时不销毁 PageModel / ViewModel；
- 语言切换后统一释放缓存并重建页面；
- 壳层只负责导航与承载，不在 `MainWindow` 中写业务逻辑。

---

## 2. 导航数据源

### 2.1 当前主导航来源

在当前 WinForms 主线中，`AM.PageModel.Navigation.NavigationCatalog` 是导航菜单与权限目录的主来源，提供：

- `All`：全部页面定义；
- `GetPrimaryItems()`：一级导航；
- `GetSecondaryItems(moduleKey)`：二级页面；
- `ToPermissionEntities()`：页面权限同步实体集合。

### 2.2 页面定义字段

每个 `NavPageDef` 包含以下信息：

| 字段 | 说明 |
|------|------|
| `ModuleKey` | 一级模块键，如 `Motion`、`PLC` |
| `ModuleName` | 一级模块显示名 |
| `PageKey` | 二级页面唯一键，如 `Motion.DI` |
| `DisplayName` | 页面显示名 |
| `Description` | 页面功能说明 |
| `DefaultRoleCodes` | 默认允许访问角色 |
| `RiskLevel` | 页面风险等级 |
| `SortOrder` | 排序号 |

---

## 3. 导航过滤链路

### 3.1 权限同步

启动时 `Program.SyncPagePermissions()` 调用：

```text
NavigationCatalog.ToPermissionEntities()
  → AuthSeedService.SyncPagePermissions(...)
  → 写入 SysPagePermission 表
```

### 3.2 运行时过滤

`MainWindowModel.LoadNavigation()` 的过滤规则：

```text
NavigationCatalog.GetPrimaryItems()
  → CanAccessPrimary(primary)
      → NavigationCatalog.GetSecondaryItems(primary.Key)
      → Any(CanAccessPage)
  → UserContext.Instance.HasPagePermission(page.PageKey)
```

即：

- 用户只会看到自己至少有一个可访问子页面的一级模块；
- 一级模块切换后，二级页面再次按页面权限过滤；
- `SelectedSecondary` 默认取当前模块下第一个可访问页面。

---

## 4. 当前导航模块分布

按 `NavigationCatalog`，当前定义的一级模块如下：

| 一级模块 | 页面数 | 说明 |
|----------|--------|------|
| `Home` | 2 | 总览与系统状态 |
| `Assembly` | 1 | 装配接线与现场调试 |
| `Motion` | 5 | DI / DO / 多轴总览 / 轴控制 / 执行器控制 |
| `Production` | 3 | 设备本地配方、生产统计、班次报表 |
| `Vision` | 3 | 相机监视、检测结果、标定 |
| `PLC` | 3 | 通讯状态、点位监视、调试工具 |
| `Peripheral` | 4 | 扫码、传感器监视与趋势 |
| `MotionConfig` | 5 | 控制卡、轴、IO、参数、执行器配置 |
| `SysConfig` | 6 | 相机、PLC、传感器、扫码器、MES、运行配置 |
| `AlarmLog` | 3 | 当前报警、报警历史、运行日志 |
| `System` | 3 | 用户、权限、登录日志 |

### 4.1 当前已实现并接入工厂的页面

`MainWindow.CreatePageFactories()` 当前已注册以下真实页面：

```text
Assembly
└── Assembly.Wiring

Motion
├── Motion.DI
├── Motion.DO
├── Motion.Monitor
├── Motion.Axis
└── Motion.Actuator

MotionConfig
├── MotionConfig.Card
├── MotionConfig.Axis
├── MotionConfig.IoMap
├── MotionConfig.AxisParam
└── MotionConfig.Actuator

PLC
├── PLC.Status
├── PLC.Monitor
└── PLC.Debug

SysConfig
└── SysConfig.Plc

AlarmLog
├── AlarmLog.Current
├── AlarmLog.History
└── AlarmLog.RunLog

System
├── System.User
├── System.Permission
└── System.LoginLog
```

### 4.2 当前仍使用占位页的模块

- `Home.*`
- `Production.*`
- `Vision.*`
- `Peripheral.*`
- `SysConfig.Camera / Sensor / Scanner / Mes / Runtime`

当前壳层中的占位页默认由 `CreatePlaceholderPage(...)` 生成。

---

## 5. 主窗体布局关系

当前 WinForms 壳层采用如下布局关系：

```text
┌─────────────────────────────────────────────────────────────┐
│ Titlebar：应用标题 / 语言 / 主题 / 用户头像                │
├────────────┬────────────┬──────────────────────────────────┤
│ 一级导航    │ 二级导航    │ 工作区 panelContent              │
│ menuPrimary │ menuSecondary│ 页面缓存实例显示区              │
├────────────┴────────────┴──────────────────────────────────┤
│ 状态栏：Motion / PLC / 状态消息 / 报警按钮                 │
└─────────────────────────────────────────────────────────────┘
```

布局实现要点：

- 当前实现中，一级导航与二级导航分列显示；
- 工作区默认只承载当前二级页面；
- 状态栏默认驻留底部；
- 报警抽屉当前从主界面右侧打开，不改变导航层级。

---

## 6. 页面工厂与缓存机制

### 6.1 核心字段

`MainWindow` 使用两个字典维护页面：

```csharp
private readonly Dictionary<string, Control> _pageCache;
private readonly Dictionary<string, Func<Control>> _pageFactories;
```

- `_pageFactories`：`PageKey -> 页面构造函数`
- `_pageCache`：`PageKey -> 已创建实例`

### 6.2 页面创建流程

```text
NavigateToSelectedPage()
  → GetOrCreatePage(page.PageKey)
      → 命中缓存：直接返回
      → 未命中：CreatePage(pageKey) → 工厂创建 → 加入缓存
  → ShowPage(page)
      → 从 panelContent 移除旧控件
      → 添加新页面 → Dock.Fill → BringToFront
```

### 6.3 页面销毁时机

| 时机 | 方法 | 说明 |
|------|------|------|
| 切换页面 | `ShowPage(page, false)` | 旧页面仅移出容器，不销毁 |
| 语言切换 | `DisposeAllCachedPages()` | 清空缓存，重新创建页面以刷新文本 |
| 窗体关闭 | `DisposeAllCachedPages()` | 释放全部缓存页面 |
| 显示占位页 | `ShowPage(page, true)` | 占位页是临时页面，可立即销毁旧临时控件 |

---

## 7. 页面生命周期约束

由于当前页面被缓存复用，当前 WinForms 业务页默认遵循以下规则：

### 7.1 首次加载规则

- 页面通过 `_isFirstLoad` 控制一次性初始化；
- 不依赖 `Load` 事件每次都执行重载；
- 页面初始化逻辑集中到 `InitializePageAsync()` 或 `ReloadAsync(true)`。

### 7.2 定时刷新规则

- 页面可见时启动刷新；
- 页面不可见时停止刷新；
- 页面离开时不释放模型；
- 页面关闭时释放 `Timer`；
- 输入搜索 / 筛选时，必要时跳过本轮刷新，避免失焦和闪烁。

### 7.3 页面职责边界

| 层级 | 允许职责 | 禁止职责 |
|------|----------|----------|
| `MainWindow` | 导航、缓存、状态栏、消息提示 | 业务查询、业务规则、设备操作 |
| 页面 `UserControl` | 事件接线、首次加载、视图绑定、低频刷新 | 直接计算筛选/分页、直接访问底层协议 |
| `PageModel` | 查询、筛选、分页、选中项、权限态、动作调用 | 直接访问控件 |
| `Service` | 配置、运行态、读写、重载 | 直接依赖 WinForms 控件 |

---

## 8. 国际化与导航文本

当前导航支持中英双语：

- 一级导航中文取 `DisplayName`，部分模块做多行显示；
- 英文直接使用 `ModuleKey` 或 `PageKey` 最后一段；
- 二级页面图标由 `CreateSecondaryIconMap()` 按 `PageKey` 映射。

语言切换流程：

```text
ApplyLanguage(language)
  → 更新 ConfigContext.Setting.Language
  → AntdUI 本地化切换
  → DisposeAllCachedPages()
  → RefreshShell()
```

---

## 9. 当前实现结论

当前 WinForms 导航与缓存架构已经较稳定。对当前 WinForms 分支内的页面开发，默认优先沿用现有导航机制，只需：

1. 在 `NavigationCatalog` 补页面定义；
2. 在 `MainWindow.CreatePageFactories()` 注册新页面；
3. 实现 `UserControl + PageModel`；
4. 遵守缓存页生命周期规则。

若后续仓库新增新的 UI 主线、导航模型或跨平台宿主，应在对应文档中定义新的导航方案，而不是把本页当作所有未来实现的永久约束。

---

## 相关文档

- [WinForms 解决方案架构](winf-solution-architecture.md)
- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [统一 UI 规范与开发模板](winf-ui-standards.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
