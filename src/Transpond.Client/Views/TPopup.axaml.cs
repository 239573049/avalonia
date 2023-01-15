using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using static Transpond.Client.EventBus.PopupEvent;

namespace Transpond.Client.Views;

public partial class TPopup : Window
{
    public event OkClick OkClick;

    public event CancelClick CancelClick;

    public TPopup()
    {
        Width = 400;
        Height = 400;
        
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
        var content = this.Find<StackPanel>("content");
        var popup = this.Find<Window>("popup");
        content.Height = (double)((decimal)Height - 50);
        Console.WriteLine(Height);
        popup.PropertyChanged += (sender, args) =>
        {
            if (args.Property == HeightProperty)
            {
                var content = this.Find<StackPanel>("content");
                content.Height = (double)((decimal)Height - 50);
            }
        };
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        CancelClick?.Invoke(this);
    }

    private void Ok_OnClick(object? sender, RoutedEventArgs e)
    {
        OkClick?.Invoke(this);
    }
}