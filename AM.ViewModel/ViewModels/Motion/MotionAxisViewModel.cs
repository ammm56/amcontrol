using AM.Core.Context;
using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Model.Motion;
using AM.Model.MotionCard;
using AM.Model.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 单轴控制页视图模型。
    /// 轴列表与控制卡为静态结构，运行态完全来自 RuntimeContext.Instance.MotionAxis。
    /// </summary>
    public class MotionAxisViewModel : ObservableObject
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly MotionAxisOperationService _operationService;
        private readonly List<MotionAxisDisplayItem> _allAxisItems;
        private readonly SynchronizationContext _uiContext;

        private MotionCardFilterItem _selectedCardFilter;
        private MotionAxisDisplayItem _selectedAxisItem;
        private string _targetPositionMmText;
        private string _moveDistanceMmText;
        private string _velocityMmText;
        private string _statusText;
        private string _lastOperationText;
        private string _operationHintText;
        private bool _isBusy;
        private bool _isRefreshingCardFilters;
        private bool _isDisposed;
        private bool _suppressVelocityReset;

        public MotionAxisViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _operationService = new MotionAxisOperationService();
            _allAxisItems = new List<MotionAxisDisplayItem>();
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();

            CardFilters = new ObservableCollection<MotionCardFilterItem>();
            AxisItems = new ObservableCollection<MotionAxisDisplayItem>();

            _targetPositionMmText = "0";
            _moveDistanceMmText = "10";
            _velocityMmText = "10";
            _statusText = "请选择控制卡和轴";
            _lastOperationText = "最近操作：未执行";
            _operationHintText = "请选择轴后执行操作";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync, CanRefresh);
            ServoOnCommand = new AsyncRelayCommand(ServoOnAsync, CanServoOn);
            ServoOffCommand = new AsyncRelayCommand(ServoOffAsync, CanServoOff);
            StopCommand = new AsyncRelayCommand(StopAsync, CanStop);
            EmergencyStopCommand = new AsyncRelayCommand(EmergencyStopAsync, CanEmergencyStop);
            HomeCommand = new AsyncRelayCommand(HomeAsync, CanHome);
            ClearStatusCommand = new AsyncRelayCommand(ClearStatusAsync, CanClearStatus);
            JogNegativeCommand = new AsyncRelayCommand(JogNegativeAsync, CanJogNegative);
            JogStopCommand = new AsyncRelayCommand(JogStopAsync, CanJogStop);
            JogPositiveCommand = new AsyncRelayCommand(JogPositiveAsync, CanJogPositive);
            MoveAbsoluteCommand = new AsyncRelayCommand(MoveAbsoluteAsync, CanMoveAbsolute);
            MoveRelativeCommand = new AsyncRelayCommand(MoveRelativeAsync, CanMoveRelative);
            ApplyVelocityCommand = new AsyncRelayCommand(ApplyVelocityAsync, CanApplyVelocity);

            RuntimeContext.Instance.MotionAxis.SnapshotChanged += MotionAxis_SnapshotChanged;
        }

        public ObservableCollection<MotionCardFilterItem> CardFilters { get; private set; }

        public ObservableCollection<MotionAxisDisplayItem> AxisItems { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IAsyncRelayCommand ServoOnCommand { get; private set; }

        public IAsyncRelayCommand ServoOffCommand { get; private set; }

        public IAsyncRelayCommand StopCommand { get; private set; }

        public IAsyncRelayCommand EmergencyStopCommand { get; private set; }

        public IAsyncRelayCommand HomeCommand { get; private set; }

        public IAsyncRelayCommand ClearStatusCommand { get; private set; }

        public IAsyncRelayCommand JogNegativeCommand { get; private set; }

        public IAsyncRelayCommand JogStopCommand { get; private set; }

        public IAsyncRelayCommand JogPositiveCommand { get; private set; }

        public IAsyncRelayCommand MoveAbsoluteCommand { get; private set; }

        public IAsyncRelayCommand MoveRelativeCommand { get; private set; }

        public IAsyncRelayCommand ApplyVelocityCommand { get; private set; }

        public MotionCardFilterItem SelectedCardFilter
        {
            get { return _selectedCardFilter; }
            set
            {
                if (IsSameCardFilter(_selectedCardFilter, value))
                {
                    return;
                }

                if (SetProperty(ref _selectedCardFilter, value))
                {
                    OnPropertyChanged(nameof(SelectedCardHeader));

                    if (!_isRefreshingCardFilters)
                    {
                        RebuildAxisItems(null);
                    }
                }
            }
        }

        public MotionAxisDisplayItem SelectedAxisItem
        {
            get { return _selectedAxisItem; }
            set
            {
                var previousAxis = _selectedAxisItem == null ? (short?)null : _selectedAxisItem.LogicalAxis;

                if (SetProperty(ref _selectedAxisItem, value))
                {
                    var currentAxis = _selectedAxisItem == null ? (short?)null : _selectedAxisItem.LogicalAxis;

                    if (!_suppressVelocityReset && currentAxis.HasValue && currentAxis != previousAxis)
                    {
                        VelocityMmText = _selectedAxisItem.DefaultVelocityMm > 0
                            ? _selectedAxisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture)
                            : "10";
                    }

                    RefreshSelectedAxisRuntimeDisplay();
                    UpdateOperationHint();
                    NotifyCommandState();
                }
            }
        }

        public string AxisRuntimeScanStateText
        {
            get
            {
                var runtime = RuntimeContext.Instance.MotionAxis;
                return runtime.IsScanServiceRunning
                    ? "运行中 / " + runtime.ScanIntervalMs + "ms"
                    : "已停止";
            }
        }

        public string SelectedAxisRuntimeUpdateTimeText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null
                    ? "—"
                    : snapshot.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedCardHeader
        {
            get
            {
                if (SelectedCardFilter == null)
                {
                    return "当前控制卡：未选择";
                }

                return "当前控制卡：" + SelectedCardFilter.DisplayText;
            }
        }

        public string SelectedAxisHeader
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "当前轴：未选择";
                }

                return "当前轴：L#" + SelectedAxisItem.LogicalAxis + "  " + SelectedAxisItem.DisplayTitle;
            }
        }

        public string SelectedAxisCardText
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "—";
                }

                return "卡#" + SelectedAxisItem.CardId + "  "
                    + (string.IsNullOrWhiteSpace(SelectedAxisItem.CardDisplayName)
                        ? "未命名控制卡"
                        : SelectedAxisItem.CardDisplayName);
            }
        }

        public string SelectedAxisStatusText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null ? "—" : snapshot.StateText;
            }
        }

        public string SelectedAxisStateDisplayText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot == null)
                {
                    return "运行态未建立";
                }

                if (snapshot.IsAlarm)
                {
                    return "报警中";
                }

                if (snapshot.IsMoving)
                {
                    return "运动中";
                }

                if (!snapshot.IsEnabled)
                {
                    return "未使能";
                }

                if (snapshot.IsDone)
                {
                    return "已使能待机";
                }

                return "空闲";
            }
        }

        public string SelectedAxisInterlockText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                if (snapshot == null)
                {
                    return "运行态未建立，禁止操作";
                }

                if (snapshot.IsAlarm)
                {
                    return "当前轴报警中，请先清状态";
                }

                if (!snapshot.IsEnabled)
                {
                    return "当前轴未使能，运动前请先伺服上电";
                }

                if (snapshot.PositiveLimit || snapshot.NegativeLimit)
                {
                    return "当前存在限位约束，请确认方向后操作";
                }

                if (!snapshot.IsAtHome)
                {
                    return "当前轴未回原点，绝对定位前建议先回零";
                }

                if (snapshot.IsMoving)
                {
                    return "当前轴正在运动中，请避免重复下发运动命令";
                }

                return "当前轴允许操作";
            }
        }

        public string SelectedAxisLimitText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null ? "—" : snapshot.LimitStateText;
            }
        }

        public string SelectedAxisEnabledText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null ? "—" : (snapshot.IsEnabled ? "已使能" : "未使能");
            }
        }

        public string SelectedAxisHomeText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null ? "—" : (snapshot.IsAtHome ? "在原点" : "未回原点");
            }
        }

        public string SelectedAxisDoneText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null ? "—" : (snapshot.IsDone ? "已到位" : "未到位");
            }
        }

        public string SelectedAxisCommandPositionText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null
                    ? "—"
                    : snapshot.CommandPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisEncoderPositionText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null
                    ? "—"
                    : snapshot.EncoderPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisPositionErrorText
        {
            get
            {
                var snapshot = GetSelectedAxisRuntime();
                return snapshot == null
                    ? "—"
                    : snapshot.PositionErrorMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisDefaultVelocityText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisJogVelocityText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.JogVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string TargetPositionMmText
        {
            get { return _targetPositionMmText; }
            set { SetProperty(ref _targetPositionMmText, value); }
        }

        public string MoveDistanceMmText
        {
            get { return _moveDistanceMmText; }
            set { SetProperty(ref _moveDistanceMmText, value); }
        }

        public string VelocityMmText
        {
            get { return _velocityMmText; }
            set { SetProperty(ref _velocityMmText, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public string LastOperationText
        {
            get { return _lastOperationText; }
            set { SetProperty(ref _lastOperationText, value); }
        }

        public string OperationHintText
        {
            get { return _operationHintText; }
            set { SetProperty(ref _operationHintText, value); }
        }

        public async Task LoadAsync()
        {
            await RefreshAsync();
            RefreshSelectedAxisRuntimeDisplay();
            UpdateOperationHint();
        }

        /// <summary>
        /// 手动刷新：重新加载控制卡/轴静态结构。
        /// 运行态实时值完全来自 MotionAxisRuntimeState。
        /// </summary>
        public async Task RefreshAsync()
        {
            if (_isBusy || _isDisposed)
            {
                return;
            }

            _isBusy = true;
            NotifyCommandState();

            try
            {
                var previousCardId = SelectedCardFilter == null ? (short?)null : SelectedCardFilter.CardId;
                var previousAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;

                var result = await Task.Run(() => _runtimeQueryService.QueryAxisDefinitions());
                if (!result.Success)
                {
                    StatusText = result.Message;
                    return;
                }

                _allAxisItems.Clear();
                _allAxisItems.AddRange(result.Items.OrderBy(x => x.CardId).ThenBy(x => x.LogicalAxis));

                RefreshCardFilters(previousCardId);
                RebuildAxisItems(previousAxis);
                RefreshSelectedAxisRuntimeDisplay();

                StatusText = string.Format(
                    "单轴控制结构已刷新，当前卡共 {0} 轴",
                    AxisItems.Count);
            }
            finally
            {
                _isBusy = false;
                UpdateOperationHint();
                NotifyCommandState();
            }
        }

        /// <summary>
        /// 释放运行态订阅。
        ///
        /// 注意：当前 MotionAxisView 被 MainWindow 页面缓存复用，
        /// 正常导航切换时不应调用本方法；否则会导致再次进入页面后实时刷新链路断开。
        /// 本方法仅适用于页面实例真正销毁的场景。
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            RuntimeContext.Instance.MotionAxis.SnapshotChanged -= MotionAxis_SnapshotChanged;
            _isDisposed = true;
        }

        private void MotionAxis_SnapshotChanged()
        {
            if (_isDisposed)
            {
                return;
            }

            _uiContext.Post(_ =>
            {
                if (_isDisposed)
                {
                    return;
                }

                RefreshSelectedAxisRuntimeDisplay();
                UpdateOperationHint();
                NotifyCommandState();
            }, null);
        }

        private MotionAxisRuntimeSnapshot GetSelectedAxisRuntime()
        {
            if (SelectedAxisItem == null)
            {
                return null;
            }

            MotionAxisRuntimeSnapshot snapshot;
            return RuntimeContext.Instance.MotionAxis.TryGetAxisSnapshot(SelectedAxisItem.LogicalAxis, out snapshot)
                ? snapshot
                : null;
        }

        private bool TryGetSelectedAxisRuntime(out MotionAxisRuntimeSnapshot snapshot)
        {
            snapshot = GetSelectedAxisRuntime();
            return snapshot != null;
        }

        private void RefreshSelectedAxisRuntimeDisplay()
        {
            RaiseSelectedAxisDisplayChanged();
        }

        private void RaiseSelectedAxisDisplayChanged()
        {
            OnPropertyChanged(nameof(SelectedAxisHeader));
            OnPropertyChanged(nameof(SelectedAxisCardText));
            OnPropertyChanged(nameof(SelectedAxisStatusText));
            OnPropertyChanged(nameof(SelectedAxisStateDisplayText));
            OnPropertyChanged(nameof(SelectedAxisInterlockText));
            OnPropertyChanged(nameof(SelectedAxisLimitText));
            OnPropertyChanged(nameof(SelectedAxisEnabledText));
            OnPropertyChanged(nameof(SelectedAxisHomeText));
            OnPropertyChanged(nameof(SelectedAxisDoneText));
            OnPropertyChanged(nameof(SelectedAxisCommandPositionText));
            OnPropertyChanged(nameof(SelectedAxisEncoderPositionText));
            OnPropertyChanged(nameof(SelectedAxisPositionErrorText));
            OnPropertyChanged(nameof(SelectedAxisDefaultVelocityText));
            OnPropertyChanged(nameof(SelectedAxisJogVelocityText));
            OnPropertyChanged(nameof(AxisRuntimeScanStateText));
            OnPropertyChanged(nameof(SelectedAxisRuntimeUpdateTimeText));
        }

        private async Task ServoOnAsync()
        {
            var validate = ValidateServoOnOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.Enable(SelectedAxisItem.LogicalAxis, true),
                "伺服上电");
        }

        private async Task ServoOffAsync()
        {
            var validate = ValidateServoOffOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.Enable(SelectedAxisItem.LogicalAxis, false),
                "伺服断电");
        }

        private async Task StopAsync()
        {
            var validate = ValidateStopOperation(false);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.Stop(SelectedAxisItem.LogicalAxis, false),
                "普通停止");
        }

        private async Task EmergencyStopAsync()
        {
            var validate = ValidateStopOperation(true);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.Stop(SelectedAxisItem.LogicalAxis, true),
                "急停");
        }

        private async Task HomeAsync()
        {
            var validate = ValidateHomeOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await _operationService.HomeAsync(SelectedAxisItem.LogicalAxis);
                ApplyOperationResult(result, "回零");
            }
            finally
            {
                _isBusy = false;
                UpdateOperationHint();
                NotifyCommandState();
            }

            RefreshSelectedAxisRuntimeDisplay();
        }

        private async Task ClearStatusAsync()
        {
            var validate = ValidateClearStatusOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.ClearStatus(SelectedAxisItem.LogicalAxis),
                "清状态");
        }

        private async Task JogNegativeAsync()
        {
            var validate = ValidateJogOperation(false);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.JogMove(SelectedAxisItem.LogicalAxis, false, velocityMm),
                "负向点动");
        }

        private async Task JogStopAsync()
        {
            var validate = ValidateJogStopOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.JogStop(SelectedAxisItem.LogicalAxis),
                "点动停止");
        }

        private async Task JogPositiveAsync()
        {
            var validate = ValidateJogOperation(true);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.JogMove(SelectedAxisItem.LogicalAxis, true, velocityMm),
                "正向点动");
        }

        private async Task MoveAbsoluteAsync()
        {
            var validate = ValidateMoveOperation(true);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            double positionMm;
            double velocityMm;

            if (!TryParseNumber(TargetPositionMmText, "目标位置", out positionMm))
            {
                return;
            }

            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.MoveAbsoluteMm(SelectedAxisItem.LogicalAxis, positionMm, velocityMm),
                "绝对定位");
        }

        private async Task MoveRelativeAsync()
        {
            var validate = ValidateMoveOperation(false);
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            double distanceMm;
            double velocityMm;

            if (!TryParseNumber(MoveDistanceMmText, "相对距离", out distanceMm))
            {
                return;
            }

            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.MoveRelativeMm(SelectedAxisItem.LogicalAxis, distanceMm, velocityMm),
                "相对运动");
        }

        private async Task ApplyVelocityAsync()
        {
            var validate = ValidateApplyVelocityOperation();
            if (!validate.Success)
            {
                ApplyValidationFailure(validate);
                return;
            }

            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(
                () => _operationService.ApplyVelocityMm(SelectedAxisItem.LogicalAxis, velocityMm),
                "应用速度");
        }

        private async Task ExecuteOperationAsync(Func<Result> action, string operationName)
        {
            if (SelectedAxisItem == null)
            {
                return;
            }

            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await Task.Run(action);
                ApplyOperationResult(result, operationName);
            }
            finally
            {
                _isBusy = false;
                UpdateOperationHint();
                NotifyCommandState();
            }

            RefreshSelectedAxisRuntimeDisplay();
        }

        private Result ValidateCommonAxisOperation()
        {
            if (SelectedAxisItem == null)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "请先选择轴");
            }

            if (_isBusy)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前命令执行中，请稍后");
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            return Result.Ok();
        }

        private Result ValidateMotionOperation()
        {
            var common = ValidateCommonAxisOperation();
            if (!common.Success)
            {
                return common;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsAlarm)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴处于报警状态，请先清状态");
            }

            if (!snapshot.IsEnabled)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴未使能，请先伺服上电");
            }

            return Result.Ok();
        }

        private Result ValidateJogOperation(bool positiveDirection)
        {
            var motion = ValidateMotionOperation();
            if (!motion.Success)
            {
                return motion;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsMoving)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正在运动中，请先停止后再执行点动");
            }

            if (positiveDirection && snapshot.PositiveLimit)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正限位触发，禁止继续正向点动");
            }

            if (!positiveDirection && snapshot.NegativeLimit)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴负限位触发，禁止继续负向点动");
            }

            return Result.Ok();
        }

        private Result ValidateMoveOperation(bool requireHome)
        {
            var motion = ValidateMotionOperation();
            if (!motion.Success)
            {
                return motion;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsMoving)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正在运动中，请先停止后再下发新命令");
            }

            if (requireHome && !snapshot.IsAtHome)
            {
                return Result.Fail((int)MotionErrorCode.HomeFailed, "当前轴未回原点，禁止执行绝对定位");
            }

            return Result.Ok();
        }

        private Result ValidateHomeOperation()
        {
            var motion = ValidateMotionOperation();
            if (!motion.Success)
            {
                return motion;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsMoving)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正在运动中，禁止重复执行回零");
            }

            if (snapshot.PositiveLimit || snapshot.NegativeLimit)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴存在限位触发，请确认后再执行回零");
            }

            return Result.Ok();
        }

        private Result ValidateServoOnOperation()
        {
            var common = ValidateCommonAxisOperation();
            if (!common.Success)
            {
                return common;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsEnabled)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴已使能，无需重复上电");
            }

            return Result.Ok();
        }

        private Result ValidateServoOffOperation()
        {
            var common = ValidateCommonAxisOperation();
            if (!common.Success)
            {
                return common;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (!snapshot.IsEnabled)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴未使能，无需重复断电");
            }

            if (snapshot.IsMoving)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正在运动中，禁止直接断电");
            }

            return Result.Ok();
        }

        private Result ValidateStopOperation(bool isEmergency)
        {
            var common = ValidateCommonAxisOperation();
            if (!common.Success)
            {
                return common;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (!snapshot.IsMoving && !isEmergency)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴未处于运动中，无需普通停止");
            }

            return Result.Ok();
        }

        private Result ValidateJogStopOperation()
        {
            return ValidateCommonAxisOperation();
        }

        private Result ValidateClearStatusOperation()
        {
            return ValidateCommonAxisOperation();
        }

        private Result ValidateApplyVelocityOperation()
        {
            var motion = ValidateMotionOperation();
            if (!motion.Success)
            {
                return motion;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                return Result.Fail((int)MotionErrorCode.AxisMapNotFound, "当前轴运行态未建立");
            }

            if (snapshot.IsMoving)
            {
                return Result.Fail((int)MotionErrorCode.InvalidAxisConfig, "当前轴正在运动中，禁止修改运行速度");
            }

            return Result.Ok();
        }

        private void ApplyValidationFailure(Result result)
        {
            if (result == null)
            {
                return;
            }

            StatusText = "联锁限制：" + result.Message;
            LastOperationText = "最近操作：未执行 / " + result.Message;
            OperationHintText = result.Message;
            NotifyCommandState();
        }

        private void ApplyOperationResult(Result result, string operationName)
        {
            if (result == null)
            {
                StatusText = operationName + "失败";
                LastOperationText = "最近操作：" + operationName + " / 未返回结果";
                OperationHintText = "请检查当前轴状态后重试";
                return;
            }

            if (result.Success)
            {
                StatusText = operationName + "成功：" + result.Message;
                LastOperationText = "最近操作：" + operationName + " / 成功";
                OperationHintText = "当前轴允许继续操作";
            }
            else
            {
                StatusText = operationName + "失败：" + result.Message;
                LastOperationText = "最近操作：" + operationName + " / 失败";
                OperationHintText = result.Message;
            }
        }

        private void UpdateOperationHint()
        {
            if (SelectedAxisItem == null)
            {
                OperationHintText = "请选择轴后执行操作";
                return;
            }

            MotionAxisRuntimeSnapshot snapshot;
            if (!TryGetSelectedAxisRuntime(out snapshot))
            {
                OperationHintText = "当前轴运行态未建立";
                return;
            }

            if (snapshot.IsAlarm)
            {
                OperationHintText = "当前轴报警中，请先清状态";
                return;
            }

            if (!snapshot.IsEnabled)
            {
                OperationHintText = "当前轴未使能，运动前请先伺服上电";
                return;
            }

            if (snapshot.IsMoving)
            {
                OperationHintText = "当前轴正在运动中，请避免重复下发运动命令";
                return;
            }

            if (snapshot.PositiveLimit || snapshot.NegativeLimit)
            {
                OperationHintText = "当前存在限位约束，请确认方向后操作";
                return;
            }

            if (!snapshot.IsAtHome)
            {
                OperationHintText = "当前轴未回原点，绝对定位前建议先回零";
                return;
            }

            OperationHintText = "当前轴允许操作";
        }

        private void RefreshCardFilters(short? previousCardId)
        {
            CardFilters.Clear();

            foreach (var item in _allAxisItems
                .GroupBy(x => x.CardId)
                .Select(g => new MotionCardFilterItem
                {
                    CardId = g.Key,
                    DisplayName = g.Select(x => x.CardDisplayName).FirstOrDefault()
                })
                .OrderBy(x => x.CardId))
            {
                CardFilters.Add(item);
            }

            MotionCardFilterItem selected = null;

            if (previousCardId.HasValue)
            {
                selected = CardFilters.FirstOrDefault(x => x.CardId == previousCardId.Value);
            }

            if (selected == null && CardFilters.Count > 0)
            {
                selected = CardFilters[0];
            }

            _isRefreshingCardFilters = true;
            try
            {
                SelectedCardFilter = selected;
            }
            finally
            {
                _isRefreshingCardFilters = false;
            }

            OnPropertyChanged(nameof(SelectedCardHeader));
        }

        private void RebuildAxisItems(short? previousAxis)
        {
            AxisItems.Clear();

            IEnumerable<MotionAxisDisplayItem> query = _allAxisItems;

            if (SelectedCardFilter != null)
            {
                query = query.Where(x => x.CardId == SelectedCardFilter.CardId);
            }

            var list = query
                .OrderBy(x => x.LogicalAxis)
                .ToList();

            foreach (var item in list)
            {
                AxisItems.Add(item);
            }

            _suppressVelocityReset = true;
            try
            {
                if (previousAxis.HasValue)
                {
                    SelectedAxisItem = AxisItems.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                        ?? FirstAxisOrNull();
                }
                else
                {
                    SelectedAxisItem = FirstAxisOrNull();
                }
            }
            finally
            {
                _suppressVelocityReset = false;
            }
        }

        private MotionAxisDisplayItem FirstAxisOrNull()
        {
            return AxisItems.Count > 0 ? AxisItems[0] : null;
        }

        private bool TryParseNumber(string text, string fieldName, out double value)
        {
            if (!double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                StatusText = fieldName + " 请输入有效数值";
                OperationHintText = fieldName + " 请输入有效数值";
                return false;
            }

            return true;
        }

        private bool TryParsePositiveNumber(string text, string fieldName, out double value)
        {
            if (!TryParseNumber(text, fieldName, out value))
            {
                return false;
            }

            if (value <= 0)
            {
                StatusText = fieldName + " 必须大于 0";
                OperationHintText = fieldName + " 必须大于 0";
                return false;
            }

            return true;
        }

        private bool CanRefresh()
        {
            return !_isBusy && !_isDisposed;
        }

        private bool CanServoOn()
        {
            return ValidateServoOnOperation().Success;
        }

        private bool CanServoOff()
        {
            return ValidateServoOffOperation().Success;
        }

        private bool CanStop()
        {
            return ValidateStopOperation(false).Success;
        }

        private bool CanEmergencyStop()
        {
            return ValidateStopOperation(true).Success;
        }

        private bool CanHome()
        {
            return ValidateHomeOperation().Success;
        }

        private bool CanClearStatus()
        {
            return ValidateClearStatusOperation().Success;
        }

        private bool CanJogNegative()
        {
            return ValidateJogOperation(false).Success;
        }

        private bool CanJogStop()
        {
            return ValidateJogStopOperation().Success;
        }

        private bool CanJogPositive()
        {
            return ValidateJogOperation(true).Success;
        }

        private bool CanMoveAbsolute()
        {
            return ValidateMoveOperation(true).Success;
        }

        private bool CanMoveRelative()
        {
            return ValidateMoveOperation(false).Success;
        }

        private bool CanApplyVelocity()
        {
            return ValidateApplyVelocityOperation().Success;
        }

        private void NotifyCommandState()
        {
            RefreshCommand.NotifyCanExecuteChanged();
            ServoOnCommand.NotifyCanExecuteChanged();
            ServoOffCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
            EmergencyStopCommand.NotifyCanExecuteChanged();
            HomeCommand.NotifyCanExecuteChanged();
            ClearStatusCommand.NotifyCanExecuteChanged();
            JogNegativeCommand.NotifyCanExecuteChanged();
            JogStopCommand.NotifyCanExecuteChanged();
            JogPositiveCommand.NotifyCanExecuteChanged();
            MoveAbsoluteCommand.NotifyCanExecuteChanged();
            MoveRelativeCommand.NotifyCanExecuteChanged();
            ApplyVelocityCommand.NotifyCanExecuteChanged();
        }

        private static bool IsSameCardFilter(MotionCardFilterItem left, MotionCardFilterItem right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            return left.CardId == right.CardId;
        }
    }
}