using ProtocolLib.CommonLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Net
{
    /// <summary>
    /// 网络消息基础方法
    /// </summary>
    public static class NetHelper
    {
        /// <summary>
        /// Socket传输缓冲区大小
        /// </summary>
        internal const int SocketBufferSize = 16 * 1024;

        /// <summary>
        /// socket读取数据
        /// 指定接收长度
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="receive">要接收的长度</param>
        /// <param name="reportProgress"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static byte[] ReadBytesFromSocket(Socket socket,int receive,Action<long,long> reportProgress = null)
        {
            byte[] bytesreceive = new byte[receive];
            int countreceive = 0;
            while (countreceive < receive)
            {
                int receivelength = Math.Min(receive - countreceive, SocketBufferSize);
                int count = socket.Receive(bytesreceive, countreceive, receivelength, SocketFlags.None);
                countreceive += count;

                if (count == 0) throw new Exception(CommonResources.Get.Language.Language.remoteconnectclose);

                reportProgress?.Invoke(countreceive, receive);
            }
            return bytesreceive;
        }

    }
}
