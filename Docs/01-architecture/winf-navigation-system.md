# AMControlWinF 导航系统与页面缓存

**文档编号**：ARCH-W003  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 设计目标

- 左侧两级固定可见导航：一级导航与二级导航同时显示，不使用垂直折叠展开；
- 右侧工作区显示当前二级页面的完整内容；
- 页面实例缓存复用，避免频繁创建销毁；
- 导航目录与权限目录共用同一数据源（`NavigationCatalog`）。

---

## 2. 导航架构

### 2.1 数据源

`NavigationCatalog`（位于 `AM.PageModel.Navigation`）是全局页面目录的唯一来源，同时服务于：

- **导航菜单构建**：`GetPrimaryItems()` / `GetSecondaryItems(moduleKey)`
- **权限同步**：`ToPermissionEntities()` → 同步到数据库 `SysPagePermission` 表

每条记录包含：

| 字段 | 说明 |
|------|------|
| `ModuleKey` | 一级模块键名（如 `Motion`、`System`） |
| `ModuleName` | 一级模块显示名（如 `设备`、`系统`） |
| `PageKey` | 二级页面唯一键（如 `Motion.DI`、`System.User`） |
| `DisplayName` | 二级页面显示名 |
| `Description` | 页面功能描述 |
| `AllowedRoles` | 允许访问的角色列表 |
| `RiskLevel` | 风险等级（低/中/高） |
| `SortOrder` | 排序序号 |

### 2.2 模块与页面分布

| 一级模块 | 二级页面数 | 说明 |
|----------|-----------|------|
| Home | 2 | 总览看板、系统状态 |
| Motion | 5 | DI/DO 监视、多轴总览、轴控制、执行器控制 |
| Production | 7 | 工单、配方、数据、报表、追溯、MES、上传 |
| Vision | 3 | 相机监视、检测结果、标定管理 |
| PLC | 4 | 点位监视、寄存器监视、通讯状态、写入调试 |
| Peripheral | 4 | 扫码监视/测试、传感器监视/趋势 |
| MotionConfig | 5 | 控制卡、轴拓扑、IO映射、轴参数、执行器 |
| SysConfig | 6 | 相机、PLC、传感器、扫码器、MES、运行配置 |
| Engineer | 4 | 设备诊断、原始轴/PLC/相机参数 |
| AlarmLog | 3 | 当前报警、报警历史、运行日志 |
| System | 3 | 用户管理、权限分配、登录日志 |

---

## 3. 主窗体布局

```
┌─────────────────────────────────────────────────────────┐
│  Titlebar (AM运动控制 | Version)   [语言] [主题] [头像] │
├──────┬──────────┬───────────────────────────────────────┤
│      │ 二级导航  │                                       │
│ 一级 │ (Menu)   │         工作区 (panelContent)          │
│ 导航 │          │                                       │
│(Menu)│          │                                       │
│      │          │                                       │
├──────┴──────────┴───────────────────────────────────────┤
│  状态栏 (panelStatusCard)                                │
└─────────────────────────────────────────────────────────┘
```

使用 AntdUI `GridPanel` 布局，`Span = "100% 230 86 ;100%-100% 40"`：

- 第一行高度 `100%-100%`（占满剩余空间，减去第二行高度）
- 第二行高度 `40`（状态栏固定高度）
- 第一列宽度 `100%`（占满）
- 但一级导航宽 `230px`，二级导航宽 `86px`，工作区自动填充

---

## 4. 页面缓存机制

### 4.1 缓存策略

```csharp
private readonly Dictionary<string, Control> _pageCache;
private readonly Dictionary<string, Func<Control>> _pageFactories;
```

- `_pageFactories`：`PageKey → Func<Control>` 工厂映射，在构造时一次性注册
- `_pageCache`：`PageKey → Control` 实例缓存，首次访问时创建并缓存

### 4.2 页面创建与显示

```
NavigateToSelectedPage()
  └─ GetOrCreatePage(pageKey)
      ├─ 缓存命中 → 返回已有实例
      └─ 缓存未命中 → _pageFactories[pageKey]() → 缓存 → 返回
  └─ ShowPage(page)
      ├─ panelContent.Controls 移除旧页面（不释放）
      └─ 添加新页面 → Dock.Fill → BringToFront
```

### 4.3 页面生命周期注意事项

由于被 MainWindow 页面缓存复用，页面有以下特殊要求：

| 规则 | 说明 |
|------|------|
| 不要在 `Unloaded` 中断开实时订阅 | 页面被切走时仅从 Controls 集合移除，未释放，后续还会回来 |
| 首次加载使用布尔标记控制 | 不要在 `Loaded` 后解绑事件来控制"只初始化一次" |
| 代码中需注释说明原因 | 避免后续页面重复出现相同问题 |

### 4.4 缓存清理时机

| 时机 | 方法 | 说明 |
|------|------|------|
| 语言切换 | `DisposeAllCachedPages()` | 所有页面文本需刷新，释放后重建 |
| 窗体关闭 | `MainWindow_FormClosed` | 释放全部缓存页面 |

---

## 5. 权限过滤

`MainWindowModel.LoadNavigation()` 从 `NavigationCatalog` 加载全部页面定义后，根据当前登录用户的角色与数据库权限记录进行过滤，只展示用户有权限访问的一级模块和二级页面。

---

## 6. 国际化支持

导航文本支持中/英双语：

- 一级导航：`GetPrimaryText(item)` — 按 `ModuleKey` 映射中/英文本
- 二级导航：`GetSecondaryText(item)` — 中文用 `DisplayName`，英文从 `PageKey` 提取最后一段
- 二级导航图标：`CreateSecondaryIconMap()` — `PageKey → AntdUI SVG Icon 名称`

语言切换时清空页面缓存并完整刷新壳层（`DisposeAllCachedPages()` + `RefreshShell()`）。

---

## 相关文档

- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
- [应用生命周期](winf-application-lifecycle.md)
- [主窗体壳层功能](../03-features/winf-mainwindow-shell.md)
