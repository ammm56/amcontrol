using AM.Model.Camera;
using AM.Model.Common;
using AM.Model.Entity.Device;
using System;

namespace AM.Model.Interfaces.Camera
{
    /// <summary>
    /// 相机运行服务接口。
    /// 只负责 amcontrol 本项目的相机枚举、打开、取图与编码，不保存或理解 amvision 配置。
    /// </summary>
    public interface ICameraRuntimeService : IDisposable
    {
        Result<CameraDeviceInfo> EnumerateUsbCameras(int maxIndex);

        Result Open(CameraConfigEntity config);

        Result Close(string cameraCode);

        Result ShowSettings(string cameraCode);

        Result<CameraPreviewFrame> GrabPreviewFrame(string cameraCode);

        Result<CameraFrame> GrabFrame(string cameraCode);

        Result<bool> IsOpen(string cameraCode);
    }
}
