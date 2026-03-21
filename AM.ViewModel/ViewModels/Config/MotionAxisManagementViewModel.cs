using AM.DBService.Services;
using AM.DBService.Services.Motion.App;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using AM.Model.Interfaces.DB.Motion.App;
using AM.Model.Interfaces.DB.Motion.Topology;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 轴拓扑管理视图模型。
    /// </summary>
    public class MotionAxisManagementViewModel : ObservableObject
    {
        private readonly IMotionAxisCrudService _motionAxisCrudService;
        private readonly IMachineConfigReloadService _machineConfigReloadService;

        private MotionAxisEntity _selectedItem;
        private string _statusText;
        private bool _isBusy;

        public MotionAxisManagementViewModel()
            : this(new MotionAxisCrudService(), new MachineConfigReloadService())
        {
        }

        public MotionAxisManagementViewModel(
            IMotionAxisCrudService motionAxisCrudService,
            IMachineConfigReloadService machineConfigReloadService)
        {
            _motionAxisCrudService = motionAxisCrudService;
            _machineConfigReloadService = machineConfigReloadService;

            Items = new ObservableCollection<MotionAxisEntity>();
            _statusText = "请加载轴拓扑配置";

            RefreshCommand = new AsyncRelayCommand(LoadAsync, CanExecuteCommand);
            AddCommand = new RelayCommand(AddItem, CanExecuteCommand);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSaveOrDelete);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanSaveOrDelete);
        }

        public ObservableCollection<MotionAxisEntity> Items { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand AddCommand { get; private set; }

        public IAsyncRelayCommand SaveCommand { get; private set; }

        public IAsyncRelayCommand DeleteCommand { get; private set; }

        public MotionAxisEntity SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RefreshCommand.NotifyCanExecuteChanged();
                    AddCommand.NotifyCanExecuteChanged();
                    SaveCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public async Task LoadAsync()
        {
            IsBusy = true;
            StatusText = "正在加载轴拓扑配置...";

            try
            {
                var result = await Task.Run(() => _motionAxisCrudService.QueryAll());
                Items.Clear();

                if (!result.Success)
                {
                    StatusText = result.Message;
                    SelectedItem = null;
                    return;
                }

                foreach (var item in result.Items)
                {
                    Items.Add(Clone(item));
                }

                SelectedItem = Items.Count > 0 ? Items[0] : null;
                StatusText = "轴拓扑配置加载完成";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddItem()
        {
            var nextLogicalAxis = Items.Count == 0 ? (short)101 : (short)(Items.Max(p => p.LogicalAxis) + 1);
            var now = DateTime.Now;

            var item = new MotionAxisEntity
            {
                CardId = 0,
                AxisId = 0,
                LogicalAxis = nextLogicalAxis,
                Name = "Axis-" + nextLogicalAxis,
                DisplayName = "轴-" + nextLogicalAxis,
                AxisCategory = "Linear",
                PhysicalCore = 1,
                PhysicalAxis = 0,
                IsEnabled = true,
                SortOrder = Items.Count + 1,
                Description = "新建轴拓扑配置",
                Remark = null,
                CreateTime = now,
                UpdateTime = now
            };

            Items.Add(item);
            SelectedItem = item;
            StatusText = "已新增轴拓扑，请保存";
        }

        private async Task SaveAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            StatusText = "正在保存轴拓扑配置...";

            try
            {
                var logicalAxis = SelectedItem.LogicalAxis;

                var saveResult = await Task.Run(() => _motionAxisCrudService.Save(SelectedItem));
                if (!saveResult.Success)
                {
                    StatusText = saveResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "轴拓扑保存成功并已热重载" : reloadResult.Message;

                await LoadAsync();
                RestoreSelectionByLogicalAxis(logicalAxis);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var logicalAxis = SelectedItem.LogicalAxis;

            IsBusy = true;
            StatusText = "正在删除轴拓扑配置...";

            try
            {
                var deleteResult = await Task.Run(() => _motionAxisCrudService.DeleteByLogicalAxis(logicalAxis));
                if (!deleteResult.Success)
                {
                    StatusText = deleteResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "轴拓扑删除成功并已热重载" : reloadResult.Message;

                await LoadAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RestoreSelectionByLogicalAxis(short logicalAxis)
        {
            SelectedItem = Items.FirstOrDefault(p => p.LogicalAxis == logicalAxis);
        }

        private bool CanExecuteCommand()
        {
            return !IsBusy;
        }

        private bool CanSaveOrDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

        private static MotionAxisEntity Clone(MotionAxisEntity source)
        {
            return new MotionAxisEntity
            {
                Id = source.Id,
                CardId = source.CardId,
                AxisId = source.AxisId,
                LogicalAxis = source.LogicalAxis,
                Name = source.Name,
                DisplayName = source.DisplayName,
                AxisCategory = source.AxisCategory,
                PhysicalCore = source.PhysicalCore,
                PhysicalAxis = source.PhysicalAxis,
                IsEnabled = source.IsEnabled,
                SortOrder = source.SortOrder,
                Description = source.Description,
                Remark = source.Remark,
                CreateTime = source.CreateTime,
                UpdateTime = source.UpdateTime
            };
        }
    }
}