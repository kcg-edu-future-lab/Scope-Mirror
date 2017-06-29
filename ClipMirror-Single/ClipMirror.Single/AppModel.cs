using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using Reactive.Bindings;

namespace ClipMirror.Single
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        public Int32Rect ClipBounds { get; set; } = new Int32Rect(100, 100, 300, 200);
        public ReactiveProperty<byte[]> ScreenImage { get; } = new ReactiveProperty<byte[]>();

        public DisplayScreen[] Screens { get; }
        public ReactiveProperty<DisplayScreen> SelectedScreen { get; } = new ReactiveProperty<DisplayScreen>();

        public ReactiveProperty<bool> IsClipMoving { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> IsMirroring { get; } = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public AppModel()
        {
            Screens = Screen.AllScreens
                .Select((s, i) => new DisplayScreen(i + 1, s))
                .ToArray();
            SelectedScreen.Value = Screens.FirstOrDefault(s => !s.Screen.Primary) ?? Screens.First();
        }

        IDisposable trackingImage;

        public void StartTrackingImage()
        {
            StopTrackingImage();

            trackingImage = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(0.5))
                .Subscribe(_ => ScreenImage.Value = GetClippedScreenImage());
        }

        public void StopTrackingImage()
        {
            trackingImage?.Dispose();
            trackingImage = null;
        }

        byte[] GetClippedScreenImage()
        {
            using (var bitmap = new Bitmap(ClipBounds.Width, ClipBounds.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var memory = new MemoryStream())
            {
                graphics.CopyFromScreen(ClipBounds.X, ClipBounds.Y, 0, 0, bitmap.Size);
                bitmap.Save(memory, ImageFormat.Png);
                return memory.ToArray();
            }
        }
    }

    public class DisplayScreen
    {
        public int Id { get; }
        public Screen Screen { get; }

        public Int32Rect Rectangle { get; }
        public string Info => $"{Id}: {Rectangle}";

        public DisplayScreen(int id, Screen screen)
        {
            Id = id;
            Screen = screen;

            Rectangle = screen.Bounds.ToInt32Rect();
        }
    }
}
