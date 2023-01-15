using Avalonia.Controls;
using Transpond.Client.ViewModels;

namespace Transpond.Client.Views;
public partial class ListProxy : UserControl
{
    public ListProxy()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        this.DataContextChanged += (e, args) =>
        {
            if(DataContext is MainWindowViewModel model)
            {
                
            }

            
        };
    }
}
