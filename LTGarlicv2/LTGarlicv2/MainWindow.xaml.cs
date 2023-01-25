// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using System.Diagnostics;
using components.Components;
using Microsoft.UI;
using Windows.UI.ApplicationSettings;
using LTgarlic.Components.Miscellaneous;
using Microsoft.UI.Xaml.Shapes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.


//COLOR
//private SolidColorBrush accent = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);

namespace LTGarlicv2
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static bool wireMode = false;
        public MainWindow()
        {

            this.InitializeComponent();
            

            MainLTWindow.ExtendsContentIntoTitleBar = true;
            MainLTWindow.SetTitleBar(null);

            PageBuilder start = new PageBuilder();

            start.displayMainPage(contentFrame).Click += creatButtononClick;
            nvHamburgerleft.SelectionChanged += NvSample_SelectionChanged;
        }

        void NvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;
            PageBuilder newpage = new PageBuilder();

            var currentselected = item.Content; 
            

            if (item.Tag != null && item.Tag.Equals("MainItem"))
            {
                newpage.displayMainPage(contentFrame);
            }
            else
            {
                string name = item.Content.ToString();
                newpage.displayFilePage(contentFrame, name);
            }
            
            
        }

        async void  creatButtononClick(object sender, RoutedEventArgs args)
        {
            TeachingTip errortip = new TeachingTip();

            //var hwnd = 

            //var picker = new FileSavePicker();
            //WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            //picker.FileTypeChoices.Add("Spice Circuit", new List<string>() { ".asc" });
            //picker.SuggestedFileName = "cicuit";
            //var file = await picker.PickSaveFileAsync();

            //var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            //savePicker.SuggestedStartLocation =
            //    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            //savePicker.FileTypeChoices.Add("Spice Circuit", new List<string>() { ".asc" });
            //savePicker.SuggestedFileName = "circuit1";

            //Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            //if (file != null)
            //{
            //     // Prevent updates to the remote version of the file until
            //    // we finish making changes and call CompleteUpdatesAsync.
            //    Windows.Storage.CachedFileManager.DeferUpdates(file);
            //    // write to file
            //    await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
            //    // Let Windows know that we're finished changing the file so
            //    // the other app can update the remote version of the file.
            //    // Completing updates may require Windows to ask for user input.
            //    Windows.Storage.Provider.FileUpdateStatus status =
            //    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            //    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            //    {
            //        errortip.Title = "File Saved";
            //        errortip.Subtitle = "File " + file.Name + " was saved. You can open it on the left side";
            //        errortip.IsOpen = true;
            //    }
            //    else
            //    {
            //        errortip.Title = "File couldn't Saved";
            //        errortip.Subtitle = "File " + file.Name + " wasnt saved, please retry";
            //        errortip.IsOpen = true;
            //    }
            //}
            //else
            //{
            //    errortip.Title = "A error accoured ";
            //    errortip.IsOpen = true;
            //}



            NavigationViewItem newproject = new NavigationViewItem();
            newproject.Content = "file.Name";  // to be replaced with file name
            nvHamburgerleft.MenuItems.Add(newproject);

        }

        private List<List<Ellipse>> pads = new();
        public static List<wire> allWires = new();
        public List<Ellipse> connections = new();
        private double gridSize = 30;

        public static int wireClickCnt = 0;
        public static bool wireStart = false;
        public static Point startPoint = new();
        public static Point endPoint = new();

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
        }
    }
}
