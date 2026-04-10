using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using AM.Model.Runtime;
using System;
using System.Globalization;
using System.Linq;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 运行时操作服务。
    /// 统一承载调试页与运行时业务层的读写入口，避免页面直接访问底层客户端。
    ///
    /// 当前版本采用最简单实现：
    /// 1. 优先按点位配置逐点读写；
    /// 2. 直接地址调试也统一走点位请求模型；
    /// 3. 不再在服务层做协议块切片与字节解析。
    /// </summary>
    public class PlcOperationService : ServiceBase
    {
        protected override string MessageSourceName
        {
            get { return "PlcOperation"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Plc; }
        }

        public PlcOperationService()
            : this(SystemContext.Instance.Reporter)
        {
        }

        public PlcOperationService(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 按配置点位名称执行写入。
        /// </summary>
        public Result WritePoint(string pointName, object value, bool confirmed = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pointName))
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "点位名称不能为空");
                }

                if (value == null)
                {
                    return Fail((int)DbErrorCode.InvalidArgument, "写入值不能为空");
                }

                var point = FindPointConfig(pointName.Trim());
                if (point == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到对应 PLC 点位配置");
                }

                var safetyResult = ValidatePointWrite(point, confirmed);
                if (!safetyResult.Success)
                {
                    return safetyResult;
                }

                var clientResult = GetConnectedClient(point.PlcName);
                if (!clientResult.Success)
                {
                    return Fail(clientResult.Code, clientResult.Message);
                }

                var writeResult = clientResult.Item.WritePoint(new PlcPointWriteRequest
                {
                    PlcName = point.PlcName,
                    AreaType = point.AreaType,
                    Address = point.Address,
                    DataType = point.DataType,
                    Value = value,
                    BitIndex = point.BitIndex,
                    StringLength = point.StringLength,
                    ArrayLength = point.ArrayLength
                });

                if (!writeResult.Success)
                {
                    return Fail(writeResult.Code, writeResult.Message);
                }

                UpdatePointRuntimeAfterWrite(point, value);
                return OkLogOnly("PLC点位写入成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "PLC点位写入失败");
            }
        }

        /// <summary>
        /// 按直接地址执行写入。
        /// </summary>
        public Result WriteAddress(
            string plcName,
            string areaType,
            string address,
            string dataType,
            object value,
            bool confirmed,
            short? bitIndex = null,
            int stringLength = 0,
            int arrayLength = 0)
        {
            try
            {
                var validateResult = ValidateDirectAddressArguments(
                    plcName,
                    areaType,
                    address,
                    dataType,
                    value,
                    confirmed,
                    stringLength,
                    arrayLength);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                var clientResult = GetConnectedClient(plcName.Trim());
                if (!clientResult.Success)
                {
                    return Fail(clientResult.Code, clientResult.Message);
                }

                var writeResult = clientResult.Item.WritePoint(new PlcPointWriteRequest
                {
                    PlcName = plcName.Trim(),
                    AreaType = areaType.Trim(),
                    Address = address.Trim(),
                    DataType = dataType.Trim(),
                    Value = value,
                    BitIndex = bitIndex,
                    StringLength = stringLength,
                    ArrayLength = arrayLength
                });

                if (!writeResult.Success)
                {
                    return Fail(writeResult.Code, writeResult.Message);
                }

                return OkLogOnly("PLC地址写入成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "PLC地址写入失败");
            }
        }

        /// <summary>
        /// 按配置点位执行测试读取。
        /// </summary>
        public Result<PlcPointRuntimeSnapshot> TestReadPoint(string pointName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pointName))
                {
                    return Fail<PlcPointRuntimeSnapshot>((int)DbErrorCode.InvalidArgument, "点位名称不能为空");
                }

                var point = FindPointConfig(pointName.Trim());
                if (point == null)
                {
                    return Warn<PlcPointRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 点位配置");
                }

                var readResult = ReadPointInternal(point);
                if (!readResult.Success)
                {
                    return readResult;
                }

                RuntimeContext.Instance.Plc.SetPointSnapshot(readResult.Item);
                RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
                return OkLogOnly(readResult.Item, "PLC点位测试读取成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.Unknown, "PLC点位测试读取失败");
            }
        }

        /// <summary>
        /// 按直接地址执行测试读取。
        /// </summary>
        public Result<PlcPointRuntimeSnapshot> TestReadAddress(
            string plcName,
            string areaType,
            string address,
            string dataType,
            short? bitIndex = null,
            int stringLength = 0,
            int arrayLength = 0)
        {
            try
            {
                var validateResult = ValidateDirectReadArguments(plcName, areaType, address, dataType, stringLength, arrayLength);
                if (!validateResult.Success)
                {
                    return Fail<PlcPointRuntimeSnapshot>(validateResult.Code, validateResult.Message);
                }

                var clientResult = GetConnectedClient(plcName.Trim());
                if (!clientResult.Success)
                {
                    return Fail<PlcPointRuntimeSnapshot>(clientResult.Code, clientResult.Message);
                }

                var tempPoint = new PlcPointConfig
                {
                    PlcName = plcName.Trim(),
                    Name = string.Format("{0}_{1}_{2}", plcName.Trim(), areaType.Trim(), address.Trim()),
                    DisplayName = "地址测试读取",
                    GroupName = "Debug",
                    AreaType = areaType.Trim(),
                    Address = address.Trim(),
                    BitIndex = bitIndex,
                    DataType = dataType.Trim(),
                    StringLength = stringLength,
                    ArrayLength = arrayLength,
                    ReadLength = 0,
                    Scale = 1D,
                    Offset = 0D,
                    Unit = null,
                    AccessMode = "ReadOnly",
                    ReadMode = "Single",
                    IsEnabled = true
                };

                return ReadPointWithClient(clientResult.Item, tempPoint);
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.Unknown, "PLC地址测试读取失败");
            }
        }

        private Result ValidatePointWrite(PlcPointConfig point, bool confirmed)
        {
            if (point == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "点位配置不能为空");
            }

            if (string.Equals(point.AccessMode, "ReadOnly", StringComparison.OrdinalIgnoreCase))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "只读点位禁止写入: " + point.DisplayTitle);
            }

            if (RequiresExplicitConfirmation(point) && !confirmed)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "该点位写入属于高风险操作，必须显式确认后才能执行");
            }

            if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase) && point.StringLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "String 类型点位必须配置大于 0 的字符串长度");
            }

            if (string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase) && point.ArrayLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "ByteArray 类型点位必须配置大于 0 的数组长度");
            }

            return OkSilent("PLC点位写入校验通过");
        }

        private Result ValidateDirectAddressArguments(
            string plcName,
            string areaType,
            string address,
            string dataType,
            object value,
            bool confirmed,
            int stringLength,
            int arrayLength)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(areaType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "区域类型不能为空");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(dataType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "数据类型不能为空");
            }

            if (value == null)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "写入值不能为空");
            }

            if (!confirmed)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "直接地址写入属于高风险操作，必须显式确认后才能执行");
            }

            if (string.Equals(dataType.Trim(), "String", StringComparison.OrdinalIgnoreCase) && stringLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "String 类型直接写入必须提供大于 0 的字符串长度");
            }

            if (string.Equals(dataType.Trim(), "ByteArray", StringComparison.OrdinalIgnoreCase) && arrayLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "ByteArray 类型直接写入必须提供大于 0 的数组长度");
            }

            return OkSilent("PLC地址写入校验通过");
        }

        private Result ValidateDirectReadArguments(
            string plcName,
            string areaType,
            string address,
            string dataType,
            int stringLength,
            int arrayLength)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(areaType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "区域类型不能为空");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(dataType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "数据类型不能为空");
            }

            if (string.Equals(dataType.Trim(), "String", StringComparison.OrdinalIgnoreCase) && stringLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "String 类型测试读取必须提供大于 0 的字符串长度");
            }

            if (string.Equals(dataType.Trim(), "ByteArray", StringComparison.OrdinalIgnoreCase) && arrayLength <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "ByteArray 类型测试读取必须提供大于 0 的数组长度");
            }

            return OkSilent("PLC地址测试读取校验通过");
        }

        private Result<IPlcClient> GetConnectedClient(string plcName)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail<IPlcClient>((int)DbErrorCode.InvalidArgument, "PLC名称不能为空");
            }

            IPlcClient client;
            if (!MachineContext.Instance.Plcs.TryGetValue(plcName, out client) || client == null)
            {
                return Fail<IPlcClient>((int)DbErrorCode.NotFound, "未找到对应 PLC 客户端: " + plcName);
            }

            var isConnectedResult = client.IsConnected();
            if (isConnectedResult.Success && isConnectedResult.Item)
            {
                return OkSilent(client, "PLC客户端获取成功");
            }

            var connectResult = client.Connect();
            if (!connectResult.Success)
            {
                return Fail<IPlcClient>(connectResult.Code, connectResult.Message);
            }

            return OkSilent(client, "PLC客户端连接成功");
        }

        private PlcPointConfig FindPointConfig(string pointName)
        {
            var config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
            var points = config.Points ?? new System.Collections.Generic.List<PlcPointConfig>();
            return points.FirstOrDefault(p => p != null && string.Equals(p.Name, pointName, StringComparison.OrdinalIgnoreCase));
        }

        private Result<PlcPointRuntimeSnapshot> ReadPointInternal(PlcPointConfig point)
        {
            var clientResult = GetConnectedClient(point.PlcName);
            if (!clientResult.Success)
            {
                return Fail<PlcPointRuntimeSnapshot>(clientResult.Code, clientResult.Message);
            }

            return ReadPointWithClient(clientResult.Item, point);
        }

        private Result<PlcPointRuntimeSnapshot> ReadPointWithClient(IPlcClient client, PlcPointConfig point)
        {
            var readResult = client.ReadPoint(new PlcPointReadRequest
            {
                PlcName = point.PlcName,
                AreaType = point.AreaType,
                Address = point.Address,
                DataType = point.DataType,
                BitIndex = point.BitIndex,
                StringLength = point.StringLength,
                ArrayLength = point.ArrayLength
            });

            if (!readResult.Success || readResult.Item == null)
            {
                return Fail<PlcPointRuntimeSnapshot>(readResult.Code, readResult.Message);
            }

            string rawValueText = readResult.Item.ValueText ?? string.Empty;
            string displayValueText = BuildReadDisplay(point, rawValueText);

            var snapshot = new PlcPointRuntimeSnapshot
            {
                PlcName = point.PlcName,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                AreaType = point.AreaType,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = "Good",
                UpdateTime = DateTime.Now,
                IsConnected = true,
                HasError = false,
                ErrorMessage = null
            };

            return OkSilent(snapshot, "PLC点位读取成功");
        }

        private void UpdatePointRuntimeAfterWrite(PlcPointConfig point, object value)
        {
            if (point == null)
            {
                return;
            }

            string rawValueText;
            string displayValueText;
            BuildWriteDisplay(point, value, out rawValueText, out displayValueText);

            RuntimeContext.Instance.Plc.SetPointSnapshot(new PlcPointRuntimeSnapshot
            {
                PlcName = point.PlcName,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                AreaType = point.AreaType,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = "Good",
                UpdateTime = DateTime.Now,
                IsConnected = true,
                HasError = false,
                ErrorMessage = null
            });

            RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
        }

        private static bool RequiresExplicitConfirmation(PlcPointConfig point)
        {
            if (point == null)
            {
                return true;
            }

            if (string.Equals(point.AccessMode, "WriteOnly", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return point.ArrayLength > 1;
        }

        private static string BuildReadDisplay(PlcPointConfig point, string rawValueText)
        {
            if (point == null)
            {
                return rawValueText ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(rawValueText))
            {
                return string.Empty;
            }

            if (string.Equals(point.DataType, "Bit", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "Bool", StringComparison.OrdinalIgnoreCase))
            {
                bool boolValue;
                if (TryConvertToBooleanText(rawValueText, out boolValue))
                {
                    return boolValue ? "ON" : "OFF";
                }

                return rawValueText;
            }

            if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase))
            {
                return rawValueText;
            }

            double numericValue;
            if (TryConvertToDoubleText(rawValueText, out numericValue))
            {
                string rawText;
                string displayText;
                BuildNumericDisplay(point, numericValue, out rawText, out displayText);
                return displayText;
            }

            return rawValueText;
        }

        private static void BuildWriteDisplay(PlcPointConfig point, object value, out string rawValueText, out string displayValueText)
        {
            rawValueText = value == null ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
            displayValueText = rawValueText;

            if (value == null || point == null)
            {
                return;
            }

            if (string.Equals(point.DataType, "Bit", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(point.DataType, "Bool", StringComparison.OrdinalIgnoreCase))
            {
                bool boolValue;
                if (TryConvertToBoolean(value, out boolValue))
                {
                    rawValueText = boolValue ? "True" : "False";
                    displayValueText = boolValue ? "ON" : "OFF";
                }
                return;
            }

            if (string.Equals(point.DataType, "String", StringComparison.OrdinalIgnoreCase))
            {
                displayValueText = rawValueText;
                return;
            }

            if (string.Equals(point.DataType, "ByteArray", StringComparison.OrdinalIgnoreCase))
            {
                var bytes = value as byte[];
                if (bytes != null)
                {
                    rawValueText = ToHexString(bytes);
                    displayValueText = rawValueText;
                }
                return;
            }

            double numericValue;
            if (TryConvertToDouble(value, out numericValue))
            {
                BuildNumericDisplay(point, numericValue, out rawValueText, out displayValueText);
            }
        }

        private static bool BuildNumericDisplay(PlcPointConfig point, double rawNumeric, out string rawValueText, out string displayValueText)
        {
            rawValueText = rawNumeric.ToString("0.########", CultureInfo.InvariantCulture);
            var scaled = (rawNumeric * (point == null || point.Scale == 0D ? 1D : point.Scale)) + (point == null ? 0D : point.Offset);
            displayValueText = point == null || string.IsNullOrWhiteSpace(point.Unit)
                ? scaled.ToString("0.########", CultureInfo.InvariantCulture)
                : string.Format(CultureInfo.InvariantCulture, "{0:0.########} {1}", scaled, point.Unit);
            return true;
        }

        private static bool TryConvertToBoolean(object value, out bool result)
        {
            result = false;
            if (value is bool)
            {
                result = (bool)value;
                return true;
            }

            if (value is string)
            {
                return TryConvertToBooleanText((string)value, out result);
            }

            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture) != 0D;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryConvertToBooleanText(string valueText, out bool result)
        {
            result = false;
            if (string.IsNullOrWhiteSpace(valueText))
            {
                return false;
            }

            if (string.Equals(valueText, "1", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "on", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }

            if (string.Equals(valueText, "0", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "false", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(valueText, "off", StringComparison.OrdinalIgnoreCase))
            {
                result = false;
                return true;
            }

            return bool.TryParse(valueText, out result);
        }

        private static bool TryConvertToDouble(object value, out double result)
        {
            result = 0D;
            if (value == null)
            {
                return false;
            }

            try
            {
                result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryConvertToDoubleText(string valueText, out double result)
        {
            return double.TryParse(valueText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        private static string ToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return string.Empty;
            }

            return BitConverter.ToString(bytes).Replace("-", " ");
        }
    }
}