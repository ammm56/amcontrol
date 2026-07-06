using System;

namespace AM.Model.Vision
{
    /// <summary>
    /// 本项目视觉 SDK 调用结果。
    /// </summary>
    public class VisionInvokeResult
    {
        public string CallMode { get; set; }

        public string RuntimeName { get; set; }

        public string TriggerSourceName { get; set; }

        public DateTime RequestTime { get; set; }

        public int ElapsedMs { get; set; }

        public bool IsSuccess { get; set; }

        public string State { get; set; }

        public string WorkflowRunId { get; set; }

        public string ResponseJson { get; set; }

        public string ErrorMessage { get; set; }
    }
}
