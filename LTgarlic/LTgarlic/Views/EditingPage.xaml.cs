using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using components.Components;
using LTgarlic.Components.Miscellaneous;
using LTgarlic.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Composition.Scenes;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using Windows.ApplicationModel.UserDataTasks;
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
    public List<Ellipse> connections = new();
    private double gridSize = 30;

    public static int wireClickCnt = 0;
    public static bool wireStart = false;
    public static Point startPoint = new();
    public static Point endPoint = new();

    private SolidColorBrush accent = new SolidColorBrush((Color) Application.Current.Resources["SystemAccentColor"]);

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

    private static readonly List<component> components = new();
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
        firstAccessComponent = true;
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
            #region redraw Component
            switch (SettingsPage.theme)
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
            #endregion
        }
    }

    private void mirrorButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {

    }

    private Point actualMousePos = new();
    private Point gridMousePos = new();
    private bool firstAccessComponent = true;
    public static bool oneLineUsed;
    private bool firstWireAccess = true;
    private void drawingTable_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        actualMousePos = e.GetCurrentPoint(drawingTable).Position;

        #region grid Size
        gridMousePos.X = actualMousePos.X + (gridSize - (actualMousePos.X % gridSize)) - gridSize / 2;
        gridMousePos.Y = actualMousePos.Y + (gridSize - (actualMousePos.Y % gridSize)) - gridSize / 2;
        #endregion

        #region move components
        if (SettingsPage.theme == "Dark")
        {
            if (placeComponentSelected)
            {
                //the first time the mouse moves the components needs to be drawn, because the moveComponent
                //function works by deleting the component, then redrawing it
                if (firstAccessComponent)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    firstAccessComponent = false;
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
                if (firstAccessComponent)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.Black));
                    firstAccessComponent = false;
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
                if (firstAccessComponent)
                {
                    components[components.Count - 1].drawComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    firstAccessComponent = false;
                    pads.Add(components[components.Count - 1].pads);
                }
                else
                {
                    components[components.Count - 1].moveComponent(gridMousePos, rotation, new SolidColorBrush(Colors.White));
                    pads.Add(components[components.Count - 1].pads);
                }
            }
        }
        #endregion

        #region draw wires while moving mouse
        if (ShellPage.wireMode && wireStart)
        {
            if (startPoint != gridMousePos)
            {
                if (wire.wiringType)
                {
                    if (!firstWireAccess)
                    {
                        if (!wireContinues)
                        {
                            if (oneLineUsed)
                            {
                                allWires[allWires.Count - 1].deleteWire();
                                allWires.Remove(allWires[allWires.Count - 1]);
                            }
                            else
                            {
                                allWires[allWires.Count - 1].deleteWire();
                                allWires[allWires.Count - 2].deleteWire();
                                allWires.Remove(allWires[allWires.Count - 1]);
                                allWires.Remove(allWires[allWires.Count - 1]);
                            }
                        }
                        if (wireContinues)
                            wireContinues = false;
                    }
                    firstWireAccess = false;

                    if (startPoint.Y == gridMousePos.Y)//horizontal
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                        oneLineUsed = true;
                    }
                    else if (startPoint.X == gridMousePos.X)//vertical
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                        oneLineUsed = true;
                    }
                    else
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, new Point(gridMousePos.X, startPoint.Y), accent);
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(new Point(gridMousePos.X, startPoint.Y), gridMousePos, accent);
                        oneLineUsed = false;
                    }
                }
                else
                {
                    if (!firstWireAccess)
                    {
                        if (!wireContinues)
                        {
                            if (oneLineUsed)
                            {
                                allWires[allWires.Count - 1].deleteWire();
                                allWires.Remove(allWires[allWires.Count - 1]);
                            }
                            else
                            {
                                allWires[allWires.Count - 1].deleteWire();
                                allWires[allWires.Count - 2].deleteWire();
                                allWires.Remove(allWires[allWires.Count - 1]);
                                allWires.Remove(allWires[allWires.Count - 1]);
                            }
                        }
                        if (wireContinues)
                            wireContinues = false;
                    }
                    firstWireAccess = false;

                    if (startPoint.Y == gridMousePos.Y)//horizontal
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                        oneLineUsed = true;
                    }
                    else if (startPoint.X == gridMousePos.X)//vertical
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                        oneLineUsed = true;
                    }
                    else
                    {
                        allWires.Add(new wire(drawingTable));
                        allWires.Add(new wire(drawingTable));
                        allWires[allWires.Count - 1].drawWire(startPoint, new Point(startPoint.X, gridMousePos.Y), accent);
                        allWires[allWires.Count - 2].drawWire(new Point(startPoint.X, gridMousePos.Y), gridMousePos, accent);
                        oneLineUsed = false;
                    }
                }
            }
            
        }
        #endregion
    }

    private bool wireContinues = false;
    private void drawingTable_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            #region placeComponent
            if (placeComponentSelected)
                placeComponentSelected = false;
            #endregion

            #region wireMode

            //wireMode is enabled, if w has been pressed. This means, by leftclicking, a wire can be drawn.
            else if (ShellPage.wireMode)
            {
                wireClickCnt++;

                if (wireClickCnt == 1)//the start point of the wire is set, as soon as the user leftclicks.
                {
                    startPoint = gridMousePos;
                    wireStart = true;

                    foreach (var wire in allWires)
                    {
                        if (wire != allWires[allWires.Count - 2] && wire != allWires[allWires.Count - 1])
                        {
                            if (wire.startPoint.Y == wire.endPoint.Y) //horizontal
                            {
                                if (allWires[allWires.Count - 2].startPoint.Y == wire.startPoint.Y) //if the line has the same y value
                                {
                                    if (wire.startPoint.X == wire.endPoint.X) //line has been drawn from left to right
                                    {
                                        if (allWires[allWires.Count - 1].startPoint.X > wire.startPoint.X && allWires[allWires.Count - 1].startPoint.X < wire.endPoint.X)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                    else if (wire.startPoint.X > wire.endPoint.X)
                                    {

                                        if (allWires[allWires.Count - 1].startPoint.X < wire.startPoint.X && allWires[allWires.Count - 1].startPoint.X > wire.endPoint.X)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                }
                            }
                            if (wire.startPoint.X == wire.endPoint.X) //vertical
                            {
                                if (allWires[allWires.Count - 2].startPoint.X == wire.startPoint.X) //if the line has the same x value
                                {
                                    if (wire.startPoint.Y < wire.endPoint.Y) //line has been drawn from top to bottom
                                    {
                                        if (allWires[allWires.Count - 2].startPoint.Y > wire.startPoint.Y && allWires[allWires.Count - 2].startPoint.Y < wire.endPoint.Y)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                    else
                                    {
                                        if (allWires[allWires.Count - 2].startPoint.Y < wire.startPoint.Y && allWires[allWires.Count - 2].startPoint.Y > wire.endPoint.Y)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //if the user clicks again, the wire is placed, and the new startPoint becomes the current mouse Position,
                //making the wires connect.
                if (wireClickCnt > 1)
                {
                    startPoint = gridMousePos;
                    wireContinues = true;

                    foreach (var wire in allWires)
                    {
                        if (wire != allWires[allWires.Count - 1])
                        {
                            if (wire.startPoint.X == wire.endPoint.X) //vertical
                            {
                                if (allWires[allWires.Count - 1].endPoint.X == wire.startPoint.X) //if the line has the same x value
                                {
                                    if (wire.startPoint.Y < wire.endPoint.Y) //line has been drawn from top to bottom
                                    {
                                        if (allWires[allWires.Count - 1].endPoint.Y > wire.startPoint.Y && allWires[allWires.Count - 1].endPoint.Y < wire.endPoint.Y)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                    else
                                    {
                                        if (allWires[allWires.Count - 1].endPoint.Y < wire.startPoint.Y && allWires[allWires.Count - 1].endPoint.Y > wire.endPoint.Y)
                                        {
                                            Ellipse connection = new Ellipse()
                                            {
                                                Width = 10,
                                                Height = 10,
                                                Fill = accent,
                                                Stroke = accent,
                                                StrokeThickness = 1
                                            };

                                            Canvas.SetLeft(connection, allWires[allWires.Count - 2].startPoint.X - connection.Width / 2);
                                            Canvas.SetTop(connection, allWires[allWires.Count - 2].startPoint.Y - connection.Width / 2);

                                            connections.Add(connection);
                                            drawingTable.Children.Add(connection);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed && ShellPage.wireMode)
        {
            #region change wire type
            wire.changeWiringType();
            if (wire.wiringType)
            {
                allWires[allWires.Count - 1].deleteWire();
                allWires[allWires.Count - 2].deleteWire();
                allWires[allWires.Count - 1].drawWire(startPoint, new Point(gridMousePos.X, startPoint.Y), new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]));
                allWires[allWires.Count - 2].drawWire(new Point(gridMousePos.X, startPoint.Y), gridMousePos, new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]));
            }
            else
            {
                allWires[allWires.Count - 1].deleteWire();
                allWires[allWires.Count - 2].deleteWire();
                allWires[allWires.Count - 1].drawWire(startPoint, new Point(startPoint.X, gridMousePos.Y), new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]));
                allWires[allWires.Count - 2].drawWire(new Point(startPoint.X, gridMousePos.Y), gridMousePos, new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]));
            }
            #endregion
        }
    }

    private void drawingTable_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
    {
        #region enable placing wires by doubleclicking
        if (ShellPage.wireMode)
        {
            ShellPage.wireMode = false;
            wireStart = false;
            wireClickCnt = 0;
            allWires[allWires.Count - 1].deleteWire();
            allWires[allWires.Count - 2].deleteWire();
        }
        #endregion

        encodeFile();
    }

    public static void encodeFile()
    {
        string spiceString = "";
        string fileName = @"C:\Users\gabri\Downloads\firstCircuit.asc";

        spiceString += "Version 4\n";

        foreach (wire wire in allWires)
        {
            spiceString += "WIRE " + (wire.startPoint.X / 2) + " " + (wire.startPoint.Y / 2) + " " + (wire.endPoint.X / 2) + " " + (wire.endPoint.Y / 2) + "\n";
        }

        foreach (component component in components)
        {
            if (component is resistor)
            {

                spiceString += "SYMBOL " + ((resistor)component).name + " " + ((((resistor)component).location.X - 15) / 30 * 16) + " " + ((((resistor)component).location.Y - 15) / 30 * 16) + " R" + ((resistor)component).rotation + "\n";
                spiceString += "SYMATTR InstName R" + components.IndexOf(component) + "\n";
            }

            if (component is inductance)
            {
                spiceString += "SYMBOL " + ((inductance)component).name + " " + ((((inductance)component).location.X - 15) / 30 * 16) + " " + ((((inductance)component).location.Y - 15) / 30 * 16) + " R" + ((inductance)component).rotation + "\n";
                spiceString += "SYMATTR InstName L" + components.IndexOf(component) + "\n";
            }

            if (component is capacitor)
            {
                spiceString += "SYMBOL " + ((capacitor)component).name + " " + ((((capacitor)component).location.X - 15) / 30 * 16) + " " + ((((capacitor)component).location.Y - 15) / 30 * 16 + 16) + " R" + ((capacitor)component).rotation + "\n";
                spiceString += "SYMATTR InstName C" + components.IndexOf(component) + "\n";
            }

            if (component is diode)
            {
                spiceString += "SYMBOL " + ((diode)component).name + " " + ((((diode)component).location.X - 15) / 30 * 16) + " " + ((((diode)component).location.Y - 15) / 30 * 16 + 16) + " R" + ((diode)component).rotation + "\n";
                spiceString += "SYMATTR InstName D" + components.IndexOf(component) + "\n";
            }
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(spiceString);
            }
        }
        catch (Exception exp)
        {
            Console.Write(exp.Message);
        }
    }

    private void decodeFile(Canvas canvas, string path)
    {
        string[] data = File.ReadAllLines(path);
        bool containsSheet = false;
        if (data[1].Contains("SHEET"))
            containsSheet = true;

        Point location = new Point();
        int rotation;

        if (containsSheet)
        {
            for (var i = 2; i < data.Length; i++)
            {
                if (data[i].Contains("res"))
                {
                    resistor res = new resistor(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        res.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        res.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("cap"))
                {
                    capacitor cap = new capacitor(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        cap.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        cap.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("diode"))
                {
                    diode dio = new diode(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        dio.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        dio.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("ind"))
                {
                    inductance ind = new inductance(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        ind.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        ind.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
            }
        }
        else
        {
            for (var i = 1; i < data.Length; i++)
            {
                if (data[i].Contains("res"))
                {
                    resistor res = new resistor(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        res.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        res.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("cap"))
                {
                    capacitor cap = new capacitor(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        cap.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        cap.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("diode"))
                {
                    diode dio = new diode(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        dio.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        dio.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
                if (data[i].Contains("ind"))
                {
                    inductance ind = new inductance(canvas);
                    string[] substrings = data[i].Split(' ');
                    location.X = Convert.ToDouble(substrings[2]);
                    location.Y = Convert.ToDouble(substrings[3]);
                    rotation = Convert.ToInt32(substrings[4].Substring(1));

                    if (SettingsPage.theme == "Dark")
                    {
                        ind.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                    }
                    else if (SettingsPage.theme == "Light")
                    {
                        ind.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                    }
                }
            }
        }

    }
}
