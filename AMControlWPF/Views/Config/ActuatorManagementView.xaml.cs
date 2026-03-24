// AMControlWPF\Views\Config\ActuatorManagementView.xaml.cs
using AM.Model.Entity.Motion.Actuator;
using AM.ViewModel.ViewModels.Config;
using HandyControl.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControlWPF.Views.Config
{
    public partial class ActuatorManagementView : UserControl
    {
        private readonly ActuatorManagementViewModel _vm;
        private string _currentTypeFilter = "All";

        public ActuatorManagementView()
        {
            InitializeComponent();
            _vm = new ActuatorManagementViewModel();
            DataContext = _vm;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "正在加载...";

            await _vm.LoadCylindersAsync();
            await _vm.LoadVacuumsAsync();
            await _vm.LoadStackLightsAsync();
            await _vm.LoadGrippersAsync();

            _vm.RebuildUnifiedList(_currentTypeFilter);
            UpdateFilterButtonStyles();

            TextBlockStatus.Text = $"共 {_vm.AllActuatorItems.Count} 条记录";
        }

        // ── 类型筛选 ──────────────────────────────────────────────────────

        private void ButtonFilter_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string typeFilter)
            {
                _currentTypeFilter = typeFilter;
                _vm.RebuildUnifiedList(_currentTypeFilter);
                UpdateFilterButtonStyles();
                TextBlockStatus.Text = $"{GetFilterDisplayName(typeFilter)} - 共 {_vm.AllActuatorItems.Count} 条记录";
            }
        }

        private void UpdateFilterButtonStyles()
        {
            var successStyle = TryFindResource("ButtonSuccess") as Style;
            var defaultStyle = TryFindResource("ButtonDefault") as Style;

            ButtonFilterAll.Style = _currentTypeFilter == "All" ? successStyle : defaultStyle;
            ButtonFilterCylinder.Style = _currentTypeFilter == "Cylinder" ? successStyle : defaultStyle;
            ButtonFilterVacuum.Style = _currentTypeFilter == "Vacuum" ? successStyle : defaultStyle;
            ButtonFilterStackLight.Style = _currentTypeFilter == "StackLight" ? successStyle : defaultStyle;
            ButtonFilterGripper.Style = _currentTypeFilter == "Gripper" ? successStyle : defaultStyle;
        }

        private string GetFilterDisplayName(string typeFilter)
        {
            switch (typeFilter)
            {
                case "Cylinder": return "气缸";
                case "Vacuum": return "真空";
                case "StackLight": return "灯塔";
                case "Gripper": return "夹爪";
                default: return "全部";
            }
        }

        // ── 列表选中事件 ──────────────────────────────────────────────────

        private void ListBoxActuators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetailPanel();
        }

        private void RefreshDetailPanel()
        {
            DetailContent.Children.Clear();

            if (_vm.SelectedActuatorItem == null)
                return;

            var item = _vm.SelectedActuatorItem;

            // 类型徽章
            var typeBadge = new Border
            {
                Padding = new Thickness(6, 2, 6, 2),
                CornerRadius = new CornerRadius(3),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 8),
                Child = new TextBlock
                {
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = Brushes.White,
                    Text = item.TypeDisplay
                }
            };

            switch (item.ActuatorType)
            {
                case "Cylinder":
                    typeBadge.Background = TryFindResource("PrimaryBrush") as Brush;
                    break;
                case "Vacuum":
                    typeBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03A9F4"));
                    break;
                case "StackLight":
                    typeBadge.Background = TryFindResource("WarningBrush") as Brush;
                    break;
                case "Gripper":
                    typeBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9C27B0"));
                    break;
            }

            DetailContent.Children.Add(typeBadge);

            // 显示名称
            DetailContent.Children.Add(new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 12),
                Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                Text = item.DisplayName,
                TextWrapping = TextWrapping.Wrap
            });

            // 根据类型显示详细信息
            switch (item.ActuatorType)
            {
                case "Cylinder":
                    ShowCylinderDetail(item.SourceEntity as CylinderConfigEntity);
                    break;
                case "Vacuum":
                    ShowVacuumDetail(item.SourceEntity as VacuumConfigEntity);
                    break;
                case "StackLight":
                    ShowStackLightDetail(item.SourceEntity as StackLightConfigEntity);
                    break;
                case "Gripper":
                    ShowGripperDetail(item.SourceEntity as GripperConfigEntity);
                    break;
            }
        }

        private void ShowCylinderDetail(CylinderConfigEntity entity)
        {
            if (entity == null) return;

            AddDetailField("名称", entity.Name);
            AddDetailField("驱动模式", entity.DriveMode);
            AddDetailField("伸出位 (DO)", entity.ExtendOutputBit.ToString());
            AddDetailField("缩回位 (DO)", entity.RetractOutputBit.ToString());
            AddDetailField("伸出反馈 (DI)", entity.ExtendFeedbackBit.ToString());
            AddDetailField("缩回反馈 (DI)", entity.RetractFeedbackBit.ToString());
            AddDetailField("反馈校验", entity.UseFeedbackCheck ? "是" : "否");
            AddDetailField("伸出超时 (ms)", entity.ExtendTimeoutMs.ToString());
            AddDetailField("缩回超时 (ms)", entity.RetractTimeoutMs.ToString());
            AddDetailField("允许双OFF", entity.AllowBothOff ? "是" : "否");
            AddDetailField("允许双ON", entity.AllowBothOn ? "是" : "否");
            AddDetailField("启用", entity.IsEnabled ? "是" : "否");
            AddDetailField("排序", entity.SortOrder.ToString());
            if (!string.IsNullOrWhiteSpace(entity.Description))
                AddDetailField("描述", entity.Description);
            if (!string.IsNullOrWhiteSpace(entity.Remark))
                AddDetailField("备注", entity.Remark);
        }

        private void ShowVacuumDetail(VacuumConfigEntity entity)
        {
            if (entity == null) return;

            AddDetailField("名称", entity.Name);
            AddDetailField("吸真空位 (DO)", entity.VacuumOnOutputBit.ToString());
            AddDetailField("破真空位 (DO)", entity.BlowOffOutputBit.ToString());
            AddDetailField("真空反馈 (DI)", entity.VacuumFeedbackBit.ToString());
            AddDetailField("释放反馈 (DI)", entity.ReleaseFeedbackBit.ToString());
            AddDetailField("工件检测 (DI)", entity.WorkpiecePresentBit.ToString());
            AddDetailField("反馈校验", entity.UseFeedbackCheck ? "是" : "否");
            AddDetailField("工件检测", entity.UseWorkpieceCheck ? "是" : "否");
            AddDetailField("检测后保持", entity.KeepVacuumOnAfterDetected ? "是" : "否");
            AddDetailField("建压超时 (ms)", entity.VacuumBuildTimeoutMs.ToString());
            AddDetailField("释放超时 (ms)", entity.ReleaseTimeoutMs.ToString());
            AddDetailField("启用", entity.IsEnabled ? "是" : "否");
            AddDetailField("排序", entity.SortOrder.ToString());
            if (!string.IsNullOrWhiteSpace(entity.Description))
                AddDetailField("描述", entity.Description);
            if (!string.IsNullOrWhiteSpace(entity.Remark))
                AddDetailField("备注", entity.Remark);
        }

        private void ShowStackLightDetail(StackLightConfigEntity entity)
        {
            if (entity == null) return;

            AddDetailField("名称", entity.Name);
            AddDetailField("红灯 (DO)", entity.RedOutputBit.HasValue ? entity.RedOutputBit.Value.ToString() : "-");
            AddDetailField("黄灯 (DO)", entity.YellowOutputBit.HasValue ? entity.YellowOutputBit.Value.ToString() : "-");
            AddDetailField("绿灯 (DO)", entity.GreenOutputBit.HasValue ? entity.GreenOutputBit.Value.ToString() : "-");
            AddDetailField("蓝灯 (DO)", entity.BlueOutputBit.HasValue ? entity.BlueOutputBit.Value.ToString() : "-");
            AddDetailField("蜂鸣 (DO)", entity.BuzzerOutputBit.HasValue ? entity.BuzzerOutputBit.Value.ToString() : "-");
            AddDetailField("警告蜂鸣", entity.EnableBuzzerOnWarning ? "是" : "否");
            AddDetailField("报警蜂鸣", entity.EnableBuzzerOnAlarm ? "是" : "否");
            AddDetailField("多段同时亮", entity.AllowMultiSegmentOn ? "是" : "否");
            AddDetailField("启用", entity.IsEnabled ? "是" : "否");
            AddDetailField("排序", entity.SortOrder.ToString());
            if (!string.IsNullOrWhiteSpace(entity.Description))
                AddDetailField("描述", entity.Description);
            if (!string.IsNullOrWhiteSpace(entity.Remark))
                AddDetailField("备注", entity.Remark);
        }

        private void ShowGripperDetail(GripperConfigEntity entity)
        {
            if (entity == null) return;

            AddDetailField("名称", entity.Name);
            AddDetailField("驱动模式", entity.DriveMode);
            AddDetailField("夹紧位 (DO)", entity.CloseOutputBit.ToString());
            AddDetailField("打开位 (DO)", entity.OpenOutputBit.HasValue ? entity.OpenOutputBit.Value.ToString() : "-");
            AddDetailField("夹紧反馈 (DI)", entity.CloseFeedbackBit.HasValue ? entity.CloseFeedbackBit.Value.ToString() : "-");
            AddDetailField("打开反馈 (DI)", entity.OpenFeedbackBit.HasValue ? entity.OpenFeedbackBit.Value.ToString() : "-");
            AddDetailField("工件检测 (DI)", entity.WorkpiecePresentBit.HasValue ? entity.WorkpiecePresentBit.Value.ToString() : "-");
            AddDetailField("反馈校验", entity.UseFeedbackCheck ? "是" : "否");
            AddDetailField("工件检测", entity.UseWorkpieceCheck ? "是" : "否");
            AddDetailField("夹紧超时 (ms)", entity.CloseTimeoutMs.ToString());
            AddDetailField("打开超时 (ms)", entity.OpenTimeoutMs.ToString());
            AddDetailField("允许双OFF", entity.AllowBothOff ? "是" : "否");
            AddDetailField("允许双ON", entity.AllowBothOn ? "是" : "否");
            AddDetailField("启用", entity.IsEnabled ? "是" : "否");
            AddDetailField("排序", entity.SortOrder.ToString());
            if (!string.IsNullOrWhiteSpace(entity.Description))
                AddDetailField("描述", entity.Description);
            if (!string.IsNullOrWhiteSpace(entity.Remark))
                AddDetailField("备注", entity.Remark);
        }

        private void AddDetailField(string label, string value)
        {
            var panel = new DockPanel { Margin = new Thickness(0, 0, 0, 8) };

            var labelBlock = new TextBlock
            {
                FontSize = 12,
                Opacity = 0.55,
                Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                Text = label,
                MinWidth = 100
            };
            DockPanel.SetDock(labelBlock, Dock.Left);
            panel.Children.Add(labelBlock);

            var valueBlock = new TextBlock
            {
                FontSize = 12,
                Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                Text = value,
                TextWrapping = TextWrapping.Wrap
            };
            panel.Children.Add(valueBlock);

            DetailContent.Children.Add(panel);
        }

        // ── 刷新 ──────────────────────────────────────────────────────────

        private async void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlockStatus.Text = "正在刷新...";

            await _vm.LoadCylindersAsync();
            await _vm.LoadVacuumsAsync();
            await _vm.LoadStackLightsAsync();
            await _vm.LoadGrippersAsync();

            _vm.RebuildUnifiedList(_currentTypeFilter);

            TextBlockStatus.Text = $"{GetFilterDisplayName(_currentTypeFilter)} - 共 {_vm.AllActuatorItems.Count} 条记录";
        }

        // ── 新增 ──────────────────────────────────────────────────────────

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            // 根据当前筛选类型打开对应的新增对话框
            switch (_currentTypeFilter)
            {
                case "Cylinder":
                    AddCylinder();
                    break;
                case "Vacuum":
                    AddVacuum();
                    break;
                case "StackLight":
                    AddStackLight();
                    break;
                case "Gripper":
                    AddGripper();
                    break;
                default:
                    Growl.Warning("请先选择执行器类型");
                    break;
            }
        }

        private async void AddCylinder()
        {
            var dialog = new CylinderEditDialog(null, true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveCylinder(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("气缸保存成功");
                    await _vm.LoadCylindersAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                }
                else
                {
                    Growl.Error($"气缸保存失败:{result.Message}");
                }
            }
        }

        private async void AddVacuum()
        {
            var dialog = new VacuumEditDialog(null, true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveVacuum(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("真空保存成功");
                    await _vm.LoadVacuumsAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                }
                else
                {
                    Growl.Error($"真空保存失败:{result.Message}");
                }
            }
        }

        private async void AddStackLight()
        {
            var dialog = new StackLightEditDialog(null, true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveStackLight(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("灯塔保存成功");
                    await _vm.LoadStackLightsAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                }
                else
                {
                    Growl.Error($"灯塔保存失败:{result.Message}");
                }
            }
        }

        private async void AddGripper()
        {
            var dialog = new GripperEditDialog(null, true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveGripper(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("夹爪保存成功");
                    await _vm.LoadGrippersAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                }
                else
                {
                    Growl.Error($"夹爪保存失败:{result.Message}");
                }
            }
        }

        // ── 编辑 ──────────────────────────────────────────────────────────

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedActuatorItem == null)
            {
                Growl.Warning("请先选择要编辑的执行器");
                return;
            }

            switch (_vm.SelectedActuatorItem.ActuatorType)
            {
                case "Cylinder":
                    EditCylinder(_vm.SelectedActuatorItem.SourceEntity as CylinderConfigEntity);
                    break;
                case "Vacuum":
                    EditVacuum(_vm.SelectedActuatorItem.SourceEntity as VacuumConfigEntity);
                    break;
                case "StackLight":
                    EditStackLight(_vm.SelectedActuatorItem.SourceEntity as StackLightConfigEntity);
                    break;
                case "Gripper":
                    EditGripper(_vm.SelectedActuatorItem.SourceEntity as GripperConfigEntity);
                    break;
            }
        }

        private async void EditCylinder(CylinderConfigEntity entity)
        {
            if (entity == null) return;

            var dialog = new CylinderEditDialog(entity,true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveCylinder(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("气缸更新成功");
                    await _vm.LoadCylindersAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    RefreshDetailPanel();
                }
                else
                {
                    Growl.Error($"气缸更新失败：{result.Message}");
                }
            }
        }

        private async void EditVacuum(VacuumConfigEntity entity)
        {
            if (entity == null) return;

            var dialog = new VacuumEditDialog(entity,true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveVacuum(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("真空更新成功");
                    await _vm.LoadVacuumsAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    RefreshDetailPanel();
                }
                else
                {
                    Growl.Error($"真空更新失败：{result.Message}");
                }
            }
        }

        private async void EditStackLight(StackLightConfigEntity entity)
        {
            if (entity == null) return;

            var dialog = new StackLightEditDialog(entity,true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveStackLight(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("灯塔更新成功");
                    await _vm.LoadStackLightsAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    RefreshDetailPanel();
                }
                else
                {
                    Growl.Error($"灯塔更新失败：{result.Message}");
                }
            }
        }

        private async void EditGripper(GripperConfigEntity entity)
        {
            if (entity == null) return;

            var dialog = new GripperEditDialog(entity,true);
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveGripper(dialog.ResultEntity);
                if (result.Success)
                {
                    Growl.Success("夹爪更新成功");
                    await _vm.LoadGrippersAsync();
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    RefreshDetailPanel();
                }
                else
                {
                    Growl.Error($"夹爪更新失败：{result.Message}");
                }
            }
        }

        // ── 删除 ──────────────────────────────────────────────────────────

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedActuatorItem == null)
            {
                Growl.Warning("请先选择要删除的执行器");
                return;
            }

            var item = _vm.SelectedActuatorItem;
            var confirmResult = HandyControl.Controls.MessageBox.Show(
                $"确定要删除执行器「{item.DisplayName}」吗？此操作不可恢复！",
                "确认删除",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (confirmResult != MessageBoxResult.OK)
                return;

            AM.Model.Common.Result result = null;

            switch (item.ActuatorType)
            {
                case "Cylinder":
                    result = _vm.DeleteCylinder(item.Name);
                    if (result.Success)
                        await _vm.LoadCylindersAsync();
                    break;
                case "Vacuum":
                    result = _vm.DeleteVacuum(item.Name);
                    if (result.Success)
                        await _vm.LoadVacuumsAsync();
                    break;
                case "StackLight":
                    result = _vm.DeleteStackLight(item.Name);
                    if (result.Success)
                        await _vm.LoadStackLightsAsync();
                    break;
                case "Gripper":
                    result = _vm.DeleteGripper(item.Name);
                    if (result.Success)
                        await _vm.LoadGrippersAsync();
                    break;
            }

            if (result != null)
            {
                if (result.Success)
                {
                    Growl.Success("删除成功");
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    _vm.SelectedActuatorItem = null;
                }
                else
                {
                    Growl.Error($"删除失败：{result.Message}");
                }
            }
        }
    }
}