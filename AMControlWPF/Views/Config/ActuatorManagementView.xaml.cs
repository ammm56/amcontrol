using AM.Model.Entity.Motion.Actuator;
using AM.ViewModel.ViewModels.Config;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AMControlWPF.Views.Config
{
    public partial class ActuatorManagementView : UserControl
    {
        private readonly ActuatorManagementViewModel _vm;
        private bool _cylindersLoaded;
        private bool _vacuumsLoaded;
        private bool _stackLightsLoaded;
        private bool _grippersLoaded;

        public ActuatorManagementView()
        {
            InitializeComponent();
            _vm = new ActuatorManagementViewModel();
            DataContext = _vm;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 首次加载全部，后续刷新按需触发
            await _vm.LoadCylindersAsync();
            _cylindersLoaded = true;
            UpdateCylinderStatus();

            await _vm.LoadVacuumsAsync();
            _vacuumsLoaded = true;
            UpdateVacuumStatus();

            await _vm.LoadStackLightsAsync();
            _stackLightsLoaded = true;
            UpdateStackLightStatus();

            await _vm.LoadGrippersAsync();
            _grippersLoaded = true;
            UpdateGripperStatus();
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 预留：按需执行 tab 切换联动逻辑
        }

        // ── 状态栏更新 ────────────────────────────────────────────────────

        private void UpdateCylinderStatus()
        {
            TextBlockCylinderStatus.Text = "共 " + _vm.Cylinders.Count + " 条记录";
        }

        private void UpdateVacuumStatus()
        {
            TextBlockVacuumStatus.Text = "共 " + _vm.Vacuums.Count + " 条记录";
        }

        private void UpdateStackLightStatus()
        {
            TextBlockStackLightStatus.Text = "共 " + _vm.StackLights.Count + " 条记录";
        }

        private void UpdateGripperStatus()
        {
            TextBlockGripperStatus.Text = "共 " + _vm.Grippers.Count + " 条记录";
        }

        // ═══════════════════════════════════════════════════════════════
        // 气缸
        // ═══════════════════════════════════════════════════════════════

        private async void ButtonRefreshCylinders_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadCylindersAsync();
            UpdateCylinderStatus();
        }

        private void ButtonAddCylinder_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = new CylinderConfigEntity
            {
                DriveMode = "Double",
                IsEnabled = true,
                ExtendTimeoutMs = 3000,
                RetractTimeoutMs = 3000
            };
            OpenCylinderEditDialog(entity, isAdd: true);
        }

        private void ButtonEditCylinder_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as CylinderConfigEntity;
            if (entity != null)
                OpenCylinderEditDialog(entity, isAdd: false);
        }

        private void DataGridCylinders_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var entity = DataGridCylinders.SelectedItem as CylinderConfigEntity;
            if (entity != null)
                OpenCylinderEditDialog(entity, isAdd: false);
        }

        private async void ButtonDeleteCylinder_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as CylinderConfigEntity;
            if (entity == null) return;

            var label = string.IsNullOrWhiteSpace(entity.DisplayName) ? entity.Name : entity.DisplayName;
            if (MessageBox.Show("确定要删除气缸「" + label + "」吗？\n此操作不可撤销。",
                    "确认删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                return;

            var result = _vm.DeleteCylinder(entity.Name);
            if (result.Success)
            {
                await _vm.LoadCylindersAsync();
                UpdateCylinderStatus();
            }
            else
            {
                MessageBox.Show(result.Message, "删除失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OpenCylinderEditDialog(CylinderConfigEntity entity, bool isAdd)
        {
            var dialog = new CylinderEditDialog(entity, isAdd) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveCylinder(dialog.ResultEntity);
                if (result.Success)
                {
                    await _vm.LoadCylindersAsync();
                    UpdateCylinderStatus();
                }
                else
                {
                    MessageBox.Show(result.Message, "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // 真空
        // ═══════════════════════════════════════════════════════════════

        private async void ButtonRefreshVacuums_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadVacuumsAsync();
            UpdateVacuumStatus();
        }

        private void ButtonAddVacuum_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = new VacuumConfigEntity
            {
                IsEnabled = true,
                VacuumBuildTimeoutMs = 3000,
                ReleaseTimeoutMs = 2000,
                KeepVacuumOnAfterDetected = true
            };
            OpenVacuumEditDialog(entity, isAdd: true);
        }

        private void ButtonEditVacuum_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as VacuumConfigEntity;
            if (entity != null)
                OpenVacuumEditDialog(entity, isAdd: false);
        }

        private void DataGridVacuums_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var entity = DataGridVacuums.SelectedItem as VacuumConfigEntity;
            if (entity != null)
                OpenVacuumEditDialog(entity, isAdd: false);
        }

        private async void ButtonDeleteVacuum_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as VacuumConfigEntity;
            if (entity == null) return;

            var label = string.IsNullOrWhiteSpace(entity.DisplayName) ? entity.Name : entity.DisplayName;
            if (MessageBox.Show("确定要删除真空「" + label + "」吗？\n此操作不可撤销。",
                    "确认删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                return;

            var result = _vm.DeleteVacuum(entity.Name);
            if (result.Success)
            {
                await _vm.LoadVacuumsAsync();
                UpdateVacuumStatus();
            }
            else
            {
                MessageBox.Show(result.Message, "删除失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OpenVacuumEditDialog(VacuumConfigEntity entity, bool isAdd)
        {
            var dialog = new VacuumEditDialog(entity, isAdd) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveVacuum(dialog.ResultEntity);
                if (result.Success)
                {
                    await _vm.LoadVacuumsAsync();
                    UpdateVacuumStatus();
                }
                else
                {
                    MessageBox.Show(result.Message, "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // 灯塔
        // ═══════════════════════════════════════════════════════════════

        private async void ButtonRefreshStackLights_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadStackLightsAsync();
            UpdateStackLightStatus();
        }

        private void ButtonAddStackLight_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = new StackLightConfigEntity
            {
                IsEnabled = true,
                EnableBuzzerOnAlarm = true
            };
            OpenStackLightEditDialog(entity, isAdd: true);
        }

        private void ButtonEditStackLight_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as StackLightConfigEntity;
            if (entity != null)
                OpenStackLightEditDialog(entity, isAdd: false);
        }

        private void DataGridStackLights_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var entity = DataGridStackLights.SelectedItem as StackLightConfigEntity;
            if (entity != null)
                OpenStackLightEditDialog(entity, isAdd: false);
        }

        private async void ButtonDeleteStackLight_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as StackLightConfigEntity;
            if (entity == null) return;

            var label = string.IsNullOrWhiteSpace(entity.DisplayName) ? entity.Name : entity.DisplayName;
            if (MessageBox.Show("确定要删除灯塔「" + label + "」吗？\n此操作不可撤销。",
                    "确认删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                return;

            var result = _vm.DeleteStackLight(entity.Name);
            if (result.Success)
            {
                await _vm.LoadStackLightsAsync();
                UpdateStackLightStatus();
            }
            else
            {
                MessageBox.Show(result.Message, "删除失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OpenStackLightEditDialog(StackLightConfigEntity entity, bool isAdd)
        {
            var dialog = new StackLightEditDialog(entity, isAdd) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveStackLight(dialog.ResultEntity);
                if (result.Success)
                {
                    await _vm.LoadStackLightsAsync();
                    UpdateStackLightStatus();
                }
                else
                {
                    MessageBox.Show(result.Message, "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // 夹爪
        // ═══════════════════════════════════════════════════════════════

        private async void ButtonRefreshGrippers_OnClick(object sender, RoutedEventArgs e)
        {
            await _vm.LoadGrippersAsync();
            UpdateGripperStatus();
        }

        private void ButtonAddGripper_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = new GripperConfigEntity
            {
                DriveMode = "Double",
                IsEnabled = true,
                CloseTimeoutMs = 3000,
                OpenTimeoutMs = 3000
            };
            OpenGripperEditDialog(entity, isAdd: true);
        }

        private void ButtonEditGripper_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as GripperConfigEntity;
            if (entity != null)
                OpenGripperEditDialog(entity, isAdd: false);
        }

        private void DataGridGrippers_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var entity = DataGridGrippers.SelectedItem as GripperConfigEntity;
            if (entity != null)
                OpenGripperEditDialog(entity, isAdd: false);
        }

        private async void ButtonDeleteGripper_OnClick(object sender, RoutedEventArgs e)
        {
            var entity = (sender as Button)?.Tag as GripperConfigEntity;
            if (entity == null) return;

            var label = string.IsNullOrWhiteSpace(entity.DisplayName) ? entity.Name : entity.DisplayName;
            if (MessageBox.Show("确定要删除夹爪「" + label + "」吗？\n此操作不可撤销。",
                    "确认删除", MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK)
                return;

            var result = _vm.DeleteGripper(entity.Name);
            if (result.Success)
            {
                await _vm.LoadGrippersAsync();
                UpdateGripperStatus();
            }
            else
            {
                MessageBox.Show(result.Message, "删除失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OpenGripperEditDialog(GripperConfigEntity entity, bool isAdd)
        {
            var dialog = new GripperEditDialog(entity, isAdd) { Owner = Window.GetWindow(this) };
            if (dialog.ShowDialog() == true && dialog.ResultEntity != null)
            {
                var result = _vm.SaveGripper(dialog.ResultEntity);
                if (result.Success)
                {
                    await _vm.LoadGrippersAsync();
                    UpdateGripperStatus();
                }
                else
                {
                    MessageBox.Show(result.Message, "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}