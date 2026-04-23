# AMControlWinF 页面开发模板与实施基线

**文档编号**：DEV-W002  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 文档目标

本文档提供 `AMControlWinF` 页面开发的当前标准模板，帮助当前 WinForms 分支的后续页面在现有代码风格上持续扩展。

本文档的作用边界如下：

1. 适用于当前 `AMControlWinF` 分支内新增或收口页面；
2. 用于减少当前 WinForms 分支内部的重复实现和无序分化；
3. 不用于否定未来新的前端壳层、跨平台 UI 或大规模页面组织重构；
4. 若后续出现新的 UI 主线，应为新主线建立自己的开发模板与实施基线。

适用于：

- 新增 `UserControl` 页面；
- 已有页面的结构收口；
- 新页面对应的 `PageModel`、`Service`、文档和测试同步实施。

---

## 2. 当前 WinForms 标准实施顺序

### 2.1 页面开发七步法

1. 在 `NavigationCatalog` 中定义页面；
2. 在 `MainWindow.CreatePageFactories()` 中注册工厂；
3. 创建 `PageModel`；
4. 创建 `UserControl` 页面与必要子控件；
5. 复用或补充 `Service`；
6. 接入文档与操作手册；
7. 回归验证页面导航、权限、刷新和消息行为。

---

## 3. 当前 WinForms 目录放置规范

### 3.1 页面文件

```text
AMControlWinF/Views/<模块>/
├── XxxPage.cs
├── XxxPage.Designer.cs
└── 必要的子控件 / 对话框
```

### 3.2 页面模型

```text
AM.PageModel/<模块>/
└── XxxPageModel.cs
```

### 3.3 服务位置

| 场景 | 位置 |
|------|------|
| CRUD | `AM.DBService/Services/<模块>/Config` |
| 应用服务 / 重载服务 | `AM.DBService/Services/<模块>/App` |
| 运行时查询 | `AM.DBService/Services/<模块>/Runtime` |
| 调试读写 | `AM.DBService/Services/<模块>/Runtime` 或对应功能目录 |

---

## 4. 页面代码骨架

## 4.1 `UserControl` 页面骨架

```csharp
public partial class XxxPage : UserControl
{
    private readonly XxxPageModel _model;
    private readonly Timer _refreshTimer;

    private bool _isFirstLoad;
    private bool _isBusy;
    private bool _isBindingView;

    public XxxPage()
    {
        InitializeComponent();

        _model = new XxxPageModel();
        _refreshTimer = new Timer();
        _refreshTimer.Interval = 500;

        BindEvents();

        Disposed += (s, e) =>
        {
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
        };
    }

    private void BindEvents()
    {
        Load += XxxPage_Load;
        VisibleChanged += XxxPage_VisibleChanged;
        _refreshTimer.Tick += RefreshTimer_Tick;
    }

    private async void XxxPage_Load(object sender, EventArgs e)
    {
        if (_isFirstLoad)
            return;

        _isFirstLoad = true;
        await InitializePageAsync();
    }
}
```

### 4.2 `PageModel` 骨架

```csharp
public class XxxPageModel : BindableBase
{
    public async Task<Result> LoadAsync()
    {
        return await RefreshAsync();
    }

    public async Task<Result> RefreshAsync()
    {
        return await Task.Run(() =>
        {
            // 查询 / 筛选 / 组装显示结果
            return Result.Ok("加载成功");
        });
    }
}
```

---

## 5. 典型页面模板

### 5.1 配置管理页模板

适用于：`MotionConfig.*`、`SysConfig.Plc`

#### 控件结构

```text
顶部：搜索 / 刷新 / 新增 / 重载
中部：表格或卡片列表
底部：分页
弹窗：新增 / 编辑
```

#### 数据来源

- CRUD Service
- AppService / ReloadService
- `ConfigContext` / `MachineContext`

#### 交互动作

- 查询
- 新增 / 编辑 / 删除
- 保存后重载运行配置

#### DoD

- 保存后页面刷新正确
- 配置重载成功
- 下游页面能看到更新结果

### 5.2 运行监视页模板

适用于：`DIMotionPage`、`MotionMonitorPage`、`PlcStatusPage`

#### 控件结构

```text
顶部：筛选与刷新按钮
第二行：统计卡片
主体：列表区 + 详情区
底部：分页（按需）
```

#### 数据来源

- RuntimeQueryService
- `RuntimeContext`
- 选择弹窗（按需）

#### 交互动作

- 搜索
- 选择卡 / 选择 PLC
- 分页
- 选中项切换
- 低频自动刷新

#### DoD

- 选中项稳定
- 搜索期间不失焦
- 页面不可见时停止刷新

### 5.3 调试控制页模板

适用于：`MotionAxisPage`、`PlcDebugPage`

#### 控件结构

```text
顶部：上下文选择 / 刷新 / 状态摘要
中部：留白或提示区
底部：左操作区 + 右结果区
```

#### 数据来源

- OperationService
- RuntimeQueryService
- 运行时摘要

#### 交互动作

- 读 / 写 / 测试动作
- 参数输入与确认
- 历史结果查看

#### DoD

- 所有动作统一写日志
- 危险写入必须确认
- 执行期间禁用重复提交

### 5.4 日志页模板

适用于：`LoginLogPage`、`AlarmHistoryPage`、`RunLogPage`

#### 控件结构

```text
顶部：搜索 / 日期 / 快捷筛选 / 查询
第二行：统计卡片
第三行：Table
底部：分页
```

#### 数据来源

- QueryService / PageModel
- NLog 文件 / 登录日志表 / 报警历史表

#### DoD

- 分页与页大小切换稳定
- 快捷筛选按钮状态与结果一致
- 表头和统计卡片字段统一

---

## 6. 当前 WinForms 页面必须同步的代码点

每新增一个页面，必须同步检查：

| 位置 | 必做项 |
|------|--------|
| `NavigationCatalog` | 新增 `NavPageDef` |
| `MainWindow.CreatePageFactories()` | 注册页面工厂 |
| `MainWindow.CreateSecondaryIconMap()` | 注册图标 |
| `MainWindowModel` | 默认无需改动，但需验证权限过滤结果 |
| `Docs` | 更新项目总览、进展、手册或模块文档 |

---

## 7. 参数与调用示例模板

### 7.1 运行时查询示例

```csharp
var result = await _model.LoadAsync();
if (!result.Success)
    return;

RefreshView();
```

### 7.2 执行动作示例

```csharp
private async Task ExecuteAsync(Func<Result> action)
{
    if (_isBusy || action == null)
        return;

    _isBusy = true;
    try
    {
        Result result = await Task.Run(action);
        RefreshView();

        if (!result.Success)
            return;
    }
    finally
    {
        _isBusy = false;
    }
}
```

### 7.3 配置重载示例

```csharp
var reloadService = new PlcConfigAppService();
var reloadResult = reloadService.ReloadFromDatabase();
```

---

## 8. 页面交付检查清单

### 8.1 开发完成前检查

- [ ] 页面已注册到导航与工厂
- [ ] 页面权限正确
- [ ] `_isFirstLoad` / `VisibleChanged` / `Disposed` 已处理
- [ ] PageModel 不直接访问控件
- [ ] Service 不直接依赖 UI
- [ ] 成功 / 失败提示未重复弹窗
- [ ] 文档已同步

### 8.2 提交前检查

- [ ] 页面文案、按钮、空态一致
- [ ] 对应日志与消息行为正常
- [ ] 危险操作确认完整
- [ ] 相关页面联动已验证

---

## 9. 推荐页面落地顺序

对于新的业务模块，建议按照以下顺序：

1. 配置页
2. 运行监视页
3. 调试页
4. 日志 / 历史页
5. 首页摘要入口

原因：

- 先有配置，后有运行；
- 先有监视，后有调试；
- 先有底层链路，后做首页聚合。

---

## 10. 当前结论

`AMControlWinF` 已具备一套成熟的页面开发模板。对于当前 WinForms 分支内的新页面，默认优先复用现有模式；若后续仓库新增新的 UI 主线或重构页面组织方式，应新增对应模板文档，而不是继续修改本页去覆盖不同实现路线。

---

## 相关文档

- [WinForms 项目总览](winf-project-overview.md)
- [WinForms 解决方案架构](../01-architecture/winf-solution-architecture.md)
- [统一 UI 规范与开发模板](../01-architecture/winf-ui-standards.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)