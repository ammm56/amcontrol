using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// LRC校验
    /// </summary>
    public class SoftLRC
    {
        /// <summary>
        /// 获取对应数据的LRC校验码
        /// </summary>
        /// <param name="value">需要校验的数据，不包含LRC字节</param>
        /// <returns>返回带LRC校验码的字节数组</returns>
        public static byte[] LRC(byte[] value)
        {
            if (value == null) return null;
            int sum = 0;
            for (int i = 0; i < value.Length; i++)
            {
                sum += value[i];
            }
            sum = sum % 256;
            sum = 256 - sum;
            byte[] LRC = new byte[] { (byte)sum };
            return ToolBasic.SpliceArray(value, LRC);
        }

        /// <summary>
        /// 检查数据是否符合LRC验证
        /// </summary>
        /// <param name="value">等待校验数据</param>
        /// <returns></returns>
        public static bool CheckLRC(byte[] value)
        {
            if (value == null) return false;
            int length = value.Length;
            byte[] buffer = new byte[length - 1];
            Array.Copy(value, 0, buffer, 0, buffer.Length);

            byte[] LRCbuffer = LRC(buffer);
            if (LRCbuffer[length - 1] == value[length - 1])
            {
                return true;
            }
            return false;
        }

    }
}
