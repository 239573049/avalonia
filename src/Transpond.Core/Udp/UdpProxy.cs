using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Transpond.Core.Udp
{
    public class UdpProxy : IProxy
    {
        private ConcurrentDictionary<IPEndPoint, UdpConnection>? _connections;

        private UdpConnection? _udpConnection;

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int ConnectionTimeout { get; set; } = 4 * 60 * 1000;

        private UdpClient? _localServer;

        public async Task Start(ProxyOptions config)
        {
            _connections = new ConcurrentDictionary<IPEndPoint, UdpConnection>();

            // TCP will lookup every time while this is only once.
            var ips = await Dns.GetHostAddressesAsync(config.ForwardIp!).ConfigureAwait(false);
            var remoteServerEndPoint = new IPEndPoint(ips[0], config.ForwardPort!.Value);

            _localServer = new UdpClient(AddressFamily.InterNetworkV6);
            _localServer.Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            IPAddress localIpAddress = string.IsNullOrEmpty(config.LocalIp) ? IPAddress.IPv6Any : IPAddress.Parse(config.LocalIp);
            _localServer.Client.Bind(new IPEndPoint(localIpAddress, config.LocalPort!.Value));

            Console.WriteLine($"UDP proxy started [{localIpAddress}]:{config.LocalPort!.Value} -> [{config.ForwardPort.Value}]:{config.ForwardPort.Value}");

            var _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
                    foreach (var connection in _connections.ToArray())
                    {
                        if (connection.Value.LastActivity + ConnectionTimeout < Environment.TickCount64)
                        {
                            _connections.TryRemove(connection.Key, out UdpConnection? c);
                            connection.Value.Stop();
                        }
                    }
                }
            });

            while (true)
            {
                try
                {
                    var message = await _localServer.ReceiveAsync().ConfigureAwait(false);
                    var sourceEndPoint = message.RemoteEndPoint;
                    _udpConnection = _connections.GetOrAdd(sourceEndPoint,
                        ep =>
                        {
                            var udpConnection = new UdpConnection(_localServer, sourceEndPoint, remoteServerEndPoint);
                            udpConnection.Run();
                            return udpConnection;
                        });
                    await _udpConnection.SendToServerAsync(message.Buffer).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"an exception occurred on receiving a client datagram: {ex}");
                }
            }
        }

        public async Task Stop()
        {
            if (_connections != null)
            {
                foreach (var key in _connections)
                {
                    try
                    {
                        key.Value.Stop();
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(exception);
                        Console.ResetColor();
                    }
                }
                _connections?.Clear();
                _localServer?.Close();
            }

            await Task.CompletedTask;
        }
    }

}
