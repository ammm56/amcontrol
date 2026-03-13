using AM.DBService.Tables;
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
            Result<ConfigAxisArg> res = new Result<ConfigAxisArg>
            {
                Items = new DBTable<ConfigAxisArg>().QueryAll(),
                Success = true
            };
            if(res.Items.Count > 0 ) res.Item = res.Items[0];

            res.Message = JsonConvert.SerializeObject(res.Item, Newtonsoft.Json.Formatting.Indented);

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
