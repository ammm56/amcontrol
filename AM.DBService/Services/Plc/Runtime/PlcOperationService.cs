using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using AM.Model.Runtime;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.Plc.Runtime
{
    /// <summary>
    /// PLC 运行时操作服务。
    /// 当前版本只保留最小职责：
    /// 1. 按点位配置直接读写；
    /// 2. 调试页直接地址读写统一走 M_PointRequest；
    /// 3. 统一使用 Length；
    /// 4. 不再承担 AreaType、BitIndex、块切片等复杂逻辑。
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

                PlcPointConfig point = FindPointConfig(pointName.Trim());
                if (point == null)
                {
                    return Warn((int)DbErrorCode.NotFound, "未找到对应 PLC 点位配置");
                }

                Result validateResult = ValidatePointWrite(point, confirmed);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                Result<IPlcClient> clientResult = GetConnectedClient(point.PlcName);
                if (!clientResult.Success)
                {
                    return Fail(clientResult.Code, clientResult.Message);
                }

                Result<M_PointData> writeResult = clientResult.Item.WritePoint(new M_PointWriteRequest
                {
                    address = point.Address,
                    dataType = NormalizeDataType(point.DataType),
                    value = value,
                    length = ResolvePointLength(point)
                });

                if (!writeResult.Success || writeResult.Item == null)
                {
                    return Fail(writeResult.Code, writeResult.Message);
                }

                UpdatePointRuntimeAfterWrite(point, writeResult.Item);
                return OkLogOnly("PLC 点位写入成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "PLC 点位写入失败");
            }
        }

        /// <summary>
        /// 按直接地址执行写入。
        /// 保留兼容签名，内部仅使用 address/dataType/length。
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
                int length = ResolveLengthByArguments(dataType, stringLength, arrayLength);

                Result validateResult = ValidateDirectAddressArguments(
                    plcName,
                    address,
                    dataType,
                    value,
                    confirmed,
                    length);

                if (!validateResult.Success)
                {
                    return validateResult;
                }

                Result<IPlcClient> clientResult = GetConnectedClient(plcName.Trim());
                if (!clientResult.Success)
                {
                    return Fail(clientResult.Code, clientResult.Message);
                }

                Result<M_PointData> writeResult = clientResult.Item.WritePoint(new M_PointWriteRequest
                {
                    address = address.Trim(),
                    dataType = NormalizeDataType(dataType),
                    value = value,
                    length = length
                });

                if (!writeResult.Success || writeResult.Item == null)
                {
                    return Fail(writeResult.Code, writeResult.Message);
                }

                return OkLogOnly("PLC 地址写入成功");
            }
            catch (Exception ex)
            {
                return HandleException(ex, (int)DbErrorCode.Unknown, "PLC 地址写入失败");
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

                PlcPointConfig point = FindPointConfig(pointName.Trim());
                if (point == null)
                {
                    return Warn<PlcPointRuntimeSnapshot>((int)DbErrorCode.NotFound, "未找到对应 PLC 点位配置");
                }

                Result<PlcPointRuntimeSnapshot> readResult = ReadPointInternal(point);
                if (!readResult.Success)
                {
                    return readResult;
                }

                RuntimeContext.Instance.Plc.SetPointSnapshot(readResult.Item);
                RuntimeContext.Instance.Plc.NotifyPointSnapshotChanged(point.Name);
                return OkLogOnly(readResult.Item, "PLC 点位测试读取成功");
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.Unknown, "PLC 点位测试读取失败");
            }
        }

        /// <summary>
        /// 按直接地址执行测试读取。
        /// 保留兼容签名，内部仅使用 address/dataType/length。
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
                int length = ResolveLengthByArguments(dataType, stringLength, arrayLength);

                Result validateResult = ValidateDirectReadArguments(plcName, address, dataType, length);
                if (!validateResult.Success)
                {
                    return Fail<PlcPointRuntimeSnapshot>(validateResult.Code, validateResult.Message);
                }

                Result<IPlcClient> clientResult = GetConnectedClient(plcName.Trim());
                if (!clientResult.Success)
                {
                    return Fail<PlcPointRuntimeSnapshot>(clientResult.Code, clientResult.Message);
                }

                PlcPointConfig tempPoint = new PlcPointConfig
                {
                    PlcName = plcName.Trim(),
                    Name = string.Format("{0}_{1}", plcName.Trim(), address.Trim()),
                    DisplayName = "地址测试读取",
                    GroupName = "Debug",
                    Address = address.Trim(),
                    DataType = NormalizeDataType(dataType),
                    Length = length,
                    AccessMode = "ReadOnly",
                    IsEnabled = true
                };

                return ReadPointWithClient(clientResult.Item, tempPoint);
            }
            catch (Exception ex)
            {
                return HandleException<PlcPointRuntimeSnapshot>(ex, (int)DbErrorCode.Unknown, "PLC 地址测试读取失败");
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

            if (string.IsNullOrWhiteSpace(point.Address))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "点位地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(point.DataType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "点位数据类型不能为空");
            }

            if (ResolvePointLength(point) <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "点位 Length 必须大于 0");
            }

            return OkSilent("PLC 点位写入校验通过");
        }

        private Result ValidateDirectAddressArguments(
            string plcName,
            string address,
            string dataType,
            object value,
            bool confirmed,
            int length)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC 名称不能为空");
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

            if (length <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "Length 必须大于 0");
            }

            return OkSilent("PLC 地址写入校验通过");
        }

        private Result ValidateDirectReadArguments(
            string plcName,
            string address,
            string dataType,
            int length)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "PLC 名称不能为空");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "地址不能为空");
            }

            if (string.IsNullOrWhiteSpace(dataType))
            {
                return Fail((int)DbErrorCode.InvalidArgument, "数据类型不能为空");
            }

            if (length <= 0)
            {
                return Fail((int)DbErrorCode.InvalidArgument, "Length 必须大于 0");
            }

            return OkSilent("PLC 地址测试读取校验通过");
        }

        private Result<IPlcClient> GetConnectedClient(string plcName)
        {
            if (string.IsNullOrWhiteSpace(plcName))
            {
                return Fail<IPlcClient>((int)DbErrorCode.InvalidArgument, "PLC 名称不能为空");
            }

            IPlcClient client;
            if (!MachineContext.Instance.Plcs.TryGetValue(plcName, out client) || client == null)
            {
                return Fail<IPlcClient>((int)DbErrorCode.NotFound, "未找到对应 PLC 客户端: " + plcName);
            }

            Result<bool> isConnectedResult = client.IsConnected();
            if (isConnectedResult.Success && isConnectedResult.Item)
            {
                return OkSilent(client, "PLC 客户端获取成功");
            }

            Result connectResult = client.Connect();
            if (!connectResult.Success)
            {
                return Fail<IPlcClient>(connectResult.Code, connectResult.Message);
            }

            return OkSilent(client, "PLC 客户端连接成功");
        }

        private PlcPointConfig FindPointConfig(string pointName)
        {
            PlcConfig config = ConfigContext.Instance.Config.PlcConfig ?? new PlcConfig();
            var points = config.Points ?? new List<PlcPointConfig>();
            return points.FirstOrDefault(p => p != null && string.Equals(p.Name, pointName, StringComparison.OrdinalIgnoreCase));
        }

        private Result<PlcPointRuntimeSnapshot> ReadPointInternal(PlcPointConfig point)
        {
            Result<IPlcClient> clientResult = GetConnectedClient(point.PlcName);
            if (!clientResult.Success)
            {
                return Fail<PlcPointRuntimeSnapshot>(clientResult.Code, clientResult.Message);
            }

            return ReadPointWithClient(clientResult.Item, point);
        }

        private Result<PlcPointRuntimeSnapshot> ReadPointWithClient(IPlcClient client, PlcPointConfig point)
        {
            Result<M_PointData> readResult = client.ReadPoint(new M_PointReadRequest
            {
                address = point.Address,
                dataType = NormalizeDataType(point.DataType),
                length = ResolvePointLength(point)
            });

            if (!readResult.Success || readResult.Item == null)
            {
                return Fail<PlcPointRuntimeSnapshot>(readResult.Code, readResult.Message);
            }

            string rawValueText = readResult.Item.value ?? string.Empty;
            string displayValueText = BuildReadDisplay(point, rawValueText);

            PlcPointRuntimeSnapshot snapshot = new PlcPointRuntimeSnapshot
            {
                PlcName = point.PlcName,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = string.IsNullOrWhiteSpace(readResult.Item.quality) ? "Good" : readResult.Item.quality,
                UpdateTime = DateTime.Now,
                IsConnected = true,
                HasError = false,
                ErrorMessage = null
            };

            return OkSilent(snapshot, "PLC 点位读取成功");
        }

        private void UpdatePointRuntimeAfterWrite(PlcPointConfig point, M_PointData pointData)
        {
            if (point == null)
            {
                return;
            }

            string rawValueText = pointData == null ? string.Empty : (pointData.value ?? string.Empty);
            string displayValueText = BuildReadDisplay(point, rawValueText);

            RuntimeContext.Instance.Plc.SetPointSnapshot(new PlcPointRuntimeSnapshot
            {
                PlcName = point.PlcName,
                PointName = point.Name,
                DisplayName = point.DisplayName,
                GroupName = point.GroupName,
                AddressText = point.AddressText,
                DataType = point.DataType,
                ValueText = displayValueText,
                RawValue = rawValueText,
                Quality = pointData == null || string.IsNullOrWhiteSpace(pointData.quality) ? "Good" : pointData.quality,
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

            return ResolvePointLength(point) > 1;
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

            if (string.Equals(NormalizeDataType(point.DataType), "bool", StringComparison.OrdinalIgnoreCase))
            {
                bool boolValue;
                if (TryConvertToBooleanText(rawValueText, out boolValue))
                {
                    return boolValue ? "ON" : "OFF";
                }
            }

            return rawValueText;
        }

        private static int ResolvePointLength(PlcPointConfig point)
        {
            if (point == null || point.Length <= 0)
            {
                return 1;
            }

            return point.Length;
        }

        private static int ResolveLengthByArguments(string dataType, int stringLength, int arrayLength)
        {
            if (string.Equals(NormalizeDataType(dataType), "string", StringComparison.OrdinalIgnoreCase))
            {
                return stringLength > 0 ? stringLength : 1;
            }

            if (arrayLength > 0)
            {
                return arrayLength;
            }

            return 1;
        }

        private static string NormalizeDataType(string dataType)
        {
            return string.IsNullOrWhiteSpace(dataType)
                ? string.Empty
                : dataType.Trim().Replace(" ", string.Empty).ToLowerInvariant();
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
    }
}