using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 主界面背景纹理控件。
    /// 使用 16x16 平铺纹理，效果参考 WPF ShellTextureBrush。
    /// </summary>
    public partial class TextureBackgroundControl : Control
    {
        private bool _isDarkMode;
        private Bitmap _textureTile;

        public TextureBackgroundControl()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);

            Dock = DockStyle.Fill;
            _isDarkMode = false;

            RebuildTextureTile();
        }

        public bool IsDarkMode
        {
            get { return _isDarkMode; }
        }

        public void SetTheme(bool isDarkMode)
        {
            if (_isDarkMode == isDarkMode && _textureTile != null)
            {
                return;
            }

            _isDarkMode = isDarkMode;
            RebuildTextureTile();
            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeTextureTile();
            base.OnHandleDestroyed(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            DrawBackground(g, rect);
            DrawTexture(g, rect);
        }

        private void DrawBackground(Graphics g, Rectangle rect)
        {
            using (var brush = new SolidBrush(GetBackgroundColor()))
            {
                g.FillRectangle(brush, rect);
            }
        }

        private void DrawTexture(Graphics g, Rectangle rect)
        {
            if (_textureTile == null)
            {
                RebuildTextureTile();
            }

            if (_textureTile == null)
            {
                return;
            }

            using (var brush = new TextureBrush(_textureTile, WrapMode.Tile))
            {
                g.FillRectangle(brush, rect);
            }
        }

        private void RebuildTextureTile()
        {
            DisposeTextureTile();
            _textureTile = CreateTextureTile();
        }

        private void DisposeTextureTile()
        {
            if (_textureTile != null)
            {
                _textureTile.Dispose();
                _textureTile = null;
            }
        }

        private Bitmap CreateTextureTile()
        {
            var bitmap = new Bitmap(16, 16);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.None;
                g.PixelOffsetMode = PixelOffsetMode.None;

                using (var fillBrush = new SolidBrush(GetTileFillColor()))
                {
                    g.FillRectangle(fillBrush, 0, 0, 16, 16);
                }

                using (var topBorderPen = new Pen(GetStrongShadowColor(), 1F))
                {
                    g.DrawLine(topBorderPen, 0, 0, 15, 0);
                }

                using (var leftBorderPen = new Pen(GetStrongShadowColor(), 1F))
                {
                    g.DrawLine(leftBorderPen, 0, 0, 0, 15);
                }

                using (var innerHorizontalPen = new Pen(GetSoftHighlightColor(), 1F))
                {
                    g.DrawLine(innerHorizontalPen, 0, 10, 15, 10);
                }

                using (var innerVerticalPen = new Pen(GetSoftHighlightColor(), 1F))
                {
                    g.DrawLine(innerVerticalPen, 10, 0, 10, 15);
                }

                //using (var farRightPen = new Pen(GetWeakShadowColor(), 1F))
                //{
                //    g.DrawLine(farRightPen, 15, 0, 15, 15);
                //}

                //using (var bottomPen = new Pen(GetWeakShadowColor(), 1F))
                //{
                //    g.DrawLine(bottomPen, 0, 15, 15, 15);
                //}
            }

            return bitmap;
        }

        private Color GetBackgroundColor()
        {
            return _isDarkMode
                ? Color.FromArgb(31, 31, 31)
                : Color.FromArgb(245, 247, 250);
        }

        private Color GetTileFillColor()
        {
            return _isDarkMode
                ? Color.FromArgb(10, 255, 255, 255)
                : Color.FromArgb(16, 255, 255, 255);
        }

        private Color GetStrongShadowColor()
        {
            return _isDarkMode
                ? Color.FromArgb(28, 0, 0, 0)
                : Color.FromArgb(24, 0, 0, 0);
        }

        private Color GetWeakShadowColor()
        {
            return _isDarkMode
                ? Color.FromArgb(18, 0, 0, 0)
                : Color.FromArgb(12, 0, 0, 0);
        }

        private Color GetSoftHighlightColor()
        {
            return _isDarkMode
                ? Color.FromArgb(10, 255, 255, 255)
                : Color.FromArgb(12, 255, 255, 255);
        }
    }
}