using AM.Core.Context;
using AM.Model.Common;
using System;
using System.Linq;
using System.Windows;

namespace AMControlWPF.Tools.Helper
{
    public static class LangThemeHelper
    {
        private const string DefaultLanguage = "zh-CN";
        private const string EnglishLanguage = "en-US";

        private const string DefaultTheme = "SkinDefault";
        private const string DarkTheme = "SkinDark";

        private const string DefaultThemeResource = "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml";
        private const string DarkThemeResource = "pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml";
        private const string HandyControlThemeResource = "pack://application:,,,/HandyControl;component/Themes/Theme.xaml";

        public static void ApplyFromConfig()
        {
            var setting = EnsureSetting();

            ApplyTheme(setting.Theme);
            ApplyLanguage(setting.Language);
        }

        public static void SetLanguage(string language)
        {
            var setting = EnsureSetting();
            setting.Language = NormalizeLanguage(language);

            ApplyLanguage(setting.Language);
            SaveCurrentConfig();
        }

        public static void SetTheme(string theme)
        {
            var setting = EnsureSetting();
            setting.Theme = NormalizeTheme(theme);

            ApplyTheme(setting.Theme);
            SaveCurrentConfig();
        }

        private static Setting EnsureSetting()
        {
            if (ConfigContext.Instance.Config == null)
            {
                ConfigContext.Instance.Initialize(new Config());
            }

            if (ConfigContext.Instance.Config.Setting == null)
            {
                ConfigContext.Instance.Config.Setting = new Setting();
            }

            return ConfigContext.Instance.Config.Setting;
        }

        private static void SaveCurrentConfig()
        {
            if (ConfigContext.Instance.Config == null)
            {
                return;
            }

            AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config);
        }

        private static string NormalizeLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return DefaultLanguage;
            }

            switch (language.Trim().ToLowerInvariant())
            {
                case "zh":
                case "zh-cn":
                    return DefaultLanguage;

                case "en":
                case "en-us":
                    return EnglishLanguage;

                default:
                    return DefaultLanguage;
            }
        }

        private static string NormalizeTheme(string theme)
        {
            if (string.IsNullOrWhiteSpace(theme))
            {
                return DefaultTheme;
            }

            switch (theme.Trim())
            {
                case DefaultTheme:
                case DarkTheme:
                    return theme.Trim();

                default:
                    return DefaultTheme;
            }
        }

        private static void ApplyLanguage(string language)
        {
            var normalizedLanguage = NormalizeLanguage(language);
            var resourceUri = new Uri(
                string.Format(
                    "/AMControlWPF;component/Resources/Languages/Lang.{0}.xaml",
                    normalizedLanguage),
                UriKind.Relative);

            ReplaceTopLevelDictionary("Resources/Languages/Lang.", resourceUri, 2);
        }

        private static void ApplyTheme(string theme)
        {
            var normalizedTheme = NormalizeTheme(theme);
            var skinUri = new Uri(
                normalizedTheme == DarkTheme ? DarkThemeResource : DefaultThemeResource,
                UriKind.Absolute);

            EnsureThemeContainers();

            var skinContainer = Application.Current.Resources.MergedDictionaries[0];
            skinContainer.MergedDictionaries.Clear();
            skinContainer.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = skinUri
            });

            var themeContainer = Application.Current.Resources.MergedDictionaries[1];
            themeContainer.MergedDictionaries.Clear();
            themeContainer.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri(HandyControlThemeResource, UriKind.Absolute)
            });
        }

        private static void EnsureThemeContainers()
        {
            if (Application.Current == null || Application.Current.Resources == null)
            {
                return;
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            while (dictionaries.Count < 2)
            {
                dictionaries.Add(new ResourceDictionary());
            }

            if (dictionaries[0] == null)
            {
                dictionaries[0] = new ResourceDictionary();
            }

            if (dictionaries[1] == null)
            {
                dictionaries[1] = new ResourceDictionary();
            }
        }

        private static void ReplaceTopLevelDictionary(string matchText, Uri newSource, int fallbackInsertIndex)
        {
            if (Application.Current == null || Application.Current.Resources == null)
            {
                return;
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;
            var oldDictionary = dictionaries.FirstOrDefault(p =>
                p.Source != null &&
                p.Source.OriginalString.IndexOf(matchText, StringComparison.OrdinalIgnoreCase) >= 0);

            var newDictionary = new ResourceDictionary
            {
                Source = newSource
            };

            if (oldDictionary != null)
            {
                var index = dictionaries.IndexOf(oldDictionary);
                dictionaries.RemoveAt(index);
                dictionaries.Insert(index, newDictionary);
                return;
            }

            if (fallbackInsertIndex < 0)
            {
                fallbackInsertIndex = 0;
            }

            if (fallbackInsertIndex > dictionaries.Count)
            {
                fallbackInsertIndex = dictionaries.Count;
            }

            dictionaries.Insert(fallbackInsertIndex, newDictionary);
        }
    }
}
