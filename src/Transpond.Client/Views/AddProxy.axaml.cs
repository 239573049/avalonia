using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Transpond.Client.ViewModels;

namespace Transpond.Client.Views;

public partial class AddProxy : UserControl
{
    public AddProxy()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContextChanged += (sender, args) =>
        {
            if (DataContext is AddProxyViewModel model)
            {
                
            }
            
        };
    }

    private void cs_OnClick(object? sender, RoutedEventArgs e)
    {

        if (DataContext is AddProxyViewModel model)
        {
            var options = model.ProxyOptions;
        }
    }
}