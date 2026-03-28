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
using System.Text;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 运行时操作服务。
    /// 统一承载调试页与运行时业务层的读写入口，避免页面直接访问底层客户端。
    ///
    /// 设计原则：
    /// 1. 配置点位写入优先，先走配置校验再调用驱动；
    /// 2. 直接地址写入视为高风险操作，要求显式确认；
    /// 3. 成功操作记日志但不弹消息，避免工程调试频繁干扰；
    /// 4. 成功读取/写入后尽量同步更新 PLC 运行态缓存，便于监视页立即可见。
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

                var writeResult = clientResult.Item.Write(
                    point.AreaType,
                    point.Address,
                    point.DataType,
                    value,
                    point.BitIndex,
                    point.StringLength,
                    point.ArrayLength);

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

                var writeResult = clientResult.Item.Write(
                    areaType.Trim(),
                    address.Trim(),
                    dataType.Trim(),
                    value,
                    bitIndex,
                    stringLength,
                    arrayLength);

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
            return config.Points.FirstOrDefault(p => p != null && string.Equals(p.Name, pointName, StringComparison.OrdinalIgnoreCase));
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
            var readResult = client.ReadBlock(
                point.AreaType,
                point.Address,
                GetPointReadLength(point),
                point.DataType);

            if (!readResult.Success || readResult.Item == null || readResult.Item.Buffer == null)
            {
                return Fail<PlcPointRuntimeSnapshot>(readResult.Code, readResult.Message);
            }

            object rawValue;
            string rawValueText;
            string displayValueText;
            string errorMessage;
            if (!TryParsePointValue(point, readResult.Item.Buffer, out rawValue, out rawValueText, out displayValueText, out errorMessage))
            {
                return Fail<PlcPointRuntimeSnapshot>((int)DbErrorCode.QueryFailed, string.IsNullOrWhiteSpace(errorMessage) ? "PLC点位解析失败" : errorMessage);
            }

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

        private static int GetPointReadLength(PlcPointConfig point)
        {
            if (point.ReadLength > 0)
            {
                return point.ReadLength;
            }

            switch ((point.DataType ?? string.Empty).Trim())
            {
                case "Bit":
                case "Bool":
                case "Byte":
                    return 1;
                case "Short":
                case "UShort":
                    return 1;
                case "Int":
                case "UInt":
                case "Float":
                    return 2;
                case "Double":
                case "Long":
                case "ULong":
                    return 4;
                case "String":
                    return point.StringLength > 0 ? point.StringLength : 1;
                case "ByteArray":
                    return point.ArrayLength > 0 ? point.ArrayLength : 1;
                default:
                    return 1;
            }
        }

        private static bool TryParsePointValue(
            PlcPointConfig point,
            byte[] bytes,
            out object rawValue,
            out string rawValueText,
            out string displayValueText,
            out string errorMessage)
        {
            rawValue = null;
            rawValueText = null;
            displayValueText = null;
            errorMessage = null;

            if (bytes == null || bytes.Length == 0)
            {
                errorMessage = "原始字节为空";
                return false;
            }

            try
            {
                switch ((point.DataType ?? string.Empty).Trim())
                {
                    case "Bit":
                    case "Bool":
                        rawValue = ReadBooleanValue(point, bytes);
                        rawValueText = ((bool)rawValue) ? "True" : "False";
                        displayValueText = ((bool)rawValue) ? "ON" : "OFF";
                        return true;
                    case "Byte":
                        rawValue = bytes[0];
                        rawValueText = Convert.ToString(rawValue, CultureInfo.InvariantCulture);
                        displayValueText = rawValueText;
                        return true;
                    case "Short":
                        rawValue = BitConverter.ToInt16(ApplyEndian(bytes, point, 2), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "UShort":
                        rawValue = BitConverter.ToUInt16(ApplyEndian(bytes, point, 2), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "Int":
                        rawValue = BitConverter.ToInt32(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "UInt":
                        rawValue = BitConverter.ToUInt32(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "Float":
                        rawValue = BitConverter.ToSingle(ApplyEndian(bytes, point, 4), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "Double":
                        rawValue = BitConverter.ToDouble(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "Long":
                        rawValue = BitConverter.ToInt64(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "ULong":
                        rawValue = BitConverter.ToUInt64(ApplyEndian(bytes, point, 8), 0);
                        return BuildNumericDisplay(point, Convert.ToDouble(rawValue, CultureInfo.InvariantCulture), out rawValueText, out displayValueText);
                    case "String":
                        rawValue = ReadStringValue(point, bytes);
                        rawValueText = rawValue == null ? string.Empty : rawValue.ToString();
                        displayValueText = rawValueText;
                        return true;
                    case "ByteArray":
                        rawValue = bytes.ToArray();
                        rawValueText = ToHexString(bytes);
                        displayValueText = rawValueText;
                        return true;
                    default:
                        rawValue = bytes.ToArray();
                        rawValueText = ToHexString(bytes);
                        displayValueText = rawValueText;
                        return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static void BuildWriteDisplay(PlcPointConfig point, object value, out string rawValueText, out string displayValueText)
        {
            rawValueText = value == null ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
            displayValueText = rawValueText;

            if (value == null)
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

            double numericValue;
            if (TryConvertToDouble(value, out numericValue))
            {
                BuildNumericDisplay(point, numericValue, out rawValueText, out displayValueText);
                return;
            }

            if (value is byte[])
            {
                rawValueText = ToHexString((byte[])value);
                displayValueText = rawValueText;
            }
        }

        private static bool BuildNumericDisplay(PlcPointConfig point, double rawNumeric, out string rawValueText, out string displayValueText)
        {
            rawValueText = rawNumeric.ToString("0.########", CultureInfo.InvariantCulture);
            var scaled = (rawNumeric * (point.Scale == 0D ? 1D : point.Scale)) + point.Offset;
            displayValueText = string.IsNullOrWhiteSpace(point.Unit)
                ? scaled.ToString("0.########", CultureInfo.InvariantCulture)
                : string.Format(CultureInfo.InvariantCulture, "{0:0.########} {1}", scaled, point.Unit);
            return true;
        }

        private static bool ReadBooleanValue(PlcPointConfig point, byte[] bytes)
        {
            if (!point.BitIndex.HasValue)
            {
                return bytes[0] != 0;
            }

            var bitIndex = point.BitIndex.Value;
            if (bitIndex < 0)
            {
                bitIndex = 0;
            }

            if (bytes.Length >= 2 && bitIndex < 16)
            {
                var word = BitConverter.ToUInt16(bytes.Length >= 2 ? bytes : new[] { bytes[0], (byte)0 }, 0);
                return (word & (1 << bitIndex)) != 0;
            }

            if (bitIndex > 7)
            {
                bitIndex = 7;
            }

            return (bytes[0] & (1 << bitIndex)) != 0;
        }

        private static string ReadStringValue(PlcPointConfig point, byte[] bytes)
        {
            var buffer = bytes;
            if (point.StringLength > 0 && point.StringLength < buffer.Length)
            {
                buffer = buffer.Take(point.StringLength).ToArray();
            }

            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(string.IsNullOrWhiteSpace(point.StringEncoding) ? "ASCII" : point.StringEncoding.Trim());
            }
            catch
            {
                encoding = Encoding.ASCII;
            }

            return encoding.GetString(buffer).TrimEnd('\0', ' ');
        }

        private static byte[] ApplyEndian(byte[] bytes, PlcPointConfig point, int expectedLength)
        {
            var buffer = bytes.Take(expectedLength).ToArray();
            var wordOrder = point.WordOrder ?? string.Empty;
            if (buffer.Length == 4 && string.Equals(wordOrder, "HighLow", StringComparison.OrdinalIgnoreCase))
            {
                buffer = new[] { buffer[2], buffer[3], buffer[0], buffer[1] };
            }
            else if (buffer.Length == 8 && string.Equals(wordOrder, "HighLow", StringComparison.OrdinalIgnoreCase))
            {
                buffer = new[] { buffer[6], buffer[7], buffer[4], buffer[5], buffer[2], buffer[3], buffer[0], buffer[1] };
            }

            if (string.Equals(point.ByteOrder, "BigEndian", StringComparison.OrdinalIgnoreCase))
            {
                Array.Reverse(buffer);
            }

            return buffer;
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
                return bool.TryParse((string)value, out result);
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
