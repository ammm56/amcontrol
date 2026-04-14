# AMControlWinF 统一 UI 规范与开发模板

**文档编号**：ARCH-W004  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 文档目标

本文档用于沉淀当前 `AMControlWinF` 已落地页面的统一 UI 规范，作为后续新页面与旧页面收口的共同基线。

目标不是发明新的样式体系，而是从当前实现中提炼出**可复用的、与现有代码一致的规范**。

---

## 2. 统一视觉原则

### 2.1 总体风格

- 使用 `AntdUI` 原生卡片风格；
- 亮 / 暗主题优先走 `AppThemeHelper` 与 AntdUI 自带渲染；
- 不手动为每个控件写大量颜色逻辑；
- 卡片面板统一圆角、阴影、小间距；
- 重点信息通过统计卡片、状态色、图标表达，不堆砌文字。

### 2.2 卡片规范

统一原则：

- 使用 `AntdUI.Panel` 作为主要容器；
- `BackColor = Transparent`；
- 由 AntdUI 负责卡片背景与主题渲染；
- 常用 `Radius = 12`；
- 常用 `Shadow = 4`；
- 卡片内部统一 `Padding = 8~12`。

### 2.3 间距规范

| 类型 | 推荐值 |
|------|--------|
| 页面外边距 | `8` |
| 区域间距 | `8` |
| 卡片内边距 | `12` |
| 控件之间纵向间距 | `6~8` |
| 表格 / 列表上方标题区高度 | `36~44` |

---

## 3. 页面职责边界

### 3.1 页面 `UserControl`

负责：

- 页面生命周期；
- 事件接线；
- 调用 PageModel；
- 低频刷新；
- 把 PageModel 结果绑定到控件。

不负责：

- 业务规则计算；
- 数据库直接操作；
- 协议层调用；
- 复杂筛选、分页、选中逻辑。

### 3.2 `PageModel`

负责：

- 数据加载；
- 运行态刷新；
- 关键字搜索、筛选、分页；
- 当前选中项维护；
- 动作权限态；
- 调用 Service 执行业务动作。

不负责：

- 控件引用；
- WinForms 布局；
- 直接弹窗。

### 3.3 `Service`

负责：

- CRUD；
- 运行时查询；
- 调试读写；
- 配置重载；
- 扫描控制；
- 日志 / 消息 / 报警统一输出。

---

## 4. 生命周期规范

### 4.1 缓存页规则

所有被 `MainWindow` 缓存的页面必须遵守：

- 使用 `_isFirstLoad` 控制首次初始化；
- 页面切走时不释放模型；
- 页面可见时才启动定时刷新；
- 页面不可见时停止定时刷新；
- 只在 `Disposed` 时释放 `Timer`。

### 4.2 推荐写法

```text
构造
  → InitializeComponent()
  → new PageModel()
  → BindEvents()

Load
  → if (_isFirstLoad) return;
  → _isFirstLoad = true;
  → await InitializePageAsync();

VisibleChanged
  → UpdateRefreshTimerState();

Disposed
  → Stop + Dispose Timer
```

### 4.3 刷新节奏规范

| 页面类型 | 推荐刷新周期 |
|----------|--------------|
| DI / DO / PLC.Status | `200ms` 左右 |
| Motion.Monitor / Motion.Axis | `100ms~200ms` |
| PLC.Monitor / Motion.Actuator | `500ms` 左右 |
| 日志 / 配置页 | 不做定时刷新或仅可见时弱刷新 |

说明：

- 高频刷新只允许针对运行监视页；
- 搜索输入、筛选切换时应避免刷新打断用户操作；
- 运行时变化统一进入 `RuntimeContext`，UI 自行低频拉取。

---

## 5. 页面布局模板

### 5.1 配置管理页模板

适用：`MotionConfig.*`、`SysConfig.Plc`

```text
第一行：工具栏（搜索 / 刷新 / 新增 / 重载）
第二行：主体（表格 / 卡片 / 双列表联动）
第三行：分页或底部操作栏
```

要求：

- 新增 / 编辑使用独立对话框；
- 配置修改后触发 reload / rebuild；
- 页面不直接保留脏状态过久；
- 成功提示走消息总线。

### 5.2 运行监视页模板

适用：`Motion.DI`、`Motion.DO`、`Motion.Monitor`、`PLC.Status`

```text
第一行：筛选栏（搜索 / 选择卡 / 选择 PLC / 刷新按钮）
第二行：统计卡片
第三行：左侧列表 / 卡片区 + 右侧详情区
第四行：分页（如需要）
```

要求：

- 左侧列表与右侧详情明确分工；
- 选中项刷新后尽量保持；
- 刷新不打断输入；
- 统计卡片只放关键数字。

### 5.3 调试页模板

适用：`Motion.Axis`、`PLC.Debug`

```text
第一行：工具栏 / 当前上下文
第二行：必要时留白或摘要区
第三行：左操作区 + 右结果区 / 详情区
```

要求：

- 左侧只放动作与输入；
- 右侧只放结果或实时详情；
- 高风险写入必须显式确认；
- 所有动作均通过统一服务执行并写日志。

### 5.4 日志审计页模板

适用：`LoginLogPage`、`AlarmHistoryPage`、`RunLogPage`

```text
第一行：搜索 / 时间 / 等级 / 查询按钮
第二行：统计卡片
第三行：整页 Table
第四行：分页
```

要求：

- 统一 `AntdUI.Table` 风格；
- 页大小切换、页码切换行为一致；
- 快捷筛选按钮高亮样式一致。

---

## 6. 交互规范

### 6.1 消息与弹窗

- 处理成功 / 失败优先通过消息总线统一展示；
- 页面不重复弹出“操作成功”；
- 只保留删除确认、危险写入确认、缺少前置条件提示等必要弹窗；
- 警告类优先用 `PageDialogHelper.ShowWarn(...)` 或 `AntdUI.Modal`。

### 6.2 权限规范

| 页面类别 | 默认访问策略 |
|----------|--------------|
| 监视页 | `Operator / Engineer / Am` |
| 控制页 | `Engineer / Am` |
| 配置页 | `Engineer / Am` |
| 用户与权限页 | `Am` |

页面按钮还应在自身内部再做一次权限态控制，不能只依赖导航是否可见。

### 6.3 搜索与筛选规范

- 搜索框文本变化时仅更新 PageModel 条件并刷新视图；
- 不在搜索框 `TextChanged` 中直接做重型 IO；
- 下拉绑定期间使用 `_isBindingView` / `_isBindingFilters` 防止重复触发；
- 当前选中项被筛掉时，允许退回首项或空选中，但行为应稳定可预测。

---

## 7. 已实现页面模板映射

| 页面 | 模板类型 | 当前特征 |
|------|----------|----------|
| `UserManagementPage` | 配置 / 管理页 | 搜索 + 统计卡 + Table + 弹窗 CRUD |
| `UserPermissionPage` | 权限页 | 顶部用户操作栏 + 模块按钮 + 权限卡片 |
| `LoginLogPage` | 日志审计页 | 顶部筛选 + 统计卡 + Table + 分页 |
| `DIMotionPage` / `DOMotionPage` | 运行监视页 | 选择卡 + 搜索 + 统计 + 列表 + 详情 + 分页 |
| `MotionMonitorPage` | 运行监视页 | 选择卡 + 搜索 + 统计 + 轴列表 + 详情 + 分页 |
| `MotionAxisPage` | 调试控制页 | 左动作卡片 + 右实时详情 |
| `MotionActuatorPage` | 调试控制页 | 类型筛选 + 列表 + 动作面板 + 详情 |
| `PlcStatusPage` | 运行监视页 | 搜索 + 扫描控制 + 统计 + 站列表 + 详情 |
| `PlcMonitorPage` | 运行监视页 | PLC / 分组筛选 + 搜索 + 统计 + 点位卡片列表 |
| `PlcDebugPage` | 调试页 | 顶部工具栏 + 左调试卡片 + 右执行记录 |
| `PlcConfigManagementPage` | 配置管理页 | 站表 + 点位表 + 扫描控制 + 编辑对话框 |
| `AlarmHistoryPage` / `RunLogPage` | 日志审计页 | 快捷筛选 + 统计卡 + `Table` + 分页 |

---

## 8. 页面完成定义 DoD

所有新页面至少满足以下 DoD：

1. 已在 `NavigationCatalog` 注册；
2. 已在 `MainWindow.CreatePageFactories()` 注册；
3. 具备 `_isFirstLoad` 与可见性刷新控制；
4. PageModel 不直接访问控件；
5. 页面不直接操作 DB / 协议对象；
6. 成功 / 失败消息统一；
7. 文本、按钮、空态、分页与现有页面风格一致；
8. 对应文档与操作手册同步更新。

---

## 9. 不再建议的做法

- 不再新增壳层级事件状态机；
- 不再为页面引入额外主题接口；
- 不再让 UI 直接订阅高频后台事件后整页重绘；
- 不再在页面中维护私有配置副本；
- 不再重复实现新的页面生命周期方案。

---

## 10. 当前结论

当前 WinForms 页面体系已经具备统一规范，后续开发应以**收口现有页面**和**按模板补齐新页面**为主，不做大规模 UI 结构重构。

---

## 相关文档

- [WinForms 解决方案架构](winf-solution-architecture.md)
- [导航系统与页面缓存](winf-navigation-system.md)
- [页面开发模板与实施基线](../02-development/winf-page-development-template.md)
- [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)