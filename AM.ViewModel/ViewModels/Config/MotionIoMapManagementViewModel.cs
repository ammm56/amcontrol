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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// IO 映射管理视图模型。
    /// </summary>
    public class MotionIoMapManagementViewModel : ObservableObject
    {
        private readonly IMotionIoMapCrudService _motionIoMapCrudService;
        private readonly IMachineConfigReloadService _machineConfigReloadService;

        private MotionIoMapEntity _selectedItem;
        private string _statusText;
        private bool _isBusy;

        public MotionIoMapManagementViewModel()
            : this(new MotionIoMapCrudService(), new MachineConfigReloadService())
        {
        }

        public MotionIoMapManagementViewModel(
            IMotionIoMapCrudService motionIoMapCrudService,
            IMachineConfigReloadService machineConfigReloadService)
        {
            _motionIoMapCrudService = motionIoMapCrudService;
            _machineConfigReloadService = machineConfigReloadService;

            Items = new ObservableCollection<MotionIoMapEntity>();
            _statusText = "请加载 IO 映射配置";

            RefreshCommand = new AsyncRelayCommand(LoadAsync, CanExecuteCommand);
            AddCommand = new RelayCommand(AddItem, CanExecuteCommand);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSaveOrDelete);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanSaveOrDelete);
        }

        public ObservableCollection<MotionIoMapEntity> Items { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand AddCommand { get; private set; }

        public IAsyncRelayCommand SaveCommand { get; private set; }

        public IAsyncRelayCommand DeleteCommand { get; private set; }

        public MotionIoMapEntity SelectedItem
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
            StatusText = "正在加载 IO 映射配置...";

            try
            {
                var result = await Task.Run(() => _motionIoMapCrudService.QueryAll());
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
                StatusText = "IO 映射配置加载完成";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddItem()
        {
            var nextLogicalBit = Items.Count == 0 ? (short)1001 : (short)(Items.Max(p => p.LogicalBit) + 1);

            var item = new MotionIoMapEntity
            {
                CardId = 0,
                IoType = "DI",
                LogicalBit = nextLogicalBit,
                Name = "Io-" + nextLogicalBit,
                Core = 1,
                IsExtModule = false,
                HardwareBit = 0,
                IsEnabled = true,
                SortOrder = Items.Count + 1
            };

            Items.Add(item);
            SelectedItem = item;
            StatusText = "已新增 IO 映射，请保存";
        }

        private async Task SaveAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            StatusText = "正在保存 IO 映射配置...";

            try
            {
                var logicalBit = SelectedItem.LogicalBit;
                var ioType = SelectedItem.IoType;

                var saveResult = await Task.Run(() => _motionIoMapCrudService.Save(SelectedItem));
                if (!saveResult.Success)
                {
                    StatusText = saveResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "IO 映射保存成功并已热重载" : reloadResult.Message;

                await LoadAsync();
                RestoreSelection(logicalBit, ioType);
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

            var logicalBit = SelectedItem.LogicalBit;
            var ioType = SelectedItem.IoType;

            IsBusy = true;
            StatusText = "正在删除 IO 映射配置...";

            try
            {
                var deleteResult = await Task.Run(() => _motionIoMapCrudService.DeleteByLogicalBit(logicalBit, ioType));
                if (!deleteResult.Success)
                {
                    StatusText = deleteResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "IO 映射删除成功并已热重载" : reloadResult.Message;

                await LoadAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RestoreSelection(short logicalBit, string ioType)
        {
            SelectedItem = Items.FirstOrDefault(p => p.LogicalBit == logicalBit && p.IoType == ioType);
        }

        private bool CanExecuteCommand()
        {
            return !IsBusy;
        }

        private bool CanSaveOrDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

        private static MotionIoMapEntity Clone(MotionIoMapEntity source)
        {
            return new MotionIoMapEntity
            {
                Id = source.Id,
                CardId = source.CardId,
                IoType = source.IoType,
                LogicalBit = source.LogicalBit,
                Name = source.Name,
                Core = source.Core,
                IsExtModule = source.IsExtModule,
                HardwareBit = source.HardwareBit,
                IsEnabled = source.IsEnabled,
                SortOrder = source.SortOrder,
                Remark = source.Remark
            };
        }
    }
}