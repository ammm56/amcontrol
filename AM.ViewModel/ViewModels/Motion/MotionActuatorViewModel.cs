using AM.Core.Context;
using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.MotionCard.Actuator;
using AM.Model.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 执行器控制页视图模型。
    /// 运行态对象统一来自 MachineContext；
    /// 动作执行统一走第三层对象服务；
    /// 反馈状态统一读取 RuntimeContext.MotionIo 缓存。
    /// </summary>
    public class MotionActuatorViewModel : ObservableObject
    {
        private static readonly TimeSpan RuntimeUiRefreshMinInterval = TimeSpan.FromMilliseconds(300);

        private readonly CylinderService _cylinderService;
        private readonly VacuumService _vacuumService;
        private readonly GripperService _gripperService;
        private readonly StackLightService _stackLightService;
        private readonly SynchronizationContext _uiContext;
        private readonly List<MotionActuatorDisplayItem> _allItems;

        private MotionActuatorDisplayItem _selectedItem;
        private string _searchText;
        private string _statusText;
        private string _typeFilter;
        private bool _isBusy;
        private bool _isDisposed;
        private bool _waitFeedback;
        private bool _waitWorkpiece;
        private bool _stackLightWithBuzzer;
        private int _totalCount;
        private int _cylinderCount;
        private int _vacuumCount;
        private int _gripperCount;
        private int _stackLightCount;
        private DateTime _lastRuntimeUiRefreshTimeUtc;

        public MotionActuatorViewModel()
        {
            _cylinderService = new CylinderService();
            _vacuumService = new VacuumService();
            _gripperService = new GripperService();
            _stackLightService = new StackLightService();
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();
            _allItems = new List<MotionActuatorDisplayItem>();

            Items = new ObservableCollection<MotionActuatorDisplayItem>();

            _searchText = string.Empty;
            _statusText = "请等待执行器控制页加载";
            _typeFilter = "All";
            _waitFeedback = true;
            _waitWorkpiece = false;
            _stackLightWithBuzzer = false;
            _lastRuntimeUiRefreshTimeUtc = DateTime.MinValue;

            RefreshCommand = new AsyncRelayCommand(RefreshAsync, CanRefresh);
            ChangeTypeFilterCommand = new RelayCommand<string>(ChangeTypeFilter);
            ExecutePrimaryActionCommand = new AsyncRelayCommand(ExecutePrimaryActionAsync, CanExecutePrimaryAction);
            ExecuteSecondaryActionCommand = new AsyncRelayCommand(ExecuteSecondaryActionAsync, CanExecuteSecondaryAction);
            SetStackLightStateCommand = new AsyncRelayCommand<string>(SetStackLightStateAsync, CanSetStackLightState);

            RuntimeContext.Instance.MotionIo.SnapshotChanged += MotionIo_SnapshotChanged;
        }

        public ObservableCollection<MotionActuatorDisplayItem> Items { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public RelayCommand<string> ChangeTypeFilterCommand { get; private set; }

        public IAsyncRelayCommand ExecutePrimaryActionCommand { get; private set; }

        public IAsyncRelayCommand ExecuteSecondaryActionCommand { get; private set; }

        public AsyncRelayCommand<string> SetStackLightStateCommand { get; private set; }

        public MotionActuatorDisplayItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    RaiseSelectionChanged();
                }
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    ApplyFilter(true);
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
                    ExecutePrimaryActionCommand.NotifyCanExecuteChanged();
                    ExecuteSecondaryActionCommand.NotifyCanExecuteChanged();
                    SetStackLightStateCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool WaitFeedback
        {
            get { return _waitFeedback; }
            set { SetProperty(ref _waitFeedback, value); }
        }

        public bool WaitWorkpiece
        {
            get { return _waitWorkpiece; }
            set { SetProperty(ref _waitWorkpiece, value); }
        }

        public bool StackLightWithBuzzer
        {
            get { return _stackLightWithBuzzer; }
            set { SetProperty(ref _stackLightWithBuzzer, value); }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { SetProperty(ref _totalCount, value); }
        }

        public int CylinderCount
        {
            get { return _cylinderCount; }
            set { SetProperty(ref _cylinderCount, value); }
        }

        public int VacuumCount
        {
            get { return _vacuumCount; }
            set { SetProperty(ref _vacuumCount, value); }
        }

        public int GripperCount
        {
            get { return _gripperCount; }
            set { SetProperty(ref _gripperCount, value); }
        }

        public int StackLightCount
        {
            get { return _stackLightCount; }
            set { SetProperty(ref _stackLightCount, value); }
        }

        public bool IsFilterAll
        {
            get { return _typeFilter == "All"; }
        }

        public bool IsFilterCylinder
        {
            get { return _typeFilter == "Cylinder"; }
        }

        public bool IsFilterVacuum
        {
            get { return _typeFilter == "Vacuum"; }
        }

        public bool IsFilterGripper
        {
            get { return _typeFilter == "Gripper"; }
        }

        public bool IsFilterStackLight
        {
            get { return _typeFilter == "StackLight"; }
        }

        public bool HasItems
        {
            get { return Items.Count > 0; }
        }

        public bool IsCylinderSelected
        {
            get { return SelectedItem != null && SelectedItem.ActuatorType == "Cylinder"; }
        }

        public bool IsVacuumSelected
        {
            get { return SelectedItem != null && SelectedItem.ActuatorType == "Vacuum"; }
        }

        public bool IsGripperSelected
        {
            get { return SelectedItem != null && SelectedItem.ActuatorType == "Gripper"; }
        }

        public bool IsStackLightSelected
        {
            get { return SelectedItem != null && SelectedItem.ActuatorType == "StackLight"; }
        }

        public bool CanUseWaitWorkpiece
        {
            get { return IsVacuumSelected || IsGripperSelected; }
        }

        public string SelectedItemHeader
        {
            get
            {
                if (SelectedItem == null)
                {
                    return "当前对象：未选择";
                }

                return SelectedItem.TypeDisplay + " / " + SelectedItem.DisplayTitle;
            }
        }

        public string SelectedItemSubHeader
        {
            get
            {
                if (SelectedItem == null)
                {
                    return "—";
                }

                return SelectedItem.Name;
            }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// 注意：当前页面由 MainWindow 缓存复用。
        /// 正常导航切换时不要调用本方法，否则会断开运行态订阅。
        /// 本方法仅适用于页面实例真正销毁时。
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            RuntimeContext.Instance.MotionIo.SnapshotChanged -= MotionIo_SnapshotChanged;
            _isDisposed = true;
        }

        public async Task RefreshAsync()
        {
            if (IsBusy || _isDisposed)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var previousType = SelectedItem == null ? null : SelectedItem.ActuatorType;
                var previousName = SelectedItem == null ? null : SelectedItem.Name;

                var items = await Task.Run(BuildAllItemsFromMachineContext);

                _allItems.Clear();
                _allItems.AddRange(items);

                RefreshRuntimeStateCore(_allItems);
                ApplyFilter(false, previousType, previousName);

                _lastRuntimeUiRefreshTimeUtc = DateTime.UtcNow;
                StatusText = string.Format("执行器控制页已刷新，共加载 {0} 个对象", _allItems.Count);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<MotionActuatorDisplayItem> BuildAllItemsFromMachineContext()
        {
            var machine = MachineContext.Instance;
            var list = new List<MotionActuatorDisplayItem>();

            foreach (var item in machine.Cylinders.Values
                .Where(x => x != null && x.IsEnabled)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name))
            {
                list.Add(new MotionActuatorDisplayItem
                {
                    ActuatorType = "Cylinder",
                    TypeDisplay = "气缸",
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    IsEnabled = item.IsEnabled,
                    SortOrder = item.SortOrder,
                    Description = item.Description,
                    Remark = item.Remark,
                    ControlModeText = ResolveDriveModeText(item.DriveMode),
                    PrimaryOutputText = FormatIoText("伸出输出", "DO", item.ExtendOutputBit),
                    SecondaryOutputText = FormatIoText("缩回输出", "DO", item.RetractOutputBit),
                    PrimaryFeedbackText = FormatIoText("伸出反馈", "DI", item.ExtendFeedbackBit),
                    SecondaryFeedbackText = FormatIoText("缩回反馈", "DI", item.RetractFeedbackBit),
                    WorkpieceText = "工件检测：—",
                    TimeoutText = "伸/缩超时：" + item.ExtendTimeoutMs + " / " + item.RetractTimeoutMs + " ms",
                    CardLine1Text = "模式：" + ResolveDriveModeText(item.DriveMode),
                    CardLine2Text = "DO：伸 " + item.ExtendOutputBit + " / 缩 " + FormatNullableBit(item.RetractOutputBit),
                    PrimaryActionText = "伸出",
                    SecondaryActionText = "缩回",
                    HasSecondaryAction = true,
                    UseFeedbackCheck = item.UseFeedbackCheck,
                    UseWorkpieceCheck = false
                });
            }

            foreach (var item in machine.Vacuums.Values
                .Where(x => x != null && x.IsEnabled)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name))
            {
                list.Add(new MotionActuatorDisplayItem
                {
                    ActuatorType = "Vacuum",
                    TypeDisplay = "真空",
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    IsEnabled = item.IsEnabled,
                    SortOrder = item.SortOrder,
                    Description = item.Description,
                    Remark = item.Remark,
                    ControlModeText = item.KeepVacuumOnAfterDetected ? "吸附保持" : "普通释放",
                    PrimaryOutputText = FormatIoText("吸真空输出", "DO", item.VacuumOnOutputBit),
                    SecondaryOutputText = FormatIoText("破真空输出", "DO", item.BlowOffOutputBit),
                    PrimaryFeedbackText = FormatIoText("建压反馈", "DI", item.VacuumFeedbackBit),
                    SecondaryFeedbackText = FormatIoText("释放反馈", "DI", item.ReleaseFeedbackBit),
                    WorkpieceText = FormatIoText("工件检测", "DI", item.WorkpiecePresentBit),
                    TimeoutText = "建压/释放超时：" + item.VacuumBuildTimeoutMs + " / " + item.ReleaseTimeoutMs + " ms",
                    CardLine1Text = "模式：" + (item.KeepVacuumOnAfterDetected ? "保持吸附" : "允许释放"),
                    CardLine2Text = "DO：吸 " + item.VacuumOnOutputBit + " / 破 " + FormatNullableBit(item.BlowOffOutputBit),
                    PrimaryActionText = "吸真空",
                    SecondaryActionText = "关闭真空",
                    HasSecondaryAction = true,
                    UseFeedbackCheck = item.UseFeedbackCheck,
                    UseWorkpieceCheck = item.UseWorkpieceCheck
                });
            }

            foreach (var item in machine.Grippers.Values
                .Where(x => x != null && x.IsEnabled)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name))
            {
                list.Add(new MotionActuatorDisplayItem
                {
                    ActuatorType = "Gripper",
                    TypeDisplay = "夹爪",
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    IsEnabled = item.IsEnabled,
                    SortOrder = item.SortOrder,
                    Description = item.Description,
                    Remark = item.Remark,
                    ControlModeText = ResolveDriveModeText(item.DriveMode),
                    PrimaryOutputText = FormatIoText("夹紧输出", "DO", item.CloseOutputBit),
                    SecondaryOutputText = FormatIoText("打开输出", "DO", item.OpenOutputBit),
                    PrimaryFeedbackText = FormatIoText("夹紧反馈", "DI", item.CloseFeedbackBit),
                    SecondaryFeedbackText = FormatIoText("打开反馈", "DI", item.OpenFeedbackBit),
                    WorkpieceText = FormatIoText("工件检测", "DI", item.WorkpiecePresentBit),
                    TimeoutText = "夹/开超时：" + item.CloseTimeoutMs + " / " + item.OpenTimeoutMs + " ms",
                    CardLine1Text = "模式：" + ResolveDriveModeText(item.DriveMode),
                    CardLine2Text = "DO：夹 " + item.CloseOutputBit + " / 开 " + FormatNullableBit(item.OpenOutputBit),
                    PrimaryActionText = "夹紧",
                    SecondaryActionText = "打开",
                    HasSecondaryAction = true,
                    UseFeedbackCheck = item.UseFeedbackCheck,
                    UseWorkpieceCheck = item.UseWorkpieceCheck
                });
            }

            foreach (var item in machine.StackLights.Values
                .Where(x => x != null && x.IsEnabled)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name))
            {
                list.Add(new MotionActuatorDisplayItem
                {
                    ActuatorType = "StackLight",
                    TypeDisplay = "灯塔",
                    Name = item.Name,
                    DisplayName = item.DisplayName,
                    IsEnabled = item.IsEnabled,
                    SortOrder = item.SortOrder,
                    Description = item.Description,
                    Remark = item.Remark,
                    ControlModeText = item.AllowMultiSegmentOn ? "允许多段同亮" : "单段控制",
                    PrimaryOutputText = FormatIoText("红灯输出", "DO", item.RedOutputBit),
                    SecondaryOutputText = FormatIoText("黄灯输出", "DO", item.YellowOutputBit),
                    PrimaryFeedbackText = FormatIoText("绿灯输出", "DO", item.GreenOutputBit),
                    SecondaryFeedbackText = FormatIoText("蓝灯输出", "DO", item.BlueOutputBit),
                    WorkpieceText = FormatIoText("蜂鸣器输出", "DO", item.BuzzerOutputBit),
                    TimeoutText = "蜂鸣联动：警告="
                        + BoolToChinese(item.EnableBuzzerOnWarning)
                        + " / 报警="
                        + BoolToChinese(item.EnableBuzzerOnAlarm),
                    CardLine1Text = "红/黄："
                        + FormatNullableBit(item.RedOutputBit) + " / "
                        + FormatNullableBit(item.YellowOutputBit),
                    CardLine2Text = "绿/蓝/鸣："
                        + FormatNullableBit(item.GreenOutputBit) + " / "
                        + FormatNullableBit(item.BlueOutputBit) + " / "
                        + FormatNullableBit(item.BuzzerOutputBit),
                    PrimaryActionText = string.Empty,
                    SecondaryActionText = string.Empty,
                    HasSecondaryAction = false,
                    UseFeedbackCheck = false,
                    UseWorkpieceCheck = false
                });
            }

            return list
                .OrderBy(x => GetActuatorTypeSort(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();
        }

        private void MotionIo_SnapshotChanged()
        {
            if (_isDisposed || IsBusy)
            {
                return;
            }

            var nowUtc = DateTime.UtcNow;
            if (nowUtc - _lastRuntimeUiRefreshTimeUtc < RuntimeUiRefreshMinInterval)
            {
                return;
            }

            _lastRuntimeUiRefreshTimeUtc = nowUtc;

            _uiContext.Post(_ =>
            {
                if (_isDisposed || IsBusy)
                {
                    return;
                }

                var previousType = SelectedItem == null ? null : SelectedItem.ActuatorType;
                var previousName = SelectedItem == null ? null : SelectedItem.Name;

                RefreshRuntimeStateCore(_allItems);
                ApplyFilter(false, previousType, previousName);
            }, null);
        }

        private void RefreshRuntimeStateCore(IEnumerable<MotionActuatorDisplayItem> items)
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                switch (item.ActuatorType)
                {
                    case "Cylinder":
                        RefreshCylinderState(item);
                        break;

                    case "Vacuum":
                        RefreshVacuumState(item);
                        break;

                    case "Gripper":
                        RefreshGripperState(item);
                        break;

                    case "StackLight":
                        RefreshStackLightState(item);
                        break;
                }
            }
        }

        private void RefreshCylinderState(MotionActuatorDisplayItem item)
        {
            bool? isExtended = TryReadBoolResult(() => _cylinderService.IsExtended(item.Name));
            bool? isRetracted = TryReadBoolResult(() => _cylinderService.IsRetracted(item.Name));

            if (isExtended == true)
            {
                item.StateText = "已伸出";
                item.StateLevel = "Success";
            }
            else if (isRetracted == true)
            {
                item.StateText = "已缩回";
                item.StateLevel = "Primary";
            }
            else
            {
                item.StateText = "未知";
                item.StateLevel = "Secondary";
            }

            item.DetailText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=" + BoolToShortText(isRetracted);
            item.FooterText = "反馈：" + BoolToShortText(isExtended) + " / " + BoolToShortText(isRetracted);
            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshVacuumState(MotionActuatorDisplayItem item)
        {
            bool? isBuilt = TryReadBoolResult(() => _vacuumService.IsVacuumBuilt(item.Name));
            bool? isReleased = TryReadBoolResult(() => _vacuumService.IsReleased(item.Name));
            bool? hasWorkpiece = TryReadBoolResult(() => _vacuumService.HasWorkpiece(item.Name));

            if (isBuilt == true)
            {
                item.StateText = "已建压";
                item.StateLevel = "Success";
            }
            else if (isReleased == true)
            {
                item.StateText = "已释放";
                item.StateLevel = "Primary";
            }
            else
            {
                item.StateText = "未知";
                item.StateLevel = "Secondary";
            }

            item.DetailText = "建压=" + BoolToShortText(isBuilt) + " / 释放=" + BoolToShortText(isReleased);
            item.FooterText = "工件：" + BoolToShortText(hasWorkpiece);
            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshGripperState(MotionActuatorDisplayItem item)
        {
            bool? isClosed = TryReadBoolResult(() => _gripperService.IsClosed(item.Name));
            bool? isOpened = TryReadBoolResult(() => _gripperService.IsOpened(item.Name));
            bool? hasWorkpiece = TryReadBoolResult(() => _gripperService.HasWorkpiece(item.Name));

            if (isClosed == true)
            {
                item.StateText = "已夹紧";
                item.StateLevel = "Success";
            }
            else if (isOpened == true)
            {
                item.StateText = "已打开";
                item.StateLevel = "Primary";
            }
            else
            {
                item.StateText = "未知";
                item.StateLevel = "Secondary";
            }

            item.DetailText = "夹紧=" + BoolToShortText(isClosed) + " / 打开=" + BoolToShortText(isOpened);
            item.FooterText = "工件：" + BoolToShortText(hasWorkpiece);
            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshStackLightState(MotionActuatorDisplayItem item)
        {
            var red = ReadDoState(item.PrimaryOutputText);
            var yellow = ReadDoState(item.SecondaryOutputText);
            var green = ReadDoState(item.PrimaryFeedbackText);
            var blue = ReadDoState(item.SecondaryFeedbackText);
            var buzzer = ReadDoState(item.WorkpieceText);

            var onSegments = new List<string>();
            if (red == true) onSegments.Add("红");
            if (yellow == true) onSegments.Add("黄");
            if (green == true) onSegments.Add("绿");
            if (blue == true) onSegments.Add("蓝");

            if (onSegments.Count == 0)
            {
                item.StateText = "已关闭";
                item.StateLevel = "Secondary";
            }
            else
            {
                item.StateText = string.Join("/", onSegments);
                item.StateLevel = red == true ? "Danger" : (yellow == true ? "Warning" : "Success");
            }

            item.DetailText = "蜂鸣=" + BoolToShortText(buzzer);
            item.FooterText = "亮段：" + (onSegments.Count == 0 ? "无" : string.Join("/", onSegments));
            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void ChangeTypeFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "All";
            }

            if (string.Equals(_typeFilter, filter, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _typeFilter = filter;
            OnPropertyChanged(nameof(IsFilterAll));
            OnPropertyChanged(nameof(IsFilterCylinder));
            OnPropertyChanged(nameof(IsFilterVacuum));
            OnPropertyChanged(nameof(IsFilterGripper));
            OnPropertyChanged(nameof(IsFilterStackLight));

            ApplyFilter(true);
        }

        private void ApplyFilter(bool updateStatusText)
        {
            var previousType = SelectedItem == null ? null : SelectedItem.ActuatorType;
            var previousName = SelectedItem == null ? null : SelectedItem.Name;
            ApplyFilter(updateStatusText, previousType, previousName);
        }

        private void ApplyFilter(bool updateStatusText, string previousType, string previousName)
        {
            IEnumerable<MotionActuatorDisplayItem> query = _allItems;

            if (_typeFilter != "All")
            {
                query = query.Where(x => x.ActuatorType == _typeFilter);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var keyword = SearchText.Trim().ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.DisplayName) && x.DisplayName.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.TypeDisplay) && x.TypeDisplay.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.StateText) && x.StateText.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.CardLine1Text) && x.CardLine1Text.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.CardLine2Text) && x.CardLine2Text.ToLowerInvariant().Contains(keyword)));
            }

            var list = query
                .OrderBy(x => GetActuatorTypeSort(x.ActuatorType))
                .ThenBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToList();

            Items.Clear();
            foreach (var item in list)
            {
                Items.Add(item);
            }

            UpdateSummary(list);

            MotionActuatorDisplayItem selected = null;
            if (!string.IsNullOrWhiteSpace(previousType) && !string.IsNullOrWhiteSpace(previousName))
            {
                selected = Items.FirstOrDefault(x => x.ActuatorType == previousType && x.Name == previousName);
            }

            if (selected == null && Items.Count > 0)
            {
                selected = Items[0];
            }

            SelectedItem = selected;
            OnPropertyChanged(nameof(HasItems));

            if (updateStatusText)
            {
                StatusText = string.Format("当前筛选显示 {0} 个执行器对象", Items.Count);
            }
        }

        private void UpdateSummary(IList<MotionActuatorDisplayItem> list)
        {
            var source = list ?? new List<MotionActuatorDisplayItem>();

            TotalCount = source.Count;
            CylinderCount = source.Count(x => x.ActuatorType == "Cylinder");
            VacuumCount = source.Count(x => x.ActuatorType == "Vacuum");
            GripperCount = source.Count(x => x.ActuatorType == "Gripper");
            StackLightCount = source.Count(x => x.ActuatorType == "StackLight");
        }

        private async Task ExecutePrimaryActionAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Result result;

                switch (SelectedItem.ActuatorType)
                {
                    case "Cylinder":
                        result = await Task.Run(() => _cylinderService.Extend(SelectedItem.Name, WaitFeedback));
                        break;

                    case "Vacuum":
                        result = await Task.Run(() => _vacuumService.VacuumOn(SelectedItem.Name, WaitFeedback, WaitWorkpiece));
                        break;

                    case "Gripper":
                        result = await Task.Run(() => _gripperService.Close(SelectedItem.Name, WaitFeedback, WaitWorkpiece));
                        break;

                    default:
                        result = Result.Fail(-1, "当前对象不支持该操作");
                        break;
                }

                StatusText = result.Message;
            }
            finally
            {
                IsBusy = false;
            }

            RefreshRuntimeStateAfterAction();
        }

        private async Task ExecuteSecondaryActionAsync()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Result result;

                switch (SelectedItem.ActuatorType)
                {
                    case "Cylinder":
                        result = await Task.Run(() => _cylinderService.Retract(SelectedItem.Name, WaitFeedback));
                        break;

                    case "Vacuum":
                        result = await Task.Run(() => _vacuumService.VacuumOff(SelectedItem.Name, WaitFeedback));
                        break;

                    case "Gripper":
                        result = await Task.Run(() => _gripperService.Open(SelectedItem.Name, WaitFeedback));
                        break;

                    default:
                        result = Result.Fail(-1, "当前对象不支持该操作");
                        break;
                }

                StatusText = result.Message;
            }
            finally
            {
                IsBusy = false;
            }

            RefreshRuntimeStateAfterAction();
        }

        private async Task SetStackLightStateAsync(string stateKey)
        {
            if (SelectedItem == null || SelectedItem.ActuatorType != "StackLight")
            {
                return;
            }

            IsBusy = true;

            try
            {
                Result result;
                switch (stateKey)
                {
                    case "Off":
                        result = await Task.Run(() => _stackLightService.TurnOff(SelectedItem.Name));
                        break;

                    case "Idle":
                        result = await Task.Run(() => _stackLightService.SetIdle(SelectedItem.Name));
                        break;

                    case "Running":
                        result = await Task.Run(() => _stackLightService.SetRunning(SelectedItem.Name));
                        break;

                    case "Warning":
                        result = await Task.Run(() => _stackLightService.SetWarning(SelectedItem.Name, StackLightWithBuzzer));
                        break;

                    case "Alarm":
                        result = await Task.Run(() => _stackLightService.SetAlarm(SelectedItem.Name, StackLightWithBuzzer));
                        break;

                    default:
                        result = Result.Fail(-1, "不支持的灯塔状态");
                        break;
                }

                StatusText = result.Message;
            }
            finally
            {
                IsBusy = false;
            }

            RefreshRuntimeStateAfterAction();
        }

        private void RefreshRuntimeStateAfterAction()
        {
            var previousType = SelectedItem == null ? null : SelectedItem.ActuatorType;
            var previousName = SelectedItem == null ? null : SelectedItem.Name;

            RefreshRuntimeStateCore(_allItems);
            ApplyFilter(false, previousType, previousName);
        }

        private bool CanRefresh()
        {
            return !IsBusy && !_isDisposed;
        }

        private bool CanExecutePrimaryAction()
        {
            return !IsBusy
                && SelectedItem != null
                && SelectedItem.ActuatorType != "StackLight";
        }

        private bool CanExecuteSecondaryAction()
        {
            return !IsBusy
                && SelectedItem != null
                && SelectedItem.HasSecondaryAction
                && SelectedItem.ActuatorType != "StackLight";
        }

        private bool CanSetStackLightState(string stateKey)
        {
            return !IsBusy
                && SelectedItem != null
                && SelectedItem.ActuatorType == "StackLight"
                && !string.IsNullOrWhiteSpace(stateKey);
        }

        private void RaiseSelectionChanged()
        {
            OnPropertyChanged(nameof(IsCylinderSelected));
            OnPropertyChanged(nameof(IsVacuumSelected));
            OnPropertyChanged(nameof(IsGripperSelected));
            OnPropertyChanged(nameof(IsStackLightSelected));
            OnPropertyChanged(nameof(CanUseWaitWorkpiece));
            OnPropertyChanged(nameof(SelectedItemHeader));
            OnPropertyChanged(nameof(SelectedItemSubHeader));

            ExecutePrimaryActionCommand.NotifyCanExecuteChanged();
            ExecuteSecondaryActionCommand.NotifyCanExecuteChanged();
            SetStackLightStateCommand.NotifyCanExecuteChanged();
        }

        private static int GetActuatorTypeSort(string actuatorType)
        {
            switch (actuatorType)
            {
                case "Cylinder": return 10;
                case "Vacuum": return 20;
                case "Gripper": return 30;
                case "StackLight": return 40;
                default: return 100;
            }
        }

        private static string ResolveDriveModeText(string driveMode)
        {
            if (string.Equals(driveMode, "Double", StringComparison.OrdinalIgnoreCase))
            {
                return "双线圈";
            }

            if (string.Equals(driveMode, "Single", StringComparison.OrdinalIgnoreCase))
            {
                return "单线圈";
            }

            return string.IsNullOrWhiteSpace(driveMode) ? "—" : driveMode;
        }

        private static string FormatIoText(string label, string ioType, short bit)
        {
            return label + "：" + ioType + " L#" + bit;
        }

        private static string FormatIoText(string label, string ioType, short? bit)
        {
            return label + "：" + (bit.HasValue ? ioType + " L#" + bit.Value : "—");
        }

        private static string FormatNullableBit(short? bit)
        {
            return bit.HasValue ? bit.Value.ToString() : "—";
        }

        private static string BoolToChinese(bool value)
        {
            return value ? "是" : "否";
        }

        private static string BoolToShortText(bool? value)
        {
            if (!value.HasValue)
            {
                return "—";
            }

            return value.Value ? "Y" : "N";
        }

        private static bool? TryReadBoolResult(Func<Result<bool>> func)
        {
            var result = func();
            if (!result.Success)
            {
                return null;
            }

            return result.Item;
        }

        private static bool? ReadDoState(string ioText)
        {
            short logicalBit;
            if (!TryParseLogicalBit(ioText, out logicalBit))
            {
                return null;
            }

            bool value;
            if (!RuntimeContext.Instance.MotionIo.TryGetDO(logicalBit, out value))
            {
                return null;
            }

            return value;
        }

        private static bool TryParseLogicalBit(string text, out short logicalBit)
        {
            logicalBit = 0;

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var index = text.LastIndexOf("L#", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return false;
            }

            var numberText = text.Substring(index + 2).Trim();
            return short.TryParse(numberText, out logicalBit);
        }
    }

    /// <summary>
    /// 执行器卡片显示项。
    /// </summary>
    public class MotionActuatorDisplayItem : ObservableObject
    {
        private string _stateText;
        private string _stateLevel;
        private string _detailText;
        private string _footerText;
        private string _runtimeUpdateTimeText;

        public string ActuatorType { get; set; }

        public string TypeDisplay { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string ControlModeText { get; set; }

        public string PrimaryOutputText { get; set; }

        public string SecondaryOutputText { get; set; }

        public string PrimaryFeedbackText { get; set; }

        public string SecondaryFeedbackText { get; set; }

        public string WorkpieceText { get; set; }

        public string TimeoutText { get; set; }

        public string CardLine1Text { get; set; }

        public string CardLine2Text { get; set; }

        public string PrimaryActionText { get; set; }

        public string SecondaryActionText { get; set; }

        public bool HasSecondaryAction { get; set; }

        public bool UseFeedbackCheck { get; set; }

        public bool UseWorkpieceCheck { get; set; }

        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(DisplayName))
                {
                    return DisplayName;
                }

                return string.IsNullOrWhiteSpace(Name) ? "未命名对象" : Name;
            }
        }

        public string StateText
        {
            get { return _stateText; }
            set { SetProperty(ref _stateText, value); }
        }

        public string StateLevel
        {
            get { return _stateLevel; }
            set { SetProperty(ref _stateLevel, value); }
        }

        public string DetailText
        {
            get { return _detailText; }
            set { SetProperty(ref _detailText, value); }
        }

        public string FooterText
        {
            get { return _footerText; }
            set { SetProperty(ref _footerText, value); }
        }

        public string RuntimeUpdateTimeText
        {
            get { return _runtimeUpdateTimeText; }
            set { SetProperty(ref _runtimeUpdateTimeText, value); }
        }
    }
}