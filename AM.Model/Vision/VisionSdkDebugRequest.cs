namespace AM.Model.Vision
{
    /// <summary>
    /// 视觉 SDK 调试请求。
    /// </summary>
    public class VisionSdkDebugRequest
    {
        public VisionSdkDebugOperationKey OperationKey { get; set; }

        public string RuntimeName { get; set; }

        public string TriggerSourceName { get; set; }

        public string ModelDeploymentName { get; set; }

        public string ModelInputUri { get; set; }

        public string ModelInputFileId { get; set; }

        public string ModelInferenceTaskId { get; set; }

        public byte[] ImageBytes { get; set; }

        public string ImageBase64 { get; set; }

        public string ImagePath { get; set; }

        public string MediaType { get; set; }
    }
}
