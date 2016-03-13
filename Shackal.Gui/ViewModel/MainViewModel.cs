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
        private IEnumerator<string> _imagesEnumerator;

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
        /// What next round actualy does.
        /// </summary>
        public string NextRoundText
        {
            get
            {
                return ImagesEnumerator == null ? "Start game" : "Next round";
            }
        }

        /// <summary>
        /// Starts next round, or game if there it wasn't started yet.
        /// </summary>
        public ICommand NextRoundCommand { get; set; }

        /// <summary>
        /// View.
        /// </summary>
        private MainWindow MainWindow { get; set; }

        /// <summary>
        /// Start time of round.
        /// </summary>
        private DateTime RoundStartTime { get; set; }

        /// <summary>
        /// Animation, which reduces pixelisation.
        /// </summary>
        private AnimationTimeline Animation { get; set; }

        /// <summary>
        /// Iterator for rounds.
        /// </summary>
        private IEnumerator<string> ImagesEnumerator
        {
            get
            {
                return _imagesEnumerator;
            }
            set
            {
                _imagesEnumerator = value;
                RaisePropertyChanged(nameof(NextRoundText));
            }
        }

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
            if (ImagesEnumerator == null)
            {
                ImagesEnumerator = FindImages(ImagesFolder).GetEnumerator();
            }

            if (!ImagesEnumerator.MoveNext())
            {
                ImagesEnumerator = null;
                return;
            }

            LoadRound(ImagesEnumerator.Current);
        }

        public void InitializeView(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.Image.SizeChanged += OnImageSizeChanged;
        }

        private void OnImageSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (Animation != null)
            {
                Animation = CreateAnimation();
                Animation.BeginTime = -(DateTime.Now - RoundStartTime);
                PixelateEffect.BeginAnimation(PixelateEffect.SizeInPixelsProperty, Animation);
            }
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
            var width = MainWindow.Image.ActualWidth;
            var height = MainWindow.Image.ActualHeight;
            return new SizeAnimationUsingKeyFrames()
            {
                KeyFrames = new SizeKeyFrameCollection()
                {
                    new EasingSizeKeyFrame()
                    {
                        KeyTime = KeyTime.FromPercent(0),
                        Value = new Size(2.0, 2.0),
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
