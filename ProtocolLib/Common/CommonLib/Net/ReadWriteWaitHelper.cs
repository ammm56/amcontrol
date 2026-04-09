using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Net
{
    public class ReadWriteWaitHelper
    {
        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet,string address,bool waitValue,int readInterval,int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<bool> read = readWriteNet.ReadBool(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if(waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }
        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, short waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<short> read = readWriteNet.ReadInt16(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }

        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, ushort waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<ushort> read = readWriteNet.ReadUInt16(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }

        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, int waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<int> read = readWriteNet.ReadInt32(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }

        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, uint waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<uint> read = readWriteNet.ReadUInt32(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }

        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, long waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<long> read = readWriteNet.ReadInt64(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }

        public static M_OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, ulong waitValue, int readInterval, int waitTimeout)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                M_OperateResult<ulong> read = readWriteNet.ReadUInt64(address);
                if (!read.IsSuccess) return M_OperateResult.CreateFailedResult<TimeSpan>(read);
                if (read.Content == waitValue) return M_OperateResult.CreateSuccessResult<TimeSpan>(DateTime.Now - start);

                if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > waitTimeout)
                {
                    return new M_OperateResult<TimeSpan>($"{CommonResources.Get.Language.Language.waitdatatimeout}{waitTimeout}");
                }
                Thread.Sleep(readInterval);
            }
        }
    }
}
