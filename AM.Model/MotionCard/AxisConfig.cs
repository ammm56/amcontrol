using AM.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard
{
    /// <summary>
    /// 加速度 (Pulse/ms^2) 
    /// Motion Feeling(运动体感)   Acc Suggestion(Pulse/ms²)  Equivalent mm/s² () Application Scenario(适用场景)
    /// Very Soft(非常柔和)            0.1 ~ 0.5	            50 ~ 250	        Heavy load, high inertia(重负载、大惯性)
    /// Standard Industrial(标准工业)  1.0 ~ 3.0	            500 ~ 1500	        General assembly, dispensing (普通组装、点胶机)
    /// High-Speed Rhythm(高速节拍)    4.0 ~ 6.0	            2000 ~ 3000	        Vision sorting, mounter (视觉分选、贴片机)
    /// Performance Limit(性能极限)    10.0+	                5000+	            Linear motor, light belt(直线电机、轻载皮带)
    /// 
    /// 加速度 (mm/s^2)，带范围校验 [0.1, 1000]
    /// 机构类型            推荐加速度     极值(慎用) 备注
    /// 普通滚珠丝杠        500 ~ 1500	    3000	  取决于负载重量和电机扭矩。
    /// 同步带/皮带传动     1000 ~ 2500	    5000	  皮带较轻，可以快一点，但高加速容易抖动。
    /// 直线电机	        5000 ~ 20000	50000	  直线电机专门玩高响应，
    /// 重载导轨	        100 ~ 500	    1000	  负载大，惯性大，必须慢起慢停。
    /// </summary>

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
    public partial class AxisConfig
    {
        /// <summary>
        /// 轴ID
        /// </summary>
        public short AxisId { get; set; }

        /// <summary>
        /// 轴名称 (如 "X轴")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业务层调用的 ID (如: 0, 1, 2)，统一不同卡的轴号
        /// </summary>
        public short LogicalId { get; set; }

        /// <summary>
        /// 固高专用：内核号 (如 1)
        /// </summary>
        public short PhysicalCore { get; set; }

        /// <summary>
        /// 固高专用：物理轴号 (如 1)
        /// </summary>
        public short PhysicalAxis { get; set; }

        // --- 硬件配置参数 ---
        /// <summary>
        /// 是否开启报警检查
        /// </summary>
        public bool AlarmEnable { get; set; }

        /// <summary>
        /// 报警信号是否取反 (Sense)
        /// </summary>
        public bool AlarmInvert { get; set; }

        /// <summary>
        /// 脉冲模式 0: 脉冲+方向(Step/Dir), 1: 双脉冲(CW/CCW) 
        /// </summary>
        public short PulseMode { get; set; }

        /// <summary>
        /// 编码器 (Encoder) 0: 脉冲计数, 1: 外部编码器
        /// </summary>
        public bool EncoderExternal { get; set; }

        /// <summary>
        /// 编码器方向是否取反
        /// </summary>
        public bool EncoderInvert { get; set; }

        /// <summary>
        /// 信号极性 限位和原点信号是否统一取反
        /// </summary>
        public bool LimitHomeInvert { get; set; }

        /// <summary>
        /// 限位有效性 -1: 双向有效, 0: 仅正限位, 1: 仅负限位, 10: 关闭限位 
        /// </summary>
        public short LimitMode { get; set; }

        // --- 单位换算 (关联通知) ---
        /// <summary>
        /// 导程 5mm
        /// </summary>
        public double Lead { get; set; }

        /// <summary>
        /// 脉冲数/圈 10000
        /// </summary>
        public int PulsePerRev { get; set; }

        /// <summary>
        /// 减速比 1:1
        /// </summary>
        public double GearRatio { get; set; }

        /// <summary>
        /// 公式：(电机单圈脉冲 * 减速比) / 导程
        /// 脉冲/mm 系数 计算属性：1mm 对应多少脉冲 (Pulse/mm)
        /// </summary>
        [JsonIgnore]
        public double K => (PulsePerRev * GearRatio) / Lead;

        // --- 梯形波参数 (带范围校验) ---
        private double _acc = 0.1;
        /// <summary>
        /// 脉冲加速度 (Pulse/ms^2)，范围限制在 [0.001, 1.0]
        /// 默认给个合理的起步值
        /// </summary>
        public double Acc
        {
            get => _acc;
            set => value.LimitTo(0.001, 1.0);
        }

        private double _dec = 0.1;
        /// <summary>
        /// 脉冲减速度 (Pulse/ms^2)，范围限制在 [0.001, 1.0]
        /// </summary>
        public double Dec
        {
            get => _dec;
            set => value.LimitTo(0.001, 1.0);
        }

        private short _smoothTime = 25;
        /// <summary>
        /// 平滑时间 (ms)，固高通常限制在 [0, 256]
        /// </summary>
        public short SmoothTime
        {
            get => _smoothTime;
            set => value.LimitTo((short)0, (short)256);
        }

        /// <summary>
        /// 使能状态 (运行时)，不存入JSON
        /// </summary>
        [JsonIgnore]
        public bool IsServoOn { get; set; }
    }
}
