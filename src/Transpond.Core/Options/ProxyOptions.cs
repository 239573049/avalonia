namespace Transpond.Core;

public class ProxyOptions
{
    /// <summary>
    /// Id
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// 协议 TCP | UDP 
    /// </summary>
    public string? Protocol { get; set; }

    /// <summary>
    /// 本地端口
    /// </summary>
    public ushort? LocalPort { get; set; }
    
    /// <summary>
    /// 本地ip
    /// </summary>
    public string? LocalIp { get; set; }
    
    /// <summary>
    /// 远程IP
    /// </summary>
    public string? ForwardIp { get; set; }
    
    /// <summary>
    /// 远程端口
    /// </summary>
    public ushort? ForwardPort { get; set; }
}