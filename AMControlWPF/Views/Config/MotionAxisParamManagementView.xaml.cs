using AM.ViewModel.ViewModels.Config;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionAxisParamManagementView : UserControl
    {
        private readonly MotionAxisParamManagementViewModel _vm;

        public MotionAxisParamManagementView()
        {
            InitializeComponent();
            _vm = new MotionAxisParamManagementViewModel();
            DataContext = _vm;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            await _vm.LoadAxisListAsync();
        }

        private void ButtonEditParam_OnClick(object sender, RoutedEventArgs e)
        {
            var param = _vm.SelectedParam;
            if (param == null) return;

            var dlg = new AxisParamEditDialog(param) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true)
            {
                _vm.StatusText = string.Format(
                    "参数「{0}」已修改为 {1}，请确认后点击「保存当前轴参数」写入数据库",
                    param.ParamDisplayName, param.EditValue);
            }
        }

        private async void ButtonAddParam_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedAxis == null)
            {
                HandyControl.Controls.MessageBox.Show(
                    "请先在左侧选择轴，再新增参数", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new AxisParamAddDialog(_vm.SelectedAxis) { Owner = Window.GetWindow(this) };
            if (dlg.ShowDialog() == true && dlg.ResultEntity != null)
            {
                await _vm.AddParamAsync(dlg.ResultEntity);
            }
        }
    }
}