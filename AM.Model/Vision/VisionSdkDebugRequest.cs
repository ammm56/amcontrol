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

        public byte[] ImageBytes { get; set; }

        public byte[] Bgr24Bytes { get; set; }

        public int Bgr24Width { get; set; }

        public int Bgr24Height { get; set; }

        public string ImageBase64 { get; set; }

        public string ImagePath { get; set; }

        public string MediaType { get; set; }
    }
}
