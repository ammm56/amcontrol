using ProtocolLib.CommonLib.Model.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.S7Tcp.Core
{
    /// <summary>
    /// 协议采集配置
    /// </summary>
    public class M_ProtocolConfig
    {
        public string equipmentid { get; set; } = "";
        public string protocoltype { get; set; } = "";
        public string ip { get; set; }
        public int port { get; set; }
        /// <summary>
        /// modbus 字节顺序 大端0 小端 1
        /// </summary>
        public int byteorder { get; set; } = 0;
        public List<Point> pointinfo { get; set; } = new List<Point>();
    }
}
