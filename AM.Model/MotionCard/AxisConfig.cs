using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard
{
    public static class NumericExtensions
    {
        /// <summary>
        /// 取值限制在范围内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T LimitTo<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
    }
    public class AxisConfig : ObservableObject
    {
        // --- 核心标识 ---
        public short AxisId { get; set; }
        public string Name { get; set; }                // 轴名称 (如 "X轴")
        public short LogicalId { get; set; }            // 业务层调用的 ID (如: 0, 1, 2)，统一不同卡的轴号
        public short PhysicalCore { get; set; }         // 固高专用：内核号 (如 1)
        public short PhysicalAxis { get; set; }         // 固高专用：物理轴号 (如 1)

        // --- 使能状态 ---
        private bool _isServoOn;
        [JsonIgnore] // 运行时状态，不存入JSON
        public bool IsServoOn
        {
            get => _isServoOn;
            set => SetProperty(ref _isServoOn, value);
        }

        // --- 报警 (Alarm) ---
        public bool AlarmEnable { get; set; } = true;    // 是否开启报警检查
        public bool AlarmInvert { get; set; } = false;   // 报警信号是否取反 (Sense)

        // --- 脉冲模式 ---
        /// <summary> 0: 脉冲+方向(Step/Dir), 1: 双脉冲(CW/CCW) </summary>
        public short PulseMode { get; set; } = 0;

        // --- 编码器 (Encoder) ---
        public bool EncoderExternal { get; set; } = false; // 0: 脉冲计数, 1: 外部编码器
        public bool EncoderInvert { get; set; } = false;   // 编码器方向是否取反

        // --- 信号极性 ---
        public bool LimitHomeInvert { get; set; } = false; // 限位和原点信号是否统一取反

        // --- 限位有效性 ---
        /// <summary> -1: 双向有效, 0: 仅正限位, 1: 仅负限位, 10: 关闭限位 </summary>
        public short LimitMode { get; set; } = -1;

        // --- 单位换算 ---
        public double Lead { get; set; } = 5.0;            // 导程 5mm
        public int PulsePerRev { get; set; } = 10000;      // 脉冲数/圈 10000
        [JsonIgnore]
        public double K => PulsePerRev / Lead;             // 脉冲/mm 系数 计算属性：1mm 对应多少脉冲 (Pulse/mm)

        // --- 梯形波参数 ---
        private double _acc = 100.0;                        // 默认给个合理的起步值
        /// <summary>
        /// 加速度 (mm/s^2)，带范围校验 [0.1, 1000]
        /// 机构类型            推荐加速度     极值(慎用) 备注
        /// 普通滚珠丝杠        500 ~ 1500	    3000	  取决于负载重量和电机扭矩。
        /// 同步带/皮带传动     1000 ~ 2500	    5000	  皮带较轻，可以快一点，但高加速容易抖动。
        /// 直线电机	        5000 ~ 20000	50000	  直线电机专门玩高响应，
        /// 重载导轨	        100 ~ 500	    1000	  负载大，惯性大，必须慢起慢停。
        /// </summary>
        public double Acc
        {
            get => _acc;
            set => SetProperty(ref _acc, value.LimitTo(0.1, 1000.0));
        }
        private double _dec = 100.0;
        /// <summary>
        /// 减速度 (mm/s^2)
        /// 最小值 10 (保证能动)，最大值 1000 (安全上限)
        /// double validated = value < 10.0 ? 10.0 : (value > 1000.0 ? 1000.0 : value);
        /// </summary>
        public double Dec
        {
            get => _dec;
            set => SetProperty(ref _dec, value.LimitTo(0.1, 1000.0));
        }
        private short _smoothTime = 25;
        /// <summary>
        /// 平滑时间 (ms)，固高通常限制在 [0, 256]
        /// </summary>
        public short SmoothTime
        {
            get => _smoothTime;
            set => SetProperty(ref _smoothTime, value.LimitTo((short)0, (short)256));
        }
    }
}
