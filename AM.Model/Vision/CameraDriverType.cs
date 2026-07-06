namespace AM.Model.Vision
{
    /// <summary>
    /// 相机驱动类型。
    /// 第一阶段仅实现通用 USB/UVC，相机厂商 SDK 先保留类型边界。
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
