using LTgarlic.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LTgarlic.Views;

public sealed partial class SettingsPage : Page
{
    public static string theme = "Default";

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

    private void Light_Theme_Checked(object sender, RoutedEventArgs e)
    {
        theme = "Light";
    }

    private void Dark_Theme_Checked(object sender, RoutedEventArgs e)
    {
        theme = "Dark";
    }

    private void Default_Theme_Checked(object sender, RoutedEventArgs e)
    {
        theme = "Default";
    }
}
