using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Windows;
using Reactive.Bindings;

namespace ScopeMirror.Lightning.Guest
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        static string HostAddress => ConfigurationManager.AppSettings["HostAddress"];
        static int HostPort => Convert.ToInt32(ConfigurationManager.AppSettings["HostPort"]);

        public Int32Rect ScopeBounds { get; set; } = new Int32Rect(100, 100, 300, 200);
        public ReactiveProperty<byte[]> ScreenImage { get; } = new ReactiveProperty<byte[]>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public ReactiveProperty<bool> IsScopeMoving { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> IsMirroring { get; } = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public AppModel()
        {
            IsMirroring.Subscribe(b =>
            {
                if (b)
                {
                    StartTrackingImage();
                }
                else
                {
                    StopTrackingImage();
                }
            });

            var client = new UdpClient(HostAddress, HostPort);
            ScreenImage
                .Where(b => b.Length <= 64000)
                .Subscribe(b => client.Send(b, b.Length));
        }

        IDisposable trackingImage;

        public void StartTrackingImage()
        {
            StopTrackingImage();

            trackingImage = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(0.15))
                .Subscribe(_ => ScreenImage.Value = GetScopedScreenImage());
        }

        public void StopTrackingImage()
        {
            trackingImage?.Dispose();
            trackingImage = null;
        }

        byte[] GetScopedScreenImage()
        {
            using (var bitmap = new Bitmap(ScopeBounds.Width, ScopeBounds.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var memory = new MemoryStream())
            {
                graphics.CopyFromScreen(ScopeBounds.X, ScopeBounds.Y, 0, 0, bitmap.Size);
                bitmap.Save(memory, ImageFormat.Png);
                return memory.ToArray();
            }
        }
    }
}
