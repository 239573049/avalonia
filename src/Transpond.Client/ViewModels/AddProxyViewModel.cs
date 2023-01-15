using ReactiveUI;
using Transpond.Core;

namespace Transpond.Client.ViewModels;

public class AddProxyViewModel : ViewModelBase
{
    private ProxyOptions _proxyOptions = new ProxyOptions();
    
    public ProxyOptions ProxyOptions
    {
        get => _proxyOptions;
        set => this.RaiseAndSetIfChanged(ref _proxyOptions, value);
    }
}