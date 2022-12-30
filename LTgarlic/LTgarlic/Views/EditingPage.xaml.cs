using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using components.Components;
using components.Miscellaneous;
using LTgarlic.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Management.Deployment;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using WinRT;

namespace LTgarlic.Views;

public sealed partial class EditingPage : Page
{
    private List<List<Ellipse>> pads = new();
    public static List<wire> allWires = new();
    private double gridSize = 20;

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

        rotation = 0;
        placeComponentSelected = true;
        firstTimeMoveAccess = true;
    }

    private int rotation;
    private int clickCounter = 0;
    private void RotateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (placeComponentSelected)
        {
            clickCounter++;
            switch (clickCounter)
            {
                case 1:
                    rotation = 90;
                    break;
                case 2:
                    rotation = 180;
                    break;
                case 3:
                    rotation = 270;
                    break;
                case 4:
                    rotation = 0;
                    clickCounter = 0;
                    break;
            }

            switch(SettingsPage.theme)
            {
                case "Dark":
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    break;
                case "Light":
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.Black));
                    break;
                case "Default":
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    break;
            }
            pads.Add(components[components.Count - 1].pads);
        }
    }

    private void mirrorButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private Point actualMousePos = new();
    private Point gridMousePos = new();
    private bool firstTimeMoveAccess = true;
    private void drawingTable_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        actualMousePos = e.GetCurrentPoint(drawingTable).Position;

        #region grid Size
        if (gridSize / 2 > actualMousePos.X % gridSize)
            gridMousePos.X = actualMousePos.X + (gridSize - (actualMousePos.X % gridSize));

        if (gridSize / 2 > actualMousePos.Y % gridSize)
            gridMousePos.Y = actualMousePos.Y + (gridSize - (actualMousePos.Y % gridSize));
        #endregion

        if (SettingsPage.theme == "Dark")
        {
            if (placeComponentSelected)
            {
                if (firstTimeMoveAccess)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    firstTimeMoveAccess = false;
                    pads.Add(components[components.Count - 1].pads);
                }
                else
                {
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    pads.Add(components[components.Count - 1].pads);
                }
            }
        }

        else if (SettingsPage.theme == "Light")
        {
            if (placeComponentSelected)
            {
                if (firstTimeMoveAccess)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.Black));
                    firstTimeMoveAccess = false;
                    pads.Add(components[components.Count - 1].pads);
                }
                else
                {
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.Black));
                    pads.Add(components[components.Count - 1].pads);
                }
            }
        }
        else if (SettingsPage.theme == "Default")
        {
            if (placeComponentSelected)
            {
                if (firstTimeMoveAccess)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    firstTimeMoveAccess = false;
                    pads.Add(components[components.Count - 1].pads);
                }
                else
                {
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    pads.Add(components[components.Count - 1].pads);
                }
            }
        }

        if (ShellPage.wireMode && wireStart)
        {
            allWires[allWires.Count - 1].deleteWire();
            allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]));
        }
    }

    public static int wireClickCnt = 0;
    public static bool wireStart = false;
    private Point startPoint = new();
    private Point endPoint = new();
    private void drawingTable_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            #region placeComponent
            if (placeComponentSelected)
                placeComponentSelected = false;
            #endregion

            #region wireMode
            else if (ShellPage.wireMode)
            {

                allWires.Add(new wire(drawingTable));
                wireClickCnt++;

                if (wireClickCnt == 1)
                {
                    startPoint = gridMousePos;
                    wireStart = true;
                }
                if (wireClickCnt > 1)
                {
                    startPoint = gridMousePos;
                }
            }
            #endregion
        }

        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed && ShellPage.wireMode)
        {
            wire.changeWiringType();
            allWires[allWires.Count - 1].redrawWire(allWires[allWires.Count - 1]);
        }
    }

    private void drawingTable_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        ShellPage.wireMode = false;
        wireStart = false;
        wireClickCnt = 0;
        allWires[allWires.Count - 1].deleteWire();
    }
}
