using AM.Model.Entity.Motion.Actuator;
using AM.ViewModel.ViewModels.Config;
using HandyControl.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            if (!(sender is Button btn) || !(btn.Tag is string typeFilter))
                return;

            _currentTypeFilter = typeFilter;
            _vm.RebuildUnifiedList(_currentTypeFilter);
            UpdateFilterButtonStyles();
            TextBlockStatus.Text = $"{GetFilterDisplayName(typeFilter)} - 共 {_vm.AllActuatorItems.Count} 条记录";
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

        // ── 列表选中 ──────────────────────────────────────────────────────

        private void ListBoxActuators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDetailPanel();
        }

        // ── 详情面板 ──────────────────────────────────────────────────────

        private void RefreshDetailPanel()
        {
            DetailContent.Children.Clear();

            var item = _vm.SelectedActuatorItem;
            if (item == null) return;

            // 类型徽章
            var badge = new Border
            {
                Padding = new Thickness(6, 2, 6, 2),
                CornerRadius = new CornerRadius(3),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 8),
                Background = GetTypeBrush(item.ActuatorType),
                Child = new TextBlock
                {
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = Brushes.White,
                    Text = item.TypeDisplay
                }
            };
            DetailContent.Children.Add(badge);

            // 显示名称
            DetailContent.Children.Add(new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 12),
                TextWrapping = TextWrapping.Wrap,
                Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                Text = item.DisplayName
            });

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

        private Brush GetTypeBrush(string actuatorType)
        {
            switch (actuatorType)
            {
                case "Cylinder":   return TryFindResource("PrimaryBrush") as Brush ?? Brushes.Blue;
                case "Vacuum":     return HexToBrush("#03A9F4");
                case "StackLight": return TryFindResource("WarningBrush") as Brush ?? Brushes.Orange;
                case "Gripper":    return HexToBrush("#9C27B0");
                default:           return Brushes.Gray;
            }
        }

        private void ShowCylinderDetail(CylinderConfigEntity e)
        {
            if (e == null) return;
            AddShield("名称",         e.Name,                                                         "#455A64");
            AddShield("驱动模式",     e.DriveMode,                                                    "#1976D2");
            AddShield("伸出位 (DO)",  e.ExtendOutputBit > 0 ? e.ExtendOutputBit.ToString() : "-",     "#388E3C");
            AddShield("缩回位 (DO)",  e.RetractOutputBit.HasValue  ? e.RetractOutputBit.Value.ToString()  : "-", "#388E3C");
            AddShield("伸出反馈 (DI)",e.ExtendFeedbackBit.HasValue ? e.ExtendFeedbackBit.Value.ToString() : "-", "#388E3C");
            AddShield("缩回反馈 (DI)",e.RetractFeedbackBit.HasValue? e.RetractFeedbackBit.Value.ToString(): "-", "#388E3C");
            AddShield("反馈校验",     e.UseFeedbackCheck ? "是" : "否",                               "#455A64");
            AddShield("伸出超时 (ms)",e.ExtendTimeoutMs.ToString(),                                   "#F57C00");
            AddShield("缩回超时 (ms)",e.RetractTimeoutMs.ToString(),                                  "#F57C00");
            AddShield("允许双OFF",    e.AllowBothOff ? "是" : "否",                                   "#455A64");
            AddShield("允许双ON",     e.AllowBothOn  ? "是" : "否",                                   "#455A64");
            AddShield("启用",         e.IsEnabled    ? "是" : "否", e.IsEnabled ? "#388E3C" : "#B71C1C");
            AddShield("排序",         e.SortOrder.ToString(),                                         "#455A64");
            AddDescriptionAndRemark(e.Description, e.Remark);
        }

        private void ShowVacuumDetail(VacuumConfigEntity e)
        {
            if (e == null) return;
            AddShield("名称",           e.Name,                                                              "#455A64");
            AddShield("吸真空位 (DO)",  e.VacuumOnOutputBit.ToString(), "#388E3C");
            AddShield("破真空位 (DO)",  e.BlowOffOutputBit.HasValue   ? e.BlowOffOutputBit.Value.ToString()   : "-", "#388E3C");
            AddShield("真空反馈 (DI)",  e.VacuumFeedbackBit.HasValue  ? e.VacuumFeedbackBit.Value.ToString()  : "-", "#388E3C");
            AddShield("释放反馈 (DI)",  e.ReleaseFeedbackBit.HasValue ? e.ReleaseFeedbackBit.Value.ToString() : "-", "#388E3C");
            AddShield("工件检测 (DI)",  e.WorkpiecePresentBit.HasValue? e.WorkpiecePresentBit.Value.ToString(): "-", "#388E3C");
            AddShield("反馈校验",       e.UseFeedbackCheck ? "是" : "否",                                    "#455A64");
            AddShield("工件检测",       e.UseWorkpieceCheck ? "是" : "否",                                   "#455A64");
            AddShield("检测后保持真空", e.KeepVacuumOnAfterDetected ? "是" : "否",                           "#455A64");
            AddShield("建压超时 (ms)",  e.VacuumBuildTimeoutMs.ToString(),                                   "#F57C00");
            AddShield("释放超时 (ms)",  e.ReleaseTimeoutMs.ToString(),                                       "#F57C00");
            AddShield("启用",           e.IsEnabled ? "是" : "否", e.IsEnabled ? "#388E3C" : "#B71C1C");
            AddShield("排序",           e.SortOrder.ToString(),                                              "#455A64");
            AddDescriptionAndRemark(e.Description, e.Remark);
        }

        private void ShowStackLightDetail(StackLightConfigEntity e)
        {
            if (e == null) return;
            AddShield("名称",         e.Name,                                                              "#455A64");
            AddShield("红灯 (DO)",    e.RedOutputBit.HasValue    ? e.RedOutputBit.Value.ToString()    : "-", "#D32F2F");
            AddShield("黄灯 (DO)",    e.YellowOutputBit.HasValue ? e.YellowOutputBit.Value.ToString() : "-", "#F57C00");
            AddShield("绿灯 (DO)",    e.GreenOutputBit.HasValue  ? e.GreenOutputBit.Value.ToString()  : "-", "#388E3C");
            AddShield("蓝灯 (DO)",    e.BlueOutputBit.HasValue   ? e.BlueOutputBit.Value.ToString()   : "-", "#1976D2");
            AddShield("蜂鸣 (DO)",    e.BuzzerOutputBit.HasValue ? e.BuzzerOutputBit.Value.ToString() : "-", "#455A64");
            AddShield("警告时蜂鸣",   e.EnableBuzzerOnWarning  ? "是" : "否",                             "#455A64");
            AddShield("报警时蜂鸣",   e.EnableBuzzerOnAlarm    ? "是" : "否",                             "#455A64");
            AddShield("允许多段同亮", e.AllowMultiSegmentOn    ? "是" : "否",                             "#455A64");
            AddShield("启用",         e.IsEnabled ? "是" : "否", e.IsEnabled ? "#388E3C" : "#B71C1C");
            AddShield("排序",         e.SortOrder.ToString(),                                             "#455A64");
            AddDescriptionAndRemark(e.Description, e.Remark);
        }

        private void ShowGripperDetail(GripperConfigEntity e)
        {
            if (e == null) return;
            AddShield("名称",          e.Name,                                                               "#455A64");
            AddShield("驱动模式",      e.DriveMode,                                                          "#1976D2");
            AddShield("夹紧位 (DO)",   e.CloseOutputBit.ToString(), "#388E3C");
            AddShield("打开位 (DO)",   e.OpenOutputBit.HasValue    ? e.OpenOutputBit.Value.ToString()    : "-", "#388E3C");
            AddShield("夹紧反馈 (DI)", e.CloseFeedbackBit.HasValue ? e.CloseFeedbackBit.Value.ToString() : "-", "#388E3C");
            AddShield("打开反馈 (DI)", e.OpenFeedbackBit.HasValue  ? e.OpenFeedbackBit.Value.ToString()  : "-", "#388E3C");
            AddShield("工件检测 (DI)", e.WorkpiecePresentBit.HasValue ? e.WorkpiecePresentBit.Value.ToString() : "-", "#388E3C");
            AddShield("反馈校验",      e.UseFeedbackCheck   ? "是" : "否",                                   "#455A64");
            AddShield("工件检测",      e.UseWorkpieceCheck  ? "是" : "否",                                   "#455A64");
            AddShield("夹紧超时 (ms)", e.CloseTimeoutMs.ToString(),                                          "#F57C00");
            AddShield("打开超时 (ms)", e.OpenTimeoutMs.ToString(),                                           "#F57C00");
            AddShield("允许双OFF",     e.AllowBothOff ? "是" : "否",                                         "#455A64");
            AddShield("允许双ON",      e.AllowBothOn  ? "是" : "否",                                         "#455A64");
            AddShield("启用",          e.IsEnabled ? "是" : "否", e.IsEnabled ? "#388E3C" : "#B71C1C");
            AddShield("排序",          e.SortOrder.ToString(),                                               "#455A64");
            AddDescriptionAndRemark(e.Description, e.Remark);
        }

        // ── Shield 辅助方法 ───────────────────────────────────────────────

        /// <summary>
        /// Shield.Color 类型为 Brush，使用 BrushConverter 转换十六进制颜色字符串。
        /// </summary>
        private void AddShield(string subject, string status, string colorHex)
        {
            var shield = new Shield
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 0, 0, 6),
                Subject = subject,
                Status = status ?? "-",
                Color = HexToBrush(colorHex)
            };
            DetailContent.Children.Add(shield);
        }

        /// <summary>
        /// 描述和备注使用普通 TextBlock 展示（与轴参数配置右侧"说明"风格一致）。
        /// </summary>
        private void AddDescriptionAndRemark(string description, string remark)
        {
            // 分隔线
            DetailContent.Children.Add(new Rectangle
            {
                Height = 1,
                Margin = new Thickness(0, 12, 0, 12),
                Fill = TryFindResource("BorderBrush") as Brush,
                Opacity = 0.5
            });

            if (!string.IsNullOrWhiteSpace(description))
            {
                DetailContent.Children.Add(new TextBlock
                {
                    FontSize = 12,
                    Opacity = 0.55,
                    Margin = new Thickness(0, 0, 0, 4),
                    Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                    Text = "说明"
                });
                DetailContent.Children.Add(new TextBlock
                {
                    FontSize = 12,
                    Opacity = 0.8,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 8),
                    Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                    Text = description
                });
            }

            if (!string.IsNullOrWhiteSpace(remark))
            {
                DetailContent.Children.Add(new TextBlock
                {
                    FontSize = 12,
                    Opacity = 0.55,
                    Margin = new Thickness(0, 0, 0, 4),
                    Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                    Text = "备注"
                });
                DetailContent.Children.Add(new TextBlock
                {
                    FontSize = 12,
                    Opacity = 0.7,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = TryFindResource("PrimaryTextBrush") as Brush,
                    Text = remark
                });
            }
        }

        /// <summary>
        /// 将十六进制颜色字符串安全转换为 SolidColorBrush。
        /// </summary>
        private static Brush HexToBrush(string hex)
        {
            try
            {
                return (Brush)new BrushConverter().ConvertFromString(hex);
            }
            catch
            {
                return Brushes.Gray;
            }
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
            switch (_currentTypeFilter)
            {
                case "Cylinder":   AddCylinder();   break;
                case "Vacuum":     AddVacuum();     break;
                case "StackLight": AddStackLight(); break;
                case "Gripper":    AddGripper();    break;
                default: Growl.Warning("请先选择执行器类型后再新增"); break;
            }
        }

        private async void AddCylinder()
        {
            var dialog = new CylinderEditDialog(null, true) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveCylinder(dialog.ResultEntity);
                if (result.Success) { Growl.Success("气缸保存成功"); await _vm.LoadCylindersAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); }
                else Growl.Error($"气缸保存失败: {result.Message}");
            }
        }

        private async void AddVacuum()
        {
            var dialog = new VacuumEditDialog(null, true) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveVacuum(dialog.ResultEntity);
                if (result.Success) { Growl.Success("真空保存成功"); await _vm.LoadVacuumsAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); }
                else Growl.Error($"真空保存失败: {result.Message}");
            }
        }

        private async void AddStackLight()
        {
            var dialog = new StackLightEditDialog(null, true) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveStackLight(dialog.ResultEntity);
                if (result.Success) { Growl.Success("灯塔保存成功"); await _vm.LoadStackLightsAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); }
                else Growl.Error($"灯塔保存失败: {result.Message}");
            }
        }

        private async void AddGripper()
        {
            var dialog = new GripperEditDialog(null, true) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveGripper(dialog.ResultEntity);
                if (result.Success) { Growl.Success("夹爪保存成功"); await _vm.LoadGrippersAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); }
                else Growl.Error($"夹爪保存失败: {result.Message}");
            }
        }

        // ── 编辑 ──────────────────────────────────────────────────────────

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedActuatorItem == null) { Growl.Warning("请先选择要编辑的执行器"); return; }

            switch (_vm.SelectedActuatorItem.ActuatorType)
            {
                case "Cylinder":   EditCylinder(_vm.SelectedActuatorItem.SourceEntity as CylinderConfigEntity);     break;
                case "Vacuum":     EditVacuum(_vm.SelectedActuatorItem.SourceEntity as VacuumConfigEntity);         break;
                case "StackLight": EditStackLight(_vm.SelectedActuatorItem.SourceEntity as StackLightConfigEntity); break;
                case "Gripper":    EditGripper(_vm.SelectedActuatorItem.SourceEntity as GripperConfigEntity);       break;
            }
        }

        private async void EditCylinder(CylinderConfigEntity entity)
        {
            if (entity == null) return;
            var dialog = new CylinderEditDialog(entity, false) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveCylinder(dialog.ResultEntity);
                if (result.Success) { Growl.Success("气缸更新成功"); await _vm.LoadCylindersAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); RefreshDetailPanel(); }
                else Growl.Error($"气缸更新失败: {result.Message}");
            }
        }

        private async void EditVacuum(VacuumConfigEntity entity)
        {
            if (entity == null) return;
            var dialog = new VacuumEditDialog(entity, false) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveVacuum(dialog.ResultEntity);
                if (result.Success) { Growl.Success("真空更新成功"); await _vm.LoadVacuumsAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); RefreshDetailPanel(); }
                else Growl.Error($"真空更新失败: {result.Message}");
            }
        }

        private async void EditStackLight(StackLightConfigEntity entity)
        {
            if (entity == null) return;
            var dialog = new StackLightEditDialog(entity, false) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveStackLight(dialog.ResultEntity);
                if (result.Success) { Growl.Success("灯塔更新成功"); await _vm.LoadStackLightsAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); RefreshDetailPanel(); }
                else Growl.Error($"灯塔更新失败: {result.Message}");
            }
        }

        private async void EditGripper(GripperConfigEntity entity)
        {
            if (entity == null) return;
            var dialog = new GripperEditDialog(entity, false) { Owner = HandyControl.Controls.Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveGripper(dialog.ResultEntity);
                if (result.Success) { Growl.Success("夹爪更新成功"); await _vm.LoadGrippersAsync(); _vm.RebuildUnifiedList(_currentTypeFilter); RefreshDetailPanel(); }
                else Growl.Error($"夹爪更新失败: {result.Message}");
            }
        }

        // ── 删除 ──────────────────────────────────────────────────────────

        private async void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedActuatorItem == null) { Growl.Warning("请先选择要删除的执行器"); return; }

            var item = _vm.SelectedActuatorItem;
            var confirm = HandyControl.Controls.MessageBox.Show(
                $"确定要删除执行器「{item.DisplayName}」吗？此操作不可恢复！",
                "确认删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.OK) return;

            AM.Model.Common.Result result = null;
            switch (item.ActuatorType)
            {
                case "Cylinder":
                    result = _vm.DeleteCylinder(item.Name);
                    if (result.Success) await _vm.LoadCylindersAsync();
                    break;
                case "Vacuum":
                    result = _vm.DeleteVacuum(item.Name);
                    if (result.Success) await _vm.LoadVacuumsAsync();
                    break;
                case "StackLight":
                    result = _vm.DeleteStackLight(item.Name);
                    if (result.Success) await _vm.LoadStackLightsAsync();
                    break;
                case "Gripper":
                    result = _vm.DeleteGripper(item.Name);
                    if (result.Success) await _vm.LoadGrippersAsync();
                    break;
            }

            if (result != null)
            {
                if (result.Success)
                {
                    Growl.Success("删除成功");
                    _vm.SelectedActuatorItem = null;
                    _vm.RebuildUnifiedList(_currentTypeFilter);
                    TextBlockStatus.Text = $"{GetFilterDisplayName(_currentTypeFilter)} - 共 {_vm.AllActuatorItems.Count} 条记录";
                }
                else Growl.Error($"删除失败: {result.Message}");
            }
        }
    }
}