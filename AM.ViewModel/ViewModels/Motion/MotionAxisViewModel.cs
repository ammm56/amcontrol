using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Model.Motion;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Motion
{
    /// <summary>
    /// 单轴控制页视图模型。
    /// </summary>
    public class MotionAxisViewModel : ObservableObject
    {
        private readonly MotionRuntimeQueryService _runtimeQueryService;
        private readonly MotionAxisOperationService _operationService;
        private readonly List<MotionAxisDisplayItem> _allAxisItems;

        private MotionCardFilterItem _selectedCardFilter;
        private MotionAxisDisplayItem _selectedAxisItem;
        private string _targetPositionMmText;
        private string _moveDistanceMmText;
        private string _velocityMmText;
        private string _statusText;
        private bool _isBusy;
        private bool _isRefreshingCardFilters;

        public MotionAxisViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _operationService = new MotionAxisOperationService();
            _allAxisItems = new List<MotionAxisDisplayItem>();

            CardFilters = new ObservableCollection<MotionCardFilterItem>();
            AxisItems = new ObservableCollection<MotionAxisDisplayItem>();

            _targetPositionMmText = "0";
            _moveDistanceMmText = "10";
            _velocityMmText = "10";
            _statusText = "请选择控制卡和轴";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync, CanOperate);
            ServoOnCommand = new AsyncRelayCommand(ServoOnAsync, CanOperateAxis);
            ServoOffCommand = new AsyncRelayCommand(ServoOffAsync, CanOperateAxis);
            StopCommand = new AsyncRelayCommand(StopAsync, CanOperateAxis);
            EmergencyStopCommand = new AsyncRelayCommand(EmergencyStopAsync, CanOperateAxis);
            HomeCommand = new AsyncRelayCommand(HomeAsync, CanOperateAxis);
            ClearStatusCommand = new AsyncRelayCommand(ClearStatusAsync, CanOperateAxis);
            JogNegativeCommand = new AsyncRelayCommand(JogNegativeAsync, CanOperateAxis);
            JogStopCommand = new AsyncRelayCommand(JogStopAsync, CanOperateAxis);
            JogPositiveCommand = new AsyncRelayCommand(JogPositiveAsync, CanOperateAxis);
            MoveAbsoluteCommand = new AsyncRelayCommand(MoveAbsoluteAsync, CanOperateAxis);
            MoveRelativeCommand = new AsyncRelayCommand(MoveRelativeAsync, CanOperateAxis);
            ApplyVelocityCommand = new AsyncRelayCommand(ApplyVelocityAsync, CanOperateAxis);
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
                        ApplyCardFilter(null);
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

                    if (currentAxis.HasValue && currentAxis != previousAxis)
                    {
                        VelocityMmText = _selectedAxisItem.DefaultVelocityMm > 0
                            ? _selectedAxisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture)
                            : "10";
                    }

                    OnPropertyChanged(nameof(SelectedAxisHeader));
                    OnPropertyChanged(nameof(SelectedAxisCardText));
                    OnPropertyChanged(nameof(SelectedAxisStatusText));
                    OnPropertyChanged(nameof(SelectedAxisSignalText));
                    OnPropertyChanged(nameof(SelectedAxisLimitText));
                    OnPropertyChanged(nameof(SelectedAxisCommandPositionText));
                    OnPropertyChanged(nameof(SelectedAxisEncoderPositionText));
                    OnPropertyChanged(nameof(SelectedAxisPositionErrorText));
                    OnPropertyChanged(nameof(SelectedAxisDefaultVelocityText));
                    OnPropertyChanged(nameof(SelectedAxisJogVelocityText));
                    NotifyCommandState();
                }
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

                return "卡#" + SelectedAxisItem.CardId + "  " +
                    (string.IsNullOrWhiteSpace(SelectedAxisItem.CardDisplayName)
                        ? "未命名控制卡"
                        : SelectedAxisItem.CardDisplayName);
            }
        }

        public string SelectedAxisStatusText
        {
            get
            {
                return SelectedAxisItem == null ? "—" : SelectedAxisItem.StateText;
            }
        }

        public string SelectedAxisSignalText
        {
            get
            {
                return SelectedAxisItem == null ? "—" : SelectedAxisItem.SignalSummaryText;
            }
        }

        public string SelectedAxisLimitText
        {
            get
            {
                return SelectedAxisItem == null ? "—" : SelectedAxisItem.LimitStateText;
            }
        }

        public string SelectedAxisCommandPositionText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.CommandPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisEncoderPositionText
        {
            get
            {
                return SelectedAxisItem == null
                    ? "—"
                    : SelectedAxisItem.EncoderPositionMm.ToString("0.###", CultureInfo.InvariantCulture);
            }
        }

        public string SelectedAxisPositionErrorText
        {
            get
            {
                if (SelectedAxisItem == null)
                {
                    return "—";
                }

                var error = SelectedAxisItem.CommandPositionMm - SelectedAxisItem.EncoderPositionMm;
                return error.ToString("0.###", CultureInfo.InvariantCulture);
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

        public async Task LoadAsync()
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            if (_isBusy)
            {
                return;
            }

            _isBusy = true;
            NotifyCommandState();

            try
            {
                var previousCardId = SelectedCardFilter == null ? (short?)null : SelectedCardFilter.CardId;
                var previousAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;

                var result = await Task.Run(() => _runtimeQueryService.QueryAxisSnapshot());

                if (!result.Success)
                {
                    StatusText = result.Message;
                    return;
                }

                _allAxisItems.Clear();
                _allAxisItems.AddRange(result.Items.OrderBy(x => x.CardId).ThenBy(x => x.LogicalAxis));

                RefreshCardFilters(previousCardId);
                ApplyCardFilter(previousAxis);

                StatusText = string.Format(
                    "单轴控制运行态已刷新，当前卡共 {0} 轴",
                    AxisItems.Count);
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
        }

        private async Task ServoOnAsync()
        {
            await ExecuteOperationAsync(() => _operationService.Enable(SelectedAxisItem.LogicalAxis, true));
        }

        private async Task ServoOffAsync()
        {
            await ExecuteOperationAsync(() => _operationService.Enable(SelectedAxisItem.LogicalAxis, false));
        }

        private async Task StopAsync()
        {
            await ExecuteOperationAsync(() => _operationService.Stop(SelectedAxisItem.LogicalAxis, false));
        }

        private async Task EmergencyStopAsync()
        {
            await ExecuteOperationAsync(() => _operationService.Stop(SelectedAxisItem.LogicalAxis, true));
        }

        private async Task HomeAsync()
        {
            if (SelectedAxisItem == null)
            {
                return;
            }

            _isBusy = true;
            NotifyCommandState();

            try
            {
                var result = await _operationService.HomeAsync(SelectedAxisItem.LogicalAxis);
                StatusText = result.Message;
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }

            await RefreshAsync();
        }

        private async Task ClearStatusAsync()
        {
            await ExecuteOperationAsync(() => _operationService.ClearStatus(SelectedAxisItem.LogicalAxis));
        }

        private async Task JogNegativeAsync()
        {
            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(() => _operationService.JogMove(SelectedAxisItem.LogicalAxis, false, velocityMm));
        }

        private async Task JogStopAsync()
        {
            await ExecuteOperationAsync(() => _operationService.JogStop(SelectedAxisItem.LogicalAxis));
        }

        private async Task JogPositiveAsync()
        {
            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(() => _operationService.JogMove(SelectedAxisItem.LogicalAxis, true, velocityMm));
        }

        private async Task MoveAbsoluteAsync()
        {
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

            await ExecuteOperationAsync(() => _operationService.MoveAbsoluteMm(SelectedAxisItem.LogicalAxis, positionMm, velocityMm));
        }

        private async Task MoveRelativeAsync()
        {
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

            await ExecuteOperationAsync(() => _operationService.MoveRelativeMm(SelectedAxisItem.LogicalAxis, distanceMm, velocityMm));
        }

        private async Task ApplyVelocityAsync()
        {
            double velocityMm;
            if (!TryParsePositiveNumber(VelocityMmText, "速度", out velocityMm))
            {
                return;
            }

            await ExecuteOperationAsync(() => _operationService.ApplyVelocityMm(SelectedAxisItem.LogicalAxis, velocityMm));
        }

        private async Task ExecuteOperationAsync(Func<Result> action)
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
                StatusText = result.Message;
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }

            await RefreshAsync();
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

        private void ApplyCardFilter(short? previousAxis)
        {
            AxisItems.Clear();

            IEnumerable<MotionAxisDisplayItem> query = _allAxisItems;

            if (SelectedCardFilter != null)
            {
                query = query.Where(x => x.CardId == SelectedCardFilter.CardId);
            }

            var list = query.OrderBy(x => x.LogicalAxis).ToList();

            foreach (var item in list)
            {
                AxisItems.Add(item);
            }

            if (previousAxis.HasValue)
            {
                SelectedAxisItem = AxisItems.FirstOrDefault(x => x.LogicalAxis == previousAxis.Value)
                    ?? (AxisItems.Count > 0 ? AxisItems[0] : null);
            }
            else
            {
                SelectedAxisItem = AxisItems.Count > 0 ? AxisItems[0] : null;
            }
        }

        private bool TryParseNumber(string text, string fieldName, out double value)
        {
            if (!double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                StatusText = fieldName + " 请输入有效数值";
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
                return false;
            }

            return true;
        }

        private bool CanOperate()
        {
            return !_isBusy;
        }

        private bool CanOperateAxis()
        {
            return !_isBusy && SelectedAxisItem != null;
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