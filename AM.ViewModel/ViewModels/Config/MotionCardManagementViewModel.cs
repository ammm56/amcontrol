using AM.DBService.Services;
using AM.Model.Entity.Motion;
using AM.Model.Entity.Motion.Topology;
using AM.Model.Interfaces.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 控制卡管理视图模型。
    /// </summary>
    public class MotionCardManagementViewModel : ObservableObject
    {
        private readonly IMotionCardCrudService _motionCardCrudService;
        private readonly IMachineConfigReloadService _machineConfigReloadService;

        private MotionCardEntity _selectedItem;
        private string _statusText;
        private bool _isBusy;

        public MotionCardManagementViewModel()
            : this(new MotionCardCrudService(), new MachineConfigReloadService())
        {
        }

        public MotionCardManagementViewModel(
            IMotionCardCrudService motionCardCrudService,
            IMachineConfigReloadService machineConfigReloadService)
        {
            _motionCardCrudService = motionCardCrudService;
            _machineConfigReloadService = machineConfigReloadService;

            Items = new ObservableCollection<MotionCardEntity>();
            _statusText = "请加载控制卡配置";

            RefreshCommand = new AsyncRelayCommand(LoadAsync, CanExecuteCommand);
            AddCommand = new RelayCommand(AddItem, CanExecuteCommand);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSaveOrDelete);
            DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanSaveOrDelete);
        }

        public ObservableCollection<MotionCardEntity> Items { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IRelayCommand AddCommand { get; private set; }

        public IAsyncRelayCommand SaveCommand { get; private set; }

        public IAsyncRelayCommand DeleteCommand { get; private set; }

        public MotionCardEntity SelectedItem
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
            StatusText = "正在加载控制卡配置...";

            try
            {
                var result = await Task.Run(() => _motionCardCrudService.QueryAll());
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
                StatusText = "控制卡配置加载完成";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddItem()
        {
            var nextCardId = Items.Count == 0 ? (short)0 : (short)(Items.Max(p => p.CardId) + 1);

            var item = new MotionCardEntity
            {
                CardId = nextCardId,
                CardType = 90,
                Name = "MotionCard-" + nextCardId,
                ModeParam = 0,
                CoreNumber = 1,
                AxisCountNumber = 0,
                UseExtModule = false,
                IsEnabled = true,
                SortOrder = Items.Count + 1
            };

            Items.Add(item);
            SelectedItem = item;
            StatusText = "已新增控制卡，请保存";
        }

        private async Task SaveAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            StatusText = "正在保存控制卡配置...";

            try
            {
                var saveResult = await Task.Run(() => _motionCardCrudService.Save(SelectedItem));
                if (!saveResult.Success)
                {
                    StatusText = saveResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "控制卡保存成功并已热重载" : reloadResult.Message;

                await LoadAsync();
                RestoreSelectionByCardId(SelectedItem.CardId);
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

            var cardId = SelectedItem.CardId;

            IsBusy = true;
            StatusText = "正在删除控制卡配置...";

            try
            {
                var deleteResult = await Task.Run(() => _motionCardCrudService.DeleteByCardId(cardId));
                if (!deleteResult.Success)
                {
                    StatusText = deleteResult.Message;
                    return;
                }

                var reloadResult = await Task.Run(() => _machineConfigReloadService.ReloadAndRebuild());
                StatusText = reloadResult.Success ? "控制卡删除成功并已热重载" : reloadResult.Message;

                await LoadAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void RestoreSelectionByCardId(short cardId)
        {
            SelectedItem = Items.FirstOrDefault(p => p.CardId == cardId);
        }

        private bool CanExecuteCommand()
        {
            return !IsBusy;
        }

        private bool CanSaveOrDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

        private static MotionCardEntity Clone(MotionCardEntity source)
        {
            return new MotionCardEntity
            {
                Id = source.Id,
                CardId = source.CardId,
                CardType = source.CardType,
                Name = source.Name,
                ModeParam = source.ModeParam,
                CoreNumber = source.CoreNumber,
                AxisCountNumber = source.AxisCountNumber,
                UseExtModule = source.UseExtModule,
                IsEnabled = source.IsEnabled,
                SortOrder = source.SortOrder,
                Remark = source.Remark
            };
        }
    }
}