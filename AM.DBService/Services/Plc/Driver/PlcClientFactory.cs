using AM.Model.Common;
using AM.Model.Interfaces.Plc;
using AM.Model.Plc;
using ProtocolLib.CommonLib.Model;
using System;

namespace AM.DBService.Services.Plc.Driver
{
    /// <summary>
    /// PLC 客户端工厂。
    /// 当前阶段只创建 AM 侧统一门面客户端，由门面客户端内部反射创建协议库实现。
    /// </summary>
    public class PlcClientFactory : IPlcClientFactory
    {
        public Result<IPlcClient> Create(PlcStationConfig stationConfig)
        {
            try
            {
                if (stationConfig == null)
                {
                    return Result<IPlcClient>.Fail(-3501, "PLC 站配置不能为空", ResultSource.Plc);
                }

                if (string.IsNullOrWhiteSpace(stationConfig.Name))
                {
                    return Result<IPlcClient>.Fail(-3502, "PLC 站名称不能为空", ResultSource.Plc);
                }

                if (string.IsNullOrWhiteSpace(stationConfig.ProtocolType))
                {
                    return Result<IPlcClient>.Fail(-3503, "PLC 协议类型不能为空", ResultSource.Plc);
                }

                if (string.IsNullOrWhiteSpace(stationConfig.ConnectionType))
                {
                    return Result<IPlcClient>.Fail(-3504, "PLC 连接方式不能为空", ResultSource.Plc);
                }

                IPlcClient client = new ProtocolPlcClient(stationConfig);

                Result configureResult = client.Configure(BuildProtocolOptions(stationConfig));
                if (!configureResult.Success)
                {
                    return Result<IPlcClient>.Fail(configureResult.Code, configureResult.Message, ResultSource.Plc);
                }

                return Result<IPlcClient>.OkItem(client, "PLC 客户端创建成功", ResultSource.Plc)
                    .WithNotifyMode(ResultNotifyMode.Silent);
            }
            catch (Exception ex)
            {
                return Result<IPlcClient>.Fail(-3505, "PLC 客户端创建异常: " + ex.Message, ResultSource.Plc);
            }
        }

        private static M_ProtocolOptions BuildProtocolOptions(PlcStationConfig stationConfig)
        {
            return new M_ProtocolOptions
            {
                protocolType = stationConfig.ProtocolType ?? string.Empty,
                connectionType = stationConfig.ConnectionType ?? string.Empty,
                ip = stationConfig.IpAddress ?? string.Empty,
                port = stationConfig.Port ?? GetDefaultPort(stationConfig.ProtocolType),
                stationNo = stationConfig.StationNo,
                rack = stationConfig.Rack,
                slot = stationConfig.Slot,
                timeoutMs = stationConfig.TimeoutMs,
                byteOrder = string.Empty,
                wordOrder = string.Empty,
                stringEncoding = "ASCII"
            };
        }

        private static int GetDefaultPort(string protocolType)
        {
            string protocol = (protocolType ?? string.Empty).Trim();

            if (protocol.IndexOf("modbus", StringComparison.OrdinalIgnoreCase) >= 0 &&
                protocol.IndexOf("tcp", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 502;
            }

            if (protocol.IndexOf("s7", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return 102;
            }

            return 0;
        }
    }
}