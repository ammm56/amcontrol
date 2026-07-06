using SqlSugar;
using System;

namespace AM.Model.Entity.Device
{
    /// <summary>
    /// 本项目相机配置表。
    /// 第一阶段默认通用 USB/UVC 相机，不保存任何 amvision workflow/runtime/TriggerSource 配置。
    /// </summary>
    [SugarTable("device_camera_config")]
    public class CameraConfigEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 相机编码，作为业务侧唯一标识。
        /// </summary>
        public string CameraCode { get; set; }

        /// <summary>
        /// 相机显示名称。
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 驱动类型，保存 CameraDriverType 枚举名称。
        /// </summary>
        public string DriverType { get; set; }

        public bool IsEnabled { get; set; }

        /// <summary>
        /// USB/UVC 设备索引。
        /// </summary>
        public int DeviceIndex { get; set; }

        /// <summary>
        /// USB/UVC 设备路径或 moniker。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string DevicePath { get; set; }

        /// <summary>
        /// 系统枚举友好名称。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string FriendlyName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Fps { get; set; }

        [SugarColumn(IsNullable = true)]
        public string PixelFormat { get; set; }

        [SugarColumn(IsNullable = true)]
        public double? Exposure { get; set; }

        [SugarColumn(IsNullable = true)]
        public double? Gain { get; set; }

        public int GrabTimeoutMs { get; set; }

        /// <summary>
        /// SDK 调用前的图片编码格式，保存 CameraImageFormat 枚举名称。
        /// </summary>
        public string ImageFormat { get; set; }

        public int JpegQuality { get; set; }

        public int PreviewFps { get; set; }

        public bool SaveImageEnabled { get; set; }

        [SugarColumn(IsNullable = true)]
        public string SaveImageDirectory { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
