using System;

namespace AM.Model.Camera
{
    /// <summary>
    /// 相机实时预览帧。
    /// 使用 OpenCV/Windows Bitmap 兼容的 BGR24 内存布局，不经过 JPEG/PNG/BMP 编码。
    /// </summary>
    public class CameraPreviewFrame
    {
        public string CameraCode { get; set; }

        public string FrameId { get; set; }

        public DateTime Timestamp { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Stride { get; set; }

        public string PixelFormat { get; set; }

        public byte[] BgrBytes { get; set; }

        public int BytesLength
        {
            get { return BgrBytes == null ? 0 : BgrBytes.Length; }
        }
    }
}
