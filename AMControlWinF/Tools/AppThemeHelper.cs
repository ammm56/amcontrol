using AntdUI;
using System.Drawing;

namespace AMControlWinF.Tools
{
    /// <summary>
    /// 应用级主题辅助类。
    /// 参考 AntdUI Demo ThemeHelper，统一定义色值常量，统一应用入口。
    ///
    /// 核心机制：
    ///   1. AntdUI.Config.IsDark/IsLight → 所有原生 AntdUI 控件（Button/Menu/Label 等）自动重绘；
    ///   2. Window.BackColor/ForeColor   → AntdUI.Window 基础色同步；
    ///   3. ApplyCardPanel() 同时更新：
    ///      - AntdUI.Panel.Back  → 控制 AntdUI 自绘填充色；
    ///      - AntdUI.Panel.BackColor → WinForms 父控件 BackColor 继承链的起点，
    ///        使子页面中未显式设色的 AntdUI.Panel（含 Shadow 卡片）通过 BackColor
    ///        继承自动获得正确底色，无需逐页逐控件手动赋色。
    /// </summary>
    public static class AppThemeHelper
    {
        // ─── 色值定义（全局唯一来源，消除魔法数字）────────────────────────

        /// <summary>暗色：Window / 纹理背景底色。</summary>
        public static readonly Color DarkWindowBg = Color.FromArgb(31, 31, 31);

        /// <summary>暗色：悬浮卡片背景（略亮于底色，阴影+微弱色差产生层次感）。</summary>
        public static readonly Color DarkCardBack = Color.FromArgb(38, 38, 38);

        /// <summary>亮色：悬浮卡片背景。</summary>
        public static readonly Color LightCardBack = Color.White;

        // ─── 状态 ────────────────────────────────────────────────────────

        /// <summary>当前是否为暗色模式。</summary>
        public static bool IsDarkMode { get; private set; }

        /// <summary>当前主题下的卡片背景色。</summary>
        public static Color CurrentCardBack
        {
            get { return IsDarkMode ? DarkCardBack : LightCardBack; }
        }

        // ─── 应用方法 ─────────────────────────────────────────────────────

        /// <summary>
        /// 向 AntdUI.Window 应用全局明/暗主题（对应 Demo ThemeHelper.SetColorMode）。
        /// 调用后所有原生 AntdUI 控件自动适配，无需再对子控件逐个改色。
        /// </summary>
        public static void Apply(AntdUI.Window window, bool isDark)
        {
            IsDarkMode = isDark;

            if (isDark)
            {
                AntdUI.Config.IsDark = true;
                window.BackColor = DarkWindowBg;
                window.ForeColor = Color.White;
            }
            else
            {
                AntdUI.Config.IsLight = true;
                window.BackColor = LightCardBack;
                window.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 同时更新 AntdUI.Panel 的 Back 和 BackColor 属性。
        ///
        /// 原因：AntdUI.Panel.Back 是显式存储属性，不参与 Config.IsDark 自动重绘，
        /// 必须手动赋值（Back 控制 AntdUI 自绘填充色）；
        /// 同时也必须同步 WinForms BackColor，因为子页面中无显式 Back 的 AntdUI.Panel
        /// 会通过 WinForms BackColor 继承链（父→子）解析背景色——
        /// 若 BackColor 链不同步，子页面 stat 卡片等会保持白色。
        /// </summary>
        public static void ApplyCardPanel(AntdUI.Panel panel, bool isDark)
        {
            var color = isDark ? DarkCardBack : LightCardBack;
            panel.Back = color;
            panel.BackColor = color;
        }
    }
}