using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shackal.Gui
{
    public class MainViewModel : BaseViewModel
    {
        private ImageSource _image;
        private PixelateEffect _pixelateEffect;
        private string _imagesFolder;

        /// <summary>
        /// Shader which pixelates image.
        /// </summary>
        public PixelateEffect PixelateEffect
        {
            get
            {
                return _pixelateEffect;
            }
            set
            {
                _pixelateEffect = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Displaying image.
        /// </summary>
        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Place where take images from.
        /// </summary>
        public string ImagesFolder
        {
            get
            {
                return _imagesFolder;
            }
            set
            {
                _imagesFolder = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            _pixelateEffect = new PixelateEffect()
            {
                PixelatedHeight = 30,
                PixelatedWidth = 30,
            };
            _imagesFolder = "Resources";

            StartGame();
        }

        private void StartGame()
        {
            var images = FindImages(ImagesFolder);
            LoadImageSource(images.First());
        }

        private IEnumerable<string> FindImages(string imagesFolder)
        {
            if (!Directory.Exists(imagesFolder))
            {
                return Enumerable.Empty<string>();
            }

            var files = new DirectoryInfo(imagesFolder).EnumerateFiles();
            var imageExtensions = Configuration.ImageExtensions;
            return files.Where(f => imageExtensions.Contains(f.Extension)).Select(f => f.FullName);
        }

        private void LoadImageSource(string filePath)
        {
            Image = new BitmapImage(new Uri(filePath));
        }
    }
}
