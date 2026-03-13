using AM.Core.Context;
using AM.Tools;
using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace AMControlWPF
{
    /// <summary>
    /// CfgInfo.xaml 的交互逻辑
    /// </summary>
    public partial class CfgInfo : Window
    {
        public CfgInfo()
        {
            InitializeComponent();


            this.Loaded += CfgInfo_Loaded;

        }

        private void CfgInfo_Loaded(object sender, RoutedEventArgs e)
        {
            /// winform事件驱动直接辅助方式
            string cfgstr = JsonConvert.SerializeObject(ConfigContext.Instance.Config, Newtonsoft.Json.Formatting.Indented);
            //tb_cfginfo.Text = cfgstr;

            /// wpf数据驱动显示方式
            this.DataContext = ConfigContext.Instance.Config;

        }
    }
}
