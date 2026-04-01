namespace AMControlWinF.Tools
{
    /// <summary>
    /// 页面级主题感知接口。
    ///
    /// 大多数页面通过 BackColor 继承链自动同步无需实现此接口；
    /// 以下情况需要实现：
    ///   - 页面内有带 Shadow 的 AntdUI.Panel 且其父链中存在
    ///     显式 BackColor=White 阻断了继承（如 TableLayoutPanel 内层）；
    ///   - 页面内有需要独立控制的语义色（如特定标签前景色）。
    ///
    /// MainWindow 在切换主题和首次创建页面时统一调用，页面无需订阅事件。
    /// </summary>
    public interface IPageTheme
    {
        /// <summary>主题切换或页面首次创建时由 MainWindow 统一调用。</summary>
        void OnThemeChanged(bool isDarkMode);
    }
}