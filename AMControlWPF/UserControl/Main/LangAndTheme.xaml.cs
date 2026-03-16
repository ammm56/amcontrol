using AMControlWPF.Tools.Helper;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.UserControl.Main
{
    /// <summary>
    /// LangAndTheme.xaml 的交互逻辑
    /// </summary>
    public partial class LangAndTheme
    {
        public event Action<string> NavigateRequested;

        public LangAndTheme()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e)
        {
            PopupConfig.IsOpen = !PopupConfig.IsOpen;
        }

        private void LanguageButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.Tag is string language))
            {
                return;
            }

            LangThemeHelper.SetLanguage(language);
            PopupConfig.IsOpen = false;
        }

        private void ThemeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) || !(button.Tag is string theme))
            {
                return;
            }

            LangThemeHelper.SetTheme(theme);
            PopupConfig.IsOpen = false;
        }

    }
}