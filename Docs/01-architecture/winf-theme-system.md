# AMControlWinF 主题系统设计

**文档编号**：ARCH-W002  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 设计目标

当前 WinForms 分支的主题系统当前以以下目标为主：

- 明/暗主题一键切换，当前页面与控件尽量自动跟随；
- 当前 WinForms 主线默认不引入 per-page 主题接口（如 `IPageTheme`），减少页面实现负担；
- 卡片面板保持悬浮感（阴影 + 圆角 + 深度），与 LoginForm 风格协调；
- 在当前实现里尽量复用 AntdUI 原生主题机制，避免手动逐控件改色。

---

## 2. AntdUI 原生主题机制

AntdUI 框架提供两个全局开关：

```csharp
AntdUI.Config.IsDark  = true;   // 切换到暗色
AntdUI.Config.IsLight = true;   // 切换到亮色
```

设置后，所有原生 AntdUI 控件（`Button`、`Menu`、`Label`、`Input` 等）自动重绘为对应主题色。

### 2.1 AntdUI.Panel 的 Shadow 渲染规则（关键）

| 条件 | 渲染行为 |
|------|----------|
| `Shadow > 0` 且 **未设置** `Back` 属性 | AntdUI 根据 `Config.IsDark/IsLight` 自动绘制主题感知的卡片背景（暗色稍亮、亮色白色）+ 阴影 + 圆角 |
| `Shadow > 0` 且 **显式设置** `Back` 属性 | AntdUI 使用指定的 `Back` 颜色绘制，**绕过**原生主题感知渲染 |
| `Back = Transparent`（显式设置） | AntdUI 绘制透明填充，**无卡片背景效果** |

**核心发现**：不设置 `Back` ≠ 设置 `Back = Transparent`。前者让 AntdUI 全权处理，后者是明确告知"我要透明"。

---

## 3. 主题切换架构

### 3.1 AppThemeHelper

位于 `AMControlWinF\Tools\AppThemeHelper.cs`，是当前 WinForms 主线的全局主题主入口：

```csharp
public static class AppThemeHelper
{
    public static bool IsDarkMode { get; private set; }

    public static void Apply(AntdUI.Window window, bool isDark)
    {
        IsDarkMode = isDark;

        if (isDark)
        {
            AntdUI.Config.IsDark = true;
            window.BackColor = Color.FromArgb(31, 31, 31);
            window.ForeColor = Color.White;
        }
        else
        {
            AntdUI.Config.IsLight = true;
            window.BackColor = Color.White;
            window.ForeColor = Color.Black;
        }
    }
}
```

**职责边界**：在当前实现中仅负责 AntdUI 全局开关 + Window 基础色。不负责卡片面板、纹理背景等自定义控件。

### 3.2 调用链路

```
MainWindow.ApplyTheme(isDark, saveToConfig)
  ├─ AppThemeHelper.Apply(this, isDark)     ← AntdUI 全局 + Window 基色
  ├─ textureBackgroundMain.SetTheme(isDark) ← 自定义纹理背景同步
  ├─ labelStatusValue.ForeColor = ...       ← 状态栏语义色刷新
  ├─ buttonColorMode.Toggle = isDark        ← 切换按钮状态同步
  └─ ConfigContext 持久化到 config.json
```

LoginForm 使用相同策略：

```
LoginForm.ApplyThemeFromConfig()
  ├─ Config.IsDark / Config.IsLight         ← AntdUI 全局开关
  └─ textureBackgroundLogin.SetTheme(isDark) ← 自定义纹理背景
```

---

## 4. 当前 WinForms 卡片面板实现基线

### 4.1 Designer 中的正确设置

以 `panelWorkCard`（主工作区卡片）为例：

| 属性 | 值 | 说明 |
|------|----|----|
| `BackColor` | `Transparent` | WinForms 属性，使窗体/纹理背景可透过 |
| `Back` | **不设置** | 不在 Designer 中赋值，让 AntdUI 原生主题处理 |
| `Shadow` | `8` | 启用阴影 + 原生卡片渲染 |
| `Radius` | `16` | 圆角 |
| `Padding` | `10, 10, 10, 10` | 内边距 |

### 4.2 主窗体所有卡片面板

| 面板 | Shadow | Radius | Padding | 用途 |
|------|--------|--------|---------|------|
| `panelWorkCard` | 8 | 16 | 10 | 右侧工作区 |
| `panelSecondaryNavCard` | 8 | 16 | 7 | 二级导航 |
| `panelLeftCard` | 8 | 16 | 0 | 左侧一级导航 |
| `panelStatusCard` | 4 | 12 | 0 | 底部状态栏 |

### 4.3 当前不建议的做法

- ❌ 当前 WinForms 主线中，不建议在 Designer 中设置 `Back` 属性（无论 `Transparent` 还是具体色值）
- ❌ 当前实现中，不建议在代码中调用 `panel.Back = xxx` 覆盖原生渲染
- ❌ 当前主线中，通常不需要在内部子 Panel 上设置 `Back = Transparent`（多余且可能干扰继承链）

---

## 5. TextureBackgroundControl

自定义 `UserPaint` 控件，提供平铺纹理背景效果。

- 位于 `AMControlWinF\Views\Main\TextureBackgroundControl.cs`
- `SetTheme(bool isDark)` 方法切换纹理素材
- 需在主题切换时手动调用（AntdUI 自动主题不覆盖自定义 UserPaint 控件）

---

## 6. 设计决策记录

### 6.1 为什么当前未采用 IPageTheme 接口

早期考虑为每个页面实现 `IPageTheme` 接口以支持主题切换。问题：

- 后续每个页面都需要实现该接口，增加开发负担；
- 大部分页面使用 AntdUI 原生控件，本身已自动跟随主题；
- 只有 `TextureBackgroundControl` 和状态栏语义色需要手动处理。

**当前结论**：当前 WinForms 主线未采用 `IPageTheme`，而是由 `AppThemeHelper` + AntdUI 原生机制覆盖当前场景。若后续出现新的 UI 主线或更复杂的主题层级，可在新方案中重新评估是否引入页面级主题接口。

### 6.2 为什么当前未保留 ApplyCardPanel 方法

早期 `AppThemeHelper.ApplyCardPanel()` 会显式设置 `panel.Back` 和 `panel.BackColor`。问题：

- 显式设置 `Back` 后，AntdUI 使用指定颜色渲染，绕过原生主题感知；
- 导致卡片面板扁平、无深度感，与 LoginForm 的悬浮卡片效果不一致；
- LoginForm 的 `panelShell` 从未设置 `Back`，效果反而最好。

**当前结论**：当前 WinForms 主线未保留 `ApplyCardPanel()`、`DarkCardBack`、`LightCardBack`、`CurrentCardBack`，默认交由 AntdUI 原生渲染。若后续新的主题系统或跨平台宿主不再适用这一做法，应在对应文档中重新定义主题策略。

---

## 7. 配置持久化

主题偏好保存在 `ConfigContext.Instance.Config.Setting.Theme`：

| 值 | 含义 |
|----|------|
| `SkinDefault` | 亮色主题 |
| `SkinDark` | 暗色主题 |

启动时从配置读取，切换时写入 `config.json`。

---

## 相关文档

- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
- [应用生命周期](winf-application-lifecycle.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
