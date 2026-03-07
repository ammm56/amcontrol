using AM.Model.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AM.Tools
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class OSInfo
    {
        /// <summary>
        /// 获得系统信息
        /// </summary>
        public static void GetOSInfo()
        {
            try
            {
                var proc = Process.GetCurrentProcess();
                MOSInfo m_OSInfo = new MOSInfo
                {
                    Mem = proc.WorkingSet64,
                    CPU = proc.TotalProcessorTime,
                    FrameworkDesc = RuntimeInformation.FrameworkDescription,
                    OSDesc = RuntimeInformation.OSDescription,
                    OSVersion = Environment.OSVersion.ToString(),
                    OSArch = RuntimeInformation.OSArchitecture.ToString(),
                    MachineName = Environment.MachineName,
                    UserDomainName = Environment.UserDomainName,
                    Drives = Environment.GetLogicalDrives().ToList(),
                    SystemDirect = Environment.SystemDirectory,
                    TickCount = Environment.TickCount,
                    UserInter = Environment.UserInteractive,
                    UserName = Environment.UserName,
                    Version = Environment.Version.ToString()
                };

                ConfigSingle.Instance.Config.RuntimeConfig.OSInfo = m_OSInfo;
            }
            catch (Exception ex)
            {
                Tools.PrintEX($"OSInfo GetOSInfo EX {ex.Message}");
            }

        }
    }
}
