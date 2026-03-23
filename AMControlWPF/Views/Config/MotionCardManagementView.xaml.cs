using AM.Model.Entity.Motion.Topology;
using AM.ViewModel.ViewModels.Config;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF.Views.Config
{
    public partial class MotionCardManagementView : UserControl
    {
        private readonly MotionCardManagementViewModel _vm;

        public MotionCardManagementView()
        {
            InitializeComponent();
            _vm = new MotionCardManagementViewModel();
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
            var nextId = _vm.Items.Count == 0
                ? (short)0
                : (short)(_vm.Items.Max(p => p.CardId) + 1);

            var now = DateTime.Now;
            var defaultEntity = new MotionCardEntity
            {
                CardId = nextId,
                CardType = 90,
                Name = "MotionCard-" + nextId,
                DisplayName = "控制卡-" + nextId,
                DriverKey = "Virtual.Basic",
                ModeParam = 0,
                CoreNumber = 1,
                AxisCountNumber = 0,
                UseExtModule = false,
                InitOrder = _vm.Items.Count + 1,
                IsEnabled = true,
                SortOrder = _vm.Items.Count + 1,
                Description = "新建控制卡配置",
                CreateTime = now,
                UpdateTime = now
            };

            var dialog = new MotionCardEditDialog(defaultEntity, true)
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
            var dialog = new MotionCardEditDialog(clone, false)
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
                $"确认删除控制卡 [{name}] 吗？\n\n此操作不可撤销。",
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

        private void ListBoxCards_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetailPanel();
        }

        private void RefreshDetailPanel()
        {
            var hasSelection = _vm.SelectedItem != null;
            BorderNoSelection.Visibility = hasSelection ? Visibility.Collapsed : Visibility.Visible;
            BorderDetailContent.Visibility = hasSelection ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void ApplyDialogResult(MotionCardEntity source, MotionCardEntity target)
        {
            target.CardType = source.CardType;
            target.Name = source.Name;
            target.DisplayName = source.DisplayName;
            target.DriverKey = source.DriverKey;
            target.ModeParam = source.ModeParam;
            target.CoreNumber = source.CoreNumber;
            target.AxisCountNumber = source.AxisCountNumber;
            target.UseExtModule = source.UseExtModule;
            target.InitOrder = source.InitOrder;
            target.IsEnabled = source.IsEnabled;
            target.SortOrder = source.SortOrder;
            target.Description = source.Description;
            target.Remark = source.Remark;
            target.UpdateTime = source.UpdateTime;
        }

        private static MotionCardEntity CloneEntity(MotionCardEntity src)
        {
            return new MotionCardEntity
            {
                Id = src.Id,
                CardId = src.CardId,
                CardType = src.CardType,
                Name = src.Name,
                DisplayName = src.DisplayName,
                DriverKey = src.DriverKey,
                ModeParam = src.ModeParam,
                OpenConfig = src.OpenConfig,
                CoreNumber = src.CoreNumber,
                AxisCountNumber = src.AxisCountNumber,
                UseExtModule = src.UseExtModule,
                InitOrder = src.InitOrder,
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