using LTgarlic.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace LTgarlic.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
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
