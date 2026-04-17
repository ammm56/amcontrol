using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.Entity.System;
using AM.Model.License;
using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 设备硬件信息采集服务。
    /// 统一采集授权申请和本地硬件绑定校验需要的设备标识。
    /// </summary>
    public class HardwareInfoCollector : ServiceBase
    {
        private readonly ClientIdentityService _clientIdentityService;

        protected override string MessageSourceName
        {
            get { return "HardwareInfoCollector"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public HardwareInfoCollector()
            : this(new ClientIdentityService(), SystemContext.Instance.Reporter)
        {
        }

        public HardwareInfoCollector(IAppReporter reporter)
            : this(new ClientIdentityService(reporter), reporter)
        {
        }

        public HardwareInfoCollector(ClientIdentityService clientIdentityService, IAppReporter reporter)
            : base(reporter)
        {
            _clientIdentityService = clientIdentityService;
        }

        /// <summary>
        /// 采集当前设备硬件信息并执行统一归一化。
        /// </summary>
        public Result<DeviceHardwareInfo> CollectCurrent()
        {
            try
            {
                Result<SysClientIdentityEntity> identityResult = _clientIdentityService.GetCurrent();
                if (!identityResult.Success || identityResult.Item == null)
                {
                    return Fail<DeviceHardwareInfo>(identityResult.Code == 0 ? -1 : identityResult.Code, "读取客户端身份失败");
                }

                SysClientIdentityEntity identity = identityResult.Item;
                var hardware = new DeviceHardwareInfo
                {
                    ClientId = NormalizeBindingValue(identity.ClientId),
                    MachineCode = NormalizeBindingValue(identity.MachineCode),
                    MachineName = NormalizePlainText(identity.MachineName),
                    MachineModel = NormalizePlainText(ReadWmiFirstValue("Win32_ComputerSystem", "Model")),
                    OsVersion = NormalizePlainText(Environment.OSVersion.VersionString),
                    CpuId = NormalizeBindingValue(ReadWmiFirstValue("Win32_Processor", "ProcessorId")),
                    BiosSerialNumber = NormalizePlainText(ReadWmiFirstValue("Win32_BIOS", "SerialNumber")),
                    MainboardSerialNumber = NormalizePlainText(ReadWmiFirstValue("Win32_BaseBoard", "SerialNumber")),
                    DiskSerialNumber = NormalizePlainText(ReadWmiFirstValue("Win32_PhysicalMedia", "SerialNumber")),
                    MacAddress = NormalizeBindingValue(ReadMacAddress())
                };

                return OkSilent(hardware, "采集硬件信息成功");
            }
            catch (Exception ex)
            {
                return Fail<DeviceHardwareInfo>(-1, "采集硬件信息异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 归一化硬件绑定比较字段。
        /// 统一去空白、去常见分隔符、转大写。
        /// </summary>
        public static string NormalizeBindingValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            char[] chars = value.Trim()
                .Where(char.IsLetterOrDigit)
                .ToArray();

            return new string(chars).ToUpperInvariant();
        }

        /// <summary>
        /// 归一化普通诊断文本字段。
        /// </summary>
        public static string NormalizePlainText(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
        }

        /// <summary>
        /// 读取指定 WMI 类中的首个属性值。
        /// 采集失败时返回空字符串，不抛出异常影响整体流程。
        /// </summary>
        private static string ReadWmiFirstValue(string className, string propertyName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM " + className))
                using (ManagementObjectCollection collection = searcher.Get())
                {
                    foreach (ManagementObject item in collection)
                    {
                        object value = item[propertyName];
                        if (value != null)
                        {
                            string text = value.ToString();
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                return text;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取首个可用物理网卡 MAC 地址。
        /// </summary>
        private static string ReadMacAddress()
        {
            try
            {
                NetworkInterface nic = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(x => x != null)
                    .Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Where(x => x.OperationalStatus == OperationalStatus.Up)
                    .FirstOrDefault(x => x.GetPhysicalAddress() != null && x.GetPhysicalAddress().GetAddressBytes().Length > 0);

                if (nic == null)
                {
                    return string.Empty;
                }

                return nic.GetPhysicalAddress().ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}