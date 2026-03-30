using System;
using System.Drawing;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 主界面背景纹理控件。
    /// 绘制浅色/深色网格背景，供主窗体统一复用。
    /// </summary>
    public partial class TextureBackgroundControl : Control
    {
        private bool _isDarkMode;

        public TextureBackgroundControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);

            //BackColor = Color.Transparent;
            Dock = DockStyle.Fill;
            _isDarkMode = false;
        }

        public bool IsDarkMode
        {
            get { return _isDarkMode; }
        }

        public void SetTheme(bool isDarkMode)
        {
            if (_isDarkMode == isDarkMode)
            {
                return;
            }

            _isDarkMode = isDarkMode;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            var rect = ClientRectangle;

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            DrawBackground(g, rect);
            DrawGrid(g, rect);
        }

        private void DrawBackground(Graphics g, Rectangle rect)
        {
            using (var brush = new SolidBrush(_isDarkMode
                ? Color.FromArgb(31, 31, 31)
                : Color.FromArgb(245, 247, 250)))
            {
                g.FillRectangle(brush, rect);
            }
        }

        private void DrawGrid(Graphics g, Rectangle rect)
        {
            var minorColor = _isDarkMode
                ? Color.FromArgb(40, 40, 40)
                : Color.FromArgb(228, 232, 238);

            var majorColor = _isDarkMode
                ? Color.FromArgb(52, 52, 52)
                : Color.FromArgb(214, 220, 228);

            const int minorStep = 16;
            const int majorStep = 80;

            using (var minorPen = new Pen(minorColor, 1F))
            using (var majorPen = new Pen(majorColor, 1F))
            {
                for (var x = 0; x < rect.Width; x += minorStep)
                {
                    g.DrawLine(minorPen, x, 0, x, rect.Height);
                }

                for (var y = 0; y < rect.Height; y += minorStep)
                {
                    g.DrawLine(minorPen, 0, y, rect.Width, y);
                }

                for (var x = 0; x < rect.Width; x += majorStep)
                {
                    g.DrawLine(majorPen, x, 0, x, rect.Height);
                }

                for (var y = 0; y < rect.Height; y += majorStep)
                {
                    g.DrawLine(majorPen, 0, y, rect.Width, y);
                }
            }
        }
    }
}