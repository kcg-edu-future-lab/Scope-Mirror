using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace ScopeMirror.Lightning.Guest
{
    public static class BitmapHelper
    {
        public const int MaxBitmapBytes = 64000;

        public static byte[] GetScreenImage(Int32Rect r) => GetScreenImage(r.X, r.Y, r.Width, r.Height);

        public static byte[] GetScreenImage(int x, int y, int width, int height)
        {
            using (var bitmap = GetScreenBitmap(x, y, width, height))
            {
                var bytes = ImageToBytes(bitmap, ImageFormat.Png);
                if (bytes.Length <= MaxBitmapBytes) return bytes;

                using (var bitmap2 = ScaleImage(bitmap, width / 2, height / 2))
                {
                    return ImageToBytes(bitmap2, ImageFormat.Jpeg);
                }
            }
        }

        static byte[] ImageToBytes(Image image, ImageFormat format)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, format);
                return memory.ToArray();
            }
        }

        static Bitmap GetScreenBitmap(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(x, y, 0, 0, bitmap.Size);
            }
            return bitmap;
        }

        static Bitmap ScaleImage(Image source, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                // Bilinear (Default) or HighQualityBilinear.
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.DrawImage(source, 0, 0, width, height);
            }
            return bitmap;
        }
    }
}
