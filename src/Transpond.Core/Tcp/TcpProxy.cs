using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Transpond.Core.Tcp
{
    public class TcpProxy : IProxy
    {
        private List<TcpConnection>? _tcpConnections;

        /// <summary>
        /// Milliseconds
        /// </summary>
        public int ConnectionTimeout { get; set; } = 4 * 60 * 1000;

        private TcpListener _localServer;

        public async Task Start(ProxyOptions options)
        {
            var connections = new ConcurrentBag<TcpConnection>();

            IPAddress localIpAddress = string.IsNullOrEmpty(options.LocalIp) ? IPAddress.IPv6Any : IPAddress.Parse(options.LocalIp);
            _localServer = new TcpListener(new IPEndPoint(localIpAddress, options.LocalPort!.Value));
            _localServer.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            _localServer.Start();

            Console.WriteLine($"TCP proxy started [{localIpAddress}]:{options.LocalPort} -> [{options.ForwardIp}]:{options.ForwardPort}");

            var _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);

                    _tcpConnections = new List<TcpConnection>(connections.Count);
                    while (connections.TryTake(out var connection))
                    {
                        _tcpConnections.Add(connection);
                    }

                    foreach (var tcpConnection in _tcpConnections)
                    {
                        if (tcpConnection.LastActivity + ConnectionTimeout < Environment.TickCount64)
                        {
                            tcpConnection.Stop();
                        }
                        else
                        {
                            connections.Add(tcpConnection);
                        }
                    }
                }
            });

            while (true)
            {
                try
                {
                    var ips = await Dns.GetHostAddressesAsync(options.ForwardIp!).ConfigureAwait(false);

                    var tcpConnection = await TcpConnection.AcceptTcpClientAsync(_localServer,
                            new IPEndPoint(ips[0], options.ForwardPort!.Value))
                        .ConfigureAwait(false);
                    tcpConnection.Run();
                    connections.Add(tcpConnection);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    Console.ResetColor();
                }
            }
        }

        public async Task Stop()
        {
            if (_tcpConnections != null)
            {
                try
                {
                    foreach (var item in _tcpConnections)
                    {
                        try
                        {
                            item.Stop();
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(e);
                            Console.ResetColor();
                        }
                    }
                    _tcpConnections?.Clear();
                }
                finally
                {
                    _localServer.Stop();
                }
            }

            await Task.CompletedTask;
        }
    }

}
