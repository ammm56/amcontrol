using SqlSugar;
using System;

namespace AM.Model.Entity.Vision
{
    /// <summary>
    /// 本项目发起的视觉 SDK 调用记录。
    /// 仅记录 amcontrol 调用历史，不保存 amvision 调用配置。
    /// </summary>
    [SugarTable("vision_call_record")]
    public class VisionCallRecordEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? CameraId { get; set; }

        [SugarColumn(IsNullable = true)]
        public string CameraCode { get; set; }

        /// <summary>
        /// 调用方式，保存 VisionCallMode 枚举名称。
        /// </summary>
        public string CallMode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string RuntimeName { get; set; }

        [SugarColumn(IsNullable = true)]
        public string TriggerSourceName { get; set; }

        [SugarColumn(IsNullable = true)]
        public string ImagePath { get; set; }

        [SugarColumn(IsNullable = true)]
        public string MediaType { get; set; }

        public long ImageBytesLength { get; set; }

        public DateTime RequestTime { get; set; }

        public int ElapsedMs { get; set; }

        public bool IsSuccess { get; set; }

        [SugarColumn(IsNullable = true)]
        public string State { get; set; }

        [SugarColumn(IsNullable = true)]
        public string WorkflowRunId { get; set; }

        [SugarColumn(IsNullable = true, Length = 8000)]
        public string ResponseJson { get; set; }

        [SugarColumn(IsNullable = true, Length = 4000)]
        public string ErrorMessage { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? OperatorUserId { get; set; }

        [SugarColumn(IsNullable = true)]
        public string StationCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string ProductCode { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
