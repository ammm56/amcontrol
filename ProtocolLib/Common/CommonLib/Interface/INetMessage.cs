using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 连接返回消息
    /// 包含解析规则，数据提取规则
    /// </summary>
    public interface INetMessage
    {
        /// <summary>
        /// 消息头指令长度
        /// 第一次接收数据的长度
        /// </summary>
        int ProtocolHeadBytesLength { get; }

        /// <summary>
        /// 从当前头字节中提取出接下来要接收的数据长度
        /// </summary>
        /// <returns></returns>
        int GetContentLengthByHeadBytes();

        /// <summary>
        /// 检查头字节的合法性
        /// </summary>
        /// <param name="token">特殊消息的验证令牌</param>
        /// <returns></returns>
        bool CheckHeadBytesLegal(byte[] token);

        /// <summary>
        /// 获取头字节里的消息标识
        /// </summary>
        /// <returns></returns>
        int GetHeadBytesIdentity();

        /// <summary>
        /// 消息头字节
        /// </summary>
        byte[] HeadBytes { get; set; }

        /// <summary>
        /// 消息类容字节
        /// </summary>
        byte[] ContentBytes { get; set; }

        /// <summary>
        /// 发送的字节信息
        /// </summary>
        byte[] SendBytes { get; set; }

    }
}
