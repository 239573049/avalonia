using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using System.Threading.Tasks;
using Transpond.Client.ViewModels;
using Transpond.Core.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Transpond.Client.Views;
public partial class Header : UserControl
{
    public Header()
    {
        InitializeComponent();
    }

    private void DeleteProxy_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel model)
        {
            try
            {
                model.SelectProxyOptions.Stop();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                model.ProxyOptions.Remove(model.SelectProxyOptions);
            }
        }
    }

    private void AddProxy_OnClick(object? sender, RoutedEventArgs e)
    {
        var addProxyViewModel = new AddProxyViewModel();
        var popup = new TPopup
        {
            Icon = null,
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome,
            ExtendClientAreaTitleBarHeightHint = -1,
            ExtendClientAreaToDecorationsHint = true,
            DataContext = addProxyViewModel
        };

        popup.OkClick += (win) =>
        {
            var data = addProxyViewModel.ProxyOptions;

            if(DataContext is MainWindowViewModel model)
            {
                model.ProxyOptions.Add(data);
                _ = Task.WhenAll(data.AddProxy());

                win.Close();
            }
        };

        popup.CancelClick += (win) =>
        {
            win.Close();
        };

        var content =  popup.Find<StackPanel>("content");
        content.Children.Add(new AddProxy());
        popup.Show();
    }
}
