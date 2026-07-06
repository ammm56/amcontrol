using AM.Model.Common;
using AM.Model.Entity.Device;
using AM.Model.Vision;
using AntdUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// 相机配置编辑控件。
    /// 仅负责相机参数输入与实体构建，不访问数据库、不打开相机、不理解 amvision 配置。
    /// </summary>
    public partial class CameraConfigEditControl : UserControl
    {
        private CameraConfigEntity _source;

        public CameraConfigEditControl()
        {
            InitializeComponent();
            InitializeSelections();
            SetEntity(null);
        }

        public void FocusFirst()
        {
            inputCameraCode.Focus();
            inputCameraCode.SelectAll();
        }

        public void SetEntity(CameraConfigEntity entity)
        {
            _source = entity == null ? CreateDefaultEntity() : entity;

            inputCameraCode.Text = _source.CameraCode ?? string.Empty;
            inputCameraName.Text = _source.CameraName ?? string.Empty;
            SetSelectValue(selectDriverType, string.IsNullOrWhiteSpace(_source.DriverType) ? CameraDriverType.UsbUvc.ToString() : _source.DriverType);
            checkEnabled.Checked = _source.Id <= 0 || _source.IsEnabled;
            inputDeviceIndex.Text = _source.DeviceIndex.ToString();
            inputFriendlyName.Text = _source.FriendlyName ?? string.Empty;
            inputWidth.Text = (_source.Width <= 0 ? 1920 : _source.Width).ToString();
            inputHeight.Text = (_source.Height <= 0 ? 1080 : _source.Height).ToString();
            inputFps.Text = (_source.Fps <= 0 ? 30 : _source.Fps).ToString();
            inputPixelFormat.Text = string.IsNullOrWhiteSpace(_source.PixelFormat) ? "MJPG" : _source.PixelFormat;
            inputExposure.Text = _source.Exposure.HasValue ? _source.Exposure.Value.ToString("0.###") : string.Empty;
            inputGain.Text = _source.Gain.HasValue ? _source.Gain.Value.ToString("0.###") : string.Empty;
            inputGrabTimeoutMs.Text = (_source.GrabTimeoutMs <= 0 ? 3000 : _source.GrabTimeoutMs).ToString();
            SetSelectValue(selectImageFormat, string.IsNullOrWhiteSpace(_source.ImageFormat) ? CameraImageFormat.Jpeg.ToString() : _source.ImageFormat);
            inputJpegQuality.Text = (_source.JpegQuality <= 0 ? 80 : _source.JpegQuality).ToString();
            inputPreviewFps.Text = (_source.PreviewFps <= 0 ? 10 : _source.PreviewFps).ToString();
            checkSaveImageEnabled.Checked = _source.SaveImageEnabled;
            inputSaveImageDirectory.Text = string.IsNullOrWhiteSpace(_source.SaveImageDirectory) ? "Images\\Camera" : _source.SaveImageDirectory;
            inputRemark.Text = _source.Remark ?? string.Empty;
        }

        public Result<CameraConfigEntity> BuildEntity()
        {
            var cameraCode = NormalizeText(inputCameraCode.Text);
            if (string.IsNullOrWhiteSpace(cameraCode))
            {
                return Result<CameraConfigEntity>.Fail(1, "相机编码不能为空", ResultSource.UI);
            }

            int deviceIndex;
            if (!TryParseInt(inputDeviceIndex.Text, out deviceIndex) || deviceIndex < 0)
            {
                return Result<CameraConfigEntity>.Fail(2, "设备索引必须为大于等于 0 的整数", ResultSource.UI);
            }

            int width;
            if (!TryParseInt(inputWidth.Text, out width) || width <= 0)
            {
                return Result<CameraConfigEntity>.Fail(3, "宽度必须为大于 0 的整数", ResultSource.UI);
            }

            int height;
            if (!TryParseInt(inputHeight.Text, out height) || height <= 0)
            {
                return Result<CameraConfigEntity>.Fail(4, "高度必须为大于 0 的整数", ResultSource.UI);
            }

            int fps;
            if (!TryParseInt(inputFps.Text, out fps) || fps <= 0)
            {
                return Result<CameraConfigEntity>.Fail(5, "FPS 必须为大于 0 的整数", ResultSource.UI);
            }

            int grabTimeoutMs;
            if (!TryParseInt(inputGrabTimeoutMs.Text, out grabTimeoutMs) || grabTimeoutMs <= 0)
            {
                return Result<CameraConfigEntity>.Fail(6, "取图超时必须为大于 0 的整数", ResultSource.UI);
            }

            int jpegQuality;
            if (!TryParseInt(inputJpegQuality.Text, out jpegQuality) || jpegQuality < 1 || jpegQuality > 100)
            {
                return Result<CameraConfigEntity>.Fail(7, "JPEG 质量必须在 1 到 100 之间", ResultSource.UI);
            }

            int previewFps;
            if (!TryParseInt(inputPreviewFps.Text, out previewFps) || previewFps <= 0)
            {
                return Result<CameraConfigEntity>.Fail(8, "预览 FPS 必须为大于 0 的整数", ResultSource.UI);
            }

            var now = DateTime.Now;
            var pixelFormat = NormalizePixelFormat(inputPixelFormat.Text);

            var entity = new CameraConfigEntity
            {
                Id = _source == null ? 0 : _source.Id,
                CameraCode = cameraCode,
                CameraName = NormalizeText(inputCameraName.Text),
                DriverType = NormalizeText(GetSelectValue(selectDriverType)),
                IsEnabled = checkEnabled.Checked,
                DeviceIndex = deviceIndex,
                DevicePath = _source == null ? null : _source.DevicePath,
                FriendlyName = NormalizeText(inputFriendlyName.Text),
                Width = width,
                Height = height,
                Fps = fps,
                PixelFormat = pixelFormat,
                Exposure = ParseNullableDouble(inputExposure.Text),
                Gain = ParseNullableDouble(inputGain.Text),
                GrabTimeoutMs = grabTimeoutMs,
                ImageFormat = NormalizeText(GetSelectValue(selectImageFormat)),
                JpegQuality = jpegQuality,
                PreviewFps = previewFps,
                SaveImageEnabled = checkSaveImageEnabled.Checked,
                SaveImageDirectory = NormalizeText(inputSaveImageDirectory.Text),
                Remark = NormalizeText(inputRemark.Text),
                CreateTime = _source == null || _source.CreateTime == default(DateTime) ? now : _source.CreateTime,
                UpdateTime = now
            };

            if (string.IsNullOrWhiteSpace(entity.CameraName))
            {
                entity.CameraName = entity.CameraCode;
            }

            if (string.IsNullOrWhiteSpace(entity.DriverType))
            {
                entity.DriverType = CameraDriverType.UsbUvc.ToString();
            }

            if (string.IsNullOrWhiteSpace(entity.ImageFormat))
            {
                entity.ImageFormat = CameraImageFormat.Jpeg.ToString();
            }

            return Result<CameraConfigEntity>.OkItem(entity, "相机配置输入有效", ResultSource.UI);
        }

        private void InitializeSelections()
        {
            selectDriverType.Items.Clear();
            selectDriverType.Items.AddRange(Enum.GetNames(typeof(CameraDriverType)).Select(x => (object)x).ToArray());

            selectImageFormat.Items.Clear();
            selectImageFormat.Items.AddRange(Enum.GetNames(typeof(CameraImageFormat)).Select(x => (object)x).ToArray());
        }

        private static CameraConfigEntity CreateDefaultEntity()
        {
            var now = DateTime.Now;
            return new CameraConfigEntity
            {
                CameraCode = "USB_CAMERA_0",
                CameraName = "USB相机0",
                DriverType = CameraDriverType.UsbUvc.ToString(),
                IsEnabled = true,
                DeviceIndex = 0,
                FriendlyName = "OpenCV Camera 0",
                Width = 1920,
                Height = 1080,
                Fps = 30,
                PixelFormat = "MJPG",
                GrabTimeoutMs = 3000,
                ImageFormat = CameraImageFormat.Jpeg.ToString(),
                JpegQuality = 80,
                PreviewFps = 10,
                SaveImageEnabled = false,
                SaveImageDirectory = "Images\\Camera",
                Remark = "OpenCV DSHOW USB 相机",
                CreateTime = now,
                UpdateTime = now
            };
        }

        private static void SetSelectValue(Select select, string value)
        {
            if (select == null)
            {
                return;
            }

            var text = NormalizeText(value);
            if (!string.IsNullOrWhiteSpace(text))
            {
                var exists = select.Items.Cast<object>()
                    .Any(x => string.Equals(x == null ? string.Empty : x.ToString(), text, StringComparison.OrdinalIgnoreCase));

                if (!exists)
                {
                    select.Items.Add(text);
                }
            }

            select.SelectedValue = text;
        }

        private static string GetSelectValue(Select select)
        {
            if (select == null)
            {
                return string.Empty;
            }

            if (select.SelectedValue != null)
            {
                return NormalizeText(select.SelectedValue.ToString());
            }

            return NormalizeText(select.Text);
        }

        private static bool TryParseInt(string value, out int result)
        {
            return int.TryParse(NormalizeText(value), out result);
        }

        private static double? ParseNullableDouble(string value)
        {
            var text = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            double parsed;
            return double.TryParse(text, out parsed) ? (double?)parsed : null;
        }

        private static string NormalizePixelFormat(string value)
        {
            var text = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                return "MJPG";
            }

            if (string.Equals(text, "MJPEG", StringComparison.OrdinalIgnoreCase))
            {
                return "MJPG";
            }

            return text.ToUpperInvariant();
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }
    }
}
