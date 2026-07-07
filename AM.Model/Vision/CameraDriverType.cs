namespace AM.Model.Vision
{
    /// <summary>
    /// 相机驱动类型。
    /// 第一阶段以 OpenCvSharp + DSHOW 实现通用 USB 相机，厂商 SDK 相机先保留类型边界。
    /// </summary>
    public enum CameraDriverType
    {
        Usb = 0,
        Amvar = 10,
        HikvisionMvs = 20,
        Cognex = 30,
        Daheng = 40,
        VendorSdk = 90,
        Virtual = 100
    }
}
