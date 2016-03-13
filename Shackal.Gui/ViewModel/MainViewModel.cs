using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Shackal.Gui.ViewModel
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private ImageSource _image;
        private PixelateEffect _pixelateEffect;
        private string _imagesFolder;
        private TimeSpan _roundDuration;

        public DateTime RoundStartTime { get; set; }

        public AnimationTimeline Animation { get; set; }

        /// <summary>
        /// How many time to guess image.
        /// </summary>
        public TimeSpan RoundDuration
        {
            get
            {
                return _roundDuration;
            }
            set
            {
                _roundDuration = value;
                RaisePropertyChanged();
            }
        }

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

        /// <summary>
        /// Starts next round, or game if there it wasn't started yet.
        /// </summary>
        public ICommand NextRoundCommand { get; set; }

        public MainWindow MainWindow { get; set; }

        public MainViewModel()
        {
            _imagesFolder = "Resources";
            _roundDuration = TimeSpan.FromSeconds(30);
            _pixelateEffect = new PixelateEffect()
            {
                PixelatedHeight = 1,
                PixelatedWidth = 1,
            };
            NextRoundCommand = new Command(NextRound, o => true);
        }

        private void NextRound(object param)
        {
            StartGame();
        }

        public void InitializeView(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.SizeChanged += MainWindowOnSizeChanged;
        }

        private void MainWindowOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (Animation != null)
            {
                Animation = CreateAnimation();
                Animation.BeginTime = -(DateTime.Now - RoundStartTime);
                PixelateEffect.BeginAnimation(PixelateEffect.SizeInPixelsProperty, Animation);
            }
        }

        private void StartGame()
        {
            var images = FindImages(ImagesFolder);
            LoadRound(images.First());
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

        private void LoadRound(string filePath)
        {
            var imageSource = new BitmapImage(new Uri(filePath));
            Animation = CreateAnimation();
            RoundStartTime = DateTime.Now;
            PixelateEffect.BeginAnimation(PixelateEffect.SizeInPixelsProperty, Animation);
            Image = imageSource;
        }

        private SizeAnimationUsingKeyFrames CreateAnimation()
        {
            var width = MainWindow.Width;
            var height = MainWindow.Height;
            return new SizeAnimationUsingKeyFrames()
            {
                KeyFrames = new SizeKeyFrameCollection()
                {
                    new EasingSizeKeyFrame()
                    {
                        KeyTime = KeyTime.FromPercent(0),
                        Value = new Size(1.0, 1.0),
                    },
                    new EasingSizeKeyFrame()
                    {
                        KeyTime = KeyTime.FromPercent(0.33),
                        Value = new Size(width/20.0, height/20.0),
                    },
                    new EasingSizeKeyFrame()
                    {
                        KeyTime = KeyTime.FromPercent(0.66),
                        Value = new Size(width/10.0, height/10.0),
                    },
                    new EasingSizeKeyFrame()
                    {
                        KeyTime = KeyTime.FromPercent(1),
                        Value = new Size(width, height),
                    },

                },
                IsAdditive = false,
                AutoReverse = false,
                AccelerationRatio = 1.0,
                Duration = new Duration(RoundDuration)
            };
        }

        public void Dispose()
        {
            var mainWindow = MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Image.SizeChanged -= OnImageSizeChanged;
            }

            MainWindow = null;
        }
    }
}
