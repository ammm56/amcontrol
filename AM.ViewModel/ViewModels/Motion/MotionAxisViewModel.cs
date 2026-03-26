using AM.DBService.Services.Motion.Runtime;
using AM.Model.Common;
using AM.Model.Model.Motion;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
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

        private MotionAxisDisplayItem _selectedAxisItem;
        private string _targetPositionMmText;
        private string _moveDistanceMmText;
        private string _velocityMmText;
        private string _statusText;
        private bool _isBusy;

        public MotionAxisViewModel()
        {
            _runtimeQueryService = new MotionRuntimeQueryService();
            _operationService = new MotionAxisOperationService();

            AxisItems = new ObservableCollection<MotionAxisDisplayItem>();

            _targetPositionMmText = "0";
            _moveDistanceMmText = "10";
            _velocityMmText = "10";
            _statusText = "请选择左侧轴";

            RefreshCommand = new AsyncRelayCommand(RefreshAsync, CanOperate);
            ServoOnCommand = new AsyncRelayCommand(ServoOnAsync, CanOperateAxis);
            ServoOffCommand = new AsyncRelayCommand(ServoOffAsync, CanOperateAxis);
            StopCommand = new AsyncRelayCommand(StopAsync, CanOperateAxis);
            EmergencyStopCommand = new AsyncRelayCommand(EmergencyStopAsync, CanOperateAxis);
            HomeCommand = new AsyncRelayCommand(HomeAsync, CanOperateAxis);
            JogNegativeCommand = new AsyncRelayCommand(JogNegativeAsync, CanOperateAxis);
            JogPositiveCommand = new AsyncRelayCommand(JogPositiveAsync, CanOperateAxis);
            MoveAbsoluteCommand = new AsyncRelayCommand(MoveAbsoluteAsync, CanOperateAxis);
            MoveRelativeCommand = new AsyncRelayCommand(MoveRelativeAsync, CanOperateAxis);
            ApplyVelocityCommand = new AsyncRelayCommand(ApplyVelocityAsync, CanOperateAxis);
        }

        public ObservableCollection<MotionAxisDisplayItem> AxisItems { get; private set; }

        public IAsyncRelayCommand RefreshCommand { get; private set; }

        public IAsyncRelayCommand ServoOnCommand { get; private set; }

        public IAsyncRelayCommand ServoOffCommand { get; private set; }

        public IAsyncRelayCommand StopCommand { get; private set; }

        public IAsyncRelayCommand EmergencyStopCommand { get; private set; }

        public IAsyncRelayCommand HomeCommand { get; private set; }

        public IAsyncRelayCommand JogNegativeCommand { get; private set; }

        public IAsyncRelayCommand JogPositiveCommand { get; private set; }

        public IAsyncRelayCommand MoveAbsoluteCommand { get; private set; }

        public IAsyncRelayCommand MoveRelativeCommand { get; private set; }

        public IAsyncRelayCommand ApplyVelocityCommand { get; private set; }

        public MotionAxisDisplayItem SelectedAxisItem
        {
            get { return _selectedAxisItem; }
            set
            {
                if (SetProperty(ref _selectedAxisItem, value))
                {
                    if (_selectedAxisItem != null)
                    {
                        VelocityMmText = _selectedAxisItem.DefaultVelocityMm.ToString("0.###", CultureInfo.InvariantCulture);
                    }

                    OnPropertyChanged(nameof(SelectedAxisHeader));
                    NotifyCommandState();
                }
            }
        }

        public string SelectedAxisHeader
        {
            get
            {
                if (_selectedAxisItem == null)
                {
                    return "当前轴：未选择";
                }

                return "当前轴：L#" + _selectedAxisItem.LogicalAxis + "  " + _selectedAxisItem.DisplayTitle;
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
                var selectedAxis = SelectedAxisItem == null ? (short?)null : SelectedAxisItem.LogicalAxis;
                var result = await Task.Run(() => _runtimeQueryService.QueryAxisSnapshot());

                if (!result.Success)
                {
                    StatusText = result.Message;
                    return;
                }

                AxisItems.Clear();
                foreach (var item in result.Items)
                {
                    AxisItems.Add(item);
                }

                if (selectedAxis.HasValue)
                {
                    SelectedAxisItem = AxisItems.FirstOrDefault(x => x.LogicalAxis == selectedAxis.Value) ?? (AxisItems.Count > 0 ? AxisItems[0] : null);
                }
                else
                {
                    SelectedAxisItem = AxisItems.Count > 0 ? AxisItems[0] : null;
                }

                StatusText = "轴运行态已刷新，共 " + AxisItems.Count + " 条";
                OnPropertyChanged(nameof(SelectedAxisHeader));
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
                await RefreshAsync();
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
            }
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
                await RefreshAsync();
            }
            finally
            {
                _isBusy = false;
                NotifyCommandState();
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
            JogNegativeCommand.NotifyCanExecuteChanged();
            JogPositiveCommand.NotifyCanExecuteChanged();
            MoveAbsoluteCommand.NotifyCanExecuteChanged();
            MoveRelativeCommand.NotifyCanExecuteChanged();
            ApplyVelocityCommand.NotifyCanExecuteChanged();
        }
    }
}