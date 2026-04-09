using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Net
{
    /// <summary>
    /// 从iot服务获取的配置
    /// </summary>
    public class M_NetConfig
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
    /// <summary>
    /// 规则
    /// 采集地址列表-》条件-》采集地址列表
    /// </summary>
    public class Condition
    {
        public string leftsign { get; set; } = "";
        public dynamic leftvalue { get; set; }
        public string operation { get; set; } = "";
        public string rightsign { get; set; } = "";
        public dynamic rightvalue { get; set; }
    }
    /// <summary>
    /// 采集地址
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string arg { get; set; } = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string point { get; set; } = "";
        /// <summary>
        /// 转换的数据类型
        /// </summary>
        public string type { get; set; } = "";

        /// <summary>
        /// 写入时的值
        /// </summary>
        public string value { get; set; } = "";
    }
}
