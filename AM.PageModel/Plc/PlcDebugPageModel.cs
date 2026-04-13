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
    /// </summary>
    public class PlcDebugPageModel : BindableBase
    {
        private readonly PlcOperationService _operationService;

        private readonly List<string> _directDataTypeOptions;

        private List<string> _plcOptions;
        private List<DebugPointOptionItem> _pointOptions;
        private string _selectedPlcName;
        private string _selectedPointName;
        private string _configPointAddress;
        private string _configPointDataType;
        private int _configPointLength;
        private string _configWriteValueText;
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
            _selectedPlcName = string.Empty;
            _selectedPointName = string.Empty;
            _configPointAddress = string.Empty;
            _configPointDataType = string.Empty;
            _configPointLength = 1;
            _configWriteValueText = string.Empty;
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

        public string ConfigWriteValueText
        {
            get { return _configWriteValueText; }
            set { SetProperty(ref _configWriteValueText, value ?? string.Empty); }
        }

        public string DirectAddress
        {
            get { return _directAddress; }
            set { SetProperty(ref _directAddress, value ?? string.Empty); }
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

                SetProperty(ref _directDataType, normalized);
            }
        }

        public int DirectLength
        {
            get { return _directLength; }
            set { SetProperty(ref _directLength, value <= 0 ? 1 : value); }
        }

        public string DirectWriteValueText
        {
            get { return _directWriteValueText; }
            set { SetProperty(ref _directWriteValueText, value ?? string.Empty); }
        }

        public bool DirectWriteConfirmed
        {
            get { return _directWriteConfirmed; }
            set { SetProperty(ref _directWriteConfirmed, value); }
        }

        public DebugResultItem LastResult
        {
            get { return _lastResult; }
            private set { SetProperty(ref _lastResult, value); }
        }

        public string RuntimeSummaryText
        {
            get { return _runtimeSummaryText; }
            private set { SetProperty(ref _runtimeSummaryText, value ?? string.Empty); }
        }

        public async Task<Result> LoadAsync()
        {
            return await Task.Run(() =>
            {
                BuildPlcOptions();
                BuildPointOptions();
                ApplySelectedPointMeta();
                ApplyRuntimeSummary();

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

            OnPropertyChanged(nameof(SelectedPointDisplayText));
        }

        public void SetSelectedPoint(string pointName)
        {
            SelectedPointName = NormalizeText(pointName);
            ApplySelectedPointMeta();

            OnPropertyChanged(nameof(SelectedPointDisplayText));
        }

        public Result TestReadPoint()
        {
            var result = _operationService.TestReadPoint(SelectedPointName);
            LastResult = BuildReadResult("按配置点位读取", result.Success ? result.Item : null, result);
            ApplyRuntimeSummary();

            return result.Success
                ? Result.Ok(result.Message, result.Source)
                : Result.Fail(result.Code, result.Message, result.Source);
        }

        public Result WritePoint()
        {
            var result = _operationService.WritePoint(SelectedPointName, ConfigWriteValueText, true);
            LastResult = BuildWriteResult(
                "按配置点位写入",
                SelectedPlcName,
                SelectedPointName,
                ConfigPointAddress,
                ConfigPointDataType,
                ConfigWriteValueText,
                result);
            ApplyRuntimeSummary();

            return result;
        }

        public Result TestReadAddress()
        {
            var result = _operationService.TestReadAddress(
                SelectedPlcName,
                null,
                DirectAddress,
                DirectDataType,
                null,
                string.Equals(DirectDataType, "string", StringComparison.OrdinalIgnoreCase) ? DirectLength : 0,
                IsArrayType(DirectDataType) ? DirectLength : 0);

            LastResult = BuildReadResult("按直接地址读取", result.Success ? result.Item : null, result);
            ApplyRuntimeSummary();

            return result.Success
                ? Result.Ok(result.Message, result.Source)
                : Result.Fail(result.Code, result.Message, result.Source);
        }

        public Result WriteAddress()
        {
            var result = _operationService.WriteAddress(
                SelectedPlcName,
                null,
                DirectAddress,
                DirectDataType,
                DirectWriteValueText,
                DirectWriteConfirmed,
                null,
                string.Equals(DirectDataType, "string", StringComparison.OrdinalIgnoreCase) ? DirectLength : 0,
                IsArrayType(DirectDataType) ? DirectLength : 0);

            LastResult = BuildWriteResult(
                "按直接地址写入",
                SelectedPlcName,
                null,
                DirectAddress,
                DirectDataType,
                DirectWriteValueText,
                result);
            ApplyRuntimeSummary();

            return result;
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
                    Address = x.Address,
                    DataType = x.DataType,
                    Length = x.Length
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

            ConfigPointAddress = item == null ? string.Empty : (item.Address ?? string.Empty);
            ConfigPointDataType = item == null ? string.Empty : (item.DataType ?? string.Empty);
            ConfigPointLength = item == null ? 1 : item.Length;

            OnPropertyChanged(nameof(SelectedPointDisplayText));
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

        private static DebugResultItem BuildReadResult(string actionName, PlcPointRuntimeSnapshot snapshot, Result result)
        {
            return new DebugResultItem
            {
                Success = result != null && result.Success,
                ActionName = actionName,
                PlcName = snapshot == null ? string.Empty : snapshot.PlcName,
                PointName = snapshot == null ? string.Empty : snapshot.PointName,
                Address = snapshot == null ? string.Empty : snapshot.AddressText,
                DataType = snapshot == null ? string.Empty : snapshot.DataType,
                ValueText = snapshot == null ? string.Empty : snapshot.ValueText,
                RawValue = snapshot == null ? string.Empty : snapshot.RawValue,
                Quality = snapshot == null ? string.Empty : snapshot.Quality,
                Message = result == null ? string.Empty : (result.Message ?? string.Empty),
                Time = DateTime.Now
            };
        }

        private static DebugResultItem BuildWriteResult(
            string actionName,
            string plcName,
            string pointName,
            string address,
            string dataType,
            string valueText,
            Result result)
        {
            return new DebugResultItem
            {
                Success = result != null && result.Success,
                ActionName = actionName,
                PlcName = plcName ?? string.Empty,
                PointName = pointName ?? string.Empty,
                Address = address ?? string.Empty,
                DataType = dataType ?? string.Empty,
                ValueText = valueText ?? string.Empty,
                RawValue = valueText ?? string.Empty,
                Quality = result != null && result.Success ? "Good" : "Error",
                Message = result == null ? string.Empty : (result.Message ?? string.Empty),
                Time = DateTime.Now
            };
        }

        private static bool IsArrayType(string dataType)
        {
            return !string.IsNullOrWhiteSpace(dataType)
                && dataType.Trim().EndsWith("[]", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string FormatTime(DateTime? time)
        {
            return time.HasValue ? time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
        }

        public sealed class DebugPointOptionItem
        {
            public string PlcName { get; set; }

            public string PointName { get; set; }

            public string DisplayName { get; set; }

            public string Address { get; set; }

            public string DataType { get; set; }

            public int Length { get; set; }

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
        }

        public sealed class DebugResultItem
        {
            public bool Success { get; set; }

            public string ActionName { get; set; }

            public string PlcName { get; set; }

            public string PointName { get; set; }

            public string Address { get; set; }

            public string DataType { get; set; }

            public string ValueText { get; set; }

            public string RawValue { get; set; }

            public string Quality { get; set; }

            public string Message { get; set; }

            public DateTime Time { get; set; }

            public string TimeText
            {
                get { return Time == default(DateTime) ? "-" : Time.ToString("yyyy-MM-dd HH:mm:ss"); }
            }
        }
    }
}