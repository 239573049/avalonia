namespace Transpond.Core;


public interface IProxy
{
    /// <summary>
    /// 启动转发
    /// </summary>
    /// <returns></returns>
    Task Start(ProxyOptions config);

    /// <summary>
    /// 停止转发
    /// </summary>
    /// <returns></returns>
    Task Stop();
}