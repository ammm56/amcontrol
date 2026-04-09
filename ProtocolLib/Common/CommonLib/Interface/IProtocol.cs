using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 库
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        int Init();

        /// <summary>
        /// 单次采集
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        M_Return<M_GatherData> Get(string address, string type);

        /// <summary>
        /// 单次写入
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        M_Return<M_GatherData> Set(string address, string type, object value);

        /// <summary>
        /// 更新连接配置
        /// 更新点表配置
        /// </summary>
        /// <param name="netconfig"></param>
        /// <returns></returns>
        int SetCFG(M_NetConfig netconfig);

        /// <summary>
        /// 重连
        /// </summary>
        /// <returns></returns>
        M_Return<string> Reconnect();

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        M_Return<string> CloseConnected();
    }
}
