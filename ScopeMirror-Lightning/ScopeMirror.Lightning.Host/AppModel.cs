using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace ScopeMirror.Lightning.Host
{
    public class AppModel
    {
        public static AppModel Instance { get; } = new AppModel();

        static int HostPort { get; } = Convert.ToInt32(ConfigurationManager.AppSettings["HostPort"]);

        public ReactiveProperty<byte[]> ScreenImage { get; } = new ReactiveProperty<byte[]>();

        public AppModel()
        {
            var client = new UdpClient(HostPort);

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var remoteEP = default(IPEndPoint);
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
    }
}
