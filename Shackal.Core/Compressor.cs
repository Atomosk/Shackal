using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace Shackal.Core
{
    public class Compressor
    {
        public void Compress(string filePath, int partsCount)
        {
            var bitmap = Image.FromFile(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var directory = Path.GetDirectoryName(filePath);
            var length = partsCount.ToString().Length;
            for (double i = 1; i < partsCount; i++)
            {
                var scaleFactor = i/partsCount;
                var compressed = Compress(bitmap, scaleFactor);
                compressed.Save(Path.Combine(directory, $"{fileName}_{i.ToString(CultureInfo.InvariantCulture).PadLeft(length, '0')}.jpg"), ImageFormat.Jpeg);
                compressed.Dispose();
            }

            bitmap.Save(Path.Combine(directory, $"{fileName}_{partsCount}.jpg"), ImageFormat.Jpeg);
        }

        private Image Compress(Image bitmap, double scaleFactor)
        {
            var sourceWidth = bitmap.Width;
            var sourceHeight = bitmap.Height;
            var reduced = ResizeImage(bitmap, (int) (sourceWidth*scaleFactor), (int) (sourceHeight*scaleFactor));
            var restored = ResizeImage(reduced, sourceWidth, sourceHeight);
            reduced.Dispose();
            return restored;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
