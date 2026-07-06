namespace AM.Model.Camera
{
    /// <summary>
    /// 相机枚举结果。
    /// OpenCV DSHOW 后端不能稳定提供设备友好名，第一阶段以索引为主。
    /// </summary>
    public class CameraDeviceInfo
    {
        public int DeviceIndex { get; set; }

        public string DevicePath { get; set; }

        public string FriendlyName { get; set; }

        public string DriverType { get; set; }

        public string BackendName { get; set; }

        public bool IsAvailable { get; set; }

        public string Description { get; set; }
    }
}
