using AM.DB.Tables;
using AM.Model.Common;
using AM.Model.Entity;
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
    /// AxisTableInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AxisTableInfo : Window
    {
        public AxisTableInfo()
        {
            InitializeComponent();

            this.Loaded += AxisTableInfo_Loaded;

        }

        private void AxisTableInfo_Loaded(object sender, RoutedEventArgs e)
        {
            DBResponse<List<ConfigAxisArg>> res = new DBResponse<List<ConfigAxisArg>>
            {
                data = new DBTable<ConfigAxisArg>().Query()
            };
            res.message = JsonConvert.SerializeObject(res.data, Newtonsoft.Json.Formatting.Indented);

            this.DataContext = res;

        }
    }
}
