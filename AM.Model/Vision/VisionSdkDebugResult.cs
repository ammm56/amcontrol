using System;

namespace AM.Model.Vision
{
    /// <summary>
    /// 视觉 SDK 调试结果。
    /// </summary>
    public class VisionSdkDebugResult
    {
        public VisionSdkDebugOperationKey OperationKey { get; set; }

        public string OperationName { get; set; }

        public string RuntimeName { get; set; }

        public string TriggerSourceName { get; set; }

        public string ModelDeploymentName { get; set; }

        public string InferenceTaskId { get; set; }

        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 总用时。包含相机取图编码、SDK 调用到返回、返回结果处理。
        /// </summary>
        public int ElapsedMs { get; set; }

        public int CameraCaptureEncodeMs { get; set; }

        public int SdkInvokeMs { get; set; }

        public int ResponseProcessMs { get; set; }

        public int TotalElapsedMs { get; set; }

        public bool IsSuccess { get; set; }

        public string State { get; set; }

        public string WorkflowRunId { get; set; }

        public string ResponseJson { get; set; }

        public string ErrorMessage { get; set; }

        public bool ShouldSaveCallRecord { get; set; }
    }
}
