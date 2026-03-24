using AM.Model.Entity.Motion.Topology;
using AM.ViewModel.ViewModels.Config;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControlWPF.Views.Config
{
    public partial class MotionIoMapManagementView : UserControl
    {
        private readonly MotionIoMapManagementViewModel _vm;
        private short? _currentCardFilter = null;   // null = 全部控制卡
        private string _currentIoTypeFilter = "All"; // "All" / "DI" / "DO"

        public MotionIoMapManagementView()
        {
            InitializeComponent();
            _vm = new MotionIoMapManagementViewModel();
            DataContext = _vm;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            await _vm.LoadAsync();
            PopulateCardFilterComboBox();
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            UpdateFilterButtonStyles();
            RefreshDetailPanel();
        }

        private async void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadAsync();
            PopulateCardFilterComboBox();
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        private async void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var nextLogicalBit = _vm.FilteredItems.Count == 0
                ? (short)1
                : (short)(_vm.AllItems
                    .Where(x => string.IsNullOrEmpty(_currentIoTypeFilter) || _currentIoTypeFilter == "All" || x.IoType == _currentIoTypeFilter)
                    .Max(p => p.LogicalBit) + 1);

            var defaultIoType = _currentIoTypeFilter == "DI" || _currentIoTypeFilter == "DO"
                ? _currentIoTypeFilter
                : "DI";

            var defaultCardId = _currentCardFilter
                ?? (_vm.AvailableCards.Count > 0 ? _vm.AvailableCards[0].CardId : (short)0);

            var defaultEntity = new MotionIoMapEntity
            {
                CardId = defaultCardId,
                IoType = defaultIoType,
                LogicalBit = nextLogicalBit,
                Name = defaultIoType + "-" + nextLogicalBit,
                Core = 1,
                IsExtModule = false,
                HardwareBit = 0,
                IsEnabled = true,
                SortOrder = _vm.AllItems.Count + 1,
                Remark = null
            };

            var dialog = new MotionIoMapEditDialog(defaultEntity, true, _vm.AvailableCards)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            _vm.SelectedItem = dialog.ResultEntity;
            await _vm.SaveCommand.ExecuteAsync(null);
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        private async void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }

            var clone = CloneEntity(_vm.SelectedItem);
            var dialog = new MotionIoMapEditDialog(clone, false, _vm.AvailableCards)
            {
                Owner = Window.GetWindow(this)
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            ApplyDialogResult(dialog.ResultEntity, _vm.SelectedItem);
            await _vm.SaveCommand.ExecuteAsync(null);
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }

            var name = _vm.SelectedItem.Name ?? ("L#" + _vm.SelectedItem.LogicalBit);
            var result = HandyControl.Controls.MessageBox.Show(
                string.Format("确认删除 IO 点位 [{0}]（{1} L#{2}）吗？\n\n此操作不可撤销。",
                    name, _vm.SelectedItem.IoType, _vm.SelectedItem.LogicalBit),
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            await _vm.DeleteCommand.ExecuteAsync(null);
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        // ── 过滤 ────────────────────────────────────────────────────────

        private void ButtonFilter_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            _currentIoTypeFilter = btn.Tag?.ToString() ?? "All";
            UpdateFilterButtonStyles();
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        private void ComboBoxCardFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ComboBoxCardFilter.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                return;
            }

            _currentCardFilter = item.Tag is short s ? s : (short?)null;
            _vm.ApplyFilter(_currentCardFilter, _currentIoTypeFilter);
            RefreshDetailPanel();
        }

        private void PopulateCardFilterComboBox()
        {
            ComboBoxCardFilter.SelectionChanged -= ComboBoxCardFilter_OnSelectionChanged;
            ComboBoxCardFilter.Items.Clear();

            ComboBoxCardFilter.Items.Add(new ComboBoxItem { Content = "全部控制卡", Tag = null });

            foreach (var card in _vm.AvailableCards)
            {
                var label = string.IsNullOrWhiteSpace(card.DisplayName) ? card.Name : card.DisplayName;
                ComboBoxCardFilter.Items.Add(new ComboBoxItem
                {
                    Content = "#" + card.CardId + "  " + label,
                    Tag = card.CardId
                });
            }

            // 恢复已选卡，找不到则回到"全部"
            var matched = false;
            if (_currentCardFilter.HasValue)
            {
                foreach (ComboBoxItem item in ComboBoxCardFilter.Items)
                {
                    if (item.Tag is short s && s == _currentCardFilter.Value)
                    {
                        ComboBoxCardFilter.SelectedItem = item;
                        matched = true;
                        break;
                    }
                }
            }

            if (!matched)
            {
                ComboBoxCardFilter.SelectedIndex = 0;
                _currentCardFilter = null;
            }

            ComboBoxCardFilter.SelectionChanged += ComboBoxCardFilter_OnSelectionChanged;
        }

        private void UpdateFilterButtonStyles()
        {
            var activeBg   = TryFindResource("SuccessBrush") as Brush ?? Brushes.DodgerBlue;
            var inactiveBg = TryFindResource("SecondaryRegionBrush") as Brush ?? Brushes.LightGray;
            var activeFg   = Brushes.White;
            var inactiveFg = TryFindResource("PrimaryTextBrush") as Brush ?? Brushes.Black;

            ButtonFilterAll.Background = _currentIoTypeFilter == "All" ? activeBg : inactiveBg;
            ButtonFilterAll.Foreground = _currentIoTypeFilter == "All" ? activeFg : inactiveFg;
            ButtonFilterDI.Background  = _currentIoTypeFilter == "DI"  ? activeBg : inactiveBg;
            ButtonFilterDI.Foreground  = _currentIoTypeFilter == "DI"  ? activeFg : inactiveFg;
            ButtonFilterDO.Background  = _currentIoTypeFilter == "DO"  ? activeBg : inactiveBg;
            ButtonFilterDO.Foreground  = _currentIoTypeFilter == "DO"  ? activeFg : inactiveFg;
        }

        // ── 辅助 ────────────────────────────────────────────────────────

        private void ListBoxIoMaps_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetailPanel();
        }

        private void RefreshDetailPanel()
        {
            var hasSelection = _vm.SelectedItem != null;
            BorderNoSelection.Visibility = hasSelection ? Visibility.Collapsed : Visibility.Visible;
            BorderDetailContent.Visibility = hasSelection ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void ApplyDialogResult(MotionIoMapEntity source, MotionIoMapEntity target)
        {
            target.CardId = source.CardId;
            target.IoType = source.IoType;
            target.LogicalBit = source.LogicalBit;
            target.Name = source.Name;
            target.Core = source.Core;
            target.IsExtModule = source.IsExtModule;
            target.HardwareBit = source.HardwareBit;
            target.IsEnabled = source.IsEnabled;
            target.SortOrder = source.SortOrder;
            target.Remark = source.Remark;
        }

        private static MotionIoMapEntity CloneEntity(MotionIoMapEntity src)
        {
            return new MotionIoMapEntity
            {
                Id = src.Id,
                CardId = src.CardId,
                IoType = src.IoType,
                LogicalBit = src.LogicalBit,
                Name = src.Name,
                Core = src.Core,
                IsExtModule = src.IsExtModule,
                HardwareBit = src.HardwareBit,
                IsEnabled = src.IsEnabled,
                SortOrder = src.SortOrder,
                Remark = src.Remark
            };
        }
    }
}