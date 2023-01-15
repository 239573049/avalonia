using Avalonia.Controls;
using Transpond.Client.ViewModels;

namespace Transpond.Client.Views;
public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();

        DataContextChanged += (obj, args) =>
        {
            AddProxy();
        };

    }

    private void AddProxy()
    {
        if (base.DataContext is MainWindowViewModel model)
        {
            var win = this.Find<Window>("Main");

            win.PropertyChanged += (sender, e) =>
            {
                if (e.Property == WidthProperty)
                {
                    model.WindowWidth = Width;
                    // 设置列表宽度
                    model.ListDataWidth = Width - 10;
                }
            };

        }
    }
}