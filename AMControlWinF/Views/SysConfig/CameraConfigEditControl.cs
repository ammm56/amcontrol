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
        private const string DefaultSaveImageDirectory = "Images\\Camera";

        private static readonly CameraResolutionOption[] ResolutionOptions =
        {
            new CameraResolutionOption(160, 120),
            new CameraResolutionOption(320, 240),
            new CameraResolutionOption(640, 480),
            new CameraResolutionOption(800, 600),
            new CameraResolutionOption(1024, 768),
            new CameraResolutionOption(1280, 720),
            new CameraResolutionOption(1280, 960),
            new CameraResolutionOption(1600, 1200),
            new CameraResolutionOption(1920, 1080),
            new CameraResolutionOption(1920, 1200),
            new CameraResolutionOption(2048, 1536),
            new CameraResolutionOption(2560, 1440),
            new CameraResolutionOption(2560, 1600),
            new CameraResolutionOption(3840, 2160),
            new CameraResolutionOption(4096, 2160),
            new CameraResolutionOption(5120, 2880),
            new CameraResolutionOption(7680, 4320)
        };

        private CameraConfigEntity _source;
        private bool _updatingResolution;

        public CameraConfigEditControl()
        {
            InitializeComponent();
            InitializeSelections();
            BindControlEvents();
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

            var fps = _source.Fps <= 0 ? 30 : _source.Fps;
            var previewFps = _source.PreviewFps <= 0 ? fps : Math.Min(_source.PreviewFps, fps);

            inputCameraCode.Text = _source.CameraCode ?? string.Empty;
            inputCameraName.Text = _source.CameraName ?? string.Empty;
            SetSelectValue(selectDriverType, NormalizeDriverTypeValue(_source.DriverType));
            checkEnabled.Checked = _source.Id <= 0 || _source.IsEnabled;
            inputDeviceIndex.Value = Math.Max(0, _source.DeviceIndex);
            inputFriendlyName.Text = _source.FriendlyName ?? string.Empty;
            SetResolution(_source.Width <= 0 ? 1920 : _source.Width, _source.Height <= 0 ? 1080 : _source.Height);
            inputFps.Text = fps.ToString();
            inputPixelFormat.Text = string.IsNullOrWhiteSpace(_source.PixelFormat) ? "MJPG" : _source.PixelFormat;
            inputExposure.Text = _source.Exposure.HasValue ? _source.Exposure.Value.ToString("0.###") : string.Empty;
            inputGain.Text = _source.Gain.HasValue ? _source.Gain.Value.ToString("0.###") : string.Empty;
            inputGrabTimeoutMs.Text = (_source.GrabTimeoutMs <= 0 ? 3000 : _source.GrabTimeoutMs).ToString();
            SetSelectValue(selectImageFormat, NormalizeImageFormatValue(_source.ImageFormat));
            inputJpegQuality.Text = (_source.JpegQuality <= 0 ? 80 : _source.JpegQuality).ToString();
            inputPreviewFps.Text = previewFps.ToString();
            checkSaveImageEnabled.Checked = _source.Id <= 0 || _source.SaveImageEnabled;
            inputSaveImageDirectory.Text = string.IsNullOrWhiteSpace(_source.SaveImageDirectory) ? DefaultSaveImageDirectory : _source.SaveImageDirectory;
            inputRemark.Text = _source.Remark ?? string.Empty;
            ApplyImageFormatVisibility();
        }

        public Result<CameraConfigEntity> BuildEntity()
        {
            var cameraCode = NormalizeText(inputCameraCode.Text);
            if (string.IsNullOrWhiteSpace(cameraCode))
            {
                return Result<CameraConfigEntity>.Fail(1, "相机编码不能为空", ResultSource.UI);
            }

            var deviceIndex = (int)Math.Max(0, inputDeviceIndex.Value);

            int width;
            if (!TryParseInt(GetSelectValue(selectWidth), out width) || width <= 0)
            {
                return Result<CameraConfigEntity>.Fail(3, "请从列表选择相机宽度", ResultSource.UI);
            }

            int height;
            if (!TryParseInt(GetSelectValue(selectHeight), out height) || height <= 0)
            {
                return Result<CameraConfigEntity>.Fail(4, "请从列表选择相机高度", ResultSource.UI);
            }

            if (!ResolutionOptions.Any(x => x.Width == width && x.Height == height))
            {
                return Result<CameraConfigEntity>.Fail(4, "请选择支持的常用分辨率", ResultSource.UI);
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

            var imageFormat = NormalizeImageFormatValue(GetSelectValue(selectImageFormat));
            if (string.IsNullOrWhiteSpace(imageFormat))
            {
                imageFormat = CameraImageFormat.JPEG.ToString();
            }

            int jpegQuality = _source == null || _source.JpegQuality <= 0 ? 80 : _source.JpegQuality;
            if (string.Equals(imageFormat, CameraImageFormat.JPEG.ToString(), StringComparison.OrdinalIgnoreCase) &&
                (!TryParseInt(inputJpegQuality.Text, out jpegQuality) || jpegQuality < 1 || jpegQuality > 100))
            {
                return Result<CameraConfigEntity>.Fail(7, "JPEG 质量必须在 1 到 100 之间", ResultSource.UI);
            }

            int previewFps;
            if (!TryParseInt(inputPreviewFps.Text, out previewFps) || previewFps <= 0)
            {
                return Result<CameraConfigEntity>.Fail(8, "预览 FPS 必须为大于 0 的整数", ResultSource.UI);
            }

            if (previewFps > fps)
            {
                return Result<CameraConfigEntity>.Fail(9, "预览 FPS 不能高于相机 FPS", ResultSource.UI);
            }

            var now = DateTime.Now;
            var pixelFormat = NormalizePixelFormat(inputPixelFormat.Text);
            var saveDirectory = NormalizeText(inputSaveImageDirectory.Text);
            if (string.IsNullOrWhiteSpace(saveDirectory))
            {
                saveDirectory = DefaultSaveImageDirectory;
            }

            var entity = new CameraConfigEntity
            {
                Id = _source == null ? 0 : _source.Id,
                CameraCode = cameraCode,
                CameraName = NormalizeText(inputCameraName.Text),
                DriverType = NormalizeDriverTypeValue(GetSelectValue(selectDriverType)),
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
                ImageFormat = imageFormat,
                JpegQuality = jpegQuality,
                PreviewFps = previewFps,
                SaveImageEnabled = checkSaveImageEnabled.Checked,
                SaveImageDirectory = saveDirectory,
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
                entity.DriverType = CameraDriverType.Usb.ToString();
            }

            return Result<CameraConfigEntity>.OkItem(entity, "相机配置输入有效", ResultSource.UI);
        }

        private void InitializeSelections()
        {
            selectDriverType.Items.Clear();
            selectDriverType.Items.AddRange(Enum.GetNames(typeof(CameraDriverType)).Select(x => (object)x).ToArray());

            selectImageFormat.Items.Clear();
            selectImageFormat.Items.AddRange(Enum.GetNames(typeof(CameraImageFormat)).Select(x => (object)x).ToArray());

            selectWidth.Items.Clear();
            selectWidth.Items.AddRange(ResolutionOptions
                .Select(x => x.Width)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => (object)x.ToString())
                .ToArray());
        }

        private void BindControlEvents()
        {
            selectWidth.SelectedValueChanged += (s, e) => SelectWidthChanged();
            selectHeight.SelectedValueChanged += (s, e) => SelectHeightChanged();
            selectImageFormat.SelectedValueChanged += (s, e) => ApplyImageFormatVisibility();
            inputFps.TextChanged += (s, e) => ClampPreviewFpsToCameraFps();
            inputPreviewFps.Leave += (s, e) => ClampPreviewFpsToCameraFps();
        }

        private static CameraConfigEntity CreateDefaultEntity()
        {
            var now = DateTime.Now;
            return new CameraConfigEntity
            {
                CameraCode = "USB_CAMERA_0",
                CameraName = "USB相机0",
                DriverType = CameraDriverType.Usb.ToString(),
                IsEnabled = true,
                DeviceIndex = 0,
                FriendlyName = "OpenCV Camera 0",
                Width = 1920,
                Height = 1080,
                Fps = 30,
                PixelFormat = "MJPG",
                GrabTimeoutMs = 3000,
                ImageFormat = CameraImageFormat.JPEG.ToString(),
                JpegQuality = 80,
                PreviewFps = 30,
                SaveImageEnabled = true,
                SaveImageDirectory = DefaultSaveImageDirectory,
                Remark = "OpenCV DSHOW USB 相机",
                CreateTime = now,
                UpdateTime = now
            };
        }

        private void SetResolution(int width, int height)
        {
            var option = FindResolution(width, height) ?? FindResolution(1920, 1080) ?? ResolutionOptions[0];

            _updatingResolution = true;
            try
            {
                SetSelectValue(selectWidth, option.Width.ToString(), false);
                BindHeightOptions(option.Width, option.Height);
            }
            finally
            {
                _updatingResolution = false;
            }
        }

        private void SelectWidthChanged()
        {
            if (_updatingResolution)
            {
                return;
            }

            int width;
            if (!TryParseInt(GetSelectValue(selectWidth), out width))
            {
                return;
            }

            int currentHeight;
            TryParseInt(GetSelectValue(selectHeight), out currentHeight);
            var height = ResolutionOptions.Any(x => x.Width == width && x.Height == currentHeight)
                ? currentHeight
                : ResolutionOptions.Where(x => x.Width == width).OrderBy(x => x.Height).Select(x => x.Height).FirstOrDefault();

            BindHeightOptions(width, height);
        }

        private void SelectHeightChanged()
        {
            if (_updatingResolution)
            {
                return;
            }

            int width;
            int height;
            if (!TryParseInt(GetSelectValue(selectWidth), out width) || !TryParseInt(GetSelectValue(selectHeight), out height))
            {
                return;
            }

            if (ResolutionOptions.Any(x => x.Width == width && x.Height == height))
            {
                return;
            }

            var option = ResolutionOptions.FirstOrDefault(x => x.Height == height);
            if (option != null)
            {
                SetResolution(option.Width, option.Height);
            }
        }

        private void BindHeightOptions(int width, int preferredHeight)
        {
            _updatingResolution = true;
            try
            {
                var heights = ResolutionOptions
                    .Where(x => x.Width == width)
                    .OrderBy(x => x.Height)
                    .Select(x => x.Height)
                    .Distinct()
                    .ToList();

                if (heights.Count == 0)
                {
                    heights = ResolutionOptions.Select(x => x.Height).Distinct().OrderBy(x => x).ToList();
                }

                if (!heights.Contains(preferredHeight))
                {
                    preferredHeight = heights[0];
                }

                selectHeight.Items.Clear();
                selectHeight.Items.AddRange(heights.Select(x => (object)x.ToString()).ToArray());
                SetSelectValue(selectHeight, preferredHeight.ToString(), false);
            }
            finally
            {
                _updatingResolution = false;
            }
        }

        private void ApplyImageFormatVisibility()
        {
            var isJpeg = string.Equals(
                NormalizeImageFormatValue(GetSelectValue(selectImageFormat)),
                CameraImageFormat.JPEG.ToString(),
                StringComparison.OrdinalIgnoreCase);

            labelJpegQuality.Visible = isJpeg;
            inputJpegQuality.Visible = isJpeg;
        }

        private void ClampPreviewFpsToCameraFps()
        {
            int fps;
            int previewFps;
            if (!TryParseInt(inputFps.Text, out fps) ||
                !TryParseInt(inputPreviewFps.Text, out previewFps) ||
                fps <= 0 ||
                previewFps <= fps)
            {
                return;
            }

            inputPreviewFps.Text = fps.ToString();
        }

        private static CameraResolutionOption FindResolution(int width, int height)
        {
            return ResolutionOptions.FirstOrDefault(x => x.Width == width && x.Height == height);
        }

        private static void SetSelectValue(Select select, string value)
        {
            SetSelectValue(select, value, true);
        }

        private static void SetSelectValue(Select select, string value, bool allowAppend)
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

                if (!exists && allowAppend)
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

        private static string NormalizeDriverTypeValue(string value)
        {
            var text = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                return CameraDriverType.Usb.ToString();
            }

            return text;
        }

        private static string NormalizeImageFormatValue(string value)
        {
            var text = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                return CameraImageFormat.JPEG.ToString();
            }

            if (string.Equals(text, "JPG", StringComparison.OrdinalIgnoreCase))
            {
                return CameraImageFormat.JPEG.ToString();
            }

            return text.ToUpperInvariant();
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

        private sealed class CameraResolutionOption
        {
            public CameraResolutionOption(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public int Width { get; private set; }

            public int Height { get; private set; }
        }
    }
}
