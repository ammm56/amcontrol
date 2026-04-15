using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Home
{
    /// <summary>
    /// 首页总览看板占位页。
    /// 当前仅用于展示 2x2 图片轮播占位布局，后续真实数据确定后再替换内容区。
    /// </summary>
    public partial class HomeOverviewPage : UserControl
    {
        private readonly List<Image> _previewImages;
        private readonly Dictionary<string, Image> _imageMap;
        private readonly Dictionary<string, int> _previewIndexMap;

        public HomeOverviewPage()
        {
            InitializeComponent();

            _previewImages = new List<Image>();
            _imageMap = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
            _previewIndexMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            BindEvents();
            LoadOverviewImages();
            BindCarousels();

            Disposed += HomeOverviewPage_Disposed;
        }

        private void BindEvents()
        {
            carouselAreaA.Click += CarouselArea_Click;
            carouselAreaB.Click += CarouselArea_Click;
            carouselAreaC.Click += CarouselArea_Click;
            carouselAreaD.Click += CarouselArea_Click;
        }

        private void LoadOverviewImages()
        {
            var imageDirectory = GetOverviewImageDirectory();
            if (!Directory.Exists(imageDirectory))
                return;

            var imageFiles = Directory.GetFiles(imageDirectory)
                .Where(IsSupportedImageFile)
                .OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            for (var i = 0; i < imageFiles.Count; i++)
            {
                var filePath = imageFiles[i];
                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrWhiteSpace(fileName))
                    continue;

                var image = LoadImageFromPath(filePath);
                if (image == null)
                    continue;

                _previewImages.Add(image);
                _imageMap[fileName] = image;
                _previewIndexMap[fileName] = _previewImages.Count - 1;
            }
        }

        private void BindCarousels()
        {
            BindCarouselImages(carouselAreaA, "a");
            BindCarouselImages(carouselAreaB, "b");
            BindCarouselImages(carouselAreaC, "c");
            BindCarouselImages(carouselAreaD, "d");
        }

        private void BindCarouselImages(AntdUI.Carousel carousel, string prefix)
        {
            if (carousel == null || string.IsNullOrWhiteSpace(prefix))
                return;

            carousel.Image.Clear();

            var fileNames = _imageMap.Keys
                .Where(p => p.StartsWith(prefix + "_", StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var fileName in fileNames)
            {
                Image image;
                if (!_imageMap.TryGetValue(fileName, out image) || image == null)
                    continue;

                carousel.Image.Add(new CarouselItem()
                    .SetID(fileName)
                    .SetImage(image)
                    .SetTag(fileName));
            }
        }

        private void CarouselArea_Click(object sender, EventArgs e)
        {
            var carousel = sender as AntdUI.Carousel;
            if (carousel == null)
                return;

            if (carousel.Image == null || carousel.Image.Count <= 0)
                return;

            var selectedIndex = carousel.SelectIndex;
            if (selectedIndex < 0 || selectedIndex >= carousel.Image.Count)
                selectedIndex = 0;

            var selectedItem = carousel.Image[selectedIndex];
            if (selectedItem == null || string.IsNullOrWhiteSpace(selectedItem.ID))
                return;

            int previewIndex;
            if (!_previewIndexMap.TryGetValue(selectedItem.ID, out previewIndex))
                previewIndex = 0;

            var form = FindForm();
            if (form == null || _previewImages.Count == 0)
                return;

            new AntdUI.Preview.Config(form, _previewImages)
                .SetSelectIndex(previewIndex)
                .SetFit(TFit.Cover)
                .open();
        }

        private static bool IsSupportedImageFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            var extension = Path.GetExtension(filePath);
            return string.Equals(extension, ".jpg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(extension, ".bmp", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetOverviewImageDirectory()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Static",
                "Img",
                "Overview");
        }

        private static Image LoadImageFromPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            try
            {
                using (var source = Image.FromFile(filePath))
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
            foreach (var image in _previewImages)
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

            _previewImages.Clear();
            _imageMap.Clear();
            _previewIndexMap.Clear();
        }
    }
}