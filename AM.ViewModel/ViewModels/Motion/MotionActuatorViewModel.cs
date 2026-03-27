using AM.Core.Context;
using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.MotionCard;
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
                    NotifyActionCommandState();
                    RefreshCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public bool WaitFeedback
        {
            get { return _waitFeedback; }
            set
            {
                if (SetProperty(ref _waitFeedback, value))
                {
                    NotifyActionCommandState();
                }
            }
        }

        public bool WaitWorkpiece
        {
            get { return _waitWorkpiece; }
            set
            {
                if (SetProperty(ref _waitWorkpiece, value))
                {
                    NotifyActionCommandState();
                }
            }
        }

        public bool StackLightWithBuzzer
        {
            get { return _stackLightWithBuzzer; }
            set
            {
                if (SetProperty(ref _stackLightWithBuzzer, value))
                {
                    NotifyActionCommandState();
                }
            }
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

        public string PrimaryActionButtonText
        {
            get
            {
                if (SelectedItem == null)
                {
                    return "主操作";
                }

                switch (SelectedItem.ActuatorType)
                {
                    case "Cylinder":
                        return SelectedItem.PrimaryState == true && SelectedItem.SecondaryState != true
                            ? "伸出（已到位）"
                            : "伸出";

                    case "Vacuum":
                        if (SelectedItem.PrimaryState == true && (!WaitWorkpiece || SelectedItem.WorkpieceState != false))
                        {
                            return "吸真空（已建立）";
                        }

                        return WaitWorkpiece ? "吸真空+检测" : "吸真空";

                    case "Gripper":
                        if (SelectedItem.PrimaryState == true && (!WaitWorkpiece || SelectedItem.WorkpieceState != false))
                        {
                            return "夹紧（已到位）";
                        }

                        return WaitWorkpiece ? "夹紧+检测" : "夹紧";

                    default:
                        return "主操作";
                }
            }
        }

        public string SecondaryActionButtonText
        {
            get
            {
                if (SelectedItem == null)
                {
                    return "副操作";
                }

                switch (SelectedItem.ActuatorType)
                {
                    case "Cylinder":
                        return SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true
                            ? "缩回（已到位）"
                            : "缩回";

                    case "Vacuum":
                        return SelectedItem.SecondaryState == true || SelectedItem.PrimaryState == false
                            ? "关闭真空（已释放）"
                            : "关闭真空";

                    case "Gripper":
                        return SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true
                            ? "打开（已到位）"
                            : "打开";

                    default:
                        return "副操作";
                }
            }
        }

        public string PrimaryActionToolTipText
        {
            get
            {
                var result = ValidatePrimaryAction();
                return result.Success ? "执行当前主动作" : result.Message;
            }
        }

        public string SecondaryActionToolTipText
        {
            get
            {
                var result = ValidateSecondaryAction();
                return result.Success ? "执行当前副动作" : result.Message;
            }
        }

        public string StackLightOffButtonText
        {
            get
            {
                return IsStackLightCurrentState("Off") ? "熄灭（当前）" : "熄灭";
            }
        }

        public string StackLightIdleButtonText
        {
            get
            {
                return IsStackLightCurrentState("Idle") ? "空闲（当前）" : "空闲";
            }
        }

        public string StackLightRunningButtonText
        {
            get
            {
                return IsStackLightCurrentState("Running") ? "运行（当前）" : "运行";
            }
        }

        public string StackLightWarningButtonText
        {
            get
            {
                return IsStackLightCurrentState("Warning") ? "警告（当前）" : "警告";
            }
        }

        public string StackLightAlarmButtonText
        {
            get
            {
                return IsStackLightCurrentState("Alarm") ? "报警（当前）" : "报警";
            }
        }

        public string StackLightOffToolTipText
        {
            get
            {
                var result = ValidateStackLightState("Off");
                return result.Success ? "切换为全灭状态" : result.Message;
            }
        }

        public string StackLightIdleToolTipText
        {
            get
            {
                var result = ValidateStackLightState("Idle");
                return result.Success ? "切换为空闲状态" : result.Message;
            }
        }

        public string StackLightRunningToolTipText
        {
            get
            {
                var result = ValidateStackLightState("Running");
                return result.Success ? "切换为运行状态" : result.Message;
            }
        }

        public string StackLightWarningToolTipText
        {
            get
            {
                var result = ValidateStackLightState("Warning");
                return result.Success ? "切换为警告状态" : result.Message;
            }
        }

        public string StackLightAlarmToolTipText
        {
            get
            {
                var result = ValidateStackLightState("Alarm");
                return result.Success ? "切换为报警状态" : result.Message;
            }
        }

        private bool IsStackLightCurrentState(string stateKey)
        {
            if (SelectedItem == null || SelectedItem.ActuatorType != "StackLight")
            {
                return false;
            }

            StackLightState state;
            if (!TryParseStackLightState(stateKey, out state))
            {
                return false;
            }

            return IsStackLightAlreadyInTargetState(SelectedItem, state, StackLightWithBuzzer);
        }

        private void RaiseActionUiTextChanged()
        {
            OnPropertyChanged(nameof(PrimaryActionButtonText));
            OnPropertyChanged(nameof(SecondaryActionButtonText));
            OnPropertyChanged(nameof(PrimaryActionToolTipText));
            OnPropertyChanged(nameof(SecondaryActionToolTipText));
            OnPropertyChanged(nameof(StackLightOffButtonText));
            OnPropertyChanged(nameof(StackLightIdleButtonText));
            OnPropertyChanged(nameof(StackLightRunningButtonText));
            OnPropertyChanged(nameof(StackLightWarningButtonText));
            OnPropertyChanged(nameof(StackLightAlarmButtonText));
            OnPropertyChanged(nameof(StackLightOffToolTipText));
            OnPropertyChanged(nameof(StackLightIdleToolTipText));
            OnPropertyChanged(nameof(StackLightRunningToolTipText));
            OnPropertyChanged(nameof(StackLightWarningToolTipText));
            OnPropertyChanged(nameof(StackLightAlarmToolTipText));
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
                    PrimaryOutputBit = item.ExtendOutputBit,
                    SecondaryOutputBit = item.RetractOutputBit,
                    PrimaryFeedbackBit = item.ExtendFeedbackBit,
                    SecondaryFeedbackBit = item.RetractFeedbackBit,
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
                    UseWorkpieceCheck = false,
                    LastActionMessage = "最近操作：未执行",
                    LastActionLevel = "Secondary"
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
                    PrimaryOutputBit = item.VacuumOnOutputBit,
                    SecondaryOutputBit = item.BlowOffOutputBit,
                    PrimaryFeedbackBit = item.VacuumFeedbackBit,
                    SecondaryFeedbackBit = item.ReleaseFeedbackBit,
                    WorkpieceBit = item.WorkpiecePresentBit,
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
                    UseWorkpieceCheck = item.UseWorkpieceCheck,
                    LastActionMessage = "最近操作：未执行",
                    LastActionLevel = "Secondary"
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
                    PrimaryOutputBit = item.CloseOutputBit,
                    SecondaryOutputBit = item.OpenOutputBit,
                    PrimaryFeedbackBit = item.CloseFeedbackBit,
                    SecondaryFeedbackBit = item.OpenFeedbackBit,
                    WorkpieceBit = item.WorkpiecePresentBit,
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
                    UseWorkpieceCheck = item.UseWorkpieceCheck,
                    LastActionMessage = "最近操作：未执行",
                    LastActionLevel = "Secondary"
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
                    RedOutputBit = item.RedOutputBit,
                    YellowOutputBit = item.YellowOutputBit,
                    GreenOutputBit = item.GreenOutputBit,
                    BlueOutputBit = item.BlueOutputBit,
                    BuzzerOutputBit = item.BuzzerOutputBit,
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
                    UseWorkpieceCheck = false,
                    LastActionMessage = "最近操作：未执行",
                    LastActionLevel = "Secondary"
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

            NotifyActionCommandState();
        }

        private void RefreshCylinderState(MotionActuatorDisplayItem item)
        {
            bool? isExtended = TryReadBoolResult(() => _cylinderService.IsExtended(item.Name));
            bool? isRetracted = TryReadBoolResult(() => _cylinderService.IsRetracted(item.Name));

            item.PrimaryState = isExtended;
            item.SecondaryState = isRetracted;
            item.WorkpieceState = null;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置伸出/缩回反馈位";
                item.FooterText = "反馈：— / —";
            }
            else if (isExtended == true && isRetracted == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "伸出与缩回反馈同时为到位";
                item.FooterText = "伸出=Y / 缩回=Y";
                item.HasFault = true;
            }
            else if (isExtended == true)
            {
                item.StateText = "伸出到位";
                item.StateLevel = "Success";
                item.DetailText = "气缸当前处于伸出端";
                item.FooterText = "伸出=Y / 缩回=" + BoolToShortText(isRetracted);
                item.HasFault = false;
            }
            else if (isRetracted == true)
            {
                item.StateText = "缩回到位";
                item.StateLevel = "Primary";
                item.DetailText = "气缸当前处于缩回端";
                item.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=Y";
                item.HasFault = false;
            }
            else if (isExtended == false && isRetracted == false)
            {
                item.StateText = "双未到位";
                item.StateLevel = "Warning";
                item.DetailText = "伸出与缩回反馈均未到位";
                item.FooterText = "伸出=N / 缩回=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断气缸端位";
                item.FooterText = "伸出=" + BoolToShortText(isExtended) + " / 缩回=" + BoolToShortText(isRetracted);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshVacuumState(MotionActuatorDisplayItem item)
        {
            bool? isBuilt = TryReadBoolResult(() => _vacuumService.IsVacuumBuilt(item.Name));
            bool? isReleased = TryReadBoolResult(() => _vacuumService.IsReleased(item.Name));
            bool? hasWorkpiece = TryReadBoolResult(() => _vacuumService.HasWorkpiece(item.Name));

            item.PrimaryState = isBuilt;
            item.SecondaryState = isReleased;
            item.WorkpieceState = hasWorkpiece;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置建压/释放反馈位";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
            }
            else if (isBuilt == true && isReleased == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "建压与释放反馈同时成立";
                item.FooterText = "建压=Y / 释放=Y";
                item.HasFault = true;
            }
            else if (isBuilt == true && hasWorkpiece == true)
            {
                item.StateText = "已吸附";
                item.StateLevel = "Success";
                item.DetailText = "真空已建立，且已检测到工件";
                item.FooterText = "建压=Y / 工件=Y";
                item.HasFault = false;
            }
            else if (isBuilt == true)
            {
                item.StateText = "真空建立";
                item.StateLevel = "Success";
                item.DetailText = "真空已建立";
                item.FooterText = "建压=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isReleased == true)
            {
                item.StateText = "已释放";
                item.StateLevel = "Primary";
                item.DetailText = "真空当前处于释放状态";
                item.FooterText = "释放=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isBuilt == false && isReleased == false)
            {
                item.StateText = "未建压";
                item.StateLevel = "Secondary";
                item.DetailText = "当前既未检测到建压，也未检测到释放到位";
                item.FooterText = "建压=N / 释放=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断真空状态";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshGripperState(MotionActuatorDisplayItem item)
        {
            bool? isClosed = TryReadBoolResult(() => _gripperService.IsClosed(item.Name));
            bool? isOpened = TryReadBoolResult(() => _gripperService.IsOpened(item.Name));
            bool? hasWorkpiece = TryReadBoolResult(() => _gripperService.HasWorkpiece(item.Name));

            item.PrimaryState = isClosed;
            item.SecondaryState = isOpened;
            item.WorkpieceState = hasWorkpiece;

            if (!item.PrimaryFeedbackBit.HasValue && !item.SecondaryFeedbackBit.HasValue)
            {
                item.StateText = "未配反馈";
                item.StateLevel = "Warning";
                item.DetailText = "未配置夹紧/打开反馈位";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
            }
            else if (isClosed == true && isOpened == true)
            {
                item.StateText = "反馈冲突";
                item.StateLevel = "Danger";
                item.DetailText = "夹紧与打开反馈同时成立";
                item.FooterText = "夹紧=Y / 打开=Y";
                item.HasFault = true;
            }
            else if (isClosed == true && hasWorkpiece == true)
            {
                item.StateText = "夹紧有料";
                item.StateLevel = "Success";
                item.DetailText = "夹爪夹紧到位，且已检测到工件";
                item.FooterText = "夹紧=Y / 工件=Y";
                item.HasFault = false;
            }
            else if (isClosed == true)
            {
                item.StateText = "夹紧到位";
                item.StateLevel = "Success";
                item.DetailText = "夹爪已夹紧到位";
                item.FooterText = "夹紧=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isOpened == true)
            {
                item.StateText = "打开到位";
                item.StateLevel = "Primary";
                item.DetailText = "夹爪已打开到位";
                item.FooterText = "打开=Y / 工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }
            else if (isClosed == false && isOpened == false)
            {
                item.StateText = "双未到位";
                item.StateLevel = "Secondary";
                item.DetailText = "夹紧与打开反馈均未到位";
                item.FooterText = "夹紧=N / 打开=N";
                item.HasFault = false;
            }
            else
            {
                item.StateText = "状态未知";
                item.StateLevel = "Secondary";
                item.DetailText = "当前反馈不足以判断夹爪状态";
                item.FooterText = "工件=" + BoolToShortText(hasWorkpiece);
                item.HasFault = false;
            }

            item.RuntimeUpdateTimeText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void RefreshStackLightState(MotionActuatorDisplayItem item)
        {
            item.RedOn = ReadDoState(item.RedOutputBit);
            item.YellowOn = ReadDoState(item.YellowOutputBit);
            item.GreenOn = ReadDoState(item.GreenOutputBit);
            item.BlueOn = ReadDoState(item.BlueOutputBit);
            item.BuzzerOn = ReadDoState(item.BuzzerOutputBit);

            var onSegments = new List<string>();
            if (item.RedOn == true) onSegments.Add("红");
            if (item.YellowOn == true) onSegments.Add("黄");
            if (item.GreenOn == true) onSegments.Add("绿");
            if (item.BlueOn == true) onSegments.Add("蓝");

            if (!item.HasAnyStackLightOutput)
            {
                item.StateText = "未配输出";
                item.StateLevel = "Warning";
                item.DetailText = "未配置任何灯塔输出位";
                item.FooterText = "亮段：—";
                item.HasFault = false;
            }
            else if (onSegments.Count == 0 && item.BuzzerOn != true)
            {
                item.StateText = "全灭";
                item.StateLevel = "Secondary";
                item.DetailText = "当前所有灯段均关闭";
                item.FooterText = "亮段：无 / 蜂鸣=N";
                item.HasFault = false;
            }
            else if (onSegments.Count == 1)
            {
                item.StateText = onSegments[0] + (item.BuzzerOn == true ? "+蜂鸣" : "灯");
                item.StateLevel = item.RedOn == true
                    ? "Danger"
                    : (item.YellowOn == true ? "Warning" : "Success");
                item.DetailText = "红=" + BoolToShortText(item.RedOn)
                    + " / 黄=" + BoolToShortText(item.YellowOn)
                    + " / 绿=" + BoolToShortText(item.GreenOn)
                    + " / 蓝=" + BoolToShortText(item.BlueOn)
                    + " / 鸣=" + BoolToShortText(item.BuzzerOn);
                item.FooterText = "亮段：" + onSegments[0] + " / 蜂鸣=" + BoolToShortText(item.BuzzerOn);
                item.HasFault = false;
            }
            else
            {
                item.StateText = item.BuzzerOn == true ? "多段+蜂鸣" : "多段点亮";
                item.StateLevel = item.RedOn == true
                    ? "Danger"
                    : (item.YellowOn == true ? "Warning" : "Primary");
                item.DetailText = "红=" + BoolToShortText(item.RedOn)
                    + " / 黄=" + BoolToShortText(item.YellowOn)
                    + " / 绿=" + BoolToShortText(item.GreenOn)
                    + " / 蓝=" + BoolToShortText(item.BlueOn)
                    + " / 鸣=" + BoolToShortText(item.BuzzerOn);
                item.FooterText = "亮段：" + string.Join("/", onSegments) + " / 蜂鸣=" + BoolToShortText(item.BuzzerOn);
                item.HasFault = false;
            }

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
                    (!string.IsNullOrWhiteSpace(x.CardLine2Text) && x.CardLine2Text.ToLowerInvariant().Contains(keyword)) ||
                    (!string.IsNullOrWhiteSpace(x.LastActionMessage) && x.LastActionMessage.ToLowerInvariant().Contains(keyword)));
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
            NotifyActionCommandState();

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

            var validate = ValidatePrimaryAction();
            if (!validate.Success)
            {
                ApplyActionResult(validate);
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
                        result = Result.Fail((int)MotionErrorCode.NotImplemented, "当前对象不支持该操作");
                        break;
                }

                ApplyActionResult(result);
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

            var validate = ValidateSecondaryAction();
            if (!validate.Success)
            {
                ApplyActionResult(validate);
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
                        result = Result.Fail((int)MotionErrorCode.NotImplemented, "当前对象不支持该操作");
                        break;
                }

                ApplyActionResult(result);
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

            var validate = ValidateStackLightState(stateKey);
            if (!validate.Success)
            {
                ApplyActionResult(validate);
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
                        result = Result.Fail((int)MotionErrorCode.InvalidIoBit, "不支持的灯塔状态");
                        break;
                }

                ApplyActionResult(result);
            }
            finally
            {
                IsBusy = false;
            }

            RefreshRuntimeStateAfterAction();
        }

        private Result ValidatePrimaryAction()
        {
            if (SelectedItem == null)
            {
                return Result.Fail((int)MotionErrorCode.InvalidIoBit, "请先选择执行器对象");
            }

            switch (SelectedItem.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedItem.PrimaryState == true && SelectedItem.SecondaryState != true)
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前气缸已在伸出到位状态，无需重复伸出");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前气缸未配置伸出反馈位，无法等待伸出到位");
                        }
                    }

                    return Result.Ok();

                case "Vacuum":
                    if (SelectedItem.PrimaryState == true && (!WaitWorkpiece || SelectedItem.WorkpieceState != false))
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前真空已建立，无需重复吸真空");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前真空未配置建压反馈位，无法等待真空建立");
                        }
                    }

                    if (WaitWorkpiece)
                    {
                        if (!SelectedItem.UseWorkpieceCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前真空未启用工件检测校验，请取消“等待工件检测”或调整配置");
                        }

                        if (!SelectedItem.WorkpieceBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前真空未配置工件检测位，无法等待工件检测");
                        }
                    }

                    return Result.Ok();

                case "Gripper":
                    if (SelectedItem.PrimaryState == true && (!WaitWorkpiece || SelectedItem.WorkpieceState != false))
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前夹爪已在夹紧到位状态，无需重复夹紧");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.PrimaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前夹爪未配置夹紧反馈位，无法等待夹紧到位");
                        }
                    }

                    if (WaitWorkpiece)
                    {
                        if (!SelectedItem.UseWorkpieceCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前夹爪未启用工件检测校验，请取消“等待工件检测”或调整配置");
                        }

                        if (!SelectedItem.WorkpieceBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前夹爪未配置工件检测位，无法等待工件检测");
                        }
                    }

                    return Result.Ok();

                default:
                    return Result.Fail((int)MotionErrorCode.NotImplemented, "当前对象不支持主动作");
            }
        }

        private Result ValidateSecondaryAction()
        {
            if (SelectedItem == null)
            {
                return Result.Fail((int)MotionErrorCode.InvalidIoBit, "请先选择执行器对象");
            }

            switch (SelectedItem.ActuatorType)
            {
                case "Cylinder":
                    if (SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true)
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前气缸已在缩回到位状态，无需重复缩回");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前气缸未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前气缸未配置缩回反馈位，无法等待缩回到位");
                        }
                    }

                    return Result.Ok();

                case "Vacuum":
                    if (SelectedItem.SecondaryState == true || SelectedItem.PrimaryState == false)
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前真空已处于释放状态，无需重复关闭真空");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前真空未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue && !SelectedItem.PrimaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前真空未配置释放反馈位或建压反馈位，无法等待真空释放");
                        }
                    }

                    return Result.Ok();

                case "Gripper":
                    if (SelectedItem.SecondaryState == true && SelectedItem.PrimaryState != true)
                    {
                        return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前夹爪已在打开到位状态，无需重复打开");
                    }

                    if (WaitFeedback)
                    {
                        if (!SelectedItem.UseFeedbackCheck)
                        {
                            return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前夹爪未启用反馈校验，请取消“等待反馈”或调整配置");
                        }

                        if (!SelectedItem.SecondaryFeedbackBit.HasValue)
                        {
                            return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前夹爪未配置打开反馈位，无法等待打开到位");
                        }
                    }

                    return Result.Ok();

                default:
                    return Result.Fail((int)MotionErrorCode.NotImplemented, "当前对象不支持副动作");
            }
        }

        private Result ValidateStackLightState(string stateKey)
        {
            if (SelectedItem == null || SelectedItem.ActuatorType != "StackLight")
            {
                return Result.Fail((int)MotionErrorCode.InvalidIoBit, "请先选择灯塔对象");
            }

            if (!SelectedItem.HasAnyStackLightOutput)
            {
                return Result.Fail((int)MotionErrorCode.IoMapNotFound, "当前灯塔未配置任何输出位，无法执行状态切换");
            }

            StackLightState targetState;
            if (!TryParseStackLightState(stateKey, out targetState))
            {
                return Result.Fail((int)MotionErrorCode.InvalidIoBit, "不支持的灯塔状态: " + stateKey);
            }

            if (IsStackLightAlreadyInTargetState(SelectedItem, targetState, StackLightWithBuzzer))
            {
                return Result.Fail((int)MotionErrorCode.InvalidIoBit, "当前灯塔已处于目标状态，无需重复切换");
            }

            return Result.Ok();
        }

        private void ApplyActionResult(Result result)
        {
            if (SelectedItem == null || result == null)
            {
                return;
            }

            var message = BuildLayeredActionMessage(result);
            SelectedItem.LastActionMessage = message;
            SelectedItem.LastActionLevel = result.Success ? "Success" : "Danger";
            SelectedItem.HasFault = !result.Success;
            StatusText = message;
        }

        private string BuildLayeredActionMessage(Result result)
        {
            if (result == null)
            {
                return "执行失败：未返回结果";
            }

            if (result.Success)
            {
                return "操作成功：" + result.Message;
            }

            if (result.Code == (int)MotionErrorCode.IoMapNotFound)
            {
                return "配置错误：" + result.Message;
            }

            if (result.Code == (int)MotionErrorCode.InvalidIoBit)
            {
                return "联锁限制：" + result.Message;
            }

            if (result.Code == (int)MotionErrorCode.HomeTimeout)
            {
                return "动作超时：" + result.Message;
            }

            return "执行失败：" + result.Message;
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
                && SelectedItem.ActuatorType != "StackLight"
                && ValidatePrimaryAction().Success;
        }

        private bool CanExecuteSecondaryAction()
        {
            return !IsBusy
                && SelectedItem != null
                && SelectedItem.HasSecondaryAction
                && SelectedItem.ActuatorType != "StackLight"
                && ValidateSecondaryAction().Success;
        }

        private bool CanSetStackLightState(string stateKey)
        {
            return !IsBusy
                && SelectedItem != null
                && SelectedItem.ActuatorType == "StackLight"
                && !string.IsNullOrWhiteSpace(stateKey)
                && ValidateStackLightState(stateKey).Success;
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

            RaiseActionUiTextChanged();
            NotifyActionCommandState();
        }

        private void NotifyActionCommandState()
        {
            RaiseActionUiTextChanged();
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

        private static bool? ReadDoState(short? logicalBit)
        {
            if (!logicalBit.HasValue)
            {
                return null;
            }

            bool value;
            if (!RuntimeContext.Instance.MotionIo.TryGetDO(logicalBit.Value, out value))
            {
                return null;
            }

            return value;
        }

        private static bool TryParseStackLightState(string stateKey, out StackLightState state)
        {
            state = StackLightState.Off;

            switch (stateKey)
            {
                case "Off":
                    state = StackLightState.Off;
                    return true;
                case "Idle":
                    state = StackLightState.Idle;
                    return true;
                case "Running":
                    state = StackLightState.Running;
                    return true;
                case "Warning":
                    state = StackLightState.Warning;
                    return true;
                case "Alarm":
                    state = StackLightState.Alarm;
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsStackLightAlreadyInTargetState(
            MotionActuatorDisplayItem item,
            StackLightState targetState,
            bool withBuzzer)
        {
            if (item == null)
            {
                return false;
            }

            switch (targetState)
            {
                case StackLightState.Off:
                    return item.RedOn != true
                        && item.YellowOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && item.BuzzerOn != true;

                case StackLightState.Idle:
                case StackLightState.Running:
                    return item.GreenOn == true
                        && item.RedOn != true
                        && item.YellowOn != true
                        && item.BlueOn != true;

                case StackLightState.Warning:
                    return item.YellowOn == true
                        && item.RedOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && (withBuzzer ? item.BuzzerOn == true : true);

                case StackLightState.Alarm:
                    return item.RedOn == true
                        && item.YellowOn != true
                        && item.GreenOn != true
                        && item.BlueOn != true
                        && (withBuzzer ? item.BuzzerOn == true : true);

                default:
                    return false;
            }
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
        private string _lastActionMessage;
        private string _lastActionLevel;
        private bool _hasFault;

        public string ActuatorType { get; set; }

        public string TypeDisplay { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public string ControlModeText { get; set; }

        public short? PrimaryOutputBit { get; set; }

        public short? SecondaryOutputBit { get; set; }

        public short? PrimaryFeedbackBit { get; set; }

        public short? SecondaryFeedbackBit { get; set; }

        public short? WorkpieceBit { get; set; }

        public short? RedOutputBit { get; set; }

        public short? YellowOutputBit { get; set; }

        public short? GreenOutputBit { get; set; }

        public short? BlueOutputBit { get; set; }

        public short? BuzzerOutputBit { get; set; }

        public bool? PrimaryState { get; set; }

        public bool? SecondaryState { get; set; }

        public bool? WorkpieceState { get; set; }

        public bool? RedOn { get; set; }

        public bool? YellowOn { get; set; }

        public bool? GreenOn { get; set; }

        public bool? BlueOn { get; set; }

        public bool? BuzzerOn { get; set; }

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

        public bool HasAnyStackLightOutput
        {
            get
            {
                return RedOutputBit.HasValue
                    || YellowOutputBit.HasValue
                    || GreenOutputBit.HasValue
                    || BlueOutputBit.HasValue
                    || BuzzerOutputBit.HasValue;
            }
        }

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

        public string LastActionMessage
        {
            get { return _lastActionMessage; }
            set { SetProperty(ref _lastActionMessage, value); }
        }

        public string LastActionLevel
        {
            get { return _lastActionLevel; }
            set { SetProperty(ref _lastActionLevel, value); }
        }

        public bool HasFault
        {
            get { return _hasFault; }
            set { SetProperty(ref _hasFault, value); }
        }
    }
}