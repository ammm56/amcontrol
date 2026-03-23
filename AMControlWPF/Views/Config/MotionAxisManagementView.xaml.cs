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

        public MotionAxisManagementView()
        {
            InitializeComponent();
            _vm = new MotionAxisManagementViewModel();
            DataContext = _vm;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
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

            ApplyDialogResult(dialog.ResultEntity, _vm.SelectedItem);
            await _vm.SaveCommand.ExecuteAsync(null);
            RefreshDetailPanel();
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }

            var name = !string.IsNullOrEmpty(_vm.SelectedItem.DisplayName)
                ? _vm.SelectedItem.DisplayName
                : _vm.SelectedItem.Name;

            var result = HandyControl.Controls.MessageBox.Show(
                string.Format("确认删除轴 [{0}]（逻辑轴 L#{1}）吗？\n\n此操作不可撤销。",
                    name, _vm.SelectedItem.LogicalAxis),
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

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

        private static void ApplyDialogResult(MotionAxisEntity source, MotionAxisEntity target)
        {
            target.CardId = source.CardId;
            target.AxisId = source.AxisId;
            target.Name = source.Name;
            target.DisplayName = source.DisplayName;
            target.AxisCategory = source.AxisCategory;
            target.PhysicalCore = source.PhysicalCore;
            target.PhysicalAxis = source.PhysicalAxis;
            target.IsEnabled = source.IsEnabled;
            target.SortOrder = source.SortOrder;
            target.Description = source.Description;
            target.Remark = source.Remark;
            target.UpdateTime = source.UpdateTime;
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