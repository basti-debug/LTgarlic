using LTgarlic.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LTgarlic.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void changelocat1ion_click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        
    }

 
}
