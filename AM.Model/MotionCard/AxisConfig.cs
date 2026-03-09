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

        public short LogicalId { get; set; }            // 业务层调用的 ID (如: 0, 1, 2)，统一不同卡的轴号
        public short PhysicalCore { get; set; }         // 固高专用：内核号 (如 1)
        public short PhysicalAxis { get; set; }         // 固高专用：物理轴号 (如 1)
    }
}
