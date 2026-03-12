using AM.Core.Messaging;
using AM.DBService.Services;
using AM.Model.Entity;
using AM.Tools.Logging;
using AM.ViewModel.ViewModels.Config;
using AMControlWPF.MessageBus;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMControlWPF.TestView
{
    /// <summary>
    /// AxisInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class AxisInfoView : Window
    {
        public AxisInfoView()
        {
            InitializeComponent();

            this.Loaded += AxisInfoView_Loaded;

            // 创建 Service 实例（或通过依赖注入容器获取）
            var axisService = new ConfigAxisArgService(new MessageBusWPF(),new NLogLogger("nglog")); // 实现 IConfigAxisArgService

            // 注入 ViewModel
            this.DataContext = new ConfigAxisArgViewModel(axisService);

            // 异步加载数据
            // Execute() → 用于同步命令（Action）ExecuteAsync() → 用于异步命令（Func<Task>）
            var vm = (ConfigAxisArgViewModel)this.DataContext;
            //vm.LoadCommand.Execute(this);
            vm.LoadCommand.ExecuteAsync(null);
        }

        private void AxisInfoView_Loaded(object sender, RoutedEventArgs e)
        {
            // UI统一报警处理
            WeakReferenceMessenger.Default.Register<SystemMessage>(this, (r, m) =>
            {
                switch (m.Type)
                {
                    case SystemMessageType.Error:
                        MessageBox.Show(m.Message);
                        break;

                    case SystemMessageType.Alarm:
                        MessageBox.Show(m.Message);
                        break;

                    case SystemMessageType.Status:
                        MessageBox.Show(m.Message);
                        break;
                }
            });
        }
    }
}
