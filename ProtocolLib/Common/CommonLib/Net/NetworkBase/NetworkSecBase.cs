using ProtocolLib.CommonLib.Common;
using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Net.NetworkBase
{
    /// <summary>
    /// 通用客户端基类
    /// 支持长连接，短链接模式
    /// </summary>
    public class NetworkSecBase:NetworkBase,IDisposable
    {
        public NetworkSecBase()
        {
            //实例化锁
            simpleLock = new SimpleLock();
            //唯一编号
            connectionID = ToolBasic.GetGUID();
        }

        private string _ip = "127.0.0.1";
        private int _connectTimeOut = 3000;

        private Lazy<Ping> ping = new Lazy<Ping>(() => new Ping());
        //交互锁
        protected SimpleLock simpleLock;
        /// <summary>
        /// 长连接的socket是否在错误状态
        /// </summary>
        protected bool isSocketError = false;
        /// <summary>
        /// 设置日志记录信息是否时二进制
        /// </summary>
        protected bool logMsgFormatBinary = true;
        /// <summary>
        /// 新的消息对象方法
        /// 继承重写
        /// </summary>
        /// <returns></returns>
        protected virtual INetMessage GetNewNetMessage() => null;

        /// <summary>
        /// 转换类型数据时的字节顺序
        /// </summary>
        public IByteTransform byteTransform { get; set; }
        /// <summary>
        /// 获得IP
        /// </summary>
        public virtual string ip
        {
            get { return _ip; }
            set => _ip = ToolBasic.GetIpAddressFromInput(value);
        }
        /// <summary>
        /// 获得端口
        /// </summary>
        public virtual int port { get; set; }
        /// <summary>
        /// 连接超时时间 毫秒
        /// </summary>
        public virtual int connectTimeout
        {
            get { return _connectTimeOut; }
            set { _connectTimeOut = Math.Max(value, 0); }
        }
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string connectionID { get; set; } = string.Empty;
        /// <summary>
        /// 收发数据前的等待时间
        /// </summary>
        public int sleepTime { get; set; } = 0;
        /// <summary>
        /// 接收数据超时 毫秒
        /// </summary>
        public int receiveTimeout { get; set; } = 5 * 1000;
        /// <summary>
        /// 是否是长连接
        /// </summary>
        public bool iskeepConn { get; set; } = false;
        /// <summary>
        /// 设置本地地址信息
        /// </summary>
        public IPEndPoint localBinding { get; set; }

        public IPStatus Ping()
        {
            return ping.Value.Send(ip).Status;
        }

        #region 连接 断开连接
        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public M_OperateResult Connection()
        {
            iskeepConn = true;
            //重新连接前，清空旧连接信息
            //BaseSocket?.Close();
            //断开连接
            if (BaseSocket?.Connected ?? false)
            {
                BaseSocket?.Shutdown(SocketShutdown.Both);
                BaseSocket?.Disconnect(false);
                Thread.Sleep(10);
            }
            BaseSocket?.Close();
            BaseSocket = null;

            M_OperateResult<Socket> rsocket = CreateSocketAndInit();

            if (!rsocket.IsSuccess)
            {
                isSocketError = true;
                rsocket.Content = null;
                return rsocket;
            }
            else
            {
                BaseSocket = rsocket.Content;
                return rsocket;
            }
        }

        /// <summary>
        /// 创建socket并连接，成功后执行InitOnConnect具体的协议初始化数据
        /// </summary>
        /// <returns></returns>
        private M_OperateResult<Socket> CreateSocketAndInit()
        {
            M_OperateResult<Socket> result = CreateSocketConnection(new IPEndPoint(IPAddress.Parse(ip), port), connectTimeout, localBinding);
            if (result.IsSuccess)
            {
                M_OperateResult init = InitOnConnect(result.Content);
                if (!init.IsSuccess)
                {
                    result.Content?.Close();
                    result.IsSuccess = init.IsSuccess;
                    result.CopyErrorFromOther(init);
                }
            }
            return result;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public M_OperateResult ConnectClose()
        {
            M_OperateResult result = new M_OperateResult();
            result.IsSuccess = false;
            iskeepConn = false;

            simpleLock.Enter();
            try
            {
                //断开连接
                if (BaseSocket?.Connected ?? false)
                {
                    //断开连接前的额外操作
                    result = ExtraOnDisconnect(BaseSocket);

                    BaseSocket?.Shutdown(SocketShutdown.Both);
                    BaseSocket?.Disconnect(false);
                    Thread.Sleep(10);
                }
                BaseSocket?.Close();
                BaseSocket = null;
                simpleLock.Leave();
            }
            catch
            {
                simpleLock.Leave();
                throw;
            }
            result.IsSuccess = true;
            return result;
        }


        #endregion
        #region 连接后 断开前的特定指令
        /// <summary>
        /// 连接后再次发送的指令
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected virtual M_OperateResult InitOnConnect(Socket socket)
        {
            return M_OperateResult.CreateSuccessResult();
        }
        /// <summary>
        /// 断开前发送的指令
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected virtual M_OperateResult ExtraOnDisconnect(Socket socket)
        {
            return M_OperateResult.CreateSuccessResult();
        }
        /// <summary>
        /// 读写完成时调用的方法，可以根据读写结果进行一些额外操作
        /// </summary>
        /// <param name="read"></param>
        protected virtual void ExtraAfterRead(M_OperateResult read) { }

        #endregion
        #region 操作
        /// <summary>
        /// 对发送消息进行前处理
        /// 具体协议按需实现
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual byte[] PackCommandWithHeader(byte[] command)
        {
            return command;
        }
        /// <summary>
        /// 对返回的消息进行前处理
        /// 具体协议中实现
        /// </summary>
        /// <param name="send">原始消息</param>
        /// <param name="response">返回的原始消息</param>
        /// <returns></returns>
        protected virtual M_OperateResult<byte[]> ExtraResponseContent(byte[] send, byte[] response)
        {
            return M_OperateResult.CreateSuccessResult(response);
        }
        /// <summary>
        /// 获取本次操控可用的socket实例
        /// </summary>
        /// <returns></returns>
        protected M_OperateResult<Socket> GetAvailableSocket()
        {
            //长连接
            if (iskeepConn)
            {
                if(isSocketError || BaseSocket == null)
                {
                    M_OperateResult connect = Connection();
                    if (!connect.IsSuccess)
                    {
                        isSocketError = true;
                        return M_OperateResult.CreateFailedResult<Socket>(connect);
                    }
                    isSocketError = false;
                    return M_OperateResult.CreateSuccessResult(BaseSocket);
                }
                return M_OperateResult.CreateSuccessResult(BaseSocket);
            }
            //短链接
            return CreateSocketAndInit();
        }

        /// <summary>
        /// 通过socket发送数据，根据INetMessage类型，返回完整数据指令
        /// </summary>
        /// <param name="send"></param>
        /// <returns></returns>
        public M_OperateResult<byte[]> ReadData(byte[] send) => ReadData(send, true);
        /// <summary>
        /// 通过socket发送数据，根据INetMessage类型，返回完整数据指令
        /// </summary>
        /// <param name="send">发送数据</param>
        /// <param name="hasResponseData">是否等待数据返回，true</param>
        /// <returns>接收的完整的数据</returns>
        public M_OperateResult<byte[]> ReadData(byte[] send,bool hasResponseData=true)
        {
            var result = new M_OperateResult<byte[]>();
            M_OperateResult<Socket> resultSocket = null;

            simpleLock.Enter();
            try
            {
                //获得可用的socket，没有就新建
                resultSocket = GetAvailableSocket();
                if (!resultSocket.IsSuccess)
                {
                    isSocketError = true;
                    simpleLock.Leave();
                    result.CopyErrorFromOther(resultSocket);
                    return result;
                }

                M_OperateResult<byte[]> read = ReadData(resultSocket.Content, send, hasResponseData);
                if (read.IsSuccess)
                {
                    isSocketError = false;
                    result.IsSuccess = read.IsSuccess;
                    result.Content = read.Content;
                    result.Message = CommonResources.Get.Language.Language.successtext;
                }
                else
                {
                    isSocketError = true;
                    result.CopyErrorFromOther(read);
                }

                ExtraAfterRead(read);
                simpleLock.Leave();
                
            }
            catch
            {
                simpleLock.Leave();
            }

            if (!iskeepConn) resultSocket?.Content?.Close();
            return result;
        }
        /// <summary>
        /// 通过socket发送数据，根据INetMessage类型，返回完整数据指令
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="send">发送数据</param>
        /// <param name="hasResponseData">是否等待数据返回，true</param>
        /// <param name="usePackHeader">是否对数据重新处理，重写PackCommandWithHeader</param>
        /// <returns>接收的完整的数据</returns>
        public virtual M_OperateResult<byte[]> ReadData(Socket socket, byte[] send,bool hasResponseData=true,bool usePackHeader = true)
        {
            //消息前处理
            byte[] sendValue = usePackHeader ? PackCommandWithHeader(send) : send;

            //接收消息格式前处理
            INetMessage netMessage = GetNewNetMessage();
            if (netMessage != null) netMessage.SendBytes = sendValue;

            //发送
            M_OperateResult sendResult = Send(socket, sendValue);
            if (!sendResult.IsSuccess) return M_OperateResult.CreateFailedResult<byte[]>(sendResult);
            //设置的接收数据超时时间小于0 返回
            if (receiveTimeout < 0) return M_OperateResult.CreateSuccessResult(new byte[0]);
            //不接收响应消息 返回
            if (!hasResponseData) return M_OperateResult.CreateSuccessResult(new byte[0]);
            if (sleepTime > 0) Thread.Sleep(sleepTime);

            //接收返回消息
            M_OperateResult<byte[]> resultReceive = ReceiveMessage(socket, receiveTimeout, netMessage);
            if (!resultReceive.IsSuccess) return resultReceive;

            //检查头消息

            //具体协议处理后的返回消息
            return ExtraResponseContent(sendValue, resultReceive.Content);
        }

        #endregion

        /// <summary>
        /// 要检测冗余调用
        /// </summary>
        private bool disposeValue = false;
        /// <summary>
        /// 释放当前资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// 释放托管资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Dispose(bool disposing)
        {
            if (!disposeValue)
            {
                if (disposing)
                {
                    ConnectClose();
                    simpleLock?.Dispose();
                }
                disposeValue = true;
            }
        }

        public override string ToString()
        {
            return $"NetworkSecBase<{GetNewNetMessage().GetType()}, {byteTransform.GetType()}>[{ip}:{port}]";
        }
    }
}
