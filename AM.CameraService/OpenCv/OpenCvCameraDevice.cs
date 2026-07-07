using AM.Model.Camera;
using AM.Model.Entity.Device;
using AM.Model.Vision;
using OpenCvSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace AM.CameraService.OpenCv
{
    internal sealed class OpenCvCameraDevice : IDisposable
    {
        private const int WarmupFrameCount = 5;

        private readonly object _syncRoot = new object();
        private readonly CameraConfigEntity _config;
        private VideoCapture _capture;
        private bool _disposed;

        public OpenCvCameraDevice(CameraConfigEntity config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool IsOpen
        {
            get
            {
                lock (_syncRoot)
                {
                    return _capture != null && _capture.IsOpened();
                }
            }
        }

        public void Open()
        {
            lock (_syncRoot)
            {
                ThrowIfDisposed();
                CloseCore();

                var capture = new VideoCapture(_config.DeviceIndex, VideoCaptureAPIs.DSHOW);
                if (!capture.IsOpened())
                {
                    capture.Dispose();
                    throw new InvalidOperationException("OpenCV DSHOW 打开相机失败，DeviceIndex=" + _config.DeviceIndex);
                }

                ApplyRequestedProperties(capture, _config);
                Warmup(capture);
                _capture = capture;
            }
        }

        public void Close()
        {
            lock (_syncRoot)
            {
                CloseCore();
            }
        }

        public void ShowSettings()
        {
            lock (_syncRoot)
            {
                EnsureOpen();
                _capture.Set(VideoCaptureProperties.Settings, 1);
            }
        }

        public CameraFrame GrabFrame()
        {
            lock (_syncRoot)
            {
                EnsureOpen();

                using (var frame = ReadFrame())
                {
                    var imageFormat = ResolveImageFormat(_config.ImageFormat);
                    var extension = ResolveExtension(imageFormat);
                    byte[] encodedBytes;

                    var encodingParams = imageFormat == CameraImageFormat.JPEG
                        ? new[] { (int)ImwriteFlags.JpegQuality, NormalizeJpegQuality(_config.JpegQuality) }
                        : new int[0];
                    var encoded = Cv2.ImEncode(extension, frame, out encodedBytes, encodingParams);

                    if (!encoded || encodedBytes == null || encodedBytes.Length == 0)
                    {
                        throw new InvalidOperationException("OpenCV 图像编码失败，格式=" + imageFormat);
                    }

                    return new CameraFrame
                    {
                        CameraCode = _config.CameraCode,
                        FrameId = Guid.NewGuid().ToString("N"),
                        Timestamp = DateTime.Now,
                        Width = frame.Width,
                        Height = frame.Height,
                        PixelFormat = GetFourCcText(_capture),
                        MediaType = ResolveMediaType(imageFormat),
                        EncodedBytes = encodedBytes
                    };
                }
            }
        }

        public CameraPreviewFrame GrabPreviewFrame()
        {
            lock (_syncRoot)
            {
                EnsureOpen();

                using (var frame = ReadFrame())
                {
                    Mat bgr = null;
                    try
                    {
                        var source = EnsureBgr24(frame, out bgr);
                        return BuildPreviewFrame(source);
                    }
                    finally
                    {
                        if (bgr != null)
                        {
                            bgr.Dispose();
                        }
                    }
                }
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

                CloseCore();
                _disposed = true;
            }
        }

        private Mat ReadFrame()
        {
            var timeoutMs = _config.GrabTimeoutMs <= 0 ? 3000 : _config.GrabTimeoutMs;
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
            Exception lastException = null;

            while (DateTime.UtcNow <= deadline)
            {
                var frame = new Mat();
                try
                {
                    if (_capture.Read(frame) && !frame.Empty())
                    {
                        return frame;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                frame.Dispose();
                Thread.Sleep(10);
            }

            if (lastException != null)
            {
                throw new InvalidOperationException("OpenCV 取图失败", lastException);
            }

            throw new TimeoutException("OpenCV 取图超时，超时=" + timeoutMs + "ms");
        }

        private static void ApplyRequestedProperties(VideoCapture capture, CameraConfigEntity config)
        {
            capture.Set(VideoCaptureProperties.BufferSize, 1);
            SetFourCc(capture, config.PixelFormat);

            if (config.Fps > 0)
            {
                capture.Set(VideoCaptureProperties.Fps, config.Fps);
            }

            if (config.Width > 0)
            {
                capture.Set(VideoCaptureProperties.FrameWidth, config.Width);
            }

            if (config.Height > 0)
            {
                capture.Set(VideoCaptureProperties.FrameHeight, config.Height);
            }

            SetFourCc(capture, config.PixelFormat);

            if (config.Exposure.HasValue)
            {
                capture.Set(VideoCaptureProperties.Exposure, config.Exposure.Value);
            }

            if (config.Gain.HasValue)
            {
                capture.Set(VideoCaptureProperties.Gain, config.Gain.Value);
            }
        }

        private static void Warmup(VideoCapture capture)
        {
            for (var i = 0; i < WarmupFrameCount; i++)
            {
                using (var frame = new Mat())
                {
                    capture.Read(frame);
                }
            }
        }

        private static void SetFourCc(VideoCapture capture, string pixelFormat)
        {
            var fourCc = NormalizeFourCc(pixelFormat);
            if (string.IsNullOrWhiteSpace(fourCc))
            {
                return;
            }

            capture.Set(VideoCaptureProperties.FourCC, VideoWriter.FourCC(fourCc));
        }

        internal static string NormalizeFourCc(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "MJPG";
            }

            var text = value.Trim().ToUpperInvariant();
            if (text == "MJPEG")
            {
                return "MJPG";
            }

            return text.Length >= 4 ? text.Substring(0, 4) : text;
        }

        internal static string GetFourCcText(VideoCapture capture)
        {
            if (capture == null)
            {
                return string.Empty;
            }

            var fourCc = (int)capture.Get(VideoCaptureProperties.FourCC);
            if (fourCc == 0)
            {
                return string.Empty;
            }

            var chars = new[]
            {
                (char)(fourCc & 0xFF),
                (char)((fourCc >> 8) & 0xFF),
                (char)((fourCc >> 16) & 0xFF),
                (char)((fourCc >> 24) & 0xFF)
            };
            return new string(chars);
        }

        private static Mat EnsureBgr24(Mat frame, out Mat converted)
        {
            converted = null;
            var channels = frame.Channels();

            if (channels == 3)
            {
                return frame;
            }

            converted = new Mat();
            if (channels == 4)
            {
                Cv2.CvtColor(frame, converted, ColorConversionCodes.BGRA2BGR);
            }
            else if (channels == 1)
            {
                Cv2.CvtColor(frame, converted, ColorConversionCodes.GRAY2BGR);
            }
            else
            {
                throw new InvalidOperationException("不支持的预览像素通道数: " + channels);
            }

            return converted;
        }

        private CameraPreviewFrame BuildPreviewFrame(Mat frame)
        {
            var stride = checked((int)frame.Step());
            var height = frame.Rows;
            var bytes = new byte[checked(stride * height)];

            for (var row = 0; row < height; row++)
            {
                Marshal.Copy(IntPtr.Add(frame.Data, row * stride), bytes, row * stride, stride);
            }

            return new CameraPreviewFrame
            {
                CameraCode = _config.CameraCode,
                FrameId = Guid.NewGuid().ToString("N"),
                Timestamp = DateTime.Now,
                Width = frame.Width,
                Height = frame.Height,
                Stride = stride,
                PixelFormat = "BGR24",
                BgrBytes = bytes
            };
        }

        private static CameraImageFormat ResolveImageFormat(string value)
        {
            CameraImageFormat format;
            return Enum.TryParse(value, true, out format) ? format : CameraImageFormat.JPEG;
        }

        private static string ResolveExtension(CameraImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case CameraImageFormat.PNG:
                    return ".png";
                case CameraImageFormat.BMP:
                    return ".bmp";
                default:
                    return ".jpg";
            }
        }

        private static string ResolveMediaType(CameraImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case CameraImageFormat.PNG:
                    return "image/png";
                case CameraImageFormat.BMP:
                    return "image/bmp";
                default:
                    return "image/jpeg";
            }
        }

        private static int NormalizeJpegQuality(int value)
        {
            if (value < 1)
            {
                return 80;
            }

            return value > 100 ? 100 : value;
        }

        private void EnsureOpen()
        {
            ThrowIfDisposed();
            if (_capture == null || !_capture.IsOpened())
            {
                throw new InvalidOperationException("相机未打开: " + _config.CameraCode);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private void CloseCore()
        {
            if (_capture == null)
            {
                return;
            }

            try
            {
                _capture.Release();
            }
            finally
            {
                _capture.Dispose();
                _capture = null;
            }
        }
    }
}
