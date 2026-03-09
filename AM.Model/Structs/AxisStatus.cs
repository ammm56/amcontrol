using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Structs
{
    // 定义一个状态结构体
    public struct AxisStatus
    {
        public bool IsEnabled;
        public bool IsAlarm;
        public bool IsAtHome;
        public bool PositiveLimit;
        public bool NegativeLimit;
        public bool IsDone; // 是否定位完成
    }
}
