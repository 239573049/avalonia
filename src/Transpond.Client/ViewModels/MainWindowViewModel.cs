using Avalonia;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using Transpond.Core;

namespace Transpond.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<ProxyOptions> proxyOptions = new ObservableCollection<ProxyOptions>();

    private double windowHeight = 500;

    private double windowWidth = 800;
    
    private double listDataWidth = 800;

    public double ListDataWidth
    {
        get => listDataWidth;

        set => this.RaiseAndSetIfChanged(ref listDataWidth, value);
    }

    public double WindowHeight
    {
        get => windowHeight;

        set => this.RaiseAndSetIfChanged(ref windowHeight, value);
    }

    public double WindowWidth
    {
        get => windowWidth;
        set => this.RaiseAndSetIfChanged(ref windowWidth, value);
    }


    public ObservableCollection<ProxyOptions> ProxyOptions
    {
        get => proxyOptions;
        set => this.RaiseAndSetIfChanged(ref proxyOptions, value);
    }

    private ProxyOptions? selectProxyOptions;

    public ProxyOptions SelectProxyOptions
    {
        get => selectProxyOptions;
        set => this.RaiseAndSetIfChanged(ref selectProxyOptions, value);
    }


    public delegate void PropertyChanged(object dender, AvaloniaPropertyChangedEventArgs args);
}
