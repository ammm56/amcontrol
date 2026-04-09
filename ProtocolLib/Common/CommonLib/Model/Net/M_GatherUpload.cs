using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Model.Net
{
    /// <summary>
    /// 采集上传信息
    /// </summary>
    public class M_GatherUpload
    {
        public string token { get; set; } = string.Empty;
        public string equipmentid { get; set; } = string.Empty;
        public string lasttimestamp { get; set; } = string.Empty;
        public string timestamp { get; set; } = string.Empty;
        public List<M_GatherData> data { get; set; } = new List<M_GatherData>();
    }
    /// <summary>
    /// 采集上传的点位数据
    /// </summary>
    public class M_GatherData
    {
        public string functioncode { get; set; } = string.Empty;
        public string point { get; set; } = string.Empty;
        public string lastvalue { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
    }
}
