# AMControlWinF 应用生命周期与登录流程

**文档编号**：ARCH-W001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 设计目标

- 全局设备上下文 / 系统上下文 / 后台扫描只初始化一次，整个进程生命周期内不重复创建；
- `Application.Run(window)` 同步阻塞，同一时刻最多存在一个 `MainWindow`；
- 旧 `MainWindow` 的 `using` 块确保资源释放后才创建新实例；
- 切换用户 / 退出登录 / 正常关闭三条路径清晰、无竞态。

---

## 2. 生命周期流程

```
Program.Main()
  │
  ├─ Application.EnableVisualStyles()
  ├─ Application.SetCompatibleTextRenderingDefault(false)
  ├─ AppBootstrap.Initialize()          ← 全局唯一，设备/系统/扫描初始化
  ├─ SyncPagePermissions()              ← 同步页面权限到数据库（失败不阻断）
  │
  ├─ ShowLogin(null)                    ← 首次登录（模态，无 owner）
  │   └─ 取消 → return（退出进程）
  │
  └─ while (true)                       ← 主循环
      │
      ├─ using (var window = new MainWindow())
      │   ├─ Application.Run(window)    ← 阻塞，直到窗体关闭
      │   └─ exitReason = window.ExitReason
      │
      └─ switch (exitReason)
          ├─ SwitchUser  → continue     ← 直接创建新 MainWindow
          ├─ Logout      → SignOut + ShowLogin → continue 或 return
          └─ Exit        → return       ← 退出进程
```

---

## 3. MainWindowExitReason 枚举

定义在 `AM.PageModel.Main.MainWindowExitReason`：

| 值 | 名称 | 含义 | Program.cs 行为 |
|----|------|------|----------------|
| 0 | `Exit` | 正常关闭（关闭按钮 / Alt+F4） | 退出进程 |
| 1 | `SwitchUser` | 切换用户 | 已在 MainWindow 内以模态完成认证，直接下一轮创建新 MainWindow |
| 2 | `Logout` | 退出登录 | 清除登录态，重新弹出独立 LoginForm |

---

## 4. 三条退出路径详解

### 4.1 正常关闭

```
用户点击关闭按钮 / Alt+F4
  → MainWindow.ExitReason 保持默认值 Exit（= 0）
  → Application.Run 返回
  → switch 匹配 default → return
  → 进程退出
```

### 4.2 切换用户

```
用户点击头像菜单 → "切换用户"
  → UserAvatarPopoverCard.ClosePopoverThenNotify()
    → 关闭弹层（Visible=false + Dispose）
    → BeginInvoke 通知 MainWindow
  → MainWindow.UserAvatarMenuControl_SwitchUserRequested()
    → 在当前窗体上弹出模态 LoginForm
    → 登录成功 → ExitReason = SwitchUser → Close()
    → 登录取消 → 保持当前主窗体不变
  → Application.Run 返回
  → switch 匹配 SwitchUser → continue（创建新 MainWindow）
```

### 4.3 退出登录

```
用户点击头像菜单 → "退出登录"
  → UserAvatarPopoverCard.ClosePopoverThenNotify()
  → MainWindow.UserAvatarMenuControl_LogoutRequested()
    → ExitReason = Logout → Close()
  → Application.Run 返回
  → switch 匹配 Logout
    → UserContext.Instance.SignOut()
    → ShowLogin(null) — 独立登录窗（无 owner）
      → 登录成功 → continue（创建新 MainWindow）
      → 取消 → return（退出进程）
```

---

## 5. 关键设计决策

### 5.1 为什么不使用 ApplicationContext

早期版本使用 `MainApplicationContext` 内部类管理 `PendingAction` 状态机 + `RunAuthFlow` + `CloseCurrentMainWindow` 等方法，存在以下问题：

| 问题 | 说明 |
|------|------|
| 事件链过深 | Avatar Click → Popover → BeginInvoke → MenuControl → MainWindow → AppContext → RunAuthFlow(BeginInvoke) → ShowLogin → CloseCurrentMainWindow |
| 已释放对象引用 | PopoverCard 在 `Dispose()` 后仍通过 `handler(this, ...)` 引用自身 |
| `MainForm = null` 间隙 | 设置 `null` 和 `BeginInvoke(Close)` 之间存在窗口间隙 |
| 竞态条件 | `_isAuthTransitioning` 标记在 `BeginInvoke` 间隙中可被重入 |

**解决方案**：移除全部 `MainApplicationContext`，改为 `Program.Main()` 中的简单 `while(true)` 循环。`Application.Run(window)` 天然阻塞，`using` 块天然保证释放顺序。

### 5.2 为什么切换用户使用模态对话框

- `LoginForm.ShowDialog(this)` 是同步调用，无需异步状态机；
- 登录成功后设置 `ExitReason` + `Close()` 即可；
- 登录取消时当前主窗体保持不变，用户体验最佳。

### 5.3 PopoverCard 安全关闭模式

```csharp
// 在 Dispose 之前获取宿主窗体引用
var ownerForm = FindForm();

// 隐藏 + 释放
Visible = false;
Dispose();

// 延迟通知：sender 传 null（不引用已释放的 this）
if (handler != null && ownerForm != null && !ownerForm.IsDisposed && ownerForm.IsHandleCreated)
{
    ownerForm.BeginInvoke(new Action(() => handler(null, EventArgs.Empty)));
}
```

---

## 6. 全局初始化保障

`AppBootstrap.Initialize()` 在 `Main()` 入口处只调用一次，包含：

- 设备上下文初始化
- 系统上下文初始化
- 后台扫描启动
- 数据库连接建立

无论 `MainWindow` 创建 / 销毁多少次，全局上下文保持不变。

---

## 相关文档

- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
- [用户认证与登录](../03-features/winf-auth-login.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
