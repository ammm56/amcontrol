namespace AM.Model.Vision
{
    /// <summary>
    /// 相机驱动类型。
    /// 第一阶段以 OpenCvSharp + DSHOW 实现通用 USB 相机，厂商 SDK 相机先保留类型边界。
    /// </summary>
    public enum CameraDriverType
    {
        UsbUvc = 0,
        AmvarReserved = 10,
        HikvisionMvsReserved = 20,
        CognexReserved = 30,
        DahengReserved = 40,
        VendorSdkReserved = 90,
        VirtualReserved = 100
    }
}
