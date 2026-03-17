using AMControlWPF.Tools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMControlWPF.UserControls.Main
{
    /// <summary>
    /// LangAndTheme.xaml 的交互逻辑
    /// </summary>
    public partial class LangAndTheme : UserControl
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
