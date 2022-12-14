using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using ABI.Windows.Foundation;
using components.Components;
using LTgarlic.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Windows.Devices.Input;

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

    private bool firstAccessCheck = true;
    private readonly ComboBox libary = new();
    private readonly ContentDialog dialog = new();

    private async void AddButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (firstAccessCheck)
        {
            libary.Items.Add("Capacitor");
            libary.Items.Add("Resistor");
            libary.Items.Add("Inductance");
            libary.Items.Add("Diode");

            dialog.XamlRoot = this.XamlRoot;
            dialog.Title = "Add new Components";
            dialog.PrimaryButtonText = "Place";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = libary;

            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;

            var result = await dialog.ShowAsync();

            firstAccessCheck = false;
        }
        else
        {

            dialog.XamlRoot = this.XamlRoot;
            dialog.Title = "Add new Components";
            dialog.PrimaryButtonText = "Place";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = libary;

            var result = await dialog.ShowAsync();
        }

    }

    private readonly List<component> components = new();
    private bool placeComponentSelected = false;
    private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        if (Convert.ToString(libary.SelectedItem) == "Capacitor")
        {   
            components.Add(new capacitor(drawingTable));
        }
        if (Convert.ToString(libary.SelectedItem) == "Resistor")
        {
            components.Add(new resistor(drawingTable));
        }
        if (Convert.ToString(libary.SelectedItem) == "Inductance")
        {
            components.Add(new inductance(drawingTable));
        }
        if (Convert.ToString(libary.SelectedItem) == "Diode")
        {
            components.Add(new diode(drawingTable));
        }

        placeComponentSelected = true;
        firstTimeMoveAccess = true;
    }

    private void RotateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private void mirrorButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private Windows.Foundation.Point mousePos = new();
    private bool firstTimeMoveAccess = true;
    private void drawingTable_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        mousePos = e.GetCurrentPoint(drawingTable).Position;

        if (placeComponentSelected)
        {
            if (firstTimeMoveAccess)
            {
                components[components.Count - 1].drawComponent(mousePos, 0);
                firstTimeMoveAccess = false;
            }
            else
            {
                components[components.Count - 1].moveComponent(mousePos, 0);
            }
        }
    }

    private void drawingTable_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (placeComponentSelected)
        {
            placeComponentSelected = false;
        }
    }
}
