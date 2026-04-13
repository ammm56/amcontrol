using AM.Core.Context;
using AM.DBService.Services.Plc.Runtime;
using AM.Model.Common;
using AM.Model.Plc;
using AM.Model.Runtime;
using AM.PageModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AM.PageModel.Plc
{
    /// <summary>
    /// PLC 调试页面模型。
    /// 页面定位：
    /// 1. 按配置点位调试；
    /// 2. 按直接地址调试；
    /// 3. 展示最近一次结果；
    /// 4. 保留最近若干条调试历史。
    /// </summary>
    public class PlcDebugPageModel : BindableBase
    {
        private const int MaxHistoryCount = 20;

        private readonly PlcOperationService _operationService;
        private readonly List<string> _directDataTypeOptions;

        private List<string> _plcOptions;
        private List<DebugPointOptionItem> _pointOptions;
        private List<DebugResultItem> _resultHistory;

        private string _selectedPlcName;
        private string _selectedPointName;

        private string _configPointDisplayName;
        private string _configPointGroupName;
        private string _configPointAddress;
        private string _configPointDataType;
        private int _configPointLength;
        private string _configPointAccessMode;
        private bool _configPointEnabled;
        private string _configPointDescription;
        private string _configPointRemark;
        private string _configWriteValueText;
        private bool _configWriteConfirmed;

        private string _directAddress;
        private string _directDataType;
        private int _directLength;
        private string _directWriteValueText;
        private bool _directWriteConfirmed;

        private DebugResultItem _lastResult;
        private string _runtimeSummaryText;

        public PlcDebugPageModel()
        {
            _operationService = new PlcOperationService();
            _directDataTypeOptions = new List<string>
            {
                "bool",
                "uint8",
                "int8",
                "uint16",
                "int16",
                "uint32",
                "int32",
                "uint64",
                "int64",
                "float",
                "double",
                "string",
                "bool[]",
                "uint8[]",
                "int8[]",
                "uint16[]",
                "int16[]",
                "uint32[]",
                "int32[]",
                "uint64[]",
                "int64[]",
                "float[]",
                "double[]"
            };

            _plcOptions = new List<string>();
            _pointOptions = new List<DebugPointOptionItem>();
            _resultHistory = new List<DebugResultItem>();

            _selectedPlcName = string.Empty;
            _selectedPointName = string.Empty;

            _configPointDisplayName = string.Empty;
            _configPointGroupName = string.Empty;
            _configPointAddress = string.Empty;
            _configPointDataType = string.Empty;
            _configPointLength = 1;
            _configPointAccessMode = string.Empty;
            _configPointEnabled = false;
            _configPointDescription = string.Empty;
            _configPointRemark = string.Empty;
            _configWriteValueText = string.Empty;
            _configWriteConfirmed = false;

            _directAddress = string.Empty;
            _directDataType = "bool";
            _directLength = 1;
            _directWriteValueText = string.Empty;
            _directWriteConfirmed = false;

            _runtimeSummaryText = "状态未知";
        }

        public IList<string> PlcOptions
        {
            get { return _plcOptions; }
        }

        public IList<DebugPointOptionItem> PointOptions
        {
            get { return _pointOptions; }
        }

        public IList<string> DirectDataTypeOptions
        {
            get { return _directDataTypeOptions; }
        }

        public IList<DebugResultItem> ResultHistory
        {
            get { return _resultHistory; }
        }

        public string SelectedPlcName
        {
            get { return _selectedPlcName; }
            private set { SetProperty(ref _selectedPlcName, value ?? string.Empty); }
        }

        public string SelectedPointName
        {
            get { return _selectedPointName; }
            private set { SetProperty(ref _selectedPointName, value ?? string.Empty); }
        }

        public string SelectedPointDisplayText
        {
            get
            {
                DebugPointOptionItem item = _pointOptions.FirstOrDefault(x =>
                    string.Equals(x.PointName, SelectedPointName, StringComparison.OrdinalIgnoreCase));

                return item == null ? string.Empty : item.DisplayTitle;
            }
        }

        public string ConfigPointDisplayName
        {
            get { return _configPointDisplayName; }
            private set { SetProperty(ref _configPointDisplayName, value ?? string.Empty); }
        }

        public string ConfigPointGroupName
        {
            get { return _configPointGroupName; }
            private set { SetProperty(ref _configPointGroupName, value ?? string.Empty); }
        }

        public string ConfigPointAddress
        {
            get { return _configPointAddress; }
            private set { SetProperty(ref _configPointAddress, value ?? string.Empty); }
        }

        public string ConfigPointDataType
        {
            get { return _configPointDataType; }
            private set { SetProperty(ref _configPointDataType, value ?? string.Empty); }
        }

        public int ConfigPointLength
        {
            get { return _configPointLength; }
            private set { SetProperty(ref _configPointLength, value <= 0 ? 1 : value); }
        }

        public string ConfigPointAccessMode
        {
            get { return _configPointAccessMode; }
            private set
            {
                if (SetProperty(ref _configPointAccessMode, value ?? string.Empty))
                {
                    OnPropertyChanged(nameof(ConfigPointAccessModeText));
                    OnPropertyChanged(nameof(CanReadSelectedPoint));
                    OnPropertyChanged(nameof(CanWriteSelectedPoint));
                    OnPropertyChanged(nameof(ConfigPointOperationHintText));
                }
            }
        }

        public string ConfigPointAccessModeText
        {
            get { return ToAccessModeText(ConfigPointAccessMode); }
        }

        public bool ConfigPointEnabled
        {
            get { return _configPointEnabled; }
            private set
            {
                if (SetProperty(ref _configPointEnabled, value))
                {
                    OnPropertyChanged(nameof(CanReadSelectedPoint));
                    OnPropertyChanged(nameof(CanWriteSelectedPoint));
                    OnPropertyChanged(nameof(ConfigPointOperationHintText));
                }
            }
        }

        public string ConfigPointDescription
        {
            get { return _configPointDescription; }
            private set { SetProperty(ref _configPointDescription, value ?? string.Empty); }
        }

        public string ConfigPointRemark
        {
            get { return _configPointRemark; }
            private set { SetProperty(ref _configPointRemark, value ?? string.Empty); }
        }

        public string ConfigWriteValueText
        {
            get { return _configWriteValueText; }
            set { SetProperty(ref _configWriteValueText, value ?? string.Empty); }
        }

        public bool ConfigWriteConfirmed
        {
            get { return _configWriteConfirmed; }
            set { SetProperty(ref _configWriteConfirmed, value); }
        }

        public string DirectAddress
        {
            get { return _directAddress; }
            set
            {
                if (SetProperty(ref _directAddress, value ?? string.Empty))
                {
                    OnPropertyChanged(nameof(CanReadDirectAddress));
                    OnPropertyChanged(nameof(CanWriteDirectAddress));
                    OnPropertyChanged(nameof(DirectOperationHintText));
                }
            }
        }

        public string DirectDataType
        {
            get { return _directDataType; }
            set
            {
                string normalized = NormalizeText(value);
                if (string.IsNullOrWhiteSpace(normalized))
                {
                    normalized = "bool";
                }

                if (SetProperty(ref _directDataType, normalized))
                {
                    OnPropertyChanged(nameof(CanReadDirectAddress));
                    OnPropertyChanged(nameof(CanWriteDirectAddress));
                    OnPropertyChanged(nameof(DirectOperationHintText));
                }
            }
        }

        public int DirectLength
        {
            get { return _directLength; }
            set
            {
                int normalized = value <= 0 ? 1 : value;
                if (SetProperty(ref _directLength, normalized))
                {
                    OnPropertyChanged(nameof(CanReadDirectAddress));
                    OnPropertyChanged(nameof(CanWriteDirectAddress));
                    OnPropertyChanged(nameof(DirectOperationHintText));
                }
            }
        }

        public string DirectWriteValueText
        {
            get { return _directWriteValueText; }
            set
            {
                if (SetProperty(ref _directWriteValueText, value ?? string.Empty))
                {
                    OnPropertyChanged(nameof(CanWriteDirectAddress));
                }
            }
        }

        public bool DirectWriteConfirmed
        {
            get { return _directWriteConfirmed; }
            set
            {
                if (SetProperty(ref _directWriteConfirmed, value))
                {
                    OnPropertyChanged(nameof(CanWriteDirectAddress));
                }
            }
        }

        public DebugResultItem LastResult
        {
            get { return _lastResult; }
            private set
            {
                if (SetProperty(ref _lastResult, value))
                {
                    OnPropertyChanged(nameof(HasLastResult));
                }
            }
        }

        public string RuntimeSummaryText
        {
            get { return _runtimeSummaryText; }
            private set { SetProperty(ref _runtimeSummaryText, value ?? string.Empty); }
        }

        public bool IsPointSelected
        {
            get { return !string.IsNullOrWhiteSpace(SelectedPointName); }
        }

        public bool HasLastResult
        {
            get { return LastResult != null; }
        }

        public int ResultHistoryCount
        {
            get { return _resultHistory.Count; }
        }

        public bool CanReadSelectedPoint
        {
            get
            {
                return IsPointSelected
                    && ConfigPointEnabled
                    && !string.Equals(ConfigPointAccessMode, "WriteOnly", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool CanWriteSelectedPoint
        {
            get
            {
                return IsPointSelected
                    && ConfigPointEnabled
                    && !string.Equals(ConfigPointAccessMode, "ReadOnly", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool CanReadDirectAddress
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SelectedPlcName)
                    && !string.IsNullOrWhiteSpace(DirectAddress)
                    && !string.IsNullOrWhiteSpace(DirectDataType)
                    && DirectLength > 0;
            }
        }

        public bool CanWriteDirectAddress
        {
            get
            {
                return CanReadDirectAddress
                    && !string.IsNullOrWhiteSpace(DirectWriteValueText)
                    && DirectWriteConfirmed;
            }
        }

        public string ConfigPointOperationHintText
        {
            get
            {
                if (!IsPointSelected)
                {
                    return "请选择配置点位。";
                }

                if (!ConfigPointEnabled)
                {
                    return "当前点位未启用。";
                }

                if (string.Equals(ConfigPointAccessMode, "WriteOnly", StringComparison.OrdinalIgnoreCase))
                {
                    return "当前点位为只写点位，禁止按配置读取。";
                }

                if (string.Equals(ConfigPointAccessMode, "ReadOnly", StringComparison.OrdinalIgnoreCase))
                {
                    return "当前点位为只读点位，禁止写入。";
                }

                return "当前点位允许按配置读取与写入。";
            }
        }

        public string DirectOperationHintText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SelectedPlcName))
                {
                    return "请先选择 PLC。";
                }

                if (string.IsNullOrWhiteSpace(DirectAddress))
                {
                    return "请输入要调试的地址。";
                }

                if (string.IsNullOrWhiteSpace(DirectDataType))
                {
                    return "请选择数据类型。";
                }

                if (DirectLength <= 0)
                {
                    return "长度必须大于 0。";
                }

                return "当前可按直接地址执行测试读取或写入。";
            }
        }

        public string DirectWriteRiskText
        {
            get { return "直接地址写入属于高风险操作，必须确认 PLC、地址、数据类型与长度完全正确。"; }
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                BuildPlcOptions();
                BuildPointOptions();
                ApplySelectedPointMeta();
                ApplyRuntimeSummary();
                RaiseOperationStateChanged();

                return Result.Ok("PLC 调试页加载成功", ResultSource.UI);
            });
        }

        public void SetSelectedPlc(string plcName)
        {
            SelectedPlcName = NormalizeText(plcName);
            SelectedPointName = string.Empty;

            BuildPointOptions();
            ApplySelectedPointMeta();
            ApplyRuntimeSummary();
            RaiseOperationStateChanged();

            OnPropertyChanged(nameof(SelectedPointDisplayText));
        }

        public void SetSelectedPoint(string pointName)
        {
            SelectedPointName = NormalizeText(pointName);
            ApplySelectedPointMeta();
            RaiseOperationStateChanged();

            OnPropertyChanged(nameof(SelectedPointDisplayText));
        }

        public Result TestReadPoint()
        {
            Result<PlcPointRuntimeSnapshot> result = _operationService.TestReadPoint(SelectedPointName);
            DebugResultItem debugResult = BuildReadResult(
                "按配置点位读取",
                "ConfigPoint",
                result.Success ? result.Item : null,
                result);

            LastResult = debugResult;
            AppendHistory(debugResult);
            ApplyRuntimeSummary();

            return result.Success
                ? Result.Ok(result.Message, result.Source)
                : Result.Fail(result.Code, result.Message, result.Source);
        }

        public Result WritePoint()
        {
            Result result = _operationService.WritePoint(SelectedPointName, ConfigWriteValueText, ConfigWriteConfirmed);
            DebugResultItem debugResult = BuildWriteResult(
                "按配置点位写入",
                "ConfigPoint",
                SelectedPlcName,
                SelectedPointName,
                ConfigPointDisplayName,
                ConfigPointAddress,
                ConfigPointDataType,
                ConfigPointLength,
                ConfigPointAccessMode,
                ConfigWriteValueText,
                ConfigWriteConfirmed,
                result);

            LastResult = debugResult;
            AppendHistory(debugResult);
            ApplyRuntimeSummary();

            return result;
        }

        public Result TestReadAddress()
        {
            Result<PlcPointRuntimeSnapshot> result = _operationService.TestReadAddress(
                SelectedPlcName,
                null,
                DirectAddress,
                DirectDataType,
                null,
                string.Equals(DirectDataType, "string", StringComparison.OrdinalIgnoreCase) ? DirectLength : 0,
                IsArrayType(DirectDataType) ? DirectLength : 0);

            DebugResultItem debugResult = BuildReadResult(
                "按直接地址读取",
                "DirectAddress",
                result.Success ? result.Item : null,
                result);

            if (debugResult != null)
            {
                debugResult.Length = DirectLength;
                debugResult.AccessMode = "ReadOnly";
                debugResult.AccessModeText = ToAccessModeText(debugResult.AccessMode);
            }

            LastResult = debugResult;
            AppendHistory(debugResult);
            ApplyRuntimeSummary();

            return result.Success
                ? Result.Ok(result.Message, result.Source)
                : Result.Fail(result.Code, result.Message, result.Source);
        }

        public Result WriteAddress()
        {
            Result result = _operationService.WriteAddress(
                SelectedPlcName,
                null,
                DirectAddress,
                DirectDataType,
                DirectWriteValueText,
                DirectWriteConfirmed,
                null,
                string.Equals(DirectDataType, "string", StringComparison.OrdinalIgnoreCase) ? DirectLength : 0,
                IsArrayType(DirectDataType) ? DirectLength : 0);

            DebugResultItem debugResult = BuildWriteResult(
                "按直接地址写入",
                "DirectAddress",
                SelectedPlcName,
                null,
                null,
                DirectAddress,
                DirectDataType,
                DirectLength,
                "ReadWrite",
                DirectWriteValueText,
                DirectWriteConfirmed,
                result);

            LastResult = debugResult;
            AppendHistory(debugResult);
            ApplyRuntimeSummary();

            return result;
        }

        public void ClearHistory()
        {
            _resultHistory.Clear();
            OnPropertyChanged(nameof(ResultHistory));
            OnPropertyChanged(nameof(ResultHistoryCount));
        }

        private void BuildPlcOptions()
        {
            PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
            _plcOptions = (config.Stations ?? new List<PlcStationConfig>())
                .Where(x => x != null && x.IsEnabled && !string.IsNullOrWhiteSpace(x.Name))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => x.Name)
                .ToList();

            OnPropertyChanged(nameof(PlcOptions));

            if (!string.IsNullOrWhiteSpace(SelectedPlcName) &&
                !_plcOptions.Any(x => string.Equals(x, SelectedPlcName, StringComparison.OrdinalIgnoreCase)))
            {
                SelectedPlcName = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SelectedPlcName) && _plcOptions.Count > 0)
            {
                SelectedPlcName = _plcOptions[0];
            }
        }

        private void BuildPointOptions()
        {
            PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();

            _pointOptions = (config.Points ?? new List<PlcPointConfig>())
                .Where(x =>
                    x != null &&
                    x.IsEnabled &&
                    !string.IsNullOrWhiteSpace(x.Name) &&
                    (string.IsNullOrWhiteSpace(SelectedPlcName) ||
                     string.Equals(x.PlcName, SelectedPlcName, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => new DebugPointOptionItem
                {
                    PlcName = x.PlcName,
                    PointName = x.Name,
                    DisplayName = x.DisplayName,
                    GroupName = x.GroupName,
                    Address = x.Address,
                    DataType = x.DataType,
                    Length = x.Length,
                    AccessMode = x.AccessMode,
                    IsEnabled = x.IsEnabled,
                    Description = x.Description,
                    Remark = x.Remark
                })
                .ToList();

            OnPropertyChanged(nameof(PointOptions));

            if (!string.IsNullOrWhiteSpace(SelectedPointName) &&
                !_pointOptions.Any(x => string.Equals(x.PointName, SelectedPointName, StringComparison.OrdinalIgnoreCase)))
            {
                SelectedPointName = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SelectedPointName) && _pointOptions.Count > 0)
            {
                SelectedPointName = _pointOptions[0].PointName;
            }
        }

        private void ApplySelectedPointMeta()
        {
            DebugPointOptionItem item = _pointOptions.FirstOrDefault(x =>
                string.Equals(x.PointName, SelectedPointName, StringComparison.OrdinalIgnoreCase));

            ConfigPointDisplayName = item == null ? string.Empty : (item.DisplayName ?? string.Empty);
            ConfigPointGroupName = item == null ? string.Empty : (item.GroupName ?? string.Empty);
            ConfigPointAddress = item == null ? string.Empty : (item.Address ?? string.Empty);
            ConfigPointDataType = item == null ? string.Empty : (item.DataType ?? string.Empty);
            ConfigPointLength = item == null ? 1 : item.Length;
            ConfigPointAccessMode = item == null ? string.Empty : NormalizeAccessMode(item.AccessMode);
            ConfigPointEnabled = item != null && item.IsEnabled;
            ConfigPointDescription = item == null ? string.Empty : (item.Description ?? string.Empty);
            ConfigPointRemark = item == null ? string.Empty : (item.Remark ?? string.Empty);

            OnPropertyChanged(nameof(SelectedPointDisplayText));
            RaiseOperationStateChanged();
        }

        private void ApplyRuntimeSummary()
        {
            PlcRuntimeState runtimeState = RuntimeContext.Instance.Plc;
            bool scanRunning = runtimeState != null && runtimeState.IsScanServiceRunning;
            DateTime? lastScanTime = runtimeState == null ? null : runtimeState.LastScanTime;

            RuntimeSummaryText = scanRunning
                ? "运行中 " + FormatTime(lastScanTime)
                : "已停止 " + FormatTime(lastScanTime);
        }

        private void AppendHistory(DebugResultItem item)
        {
            if (item == null)
            {
                return;
            }

            _resultHistory.Insert(0, item);

            if (_resultHistory.Count > MaxHistoryCount)
            {
                _resultHistory.RemoveRange(MaxHistoryCount, _resultHistory.Count - MaxHistoryCount);
            }

            OnPropertyChanged(nameof(ResultHistory));
            OnPropertyChanged(nameof(ResultHistoryCount));
        }

        private void RaiseOperationStateChanged()
        {
            OnPropertyChanged(nameof(IsPointSelected));
            OnPropertyChanged(nameof(CanReadSelectedPoint));
            OnPropertyChanged(nameof(CanWriteSelectedPoint));
            OnPropertyChanged(nameof(CanReadDirectAddress));
            OnPropertyChanged(nameof(CanWriteDirectAddress));
            OnPropertyChanged(nameof(ConfigPointOperationHintText));
            OnPropertyChanged(nameof(DirectOperationHintText));
            OnPropertyChanged(nameof(ConfigPointAccessModeText));
        }

        private static DebugResultItem BuildReadResult(
            string actionName,
            string targetMode,
            PlcPointRuntimeSnapshot snapshot,
            Result result)
        {
            DebugResultItem item = new DebugResultItem
            {
                Success = result != null && result.Success,
                ActionName = actionName,
                TargetMode = targetMode,
                PlcName = snapshot == null ? string.Empty : snapshot.PlcName,
                PointName = snapshot == null ? string.Empty : snapshot.PointName,
                PointDisplayName = snapshot == null ? string.Empty : snapshot.DisplayName,
                Address = snapshot == null ? string.Empty : snapshot.AddressText,
                DataType = snapshot == null ? string.Empty : snapshot.DataType,
                Length = 0,
                AccessMode = snapshot == null ? string.Empty : snapshot.AccessMode,
                AccessModeText = ToAccessModeText(snapshot == null ? string.Empty : snapshot.AccessMode),
                InputValueText = string.Empty,
                ValueText = snapshot == null ? string.Empty : snapshot.ValueText,
                RawValue = snapshot == null ? string.Empty : snapshot.RawValue,
                Quality = snapshot == null ? string.Empty : snapshot.Quality,
                Message = result == null ? string.Empty : (result.Message ?? string.Empty),
                Confirmed = false,
                Time = DateTime.Now
            };

            return item;
        }

        private static DebugResultItem BuildWriteResult(
            string actionName,
            string targetMode,
            string plcName,
            string pointName,
            string pointDisplayName,
            string address,
            string dataType,
            int length,
            string accessMode,
            string inputValueText,
            bool confirmed,
            Result result)
        {
            return new DebugResultItem
            {
                Success = result != null && result.Success,
                ActionName = actionName,
                TargetMode = targetMode,
                PlcName = plcName ?? string.Empty,
                PointName = pointName ?? string.Empty,
                PointDisplayName = pointDisplayName ?? string.Empty,
                Address = address ?? string.Empty,
                DataType = dataType ?? string.Empty,
                Length = length <= 0 ? 1 : length,
                AccessMode = accessMode ?? string.Empty,
                AccessModeText = ToAccessModeText(accessMode),
                InputValueText = inputValueText ?? string.Empty,
                ValueText = inputValueText ?? string.Empty,
                RawValue = inputValueText ?? string.Empty,
                Quality = result != null && result.Success ? "Good" : "Error",
                Message = result == null ? string.Empty : (result.Message ?? string.Empty),
                Confirmed = confirmed,
                Time = DateTime.Now
            };
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string NormalizeAccessMode(string value)
        {
            string normalized = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return "ReadWrite";
            }

            return normalized;
        }

        private static bool IsArrayType(string dataType)
        {
            return !string.IsNullOrWhiteSpace(dataType)
                && dataType.Trim().EndsWith("[]", StringComparison.OrdinalIgnoreCase);
        }

        private static string FormatTime(DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
        }

        private static string ToAccessModeText(string accessMode)
        {
            if (string.Equals(accessMode, "ReadOnly", StringComparison.OrdinalIgnoreCase))
            {
                return "只读";
            }

            if (string.Equals(accessMode, "WriteOnly", StringComparison.OrdinalIgnoreCase))
            {
                return "只写";
            }

            if (string.Equals(accessMode, "ReadWrite", StringComparison.OrdinalIgnoreCase))
            {
                return "读写";
            }

            return "-";
        }

        public sealed class DebugPointOptionItem
        {
            public string PlcName { get; set; }

            public string PointName { get; set; }

            public string DisplayName { get; set; }

            public string GroupName { get; set; }

            public string Address { get; set; }

            public string DataType { get; set; }

            public int Length { get; set; }

            public string AccessMode { get; set; }

            public bool IsEnabled { get; set; }

            public string Description { get; set; }

            public string Remark { get; set; }

            public string DisplayTitle
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(DisplayName))
                    {
                        return DisplayName;
                    }

                    return string.IsNullOrWhiteSpace(PointName) ? "未命名点位" : PointName;
                }
            }

            public string AccessModeText
            {
                get { return ToAccessModeText(AccessMode); }
            }
        }

        public sealed class DebugResultItem
        {
            public bool Success { get; set; }

            public string ActionName { get; set; }

            public string TargetMode { get; set; }

            public string PlcName { get; set; }

            public string PointName { get; set; }

            public string PointDisplayName { get; set; }

            public string Address { get; set; }

            public string DataType { get; set; }

            public int Length { get; set; }

            public string AccessMode { get; set; }

            public string AccessModeText { get; set; }

            public string InputValueText { get; set; }

            public string ValueText { get; set; }

            public string RawValue { get; set; }

            public string Quality { get; set; }

            public string Message { get; set; }

            public bool Confirmed { get; set; }

            public DateTime Time { get; set; }

            public string TimeText
            {
                get { return Time == default(DateTime) ? "-" : Time.ToString("yyyy-MM-dd HH:mm:ss"); }
            }

            public string SummaryText
            {
                get
                {
                    string target = !string.IsNullOrWhiteSpace(PointName)
                        ? PointName
                        : (!string.IsNullOrWhiteSpace(Address) ? Address : "-");

                    return string.Format(
                        "{0} | {1} | {2}",
                        Success ? "成功" : "失败",
                        string.IsNullOrWhiteSpace(ActionName) ? "-" : ActionName,
                        target);
                }
            }
        }
    }
}