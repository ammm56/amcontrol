using System;

namespace AM.Model.Camera
{
    /// <summary>
    /// 相机单帧结果。
    /// 第一阶段面向视觉 SDK 统一输出编码后的 image bytes。
    /// </summary>
    public class CameraFrame
    {
        public string CameraCode { get; set; }

        public string FrameId { get; set; }

        public DateTime Timestamp { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string PixelFormat { get; set; }

        public string MediaType { get; set; }

        public byte[] EncodedBytes { get; set; }

        public int EncodedBytesLength
        {
            get { return EncodedBytes == null ? 0 : EncodedBytes.Length; }
        }
    }
}
