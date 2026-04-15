using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMControlWinF.Views.Home
{
    /// <summary>
    /// 首页总览看板占位页。
    /// 当前仅用于展示 2x2 图片轮播占位布局，后续真实数据确定后再替换内容区。
    /// </summary>
    public partial class HomeOverviewPage : UserControl
    {
        private readonly List<Image> _loadedImages;

        public HomeOverviewPage()
        {
            InitializeComponent();

            _loadedImages = new List<Image>();

            BindCarousels();

            Disposed += HomeOverviewPage_Disposed;
        }

        private void BindCarousels()
        {
            BindCarouselImages(carouselAreaA, "a_1.png", "a_2.png");
            BindCarouselImages(carouselAreaB, "b_1.png", "b_2.png");
            BindCarouselImages(carouselAreaC, "c_1.png", "c_2.png");
            BindCarouselImages(carouselAreaD, "d_1.png", "d_2.png");
        }

        private void BindCarouselImages(AntdUI.Carousel carousel, params string[] fileNames)
        {
            if (carousel == null || fileNames == null || fileNames.Length == 0)
                return;

            carousel.Image.Clear();

            foreach (var fileName in fileNames)
            {
                var image = LoadOverviewImage(fileName);
                if (image == null)
                    continue;

                _loadedImages.Add(image);
                carousel.Image.Add(new CarouselItem().SetImage(image).SetID(fileName));
            }
        }

        private Image LoadOverviewImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var imagePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Static",
                "Img",
                "Overview",
                fileName);

            if (!File.Exists(imagePath))
                return null;

            try
            {
                using (var source = Image.FromFile(imagePath))
                {
                    return new Bitmap(source);
                }
            }
            catch
            {
                return null;
            }
        }

        private void HomeOverviewPage_Disposed(object sender, EventArgs e)
        {
            foreach (var image in _loadedImages)
            {
                try
                {
                    if (image != null)
                        image.Dispose();
                }
                catch
                {
                }
            }

            _loadedImages.Clear();
        }
    }
}