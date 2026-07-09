using System;

namespace AM.Model.Camera
{
    /// <summary>
    /// 相机单帧结果。
    /// 第一阶段面向视觉 SDK 输出编码后的 image bytes；
    /// 高频 ZeroMQ 调用可直接使用连续 HWC BGR24 原始像素 bytes。
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

        public byte[] Bgr24Bytes { get; set; }

        public int Bgr24BytesLength
        {
            get { return Bgr24Bytes == null ? 0 : Bgr24Bytes.Length; }
        }

        public bool HasEncodedImage
        {
            get { return EncodedBytes != null && EncodedBytes.Length > 0; }
        }

        public bool HasBgr24Image
        {
            get { return Bgr24Bytes != null && Bgr24Bytes.Length > 0; }
        }
    }
}
