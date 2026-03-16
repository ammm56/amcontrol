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
    /// 数值扩展
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
        /// 将值限制在指定范围内
        /// </summary>
        public static T LimitTo<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
    }

    /// <summary>
    /// 轴配置
    /// 用于承载单轴的硬件参数、运动参数、回零参数与运行时状态
    /// </summary>
    public partial class AxisConfig
    {
        #region 基础标识

        /// <summary>
        /// 轴ID。
        /// 通常为数据库或配置中的唯一轴编号。
        /// </summary>
        public short AxisId { get; set; }

        /// <summary>
        /// 轴名称。
        /// 如 X轴、Z轴、上料轴。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 逻辑轴号。
        /// 业务层统一访问编号。
        /// </summary>
        public short LogicalAxis { get; set; }

        /// <summary>
        /// 物理内核号。
        /// 固高卡等多核控制卡使用。
        /// </summary>
        public short PhysicalCore { get; set; }

        /// <summary>
        /// 物理轴号。
        /// 控制卡上的实际轴编号。
        /// </summary>
        public short PhysicalAxis { get; set; }

        #endregion

        #region 硬件信号与极性

        /// <summary>
        /// 是否启用驱动器报警输入检测。
        /// true=启用，false=不启用。0=关闭该功能，1=开启该功能
        /// </summary>
        public bool AlarmEnabled { get; set; } = true;

        /// <summary>
        /// 报警信号是否取反。
        /// </summary>
        public bool AlarmInvert { get; set; }

        /// <summary>
        /// 使能信号是否取反。
        /// </summary>
        public bool EnableInvert { get; set; }

        /// <summary>
        /// 脉冲输出方式。
        /// 0=脉冲+方向(Step/Dir)，1=双脉冲(CW/CCW)。
        /// </summary>
        public short PulseMode { get; set; }

        /// <summary>
        /// 默认运动模式。
        /// 1=点位运动。
        /// </summary>
        public short DefaultMoveMode { get; set; } = 1;

        /// <summary>
        /// 是否使用外部编码器。
        /// 编码器 (Encoder) 0: 脉冲计数, 1: 外部编码器
        /// </summary>
        public bool EncoderExternal { get; set; }

        /// <summary>
        /// 编码器方向是否取反。
        /// </summary>
        public bool EncoderInvert { get; set; }

        /// <summary>
        /// 限位与原点信号是否统一取反。
        /// </summary>
        public bool LimitHomeInvert { get; set; }

        /// <summary>
        /// 限位有效模式。
        /// 0=仅正限位有效，1=仅负限位有效，-1=双向有效，10=关闭。
        /// </summary>
        public short LimitMode { get; set; } = -1;

        /// <summary>
        /// 捕获沿。
        /// 0=下降沿，1=上升沿。
        /// </summary>
        public short TriggerEdge { get; set; }

        #endregion

        #region 单位换算

        /// <summary>
        /// 导程，单位 mm。
        /// </summary>
        public double Lead { get; set; } = 5.0;

        /// <summary>
        /// 每圈脉冲数。
        /// </summary>
        public int PulsePerRev { get; set; } = 10000;

        /// <summary>
        /// 减速比。
        /// 1.0 表示 1:1。
        /// </summary>
        public double GearRatio { get; set; } = 1.0;

        /// <summary>
        /// 脉冲/mm 系数 计算属性：1mm 对应多少脉冲 (Pulse/mm)
        /// 公式：(电机单圈脉冲 * 减速比) / 导程
        /// </summary>
        [JsonIgnore]
        public double K => (PulsePerRev * GearRatio) / Lead;

        #endregion

        #region 默认运动参数

        private double _defaultVelocity = 10.0;
        /// <summary>
        /// 默认点位速度，单位 pulse/ms。
        /// PulsePerRev = 10000 Lead = 5 GearRatio = 1 则 K = 2000 pulse/mm
        /// 那么 10 pulse/ms = 10000 pulse/s = 5 mm/s
        /// </summary>
        public double DefaultVelocity
        {
            get { return _defaultVelocity; }
            set { _defaultVelocity = value.LimitTo(0.001, 200.0); }
        }

        private double _jogVelocity = 10.0;
        /// <summary>
        /// 默认 Jog 速度，单位 pulse/ms。
        /// </summary>
        public double JogVelocity
        {
            get { return _jogVelocity; }
            set { _jogVelocity = value.LimitTo(0.001, 100.0); }
        }

        #endregion

        #region 运动曲线参数

        private double _acc = 0.1;
        /// <summary>
        /// 脉冲加速度，单位 Pulse/ms^2。
        /// 范围限制在 [0.001, 1.0] 默认给个合理的起步值
        /// </summary>
        public double Acc
        {
            get { return _acc; }
            set { _acc = value.LimitTo(0.001, 1.0); }
        }

        private double _dec = 0.1;
        /// <summary>
        /// 脉冲减速度，单位 Pulse/ms^2。
        /// 范围限制在 [0.001, 1.0]
        /// </summary>
        public double Dec
        {
            get { return _dec; }
            set { _dec = value.LimitTo(0.001, 1.0); }
        }

        private short _smoothTime = 25;
        /// <summary>
        /// 平滑时间 (ms)，固高通常限制在 [0, 256]
        /// </summary>
        public short SmoothTime
        {
            get { return _smoothTime; }
            set { _smoothTime = value.LimitTo((short)0, (short)256); }
        }

        /// <summary>
        /// 回原点减速度。
        /// </summary>
        public double HomeDeceleration { get; set; } = 0.2;

        /// <summary>
        /// 平停减速度。
        /// 用于普通 Stop 联动行为。
        /// </summary>
        public double NormalStopDeceleration { get; set; } = 0.2;

        /// <summary>
        /// 急停减速度。
        /// 某些控制卡支持伪急停减速时可使用。
        /// </summary>
        public double EmergencyStopDeceleration { get; set; } = 0.5;

        #endregion

        #region 回零参数

        /// <summary>
        /// 固高标准回零模式号。
        /// 用于直接映射厂商标准回零算法。
        /// </summary>
        public short StandardHomeMode { get; set; } = 1;

        /// <summary>
        /// 复位运动方向。
        /// 1=正方向，非正数=负方向。
        /// </summary>
        public short ResetDirection { get; set; } = 1;

        private double _homeSearchVelocity = 1.0;
        /// <summary>
        /// HOME 搜索速度，单位 pulse/ms。
        /// </summary>
        public double HomeSearchVelocity
        {
            get { return _homeSearchVelocity; }
            set { _homeSearchVelocity = value.LimitTo(0.001, 20.0); }
        }

        private double _indexSearchVelocity = 0.2;
        /// <summary>
        /// INDEX 搜索速度，单位 pulse/ms。
        /// </summary>
        public double IndexSearchVelocity
        {
            get { return _indexSearchVelocity; }
            set { _indexSearchVelocity = value.LimitTo(0.001, 10.0); }
        }

        /// <summary>
        /// 原点偏移量，单位 pulse。
        /// </summary>
        public int HomeOffset { get; set; } = 1000;

        /// <summary>
        /// HOME 最大搜索距离，单位 pulse。
        /// 0=不限制。
        /// </summary>
        public int HomeMaxDistance { get; set; }

        /// <summary>
        /// INDEX 最大搜索距离，单位 pulse。
        /// 0=不限制。
        /// </summary>
        public int IndexMaxDistance { get; set; }

        /// <summary>
        /// 脱离步长，单位 pulse。
        /// 0=不限制。
        /// </summary>
        public int EscapeStep { get; set; } = 1000;

        /// <summary>
        /// INDEX 搜索方向。
        /// 0=非正数负方向，1=正方向。
        /// </summary>
        public short IndexSearchDirection { get; set; }

        /// <summary>
        /// 是否启用标准回零自检。
        /// </summary>
        public bool HomeCheck { get; set; } = true;

        /// <summary>
        /// 是否使用 Home 信号参与回零流程。
        /// 用于兼容不同控制卡或不同工艺策略。
        /// </summary>
        public bool HomeUseHomeSignal { get; set; } = true;

        /// <summary>
        /// 是否使用 Index 信号参与回零流程。
        /// </summary>
        public bool HomeUseIndexSignal { get; set; } = true;

        /// <summary>
        /// 是否使用限位信号参与回零流程。
        /// </summary>
        public bool HomeUseLimitSignal { get; set; }

        /// <summary>
        /// 回零完成后是否自动清零。
        /// </summary>
        public bool HomeAutoZeroPos { get; set; } = true;

        /// <summary>
        /// 回零超时，单位 ms。
        /// </summary>
        public int HomeTimeoutMs { get; set; } = 60000;

        #endregion

        #region 软件限位

        /// <summary>
        /// 是否启用软件限位。
        /// </summary>
        public bool SoftLimitEnabled { get; set; }

        /// <summary>
        /// 正向软件限位，单位 pulse。
        /// </summary>
        public double SoftLimitPositive { get; set; }

        /// <summary>
        /// 负向软件限位，单位 pulse。
        /// </summary>
        public double SoftLimitNegative { get; set; }

        #endregion

        #region 使能时序

        /// <summary>
        /// 使能前延时，单位 ms。
        /// </summary>
        public int EnableDelayMs { get; set; } = 50;

        /// <summary>
        /// 失能后延时，单位 ms。
        /// </summary>
        public int DisableDelayMs { get; set; } = 50;

        #endregion

        #region 安全与流程联动

        /// <summary>
        /// 急停序号。
        /// 0=无。
        /// </summary>
        public int EStopId { get; set; }

        /// <summary>
        /// 平停序号。
        /// 0=无。
        /// </summary>
        public int StopId { get; set; }

        #endregion

        #region 运行时状态

        /// <summary>
        /// 使能状态（运行时），不存入 JSON。
        /// </summary>
        [JsonIgnore]
        public bool IsServoOn { get; set; }

        #endregion
    }
}
