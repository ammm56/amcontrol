using AM.Model.Camera;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AMControlWinF.Views.Common
{
    /// <summary>
    /// 相机图像预览控件。
    /// 只负责图像显示、缩放和平移，不负责相机打开、取图或 SDK 调用。
    /// </summary>
    public partial class CameraImagePreviewControl : UserControl
    {
        private const double MinPreviewZoom = 0.05D;
        private const double MaxPreviewZoom = 16D;
        private const double PreviewZoomStep = 1.2D;

        private Bitmap _previewBitmap;
        private Size _sourceSize;
        private double _previewZoom = 1D;
        private double _imageLeft;
        private double _imageTop;
        private bool _isFitMode = true;
        private bool _isPanning;
        private Point _panLastPoint;

        public CameraImagePreviewControl()
        {
            InitializeComponent();
            BindEvents();
        }

        public bool HasImage
        {
            get { return picturePreview.Image != null; }
        }

        public void SetTitle(string title)
        {
            labelTitle.Text = string.IsNullOrWhiteSpace(title) ? "实时预览" : title;
        }

        public void SetSummary(string summary)
        {
            labelSummary.Text = string.IsNullOrWhiteSpace(summary) ? "暂无图像" : summary;
        }

        public void ShowFrame(CameraFrame frame)
        {
            if (frame == null)
            {
                return;
            }

            if (!frame.HasEncodedImage)
            {
                if (frame.HasBgr24Image)
                {
                    ShowPreviewFrame(new CameraPreviewFrame
                    {
                        CameraCode = frame.CameraCode,
                        FrameId = frame.FrameId,
                        Timestamp = frame.Timestamp,
                        Width = frame.Width,
                        Height = frame.Height,
                        Stride = checked(frame.Width * 3),
                        PixelFormat = "BGR24",
                        BgrBytes = frame.Bgr24Bytes
                    });
                }

                return;
            }

            using (var stream = new MemoryStream(frame.EncodedBytes))
            using (var image = Image.FromStream(stream))
            {
                var cloned = new Bitmap(image);
                DisposePreviewBitmap();
                var sourceChanged = _sourceSize != cloned.Size;
                _sourceSize = cloned.Size;
                var old = picturePreview.Image;
                picturePreview.Image = cloned;
                if (old != null)
                {
                    old.Dispose();
                }

                RefreshImageLayout(sourceChanged);
            }

            labelSummary.Text = string.Format(
                "{0}x{1}  {2}  {3:N0} bytes  {4:HH:mm:ss.fff}",
                frame.Width,
                frame.Height,
                string.IsNullOrWhiteSpace(frame.MediaType) ? frame.PixelFormat : frame.MediaType,
                frame.EncodedBytesLength,
                frame.Timestamp);
        }

        public void ShowPreviewFrame(CameraPreviewFrame frame)
        {
            if (frame == null || frame.BgrBytes == null || frame.BgrBytes.Length == 0)
            {
                return;
            }

            EnsurePreviewBitmap(frame.Width, frame.Height);
            CopyBgr24ToBitmap(frame, _previewBitmap);
            var sourceSize = new Size(frame.Width, frame.Height);
            var sourceChanged = _sourceSize != sourceSize;
            _sourceSize = sourceSize;

            if (!ReferenceEquals(picturePreview.Image, _previewBitmap))
            {
                var old = picturePreview.Image;
                picturePreview.Image = _previewBitmap;
                if (old != null)
                {
                    old.Dispose();
                }
            }

            RefreshImageLayout(sourceChanged);
            picturePreview.Invalidate();

            labelSummary.Text = string.Format(
                "{0}x{1}  {2}  {3:N0} bytes  {4:HH:mm:ss.fff}",
                frame.Width,
                frame.Height,
                frame.PixelFormat,
                frame.BytesLength,
                frame.Timestamp);
        }

        public void ClearImage(string summary)
        {
            DisposePictureImage();
            DisposePreviewBitmap();

            _sourceSize = Size.Empty;
            _previewZoom = 1D;
            _imageLeft = 0D;
            _imageTop = 0D;
            _isFitMode = true;
            StopPan(null);
            SetSummary(summary);
        }

        public void FitToViewport()
        {
            FitPreviewToViewport();
        }

        private void BindEvents()
        {
            panelViewport.TabStop = true;
            panelViewport.MouseEnter += (s, e) => panelViewport.Focus();
            panelViewport.MouseWheel += PreviewViewport_MouseWheel;
            panelViewport.MouseDown += PreviewViewport_MouseDown;
            panelViewport.MouseMove += PreviewViewport_MouseMove;
            panelViewport.MouseUp += PreviewViewport_MouseUp;
            panelViewport.MouseLeave += PreviewViewport_MouseLeave;
            panelViewport.Resize += (s, e) => RefreshViewportLayout();

            picturePreview.MouseEnter += (s, e) => panelViewport.Focus();
            picturePreview.MouseWheel += PreviewViewport_MouseWheel;
            picturePreview.MouseDown += PreviewViewport_MouseDown;
            picturePreview.MouseMove += PreviewViewport_MouseMove;
            picturePreview.MouseUp += PreviewViewport_MouseUp;
            picturePreview.MouseLeave += PreviewViewport_MouseLeave;
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

        private void PreviewViewport_MouseWheel(object sender, MouseEventArgs e)
        {
            if (picturePreview.Image == null || e == null)
            {
                return;
            }

            var anchor = ReferenceEquals(sender, picturePreview)
                ? panelViewport.PointToClient(picturePreview.PointToScreen(e.Location))
                : e.Location;
            ZoomAtPoint(e.Delta > 0 ? PreviewZoomStep : 1D / PreviewZoomStep, anchor);
        }

        private void PreviewViewport_MouseDown(object sender, MouseEventArgs e)
        {
            if (e == null || e.Button != MouseButtons.Right || picturePreview.Image == null)
            {
                return;
            }

            _isPanning = true;
            _isFitMode = false;
            _panLastPoint = ToViewportPoint(sender, e.Location);
            panelViewport.Cursor = Cursors.SizeAll;
            picturePreview.Cursor = Cursors.SizeAll;

            var source = sender as Control;
            if (source != null)
            {
                source.Capture = true;
            }
        }

        private void PreviewViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isPanning || e == null || picturePreview.Image == null)
            {
                return;
            }

            var current = ToViewportPoint(sender, e.Location);
            _imageLeft += current.X - _panLastPoint.X;
            _imageTop += current.Y - _panLastPoint.Y;
            _panLastPoint = current;
            ApplyImageLayout();
        }

        private void PreviewViewport_MouseUp(object sender, MouseEventArgs e)
        {
            if (e != null && e.Button != MouseButtons.Right)
            {
                return;
            }

            StopPan(sender as Control);
        }

        private void PreviewViewport_MouseLeave(object sender, EventArgs e)
        {
            var source = sender as Control;
            if (source != null && source.Capture)
            {
                return;
            }

            if ((Control.MouseButtons & MouseButtons.Right) != MouseButtons.Right)
            {
                StopPan(source);
            }
        }

        private void StopPan(Control capturedControl)
        {
            if (!_isPanning)
            {
                return;
            }

            _isPanning = false;
            if (capturedControl != null)
            {
                capturedControl.Capture = false;
            }

            panelViewport.Capture = false;
            picturePreview.Capture = false;
            panelViewport.Cursor = Cursors.Default;
            picturePreview.Cursor = Cursors.Default;
        }

        private Point ToViewportPoint(object sender, Point location)
        {
            return ReferenceEquals(sender, picturePreview)
                ? panelViewport.PointToClient(picturePreview.PointToScreen(location))
                : location;
        }

        private void ZoomAtPoint(double factor, Point anchor)
        {
            if (picturePreview.Image == null || factor <= 0D)
            {
                return;
            }

            var oldZoom = _previewZoom <= 0D ? CalculateFitZoom() : _previewZoom;
            var imageX = (anchor.X - _imageLeft) / oldZoom;
            var imageY = (anchor.Y - _imageTop) / oldZoom;

            _previewZoom = ClampZoom(oldZoom * factor);
            _imageLeft = anchor.X - imageX * _previewZoom;
            _imageTop = anchor.Y - imageY * _previewZoom;
            _isFitMode = false;
            ApplyImageLayout();
        }

        private void RefreshViewportLayout()
        {
            if (picturePreview.Image == null)
            {
                return;
            }

            if (_isFitMode)
            {
                FitPreviewToViewport();
            }
            else
            {
                ApplyImageLayout();
            }
        }

        private void RefreshImageLayout(bool sourceChanged)
        {
            if (picturePreview.Image == null)
            {
                return;
            }

            if (_isFitMode || sourceChanged)
            {
                FitPreviewToViewport();
            }
            else
            {
                ApplyImageLayout();
            }
        }

        private void FitPreviewToViewport()
        {
            if (picturePreview.Image == null)
            {
                return;
            }

            _previewZoom = CalculateFitZoom();
            _isFitMode = true;
            var clientSize = panelViewport.ClientSize;
            var width = Math.Max(1, (int)Math.Round(picturePreview.Image.Width * _previewZoom));
            var height = Math.Max(1, (int)Math.Round(picturePreview.Image.Height * _previewZoom));
            _imageLeft = (clientSize.Width - width) / 2D;
            _imageTop = (clientSize.Height - height) / 2D;
            ApplyImageLayout();
        }

        private double CalculateFitZoom()
        {
            if (picturePreview.Image == null)
            {
                return 1D;
            }

            var clientSize = panelViewport.ClientSize;
            var widthRatio = (double)Math.Max(1, clientSize.Width) / picturePreview.Image.Width;
            var heightRatio = (double)Math.Max(1, clientSize.Height) / picturePreview.Image.Height;
            return ClampZoom(Math.Min(widthRatio, heightRatio));
        }

        private void ApplyImageLayout()
        {
            if (picturePreview.Image == null)
            {
                return;
            }

            var width = Math.Max(1, (int)Math.Round(picturePreview.Image.Width * _previewZoom));
            var height = Math.Max(1, (int)Math.Round(picturePreview.Image.Height * _previewZoom));
            ClampImageOffset(width, height);

            picturePreview.Bounds = new Rectangle(
                (int)Math.Round(_imageLeft),
                (int)Math.Round(_imageTop),
                width,
                height);
            picturePreview.Invalidate();
        }

        private void ClampImageOffset(int width, int height)
        {
            var clientSize = panelViewport.ClientSize;
            if (width <= clientSize.Width)
            {
                _imageLeft = (clientSize.Width - width) / 2D;
            }
            else
            {
                _imageLeft = Math.Min(0D, Math.Max(clientSize.Width - width, _imageLeft));
            }

            if (height <= clientSize.Height)
            {
                _imageTop = (clientSize.Height - height) / 2D;
            }
            else
            {
                _imageTop = Math.Min(0D, Math.Max(clientSize.Height - height, _imageTop));
            }
        }

        private static double ClampZoom(double zoom)
        {
            if (zoom < MinPreviewZoom)
            {
                return MinPreviewZoom;
            }

            return zoom > MaxPreviewZoom ? MaxPreviewZoom : zoom;
        }

        private void DisposePictureImage()
        {
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

            if (ReferenceEquals(picturePreview.Image, _previewBitmap))
            {
                picturePreview.Image = null;
            }

            _previewBitmap.Dispose();
            _previewBitmap = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                DisposePictureImage();
                DisposePreviewBitmap();
            }

            base.Dispose(disposing);
        }
    }
}
