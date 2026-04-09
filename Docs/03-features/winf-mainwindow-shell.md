# AMControlWinF 主窗体壳层

**文档编号**：FEAT-W002  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 功能概述

`MainWindow` 是 AMControlWinF 的主窗体壳层，承载导航、页面工作区、用户菜单、主题/语言切换和系统消息通知。它不包含任何具体业务页面的逻辑。

---

## 2. 职责范围

| 职责 | 说明 |
|------|------|
| 一级/二级导航 | 构建 AntdUI Menu 菜单，响应选择事件 |
| 页面工作区 | 管理右侧 `panelContent` 中的页面实例缓存 |
| 用户头像菜单 | 协调切换用户 / 退出登录 / 修改密码 |
| 主题切换 | 明/暗模式一键切换 + 持久化 |
| 语言切换 | 中/英语言切换 + 壳层文本刷新 + 页面缓存重建 |
| 系统消息 | 订阅 `SystemContext.MessageBus`，更新状态栏 + 弹出通知 |
| 退出意图 | 设置 `ExitReason` 供 `Program.cs` 主循环读取 |

---

## 3. 布局结构

基于 AntdUI `GridPanel`，`Gap = 8`，卡片面板使用 `Shadow + Radius` 实现悬浮效果。

```
GridPanel (gridMainHost)
├── Row 1: 主内容区
│   ├── Col 1: panelLeftCard (一级导航 + 头像菜单)
│   │   ├── menuPrimary (AntdUI.Menu)
│   │   └── panelAvatarHost → userAvatarMenuControl
│   ├── Col 2: panelSecondaryNavCard (二级导航)
│   │   ├── panelSecondaryHeader (pageHeader + 标题)
│   │   └── menuSecondary (AntdUI.Menu)
│   └── Col 3: panelWorkCard (工作区)
│       └── panelContent (Dock.Fill, 页面容器)
└── Row 2: panelStatusCard (状态栏)
    ├── labelMotionStatus
    ├── labelPlcStatus
    └── labelStatusValue
```

### 3.1 卡片面板配置

所有卡片面板遵循统一模式：`BackColor = Transparent`，不设置 `Back`，启用 `Shadow` + `Radius`，由 AntdUI 原生主题渲染卡片背景。

详见 [主题系统设计](../01-architecture/winf-theme-system.md)。

---

## 4. 主题切换

### 4.1 触发方式

标题栏明/暗切换按钮（`buttonColorMode`），`Toggle` 属性表示当前状态。

### 4.2 处理流程

```csharp
private void ApplyTheme(bool isDarkMode, bool saveToConfig)
{
    _isDarkMode = isDarkMode;
    AppThemeHelper.Apply(this, isDarkMode);          // AntdUI 全局 + Window 基色
    textureBackgroundMain.SetTheme(isDarkMode);      // 自定义纹理背景
    labelStatusValue.ForeColor = GetStatusTextColor(_lastStatusMessageType); // 状态栏语义色
    buttonColorMode.Toggle = isDarkMode;
    ConfigContext.Instance.Config.Setting.Theme = isDarkMode ? "SkinDark" : "SkinDefault";
    if (saveToConfig) Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
}
```

---

## 5. 语言切换

### 5.1 触发方式

标题栏下拉菜单（`dropdownTranslate`），可选 `简体中文` / `English`。

### 5.2 处理流程

1. 设置 `ConfigContext.Instance.Config.Setting.Language`
2. 调用 `Localization.SetLanguage(language)` → AntdUI 内置国际化
3. 更新标题栏文本
4. `DisposeAllCachedPages()` → 清空所有页面缓存（页面文本需重建）
5. `RefreshShell()` → 重建导航菜单 + 头部 + 用户菜单 + 导航到当前页面
6. 持久化到 `config.json`

---

## 6. 系统消息

### 6.1 订阅

通过 `SystemContext.Instance.MessageBus` 订阅，构造时绑定，关闭时取消订阅。

### 6.2 状态栏更新

消息显示格式：`[时间] [来源] 消息内容 [错误码]`

语义色规则：

| 消息类型 | 状态栏颜色 |
|----------|-----------|
| `Warning` | 橙色 (230, 145, 56) |
| `Error` / `Alarm` | 红色 (220, 84, 84) |
| 其他 | 暗色主题灰白 (228, 228, 228) / 亮色主题深灰 (90, 90, 90) |

### 6.3 弹出通知

使用 `AntdUI.Message.open()` 在左下角（`TAlignFrom.BL`）显示：

| 消息类型 | 通知类型 | 自动关闭秒数 |
|----------|---------|-------------|
| `Error` / `Alarm` | `TType.Error` | 6s |
| `Warning` | `TType.Warn` | 5s |
| 其他 | `TType.Info` | 3s |

---

## 7. 用户菜单交互

### 7.1 切换用户

1. 弹出模态 `LoginForm`（`ShowDialog(this)`）
2. 登录成功 → `ExitReason = SwitchUser` → `Close()`
3. 登录取消 → 当前窗体保持不变

### 7.2 退出登录

1. `ExitReason = Logout` → `Close()`
2. `Program.cs` 主循环执行 `SignOut()` + 独立 `LoginForm`

### 7.3 修改密码

当前为占位 `MessageBox`，后续接入密码修改弹窗。

---

## 8. 窗体生命周期

| 事件 | 处理 |
|------|------|
| 构造 | 初始化控件 → 绑定事件 → 初始化壳层状态 → 加载模型 |
| FormClosed | 取消消息订阅 → 释放全部缓存页面 |

`ExitReason` 默认为 `Exit`（= 0），只有切换用户和退出登录会显式设置。

---

## 相关文档

- [应用生命周期](../01-architecture/winf-application-lifecycle.md)
- [主题系统设计](../01-architecture/winf-theme-system.md)
- [导航系统设计](../01-architecture/winf-navigation-system.md)
- [用户认证与登录](winf-auth-login.md)
