# AMControlWinF 用户认证与登录

**文档编号**：FEAT-W001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 功能概述

AMControlWinF 的用户认证模块提供登录、切换用户、退出登录和密码修改功能。认证信息通过 `UserContext` 全局管理，页面级权限从数据库读取。

---

## 2. 角色与权限体系

### 2.1 角色定义

| 角色编码 | 中文名 | 权限范围 |
|----------|--------|---------|
| `Operator` | 操作员 | 查看监视信息（DI/DO/Motion/PLC/Vision），限制写入操作 |
| `Engineer` | 工程师 | 操作员权限 + 动作参数修改、工程调试、配置管理 |
| `Am` | 管理员 | 全部权限，包含用户管理与权限分配 |

### 2.2 默认管理员

| 项 | 值 |
|----|----|
| 登录名 | `am` |
| 初始密码 | `am123` |

### 2.3 页面级权限

- 权限目录来源：`NavigationCatalog.ToPermissionEntities()` → 同步到数据库 `SysPagePermission` 表
- 启动时自动同步（`Program.SyncPagePermissions()`），失败不阻断主流程
- 权限分配在用户管理界面中由管理员设置
- 当前阶段只做到页面级权限

---

## 3. LoginForm 实现

### 3.1 文件结构

| 文件 | 职责 |
|------|------|
| `LoginForm.cs` | 运行时绑定、交互逻辑、主题同步 |
| `LoginForm.Designer.cs` | 静态布局、控件定义 |
| `LoginPageModel.cs` | 登录页面模型（MVVM，位于 `AM.PageModel.Auth`） |

### 3.2 界面特征

- 继承 `AntdUI.Window`
- 背景使用 `TextureBackgroundControl`（与 MainWindow 相同纹理风格）
- 登录卡片：`AntdUI.Panel` + `Shadow` + `Radius`，`BackColor = Transparent`，不设置 `Back`
- 支持根据 `ConfigContext` 主题配置自动切换明/暗色

### 3.3 数据绑定

```csharp
_bindingSource.DataSource = _model;
textBoxLoginName.DataBindings.Add("Text", _bindingSource, "LoginName", ...);
labelStatusValue.DataBindings.Add("Text", _bindingSource, "StatusText", ...);
labelErrorValue.DataBindings.Add("Text", _bindingSource, "ErrorMessage", ...);
```

密码字段由于安全考虑不绑定，通过 `textBoxPassword.Text` 直接读取。

### 3.4 主题同步

```csharp
private void ApplyThemeFromConfig()
{
    var isDarkMode = IsDarkTheme(theme);
    if (isDarkMode) AntdUI.Config.IsDark = true;
    else            AntdUI.Config.IsLight = true;
    textureBackgroundLogin.SetTheme(isDarkMode);
}
```

不逐控件手动改色，完全依赖 AntdUI 原生主题机制。

---

## 4. 登录流程

### 4.1 首次登录（程序启动）

```
Program.Main()
  └─ ShowLogin(null)           ← 无 owner 的独立模态窗体
      ├─ 登录成功 → DialogResult.OK + UserContext.IsLoggedIn = true
      └─ 取消 → return（退出进程）
```

### 4.2 切换用户（MainWindow 内）

```
用户头像菜单 → "切换用户"
  └─ MainWindow.UserAvatarMenuControl_SwitchUserRequested()
      └─ new LoginForm().ShowDialog(this)   ← 以当前窗体为 owner 的模态
          ├─ 登录成功 → ExitReason = SwitchUser → Close()
          └─ 取消 → 保持当前主窗体不变
```

### 4.3 退出登录后重新登录

```
MainWindow 关闭（ExitReason = Logout）
  → Program 主循环
    → UserContext.Instance.SignOut()
    → ShowLogin(null)
      ├─ 登录成功 → continue（新 MainWindow）
      └─ 取消 → return（退出进程）
```

---

## 5. UserAvatarMenuControl

位于 `AMControlWinF\Views\Main\UserAvatarMenuControl.cs`，嵌入 MainWindow 标题栏区域。

| 功能 | 说明 |
|------|------|
| 显示头像 + 用户名 | 从 `UserContext` 获取 |
| 点击弹出操作卡片 | 使用 AntdUI `Popover` 显示 `UserAvatarPopoverCard` |
| 转发事件 | `SwitchUserRequested` / `ChangePasswordRequested` / `LogoutRequested` |

### 5.1 UserAvatarPopoverCard 安全关闭

弹出卡片在用户点击操作后需要先关闭弹层再通知外部，避免在当前点击链路中直接触发模态对话框：

1. 设置 `_isClosing = true` 防止重复点击
2. 禁用所有按钮
3. `FindForm()` 获取宿主窗体引用（在 `Dispose` 之前）
4. `Visible = false` + `Dispose()` → 触发 AntdUI Popover 关闭
5. `ownerForm.BeginInvoke(() => handler(null, EventArgs.Empty))` → 延迟到下一轮消息通知

**关键**：`handler` 的 `sender` 传 `null` 而非 `this`，因为此时卡片已被 `Dispose`。

---

## 6. 修改密码

当前为占位实现（`MessageBox` 提示），后续接入密码修改弹窗。

设计要求：
- 修改密码作为所有用户可用的轻量弹窗操作
- 不占用右侧主工作区页面
- 仅管理员的用户管理进入一级/二级导航对应页面

---

## 相关文档

- [应用生命周期与登录流程](../01-architecture/winf-application-lifecycle.md)
- [主窗体壳层](winf-mainwindow-shell.md)
- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
