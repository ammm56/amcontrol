using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 自定义数据类型的读写操作
    /// </summary>
    public interface IDataTransfer
    {
        /// <summary>
        /// 读取的数据长度
        /// </summary>
        ushort ReadCount { get; }

        /// <summary>
        /// 从字节数组解析实际的对象
        /// </summary>
        /// <param name="Content"></param>
        void ParseSource(byte[] Content);

        /// <summary>
        /// 将对象转成字节数组
        /// </summary>
        /// <returns></returns>
        byte[] ToSource();

    }
}
