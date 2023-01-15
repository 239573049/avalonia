using Avalonia.Controls;

namespace Transpond.Client.EventBus;

public class PopupEvent
{
    /// <summary>
    /// 点击Ok事件
    /// </summary>
    public delegate void OkClick(Window window);

    /// <summary>
    /// 点击取消事件
    /// </summary>
    public delegate void CancelClick(Window window);
}
