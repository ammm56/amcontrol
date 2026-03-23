using AM.DBService.Services.Motion.App;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion.Topology;
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
    /// IO 映射配置管理视图模型。
    /// </summary>
    public class MotionIoMapManagementViewModel : ObservableObject
    {
        private readonly IMotionIoMapCrudService _motionIoMapCrudService;
        private readonly IMotionCardCrudService _motionCardCrudService;
        private readonly IMachineConfigReloadService _machineConfigReloadService;

        private MotionIoMapEntity _selectedItem;
        private string _statusText;
        private bool _isBusy;

        public MotionIoMapManagementViewModel()
            : this(new MotionIoMapCrudService(), new MotionCardCrudService(), new MachineConfigReloadService())
        {
        }

        public MotionIoMapManagementViewModel(
            IMotionIoMapCrudService motionIoMapCrudService,
            IMotionCardCrudService motionCardCrudService,
            IMachineConfigReloadService machineConfigReloadService)
        {
            _motionIoMapCrudService = motionIoMapCrudService;
            _motionCardCrudService = motionCardCrudService;
            _machineConfigReloadService = machineConfigReloadService;

            AllItems = new ObservableCollection<MotionIoMapEntity>();
            FilteredItems = new ObservableCollection<MotionIoMapEntity>();
            AvailableCards = new ObservableCollection<MotionCardEntity>();
            _statusText = "请加载 IO 映射配置";

            RefreshCommand = new AsyncRelayCommand(LoadAsync, CanExecute);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSaveOrDelete);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanSaveOrDelete);
        }

        /// <summary>从数据库加载的全量 IO 映射，过滤操作在此基础上进行。</summary>
        public ObservableCollection<MotionIoMapEntity> AllItems { get; private set; }

        /// <summary>当前过滤条件下展示给 View 的列表。</summary>
        public ObservableCollection<MotionIoMapEntity> FilteredItems { get; private set; }

        /// <summary>已配置的控制卡列表，供卡过滤下拉和对话框使用。</summary>
        public ObservableCollection<MotionCardEntity> AvailableCards { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }
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
                    OnPropertyChanged(nameof(SelectedItemCardDisplay));
                }
            }
        }

        /// <summary>详情面板与 Shield 使用：CardId + 卡名称组合显示。</summary>
        public string SelectedItemCardDisplay
        {
            get
            {
                if (_selectedItem == null)
                {
                    return "—";
                }

                var card = AvailableCards.FirstOrDefault(c => c.CardId == _selectedItem.CardId);
                if (card == null)
                {
                    return _selectedItem.CardId.ToString();
                }

                var label = string.IsNullOrWhiteSpace(card.DisplayName) ? card.Name : card.DisplayName;
                return _selectedItem.CardId + " — " + label;
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
                var cardResult = await Task.Run(() => _motionCardCrudService.QueryAll());
                AvailableCards.Clear();
                if (cardResult.Success && cardResult.Items != null)
                {
                    foreach (var card in cardResult.Items)
                    {
                        AvailableCards.Add(card);
                    }
                }

                var result = await Task.Run(() => _motionIoMapCrudService.QueryAll());
                AllItems.Clear();
                FilteredItems.Clear();

                if (!result.Success)
                {
                    StatusText = result.Message;
                    SelectedItem = null;
                    return;
                }

                foreach (var item in result.Items)
                {
                    AllItems.Add(Clone(item));
                }

                StatusText = "IO 映射配置加载完成，共 " + AllItems.Count + " 条";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 根据卡过滤和 IO 类型过滤重建 FilteredItems，由 View 在条件变化时调用。
        /// cardIdFilter = null 表示全部控制卡；ioTypeFilter = "All" 表示全部类型。
        /// </summary>
        public void ApplyFilter(short? cardIdFilter, string ioTypeFilter)
        {
            var prev = _selectedItem;

            FilteredItems.Clear();

            var source = AllItems
                .Where(x => !cardIdFilter.HasValue || x.CardId == cardIdFilter.Value)
                .Where(x => string.IsNullOrEmpty(ioTypeFilter) || ioTypeFilter == "All" || x.IoType == ioTypeFilter)
                .ToList();

            foreach (var item in source)
            {
                FilteredItems.Add(item);
            }

            if (prev != null && FilteredItems.Contains(prev))
            {
                SelectedItem = prev;
            }
            else
            {
                SelectedItem = FilteredItems.Count > 0 ? FilteredItems[0] : null;
            }

            StatusText = "已过滤显示 " + FilteredItems.Count + " 条 IO 映射";
        }

        private async Task SaveAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            StatusText = "正在保存 IO 映射...";

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
            StatusText = "正在删除 IO 映射...";

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
            SelectedItem = FilteredItems.FirstOrDefault(x => x.LogicalBit == logicalBit && x.IoType == ioType)
                ?? (FilteredItems.Count > 0 ? FilteredItems[0] : null);
        }

        private bool CanExecute()
        {
            return !IsBusy;
        }

        private bool CanSaveOrDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

        private static MotionIoMapEntity Clone(MotionIoMapEntity src)
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