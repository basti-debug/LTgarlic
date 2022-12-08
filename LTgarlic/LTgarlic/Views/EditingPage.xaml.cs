using System.Diagnostics;
using LTgarlic.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace LTgarlic.Views;

public sealed partial class EditingPage : Page
{
    public EditingViewModel ViewModel
    {
        get;
    }

    public EditingPage()
    {
        ViewModel = App.GetService<EditingViewModel>();
        InitializeComponent();
    }

    private void EditButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }
    private void SimuButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }
    private async void AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var libary = new ComboBox();
        libary.Items.Add("Capacitor");
        libary.Items.Add("Resistor");
        libary.Items.Add("Inductance");
        libary.Items.Add("Diode");
    

        ContentDialog dialog= new ContentDialog();

        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = "Add new Components";
        dialog.PrimaryButtonText = "Place";
        dialog.CloseButtonText ="Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = libary;

        var result = await dialog.ShowAsync();
    }
    private void RotateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }
    private void mirrorButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }
}
