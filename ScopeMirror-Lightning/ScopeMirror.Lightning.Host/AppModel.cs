using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace ScopeMirror.Lightning.Host
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        static int HostPort => Convert.ToInt32(ConfigurationManager.AppSettings["HostPort"]);

        public string HostAddresses { get; } = string.Join("\n", GetHostAddresses());
        public ReactiveProperty<byte[]> ScreenImage { get; } = new ReactiveProperty<byte[]>();
        public ReactiveProperty<bool> IsImageVisible { get; } = new ReactiveProperty<bool>();

        public AppModel()
        {
            ScreenImage
                .Subscribe(_ => IsImageVisible.Value = true);
            ScreenImage
                .Throttle(TimeSpan.FromSeconds(5))
                .Subscribe(_ => IsImageVisible.Value = false);

            Task.Run(() =>
            {
                var client = new UdpClient(HostPort);
                var remoteEP = default(IPEndPoint);

                while (true)
                {
                    try
                    {
                        ScreenImage.Value = client.Receive(ref remoteEP);
                    }
                    catch (SocketException ex)
                    {
                        switch (ex.SocketErrorCode)
                        {
                            case SocketError.ConnectionReset:
                                continue;
                            case SocketError.Interrupted:
                                return;
                            default:
                                throw;
                        }
                    }
                }
            });
        }

        static IEnumerable<IPAddress> GetHostAddresses() =>
            Dns.GetHostAddresses(Dns.GetHostName())
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork);
    }
}
