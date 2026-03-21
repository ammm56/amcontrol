using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.MotionCard.Actuator
{
    /// <summary>
    /// 真空对象运行时配置。
    /// 由第三层对象配置表装配而来，引用前两层逻辑 IO 点位。
    /// </summary>
    public class VacuumConfig
    {
        /// <summary>
        /// 对象名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 吸真空输出逻辑位号。
        /// </summary>
        public short VacuumOnOutputBit { get; set; }

        /// <summary>
        /// 破真空/吹气输出逻辑位号。
        /// </summary>
        public short? BlowOffOutputBit { get; set; }

        /// <summary>
        /// 真空建立反馈逻辑位号。
        /// </summary>
        public short? VacuumFeedbackBit { get; set; }

        /// <summary>
        /// 真空释放反馈逻辑位号。
        /// </summary>
        public short? ReleaseFeedbackBit { get; set; }

        /// <summary>
        /// 工件存在检测逻辑位号。
        /// </summary>
        public short? WorkpiecePresentBit { get; set; }

        /// <summary>
        /// 是否启用真空建立/释放反馈校验。
        /// </summary>
        public bool UseFeedbackCheck { get; set; }

        /// <summary>
        /// 是否启用工件存在检测。
        /// </summary>
        public bool UseWorkpieceCheck { get; set; }

        /// <summary>
        /// 建立真空超时时间，单位 ms。
        /// </summary>
        public int VacuumBuildTimeoutMs { get; set; }

        /// <summary>
        /// 释放真空超时时间，单位 ms。
        /// </summary>
        public int ReleaseTimeoutMs { get; set; }

        /// <summary>
        /// 建立真空超时报警代码。
        /// </summary>
        public int? AlarmCodeOnBuildTimeout { get; set; }

        /// <summary>
        /// 释放真空超时报警代码。
        /// </summary>
        public int? AlarmCodeOnReleaseTimeout { get; set; }

        /// <summary>
        /// 工件丢失报警代码。
        /// </summary>
        public int? AlarmCodeOnWorkpieceLost { get; set; }

        /// <summary>
        /// 检测到工件后是否保持真空输出持续导通。
        /// </summary>
        public bool KeepVacuumOnAfterDetected { get; set; }

        /// <summary>
        /// 是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序号。
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 描述说明。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        public string Remark { get; set; }
    }
}
