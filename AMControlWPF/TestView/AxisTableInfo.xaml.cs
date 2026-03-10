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
            DBResponse<ConfigAxisArg> res = new DBResponse<ConfigAxisArg>
            {
                data = new DBTable<ConfigAxisArg>().Query(),
                status = true
            };
            if(res.data.Count > 0 ) res.item = res.data[0];

            res.message = JsonConvert.SerializeObject(res.data, Newtonsoft.Json.Formatting.Indented);

            this.DataContext = res;

        }

        private void dg_axis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 1. da_axis.SelectedItem 获得选中项
            // 2. 这个监听选择事件
            if(e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as ConfigAxisArg;

            }
            // 3. 数据绑定 模型中新增一个选择项

        }
    }
}
