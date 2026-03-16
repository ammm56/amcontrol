using AM.Core.Context;
using System;
using System.Windows;

namespace AMControlWPF.Tools.Helper
{
    public static class LangThemeHelper
    {
        public static void ApplyFromConfig()
        {
            var setting = ConfigContext.Instance.Config.Setting;
            ApplyTheme(setting.Theme);
            ApplyLanguage(setting.Language);
        }

        public static void SetLanguage(string language)
        {
            ConfigContext.Instance.Config.Setting.Language = language;
            ApplyLanguage(language);
            AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
        }

        public static void SetTheme(string theme)
        {
            ConfigContext.Instance.Config.Setting.Theme = theme;
            ApplyTheme(theme);
            AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
        }

        private static void ApplyLanguage(string language)
        {
            Application.Current.Resources.MergedDictionaries[3].Source =
                new Uri($"/AMControlWPF;component/Resources/Languages/Lang.{language}.xaml", UriKind.Relative);
        }

        private static void ApplyTheme(string theme)
        {
            var skinContainer = Application.Current.Resources.MergedDictionaries[0];
            skinContainer.MergedDictionaries.Clear();
            skinContainer.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/{theme}.xaml", UriKind.Absolute)
            });

            var themeContainer = Application.Current.Resources.MergedDictionaries[1];
            themeContainer.MergedDictionaries.Clear();
            themeContainer.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml", UriKind.Absolute)
            });
        }
    }
}
