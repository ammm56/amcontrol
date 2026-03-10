using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard
{
    public class MotionCardConfig
    {
        /// <summary>
        /// 硬件通信通道（通常对应电脑插槽里的物理卡索引，从 0 开始）
        /// 对应 GTN_Open 的 channel
        /// </summary>
        public short CardId { get; set; } = 0;

        /// <summary>
        /// 1 配置模式, 1：表示使用配置文件初始化,按照文件里的参数（轴数、脉冲极性、限位开关极性等）直接配置硬件。
        /// 取值为 0（可选）：通常表示不加载配置，仅打开控制卡。这种模式下，需要通过后续代码手动设置每一个硬件参数。
        /// </summary>
        public short ModeParam { get; set; } = 0; 

        /// <summary>
        /// 控制卡内核数量，现在用的有2核，但是入门卡只有1核
        /// </summary>
        public ushort CoreNumber { get; set; } = 2;

        /// <summary>
        /// 控制卡轴的总数量
        /// </summary>
        public short AxisCountNumber { get; set; } = 4;

        /// <summary>
        /// 使用扩展模块
        /// </summary>
        public bool UseExtModule {  get; set; } = false;

        /// <summary>
        /// 该卡下的所有轴配置
        /// </summary>
        public List<AxisConfig> AxisConfigs { get; set; } = new List<AxisConfig>
        {

            new AxisConfig
            {
                AxisId = 1,
                Name = "14轴",
                LogicalId = 114,
                PhysicalCore = 1,
                PhysicalAxis = 14,

                AlarmEnable = true,
                AlarmInvert = false,

                PulseMode = 0,

                EncoderExternal = false,
                EncoderInvert = false,

                LimitHomeInvert = false,

                LimitMode = -1,

                Lead = 5.0,
                PulsePerRev = 10000,

                Acc = 100,
                Dec = 100,
                SmoothTime = 25

            }
        };
    }
}
