using System.Collections.Concurrent;
using Transpond.Core.Tcp;
using Transpond.Core.Udp;

namespace Transpond.Core.Extensions;

public static class TranspondExtension
{
    private static readonly ConcurrentDictionary<string, IProxy> ProxyLists = new();

    /// <summary>
    /// 新增转发
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IEnumerable<Task> AddProxy(this ProxyOptions options)
    {
        options.VerifyConfig();

        var protocol = options.Protocol?.ToLower();

        bool protocolHandled = false;
        if (protocol == "udp" || protocol == "any")
        {
            protocolHandled = true;
            Task task;
            try
            {
                var proxy = new UdpProxy();
                task = proxy.Start(options);
                ProxyLists.TryAdd(options!.Key, proxy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start {options.Key} : {ex.Message}");
                throw;
            }

            yield return task;
        }

        if (protocol == "tcp" || protocol == "any")
        {
            protocolHandled = true;
            Task task;
            try
            {
                var proxy = new TcpProxy();
                task = proxy.Start(options);
                ProxyLists.TryAdd(options.Key, proxy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start {options.Key} : {ex.Message}");
                throw;
            }

            yield return task;
        }

        if (!protocolHandled)
        {
            throw new InvalidOperationException($"protocol not supported {protocol}");
        }
    }

    /// <summary>
    /// 停止转发
    /// </summary>
    /// <param name="key"></param>
    public static void Stop(string key)
    {
        if (ProxyLists.TryGetValue(key, out var proxy))
        {
            proxy.Stop();
            Console.WriteLine($"{key} 被停止转发");
        }
    }

    /// <summary>
    /// 停止转发
    /// </summary>
    /// <param name="options"></param>
    public static void Stop(this ProxyOptions options)
    {
        Stop(options.Key ?? string.Empty);
    }

    /// <summary>
    /// 校验Config参数
    /// </summary>
    /// <param name="options"></param>
    public static void VerifyConfig(this ProxyOptions options)
    {
        var protocol = options.Protocol?.ToLower();

        try
        {
            if (string.IsNullOrEmpty(options.Key))
            {
                throw new Exception("Key是空");
            }

            if (ProxyLists.Any(x => x.Key == options.Key))
            {
                throw new Exception("存在相同Key");
            }

            if (options.ForwardIp == null)
            {
                throw new Exception("forwardIp是空");
            }
            if (!options.ForwardPort.HasValue)
            {
                throw new Exception("forwardPort是空");
            }

            if (!options.LocalPort.HasValue)
            {
                throw new Exception("localPort是空");
            }
            if (protocol != "udp" && protocol != "tcp" && protocol != "any")
            {
                throw new Exception($"protocol is not supported {protocol}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start {options.Key} : {ex.Message}");
            throw;
        }
    }
}
