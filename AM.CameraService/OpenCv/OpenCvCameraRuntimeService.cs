using AM.Model.Camera;
using AM.Model.Common;
using AM.Model.Entity.Device;
using AM.Model.Interfaces.Camera;
using AM.Model.Vision;
using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace AM.CameraService.OpenCv
{
    /// <summary>
    /// 基于 OpenCvSharp VideoCapture 的 USB 相机运行服务。
    /// 当前固定使用 DSHOW 后端，贴近现场已验证的 Python OpenCV 调用方式。
    /// </summary>
    public sealed class OpenCvCameraRuntimeService : ICameraRuntimeService
    {
        private const string BackendName = "OpenCvSharp DSHOW";

        private readonly object _syncRoot = new object();
        private readonly Dictionary<string, OpenCvCameraDevice> _devices =
            new Dictionary<string, OpenCvCameraDevice>(StringComparer.OrdinalIgnoreCase);

        private bool _disposed;

        public Result<CameraDeviceInfo> EnumerateUsbCameras(int maxIndex)
        {
            try
            {
                ThrowIfDisposed();

                var safeMaxIndex = maxIndex <= 0 ? 10 : maxIndex;
                var items = new List<CameraDeviceInfo>();

                for (var index = 0; index <= safeMaxIndex; index++)
                {
                    using (var capture = new VideoCapture(index, VideoCaptureAPIs.DSHOW))
                    {
                        if (!capture.IsOpened())
                        {
                            continue;
                        }

                        using (var frame = new Mat())
                        {
                            capture.Read(frame);

                            var description = frame.Empty()
                                ? "已打开，未读到有效预览帧"
                                : string.Format("已打开，预览帧 {0}x{1}，FourCC={2}", frame.Width, frame.Height, OpenCvCameraDevice.GetFourCcText(capture));

                            items.Add(new CameraDeviceInfo
                            {
                                DeviceIndex = index,
                                DevicePath = null,
                                FriendlyName = "OpenCV Camera " + index,
                                DriverType = CameraDriverType.Usb.ToString(),
                                BackendName = BackendName,
                                IsAvailable = true,
                                Description = description
                            });
                        }
                    }
                }

                return Result<CameraDeviceInfo>.OkList(items, "USB 相机枚举完成", ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result<CameraDeviceInfo>.Fail(1, "USB 相机枚举失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result Open(CameraConfigEntity config)
        {
            try
            {
                ThrowIfDisposed();
                ValidateConfig(config);

                var cameraCode = NormalizeCameraCode(config.CameraCode);
                var driverType = NormalizeText(config.DriverType);
                if (!string.Equals(driverType, CameraDriverType.Usb.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return Result.Fail(2, "当前仅实现通用 USB 相机，驱动类型=" + driverType, ResultSource.Camera);
                }

                lock (_syncRoot)
                {
                    OpenCvCameraDevice oldDevice;
                    if (_devices.TryGetValue(cameraCode, out oldDevice))
                    {
                        oldDevice.Dispose();
                        _devices.Remove(cameraCode);
                    }

                    var device = new OpenCvCameraDevice(config);
                    device.Open();
                    _devices[cameraCode] = device;
                }

                return Result.Ok("相机打开成功: " + cameraCode, ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result.Fail(3, "相机打开失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result Close(string cameraCode)
        {
            try
            {
                ThrowIfDisposed();

                var normalizedCode = NormalizeCameraCode(cameraCode);
                lock (_syncRoot)
                {
                    OpenCvCameraDevice device;
                    if (_devices.TryGetValue(normalizedCode, out device))
                    {
                        device.Dispose();
                        _devices.Remove(normalizedCode);
                    }
                }

                return Result.Ok("相机已关闭: " + normalizedCode, ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result.Fail(4, "相机关闭失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result ShowSettings(string cameraCode)
        {
            try
            {
                ThrowIfDisposed();
                var device = GetOpenedDevice(cameraCode);
                device.ShowSettings();
                return Result.Ok("已打开相机驱动设置页", ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result.Fail(5, "打开相机设置页失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result<CameraFrame> GrabFrame(string cameraCode)
        {
            try
            {
                ThrowIfDisposed();
                var device = GetOpenedDevice(cameraCode);
                var frame = device.GrabFrame();
                return Result<CameraFrame>.OkItem(frame, "相机取图成功", ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result<CameraFrame>.Fail(6, "相机取图失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result<CameraPreviewFrame> GrabPreviewFrame(string cameraCode, int maxWidth, int maxHeight)
        {
            try
            {
                ThrowIfDisposed();
                var device = GetOpenedDevice(cameraCode);
                var frame = device.GrabPreviewFrame(maxWidth, maxHeight);
                return Result<CameraPreviewFrame>.OkItem(frame, "相机预览帧获取成功", ResultSource.Camera);
            }
            catch (Exception ex)
            {
                return Result<CameraPreviewFrame>.Fail(8, "相机预览帧获取失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public Result<bool> IsOpen(string cameraCode)
        {
            try
            {
                ThrowIfDisposed();
                var normalizedCode = NormalizeCameraCode(cameraCode);
                lock (_syncRoot)
                {
                    OpenCvCameraDevice device;
                    var isOpen = _devices.TryGetValue(normalizedCode, out device) && device.IsOpen;
                    return Result<bool>.OkItem(isOpen, "相机状态查询完成", ResultSource.Camera);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(7, "相机状态查询失败: " + ex.Message, ResultSource.Camera);
            }
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    return;
                }

                foreach (var item in _devices.Values)
                {
                    item.Dispose();
                }

                _devices.Clear();
                _disposed = true;
            }
        }

        private OpenCvCameraDevice GetOpenedDevice(string cameraCode)
        {
            var normalizedCode = NormalizeCameraCode(cameraCode);
            lock (_syncRoot)
            {
                OpenCvCameraDevice device;
                if (!_devices.TryGetValue(normalizedCode, out device) || !device.IsOpen)
                {
                    throw new InvalidOperationException("相机未打开: " + normalizedCode);
                }

                return device;
            }
        }

        private static void ValidateConfig(CameraConfigEntity config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            NormalizeCameraCode(config.CameraCode);
        }

        private static string NormalizeCameraCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("相机编码不能为空");
            }

            return value.Trim();
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}
