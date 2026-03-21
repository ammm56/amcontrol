using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace AM.Model.Entity.Motion.Actuator
{
    /// <summary>
    /// 真空对象配置表。
    /// 基于前两层逻辑 IO 点位之上，定义真空吸附对象的输出、反馈、工件检测、超时与报警规则。
    /// </summary>
    [SugarTable("motion_vacuum_config")]
    public class VacuumConfigEntity
    {
        /// <summary>
        /// 自增主键。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 对象名称。
        /// 例如：PickHeadVacuum、LoadVacuum1。
        /// 建议全局唯一。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称。
        /// 例如：取料头真空、上料吸嘴1。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 吸真空输出逻辑位号。
        /// 必须引用 DO 点。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        public short VacuumOnOutputBit { get; set; }

        /// <summary>
        /// 破真空/吹气输出逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DO。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? BlowOffOutputBit { get; set; }

        /// <summary>
        /// 真空建立反馈逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DI。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? VacuumFeedbackBit { get; set; }

        /// <summary>
        /// 真空释放反馈逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DI。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? ReleaseFeedbackBit { get; set; }

        /// <summary>
        /// 工件存在检测逻辑位号。
        /// 可为空。
        /// 指向 motion_io_map 中 IoType = DI。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public short? WorkpiecePresentBit { get; set; }

        /// <summary>
        /// 是否启用真空建立/释放反馈校验。
        /// </summary>
        public bool UseFeedbackCheck { get; set; }

        /// <summary>
        /// 是否启用工件存在检测校验。
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
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnBuildTimeout { get; set; }

        /// <summary>
        /// 释放真空超时报警代码。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnReleaseTimeout { get; set; }

        /// <summary>
        /// 工件丢失报警代码。
        /// 例如吸附后搬运过程中掉料。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? AlarmCodeOnWorkpieceLost { get; set; }

        /// <summary>
        /// 检测到工件后是否保持真空输出持续导通。
        /// 一般吸附型真空对象应为 true。
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
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 备注。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }
    }
}
