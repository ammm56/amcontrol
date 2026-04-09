using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProtocolLib.CommonLib.Net.NetworkBase
{
    /// <summary>
    /// 网络基类，抽象类，无法实例化
    /// </summary>
    public abstract class NetworkBase
    {
        #region 构造函数
        public NetworkBase()
        {

        }

        #endregion
        #region 属性
        /// <summary>
        /// 日志记录
        /// </summary>
        public ILog Log { get; set; }

        #endregion
        #region 成员
        /// <summary>
        /// 基本套接字
        /// </summary>
        protected Socket BaseSocket = null;

        /// <summary>
        /// 文件传输时缓存大小 100KB
        /// </summary>
        protected int FileCacheSize = 1024 * 100;

        /// <summary>
        /// 连接错误次数
        /// </summary>
        private int connerrorcount = 0;

        #endregion
        #region 连接
        /// <summary>
        /// 创建socket连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        protected M_OperateResult<Socket> CreateSocketConnection(string ip,int port)
        {
            return CreateSocketConnection(new IPEndPoint(IPAddress.Parse(ip), port), 5 * 1000);
        }
        /// <summary>
        /// 创建socket连接
        /// </summary>
        /// <param name="endPoint">远程地址</param>
        /// <param name="timeout">超时时间毫秒</param>
        /// <param name="local">本地地址</param>
        /// <returns></returns>
        protected M_OperateResult<Socket> CreateSocketConnection(IPEndPoint endPoint,int timeout,IPEndPoint local = null)
        {
            int connectcount = 0;
            while (true)
            {
                connectcount++;
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TimeOutCheck connectTimeout = TimeOutCheck.HandleTimeOutCheck(socket, timeout);
                try
                {
                    if (local != null) socket.Bind(local);
                    socket.Connect(endPoint);
                    connectcount = 0;
                    connectTimeout.IsSuccessful = true;
                    return M_OperateResult.CreateSuccessResult(socket);
                }
                catch (Exception ex)
                {
                    socket?.Close();
                    connectTimeout.IsSuccessful = true;
                    if(connectTimeout.GetConsumeTime() < TimeSpan.FromMilliseconds(500) && connectcount < 2)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    if (connectTimeout.IsTimeout)
                    {
                        return new M_OperateResult<Socket>(1120, $"{CommonResources.Get.Language.Language.connecttime} {endPoint.Address}:{endPoint.Port} {timeout} ms");
                    }
                    else
                    {
                        return new M_OperateResult<Socket>(1121, $"{CommonResources.Get.Language.Language.connectex} {endPoint.Address}:{endPoint.Port} {ex.Message}");
                    }
                }

            }
        }

        #endregion
        #region 发送信息

        /// <summary>
        /// 用socket发送信息
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="data">字节数据</param>
        /// <returns></returns>
        protected M_OperateResult Send(Socket socket, byte[] data)
        {
            if (data == null) return M_OperateResult.CreateSuccessResult();
            return Send(socket, data, 0, data.Length);
        }
        /// <summary>
        /// 用socket发送信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name=""></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        protected M_OperateResult Send(Socket socket, byte[] data,int offset,int size)
        {
            if (data == null) return M_OperateResult.CreateSuccessResult();
            try
            {
                int sendCount = 0;
                while (true)
                {
                    int count = socket.Send(data, offset, size - sendCount, SocketFlags.None);
                    sendCount += count;
                    offset += count;
                    if (sendCount >= size) break;
                }
                return M_OperateResult.CreateSuccessResult();

            }
            catch (Exception ex)
            {
                socket?.Close();
                return new M_OperateResult<byte[]>(1100, ex.Message);
            }
        }


        #endregion
        #region 接收信息
        /// <summary>
        /// 接收固定长度的字节数组
        /// length大于0时，接收固定长度消息
        /// length小于0时，接收不大于2048字节的随机数据消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="length"></param>
        /// <param name="timeout"></param>
        /// <param name="reportProgress"></param>
        /// <returns></returns>
        protected M_OperateResult<byte[]> Receive(Socket socket,int length,int timeout = 60*1000,Action<long,long> reportProgress = null)
        {
            if(length ==0)return M_OperateResult.CreateSuccessResult(new byte[0]);
            try
            {
                socket.ReceiveTimeout = timeout;
                if(length > 0)
                {
                    byte[] data = NetHelper.ReadBytesFromSocket(socket, length, reportProgress);
                    return M_OperateResult.CreateSuccessResult(data);
                }
                else
                {
                    byte[] buffer = new byte[2048];
                    int count = socket.Receive(buffer);
                    if (count == 0) throw new Exception(CommonResources.Get.Language.Language.remoteconnectclose);
                    return M_OperateResult.CreateSuccessResult(ToolBasic.ArraySelectBegin(buffer, count));
                }
            }
            catch (Exception ex)
            {
                socket?.Close();
                return new M_OperateResult<byte[]>(1122, $"{CommonResources.Get.Language.Language.remoteconnectclose} {ex.Message}");
            }
        }
        /// <summary>
        /// 接收信息用INetMessage处理
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="timeout">接收超时，毫秒</param>
        /// <param name="netMessage">消息的格式定义</param>
        /// <param name="reportProgress">接收消息时的进度报告</param>
        /// <returns></returns>
        protected M_OperateResult<byte[]> ReceiveMessage(Socket socket,int timeout,INetMessage netMessage,Action<long,long> reportProgress = null)
        {
            if (netMessage == null) return Receive(socket, -1, timeout);

            M_OperateResult<byte[]> headResult = Receive(socket, netMessage.ProtocolHeadBytesLength, timeout);
            if(!headResult.IsSuccess)return headResult;

            netMessage.HeadBytes = headResult.Content;
            int contentlength = netMessage.GetContentLengthByHeadBytes();

            M_OperateResult<byte[]> contentresult = Receive(socket, contentlength, timeout, reportProgress);
            if(!contentresult.IsSuccess)return contentresult;

            netMessage.ContentBytes = contentresult.Content;
            return M_OperateResult.CreateSuccessResult(ToolBasic.ArraySplice(headResult.Content, contentresult.Content));

        }

        //接收一条命令数据，用结束符判断

        #endregion
        #region 读写steam
        /// <summary>
        /// 读取stream中数据到缓冲区
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        //protected M_OperateResult<int> ReadStream(Stream stream, byte[] buffer)
        //{
        //    ManualResetEvent waitDone = new ManualResetEvent(false);


        //}

        #endregion

    }
}
