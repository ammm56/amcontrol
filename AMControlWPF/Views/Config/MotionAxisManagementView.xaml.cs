using AM.Model.Entity.Motion.Topology;
using AM.ViewModel.ViewModels.Config;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionAxisManagementView : UserControl
    {
        private readonly MotionAxisManagementViewModel _vm;
        private bool _isFirstLoaded;

        public MotionAxisManagementView()
        {
            InitializeComponent();
            _vm = new MotionAxisManagementViewModel();
            DataContext = _vm;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 页面被主窗口缓存复用，不能在首次加载后解绑 Loaded。
            // 首次进入执行完整加载，后续再次进入时重新刷新列表即可。
            if (!_isFirstLoaded)
            {
                _isFirstLoaded = true;
            }
            await _vm.LoadAsync();
            RefreshDetailPanel();
        }

        private async void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadAsync();
            RefreshDetailPanel();
        }

        private async void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var nextLogicalAxis = _vm.Items.Count == 0
                ? (short)101
                : (short)(_vm.Items.Max(p => p.LogicalAxis) + 1);

            var now = DateTime.Now;
            var defaultEntity = new MotionAxisEntity
            {
                CardId = _vm.AvailableCards.Count > 0 ? _vm.AvailableCards[0].CardId : (short)0,
                AxisId = 0,
                LogicalAxis = nextLogicalAxis,
                Name = "Axis-" + nextLogicalAxis,
                DisplayName = "轴-" + nextLogicalAxis,
                AxisCategory = "Linear",
                PhysicalCore = 1,
                PhysicalAxis = 0,
                IsEnabled = true,
                SortOrder = _vm.Items.Count + 1,
                Description = "新建轴拓扑配置",
                CreateTime = now,
                UpdateTime = now
            };

            var dialog = new MotionAxisEditDialog(defaultEntity, true, _vm.AvailableCards)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            _vm.SelectedItem = dialog.ResultEntity;
            await _vm.SaveCommand.ExecuteAsync(null);
            RefreshDetailPanel();
        }

        private async void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }

            var clone = CloneEntity(_vm.SelectedItem);
            var dialog = new MotionAxisEditDialog(clone, false, _vm.AvailableCards)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            _vm.SelectedItem = dialog.ResultEntity;
            await _vm.SaveCommand.ExecuteAsync(null);
            RefreshDetailPanel();
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }

            var result = MessageBox.Show(
                "确定删除当前轴吗？",
                "删除确认",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            await _vm.DeleteCommand.ExecuteAsync(null);
            RefreshDetailPanel();
        }

        private void ListBoxAxes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetailPanel();
        }

        private void RefreshDetailPanel()
        {
            var hasSelection = _vm.SelectedItem != null;
            BorderNoSelection.Visibility = hasSelection ? Visibility.Collapsed : Visibility.Visible;
            BorderDetailContent.Visibility = hasSelection ? Visibility.Visible : Visibility.Collapsed;
        }

        private static MotionAxisEntity CloneEntity(MotionAxisEntity src)
        {
            return new MotionAxisEntity
            {
                Id = src.Id,
                CardId = src.CardId,
                AxisId = src.AxisId,
                LogicalAxis = src.LogicalAxis,
                Name = src.Name,
                DisplayName = src.DisplayName,
                AxisCategory = src.AxisCategory,
                PhysicalCore = src.PhysicalCore,
                PhysicalAxis = src.PhysicalAxis,
                IsEnabled = src.IsEnabled,
                SortOrder = src.SortOrder,
                Description = src.Description,
                Remark = src.Remark,
                CreateTime = src.CreateTime,
                UpdateTime = src.UpdateTime
            };
        }
    }
}