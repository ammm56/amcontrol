using AM.CameraService.OpenCv;
using AM.DBService.Services.Camera;
using AM.Model.Camera;
using AM.Model.Common;
using AM.Model.Entity.Device;
using AM.Model.Interfaces.Camera;
using AM.Model.Vision;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// 相机配置页面。
    /// 布局与 PLC 配置页面保持一致：顶部工具栏、统计卡片、左侧配置表、右侧实时预览。
    /// </summary>
    public partial class CameraConfigManagementPage : UserControl
    {
        private const string DefaultSaveImageDirectory = "Images\\Camera";

        private readonly CameraConfigCrudService _crudService;
        private readonly ICameraRuntimeService _cameraRuntime;
        private readonly Timer _previewTimer;
        private readonly HashSet<string> _openedCameraCodes;

        private AntList<CameraTableRow> _tableRows;
        private List<CameraConfigEntity> _allConfigs;
        private List<CameraConfigEntity> _filteredConfigs;
        private CameraConfigEntity _selectedCamera;
        private CameraFrame _lastFrame;
        private CameraPreviewFrame _lastPreviewFrame;
        private Bitmap _previewBitmap;
        private bool _isFirstLoad;
        private bool _isBusy;
        private bool _isPreviewRunning;
        private bool _isPreviewTickBusy;

        public CameraConfigManagementPage()
        {
            InitializeComponent();

            _crudService = new CameraConfigCrudService();
            _cameraRuntime = new OpenCvCameraRuntimeService();
            _previewTimer = new Timer();
            _openedCameraCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            _tableRows = new AntList<CameraTableRow>();
            _allConfigs = new List<CameraConfigEntity>();
            _filteredConfigs = new List<CameraConfigEntity>();

            InitializeTable();
            BindEvents();
            RefreshActionButtons();
        }

        private void DisposeRuntimeResources()
        {
            if (_previewTimer != null)
            {
                _previewTimer.Stop();
                _previewTimer.Dispose();
            }

            if (_cameraRuntime != null)
            {
                _cameraRuntime.Dispose();
            }

            DisposePictureImage();
            DisposePreviewBitmap();
        }

        private void InitializeTable()
        {
            tableCameras.Columns = new ColumnCollection()
            {
                new Column("DisplayName", "显示名", ColumnAlign.Left)
                {
                    Width = "120",
                    Fixed = true
                },
                new Column("DriverType", "驱动", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("DeviceIndexText", "索引", ColumnAlign.Center)
                {
                    Width = "60"
                },
                new Column("ResolutionText", "分辨率", ColumnAlign.Center)
                {
                    Width = "120"
                },
                new Column("RuntimeTag", "运行态", ColumnAlign.Center)
                {
                    Width = "80"
                },
                new Column("CameraCode", "编码", ColumnAlign.Left)
                {
                    Width = "130"
                }
            };
        }

        private void BindEvents()
        {
            Load += CameraConfigManagementPage_Load;
            inputSearch.TextChanged += InputSearch_TextChanged;
            inputSearch.KeyUp += InputSearch_KeyUp;
            inputSearch.Leave += InputSearch_Leave;
            tableCameras.CellClick += TableCameras_CellClick;

            buttonRefresh.Click += async (s, e) => await ReloadAsync(GetSelectedCameraCode());
            buttonAddCamera.Click += async (s, e) => await EditCameraAsync(null);
            buttonEditCamera.Click += async (s, e) => await EditCameraAsync(_selectedCamera);
            buttonDeleteCamera.Click += async (s, e) => await DeleteSelectedCameraAsync();
            buttonOpenCamera.Click += async (s, e) => await ToggleOpenCameraAsync();
            buttonCameraSettings.Click += async (s, e) => await ShowCameraSettingsAsync();
            buttonTogglePreview.Click += async (s, e) => await TogglePreviewAsync();
            buttonGrabFrame.Click += async (s, e) => await GrabFrameAsync(false);

            _previewTimer.Tick += async (s, e) => await PreviewTimerTickAsync();
        }

        private async void CameraConfigManagementPage_Load(object sender, EventArgs e)
        {
            if (_isFirstLoad)
            {
                return;
            }

            _isFirstLoad = true;
            await ReloadAsync(null);
        }

        private void InputSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshSearchFilter();
        }

        private void InputSearch_KeyUp(object sender, KeyEventArgs e)
        {
            RefreshSearchFilter();
        }

        private void InputSearch_Leave(object sender, EventArgs e)
        {
            RefreshSearchFilter();
        }

        private void RefreshSearchFilter()
        {
            ApplyFilter();
            SyncSelectedCameraWithFilter();
            RebindTable();
            RefreshPageState();
        }

        private async Task ReloadAsync(string preferredCameraCode)
        {
            if (_isBusy)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                await ReloadCoreAsync(preferredCameraCode);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ReloadCoreAsync(string preferredCameraCode)
        {
            var result = await Task.Run(() => _crudService.QueryAll());
            if (!result.Success)
            {
                _allConfigs = new List<CameraConfigEntity>();
                ApplyFilter();
                RebindTable();
                RefreshPageState();
                labelRuntimeSummary.Text = "相机配置加载失败：" + result.Message;
                return;
            }

            _allConfigs = result.Items.ToList();
            ApplyFilter();

            if (!string.IsNullOrWhiteSpace(preferredCameraCode))
            {
                _selectedCamera = _filteredConfigs.FirstOrDefault(x =>
                    string.Equals(x.CameraCode, preferredCameraCode, StringComparison.OrdinalIgnoreCase));
            }

            if (_selectedCamera == null && _filteredConfigs.Count > 0)
            {
                _selectedCamera = _filteredConfigs[0];
            }

            if (_selectedCamera != null &&
                !_filteredConfigs.Any(x => string.Equals(x.CameraCode, _selectedCamera.CameraCode, StringComparison.OrdinalIgnoreCase)))
            {
                _selectedCamera = _filteredConfigs.FirstOrDefault();
            }

            RebindTable();
            RefreshPageState();
            labelRuntimeSummary.Text = "相机配置已加载，共 " + _allConfigs.Count + " 项";
        }

        private void ApplyFilter()
        {
            var keyword = NormalizeText(inputSearch.Text);
            if (string.IsNullOrWhiteSpace(keyword))
            {
                _filteredConfigs = _allConfigs
                    .OrderBy(x => x.Id)
                    .ToList();
                return;
            }

            _filteredConfigs = _allConfigs
                .Where(x =>
                    ContainsText(x.CameraName, keyword) ||
                    ContainsText(x.CameraCode, keyword) ||
                    ContainsText(x.FriendlyName, keyword) ||
                    ContainsText(x.DriverType, keyword))
                .OrderBy(x => x.Id)
                .ToList();
        }

        private void SyncSelectedCameraWithFilter()
        {
            if (_selectedCamera != null &&
                _filteredConfigs.Any(x => string.Equals(x.CameraCode, _selectedCamera.CameraCode, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            _selectedCamera = _filteredConfigs.FirstOrDefault();
        }

        private void RebindTable()
        {
            _tableRows = new AntList<CameraTableRow>();

            foreach (var item in _filteredConfigs)
            {
                _tableRows.Add(new CameraTableRow
                {
                    Item = item,
                    CameraCode = new CellText(item.CameraCode ?? string.Empty),
                    DisplayName = new CellText(string.IsNullOrWhiteSpace(item.CameraName) ? item.CameraCode : item.CameraName),
                    DriverType = new CellText(item.DriverType ?? string.Empty),
                    DeviceIndexText = new CellText(item.DeviceIndex.ToString()),
                    ResolutionText = new CellText(item.Width + "x" + item.Height + "@" + item.Fps),
                    RuntimeTag = new CellTag(
                        IsCameraOpened(item.CameraCode) ? "已连接" : "未连接",
                        IsCameraOpened(item.CameraCode) ? TTypeMini.Success : TTypeMini.Default)
                });
            }

            tableCameras.Binding(_tableRows);
        }

        private void TableCameras_CellClick(object sender, TableClickEventArgs e)
        {
            var row = e == null ? null : e.Record as CameraTableRow;
            if (row == null || row.Item == null)
            {
                return;
            }

            _selectedCamera = row.Item;
            RefreshPageState();
        }

        private async Task EditCameraAsync(CameraConfigEntity entity)
        {
            if (_isBusy)
            {
                return;
            }

            using (var dialog = new CameraConfigEditDialog())
            {
                dialog.IsCreateMode = entity == null;
                dialog.SetEntity(entity == null ? BuildDefaultCameraEntity() : CloneEntity(entity));

                if (dialog.ShowDialog(FindForm()) != DialogResult.OK || dialog.ResultEntity == null)
                {
                    return;
                }

                SetBusyState(true);
                try
                {
                    var saveResult = await Task.Run(() => _crudService.Save(dialog.ResultEntity));
                    if (!saveResult.Success)
                    {
                        PageDialogHelper.ShowWarn(this, "保存相机配置", saveResult.Message);
                        return;
                    }

                    await ReloadCoreAsync(dialog.ResultEntity.CameraCode);
                }
                finally
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task DeleteSelectedCameraAsync()
        {
            if (_isBusy || _selectedCamera == null)
            {
                return;
            }

            var cameraCode = _selectedCamera.CameraCode;
            var ok = PageDialogHelper.Confirm(
                this,
                "删除相机配置",
                "确定删除相机 " + GetCameraTitle(_selectedCamera) + " 吗？");

            if (!ok)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                await Task.Run(() => _cameraRuntime.Close(cameraCode));
                _openedCameraCodes.Remove(cameraCode);

                var deleteResult = await Task.Run(() => _crudService.DeleteByCode(cameraCode));
                if (!deleteResult.Success)
                {
                    PageDialogHelper.ShowWarn(this, "删除相机配置", deleteResult.Message);
                    return;
                }

                _selectedCamera = null;
                StopPreview();
                ClearPreviewImage();
                await ReloadCoreAsync(null);
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ToggleOpenCameraAsync()
        {
            if (_isBusy || _selectedCamera == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                if (IsCameraOpened(_selectedCamera.CameraCode))
                {
                    StopPreview();
                    var closeResult = await Task.Run(() => _cameraRuntime.Close(_selectedCamera.CameraCode));
                    if (closeResult.Success)
                    {
                        _openedCameraCodes.Remove(_selectedCamera.CameraCode);
                        labelRuntimeSummary.Text = closeResult.Message;
                    }
                    else
                    {
                        PageDialogHelper.ShowWarn(this, "关闭相机", closeResult.Message);
                    }
                }
                else
                {
                    var openResult = await Task.Run(() => _cameraRuntime.Open(_selectedCamera));
                    if (openResult.Success)
                    {
                        _openedCameraCodes.Add(_selectedCamera.CameraCode);
                        labelRuntimeSummary.Text = openResult.Message;
                    }
                    else
                    {
                        PageDialogHelper.ShowWarn(this, "打开相机", openResult.Message);
                    }
                }

                RebindTable();
                RefreshPageState();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ShowCameraSettingsAsync()
        {
            if (_isBusy || _selectedCamera == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var openResult = await EnsureSelectedCameraOpenAsync();
                if (!openResult.Success)
                {
                    PageDialogHelper.ShowWarn(this, "打开相机设置页", openResult.Message);
                    return;
                }

                var result = await Task.Run(() => _cameraRuntime.ShowSettings(_selectedCamera.CameraCode));
                if (!result.Success)
                {
                    PageDialogHelper.ShowWarn(this, "打开相机设置页", result.Message);
                    return;
                }

                labelRuntimeSummary.Text = result.Message;
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task TogglePreviewAsync()
        {
            if (_isBusy || _selectedCamera == null)
            {
                return;
            }

            if (_isPreviewRunning)
            {
                StopPreview();
                RefreshPageState();
                return;
            }

            SetBusyState(true);
            try
            {
                var openResult = await EnsureSelectedCameraOpenAsync();
                if (!openResult.Success)
                {
                    PageDialogHelper.ShowWarn(this, "开始预览", openResult.Message);
                    return;
                }

                var cameraFps = _selectedCamera.Fps <= 0 ? 30 : _selectedCamera.Fps;
                var previewFps = _selectedCamera.PreviewFps <= 0 ? cameraFps : Math.Min(_selectedCamera.PreviewFps, cameraFps);
                _previewTimer.Interval = Math.Max(15, 1000 / previewFps);
                _isPreviewRunning = true;
                _previewTimer.Start();
                labelRuntimeSummary.Text = "相机预览已开始：" + GetCameraTitle(_selectedCamera);
            }
            finally
            {
                SetBusyState(false);
                RefreshPageState();
            }
        }

        private async Task PreviewTimerTickAsync()
        {
            if (!_isPreviewRunning || _isPreviewTickBusy || _selectedCamera == null)
            {
                return;
            }

            _isPreviewTickBusy = true;
            try
            {
                await GrabPreviewFrameAsync();
            }
            finally
            {
                _isPreviewTickBusy = false;
            }
        }

        private async Task GrabPreviewFrameAsync()
        {
            if (_selectedCamera == null)
            {
                return;
            }

            try
            {
                if (!IsCameraOpened(_selectedCamera.CameraCode))
                {
                    var openResult = await EnsureSelectedCameraOpenAsync();
                    if (!openResult.Success)
                    {
                        labelRuntimeSummary.Text = openResult.Message;
                        return;
                    }
                }

                var previewSize = picturePreview.ClientSize;
                var maxWidth = Math.Max(1, previewSize.Width);
                var maxHeight = Math.Max(1, previewSize.Height);
                var result = await Task.Run(() => _cameraRuntime.GrabPreviewFrame(_selectedCamera.CameraCode, maxWidth, maxHeight));
                if (!result.Success || result.Item == null)
                {
                    labelRuntimeSummary.Text = result.Message;
                    return;
                }

                _lastPreviewFrame = result.Item;
                ShowPreviewFrame(_lastPreviewFrame);
            }
            catch (Exception ex)
            {
                labelRuntimeSummary.Text = "相机预览失败：" + ex.Message;
            }
        }

        private async Task GrabFrameAsync(bool silent)
        {
            if (_selectedCamera == null)
            {
                return;
            }

            if (!silent && _isBusy)
            {
                return;
            }

            if (!silent)
            {
                SetBusyState(true);
            }

            try
            {
                var openResult = await EnsureSelectedCameraOpenAsync();
                if (!openResult.Success)
                {
                    if (!silent)
                    {
                        PageDialogHelper.ShowWarn(this, "相机取图", openResult.Message);
                    }
                    else
                    {
                        labelRuntimeSummary.Text = openResult.Message;
                    }
                    return;
                }

                var result = await Task.Run(() => _cameraRuntime.GrabFrame(_selectedCamera.CameraCode));
                if (!result.Success || result.Item == null)
                {
                    if (!silent)
                    {
                        PageDialogHelper.ShowWarn(this, "相机取图", result.Message);
                    }
                    else
                    {
                        labelRuntimeSummary.Text = result.Message;
                    }
                    return;
                }

                _lastFrame = result.Item;
                ShowFrame(_lastFrame);

                var message = result.Message;
                if (!silent)
                {
                    var saveResult = SaveGrabFrame(_selectedCamera, _lastFrame);
                    if (!saveResult.Success)
                    {
                        PageDialogHelper.ShowWarn(this, "保存取图", saveResult.Message);
                    }
                    else if (!string.IsNullOrWhiteSpace(saveResult.Item))
                    {
                        message = message + "，已保存：" + saveResult.Item;
                    }
                }

                labelRuntimeSummary.Text = message;
            }
            finally
            {
                if (!silent)
                {
                    SetBusyState(false);
                }
            }
        }

        private async Task<Result> EnsureSelectedCameraOpenAsync()
        {
            if (_selectedCamera == null || string.IsNullOrWhiteSpace(_selectedCamera.CameraCode))
            {
                return Result.Fail(1, "请先选择相机", ResultSource.UI);
            }

            var state = await Task.Run(() => _cameraRuntime.IsOpen(_selectedCamera.CameraCode));
            if (state.Success && state.Item)
            {
                _openedCameraCodes.Add(_selectedCamera.CameraCode);
                return Result.Ok("相机已连接", ResultSource.Camera);
            }

            var openResult = await Task.Run(() => _cameraRuntime.Open(_selectedCamera));
            if (openResult.Success)
            {
                _openedCameraCodes.Add(_selectedCamera.CameraCode);
            }

            return openResult;
        }

        private void ShowFrame(CameraFrame frame)
        {
            if (frame == null || frame.EncodedBytes == null || frame.EncodedBytes.Length == 0)
            {
                return;
            }

            using (var stream = new MemoryStream(frame.EncodedBytes))
            using (var image = Image.FromStream(stream))
            {
                var cloned = new Bitmap(image);
                DisposePreviewBitmap();
                var old = picturePreview.Image;
                picturePreview.Image = cloned;
                if (old != null)
                {
                    old.Dispose();
                }
            }

            labelPreviewSummary.Text = string.Format(
                "{0}x{1}  {2}  {3:N0} bytes  {4:HH:mm:ss.fff}",
                frame.Width,
                frame.Height,
                string.IsNullOrWhiteSpace(frame.PixelFormat) ? frame.MediaType : frame.PixelFormat,
                frame.EncodedBytesLength,
                frame.Timestamp);
        }

        private void ShowPreviewFrame(CameraPreviewFrame frame)
        {
            if (frame == null || frame.BgrBytes == null || frame.BgrBytes.Length == 0)
            {
                return;
            }

            EnsurePreviewBitmap(frame.Width, frame.Height);
            CopyBgr24ToBitmap(frame, _previewBitmap);

            if (!ReferenceEquals(picturePreview.Image, _previewBitmap))
            {
                var old = picturePreview.Image;
                picturePreview.Image = _previewBitmap;
                if (old != null)
                {
                    old.Dispose();
                }
            }

            picturePreview.Invalidate();

            labelPreviewSummary.Text = string.Format(
                "{0}x{1}  {2}  {3:N0} bytes  {4:HH:mm:ss.fff}",
                frame.Width,
                frame.Height,
                frame.PixelFormat,
                frame.BytesLength,
                frame.Timestamp);
        }

        private void EnsurePreviewBitmap(int width, int height)
        {
            if (_previewBitmap != null &&
                _previewBitmap.Width == width &&
                _previewBitmap.Height == height)
            {
                return;
            }

            DisposePreviewBitmap();
            _previewBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        private static void CopyBgr24ToBitmap(CameraPreviewFrame frame, Bitmap bitmap)
        {
            var rect = new Rectangle(0, 0, frame.Width, frame.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            try
            {
                var sourceStride = frame.Stride;
                var targetStride = Math.Abs(data.Stride);
                var rowBytes = Math.Min(frame.Width * 3, Math.Min(sourceStride, targetStride));

                for (var row = 0; row < frame.Height; row++)
                {
                    var sourceOffset = row * sourceStride;
                    var targetOffset = data.Stride >= 0
                        ? row * data.Stride
                        : (frame.Height - 1 - row) * targetStride;
                    Marshal.Copy(frame.BgrBytes, sourceOffset, IntPtr.Add(data.Scan0, targetOffset), rowBytes);
                }
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private void ClearPreviewImage()
        {
            DisposePictureImage();
            DisposePreviewBitmap();

            _lastFrame = null;
            _lastPreviewFrame = null;
            labelPreviewSummary.Text = "暂无图像";
        }

        private void DisposePictureImage()
        {
            if (picturePreview == null)
            {
                return;
            }

            var old = picturePreview.Image;
            picturePreview.Image = null;
            if (old != null && !ReferenceEquals(old, _previewBitmap))
            {
                old.Dispose();
            }
        }

        private void DisposePreviewBitmap()
        {
            if (_previewBitmap == null)
            {
                return;
            }

            if (picturePreview != null && ReferenceEquals(picturePreview.Image, _previewBitmap))
            {
                picturePreview.Image = null;
            }

            _previewBitmap.Dispose();
            _previewBitmap = null;
        }

        private void StopPreview()
        {
            _previewTimer.Stop();
            _isPreviewRunning = false;
            _isPreviewTickBusy = false;
        }

        private Result<string> SaveGrabFrame(CameraConfigEntity camera, CameraFrame frame)
        {
            if (camera == null || !camera.SaveImageEnabled)
            {
                return Result<string>.OkItem(null, "未启用取图保存", ResultSource.UI);
            }

            if (frame == null || frame.EncodedBytes == null || frame.EncodedBytes.Length == 0)
            {
                return Result<string>.Fail(1, "没有可保存的取图数据", ResultSource.UI);
            }

            try
            {
                var directory = ResolveSaveDirectory(camera.SaveImageDirectory);
                Directory.CreateDirectory(directory);

                var fileName = string.Format(
                    "{0}_{1:yyyyMMdd_HHmmss_fff}_{2}{3}",
                    SanitizeFileName(camera.CameraCode),
                    frame.Timestamp,
                    string.IsNullOrWhiteSpace(frame.FrameId) ? Guid.NewGuid().ToString("N") : frame.FrameId,
                    ResolveImageExtension(frame.MediaType, camera.ImageFormat));

                var path = Path.Combine(directory, fileName);
                File.WriteAllBytes(path, frame.EncodedBytes);
                return Result<string>.OkItem(path, "取图已保存", ResultSource.UI);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(2, "取图保存失败：" + ex.Message, ResultSource.UI);
            }
        }

        private void RefreshPageState()
        {
            labelCameraTotalCount.Text = _allConfigs.Count.ToString();
            labelConnectedCount.Text = _openedCameraCodes.Count.ToString();
            labelSelectedCamera.Text = _selectedCamera == null ? "未选择" : GetCameraTitle(_selectedCamera);

            if (_selectedCamera == null)
            {
                labelPreviewSummary.Text = "未选择相机";
            }
            else if (!IsCameraOpened(_selectedCamera.CameraCode) && picturePreview.Image == null)
            {
                labelPreviewSummary.Text = "未打开相机";
            }

            RefreshActionButtons();
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;

            inputSearch.Enabled = !isBusy;
            buttonRefresh.Enabled = !isBusy;
            RefreshActionButtons();
        }

        private void RefreshActionButtons()
        {
            var hasCamera = _selectedCamera != null;
            var opened = hasCamera && IsCameraOpened(_selectedCamera.CameraCode);

            buttonAddCamera.Enabled = !_isBusy;
            buttonEditCamera.Enabled = !_isBusy && hasCamera;
            buttonDeleteCamera.Enabled = !_isBusy && hasCamera;

            buttonOpenCamera.Enabled = !_isBusy && hasCamera;
            buttonOpenCamera.Text = opened ? "关闭" : "打开";
            buttonOpenCamera.IconSvg = opened ? "CloseCircleOutlined" : "PlayCircleOutlined";

            buttonCameraSettings.Enabled = !_isBusy && hasCamera;
            buttonTogglePreview.Enabled = !_isBusy && hasCamera;
            buttonTogglePreview.Text = _isPreviewRunning ? "暂停" : "开始";
            buttonTogglePreview.IconSvg = _isPreviewRunning ? "PauseCircleOutlined" : "VideoCameraOutlined";
            buttonGrabFrame.Enabled = !_isBusy && hasCamera;
        }

        private CameraConfigEntity BuildDefaultCameraEntity()
        {
            var index = NextCameraIndex();
            var now = DateTime.Now;

            return new CameraConfigEntity
            {
                CameraCode = "USB_CAMERA_" + index,
                CameraName = "USB相机" + index,
                DriverType = CameraDriverType.Usb.ToString(),
                IsEnabled = true,
                DeviceIndex = index,
                FriendlyName = "OpenCV Camera " + index,
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

        private int NextCameraIndex()
        {
            var index = 0;
            while (_allConfigs.Any(x => string.Equals(x.CameraCode, "USB_CAMERA_" + index, StringComparison.OrdinalIgnoreCase)))
            {
                index++;
            }

            return index;
        }

        private string GetSelectedCameraCode()
        {
            return _selectedCamera == null ? null : _selectedCamera.CameraCode;
        }

        private bool IsCameraOpened(string cameraCode)
        {
            return !string.IsNullOrWhiteSpace(cameraCode) && _openedCameraCodes.Contains(cameraCode);
        }

        private static string ResolveSaveDirectory(string saveDirectory)
        {
            var directory = NormalizeText(saveDirectory);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = DefaultSaveImageDirectory;
            }

            return Path.IsPathRooted(directory)
                ? directory
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);
        }

        private static string ResolveImageExtension(string mediaType, string imageFormat)
        {
            var format = NormalizeText(imageFormat).ToUpperInvariant();
            if (string.Equals(mediaType, "image/png", StringComparison.OrdinalIgnoreCase) || format == "PNG")
            {
                return ".png";
            }

            if (string.Equals(mediaType, "image/bmp", StringComparison.OrdinalIgnoreCase) || format == "BMP")
            {
                return ".bmp";
            }

            return ".jpg";
        }

        private static string SanitizeFileName(string value)
        {
            var text = NormalizeText(value);
            if (string.IsNullOrWhiteSpace(text))
            {
                text = "CAMERA";
            }

            foreach (var invalid in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(invalid, '_');
            }

            return text;
        }

        private static bool ContainsText(string source, string keyword)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(keyword))
            {
                return false;
            }

            return source.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static string GetCameraTitle(CameraConfigEntity entity)
        {
            if (entity == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(entity.CameraName))
            {
                return entity.CameraName.Trim();
            }

            return entity.CameraCode ?? string.Empty;
        }

        private static CameraConfigEntity CloneEntity(CameraConfigEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CameraConfigEntity
            {
                Id = entity.Id,
                CameraCode = entity.CameraCode,
                CameraName = entity.CameraName,
                DriverType = entity.DriverType,
                IsEnabled = entity.IsEnabled,
                DeviceIndex = entity.DeviceIndex,
                DevicePath = entity.DevicePath,
                FriendlyName = entity.FriendlyName,
                Width = entity.Width,
                Height = entity.Height,
                Fps = entity.Fps,
                PixelFormat = entity.PixelFormat,
                Exposure = entity.Exposure,
                Gain = entity.Gain,
                GrabTimeoutMs = entity.GrabTimeoutMs,
                ImageFormat = entity.ImageFormat,
                JpegQuality = entity.JpegQuality,
                PreviewFps = entity.PreviewFps,
                SaveImageEnabled = entity.SaveImageEnabled,
                SaveImageDirectory = entity.SaveImageDirectory,
                Remark = entity.Remark,
                CreateTime = entity.CreateTime,
                UpdateTime = entity.UpdateTime
            };
        }

        private sealed class CameraTableRow
        {
            public CameraConfigEntity Item { get; set; }

            public CellText CameraCode { get; set; }

            public CellText DisplayName { get; set; }

            public CellText DriverType { get; set; }

            public CellText DeviceIndexText { get; set; }

            public CellText ResolutionText { get; set; }

            public CellTag RuntimeTag { get; set; }
        }
    }
}
