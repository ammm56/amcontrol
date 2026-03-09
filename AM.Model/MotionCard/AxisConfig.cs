using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard
{
    public class AxisConfig : ObservableObject
    {
        public short AxisId { get; set; }
        public string Name { get; set; }
        public double Lead { get; set; } = 5.0;          // 导程 5mm
        public int PulsePerRev { get; set; } = 10000;    // 脉冲数 10000

        // 计算属性：1mm 对应多少脉冲 (Pulse/mm)
        [JsonIgnore]
        public double K => PulsePerRev / Lead;
    }
}
