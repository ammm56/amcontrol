using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Structs
{
    public struct AxisParam
    {
        public double Lead;          // 导程 (电机转一圈走多少mm)
        public int PulsePerRev;      // 电机旋转一圈需要的脉冲数
        public double GearRatio;     // 减速比 (默认1:1)

        // 计算系数：1mm 等于多少脉冲
        public double PulsePerMm => (PulsePerRev * GearRatio) / Lead;
    }
}
