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

        public string VersionInfo { get; }

        public LangAndTheme()
        {
            InitializeComponent();
            VersionInfo = $"v{Assembly.GetExecutingAssembly().GetName().Version}";
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

        private void ContributorsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateRequested?.Invoke("Contributors");
        }

        private void ProjectsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateRequested?.Invoke("Projects");
        }
    }
}