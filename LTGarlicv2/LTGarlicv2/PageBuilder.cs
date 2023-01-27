using components.Components;
using LTgarlic.Components.Miscellaneous;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Core;
using Microsoft.UI;
using Windows.UI.ApplicationSettings;
using System.Security.Cryptography;
using Windows.UI.Text;
using Microsoft.UI.Text;
using System.IO;
using Windows.System;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using WinRT;
using System.Runtime.InteropServices;

namespace LTGarlicv2
{
    public class PageBuilder
    {
        #region vars

        // standard grid 
        Grid a = new Grid();


        // for componentcreation

        public static readonly List<component> components = new();
        private int rotation;
        private int clickCounter = 0;
        private bool placeComponentSelected = false;
        private bool firstAccessComponent = true;

        private List<List<Ellipse>> pads = new();
        public static List<wire> allWires = new();
        public List<Ellipse> connections = new();
        private double gridSize = 30;

        public static int wireClickCnt = 0;
        public static bool wireStart = false;
        public static Point startPoint = new();
        public static Point endPoint = new();

        private Point actualMousePos = new();
        private Point gridMousePos = new();
        public static bool oneLineUsed;
        private bool firstWireAccess = true;

        private bool wireContinues = false;

        // accent color 

        private SolidColorBrush accent = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);


        // Theme

        public static string theme = "Default";

        // for UI navigation
        private Frame usedcurrentframe = null;
        private Canvas usedcanvas = null;
        private Window usedwindow = null;
        private NavigationView nvham = null;

        // default saving location

        public string buffersavingloc;
        public string defaultsavinglocation = @"C:\Users\gabri\Downloads\firstCircuit.asc";

        // window handlers

        public IntPtr hwnd;

        #endregion

        #region Pages


        public void displayFilePage(Frame currentframe, string filename, Grid b, Window window)
        {
            usedwindow = window;
            usedcurrentframe = currentframe;
            

            a = b;
            InfoBar infoBar= new InfoBar();
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.Title = "Opened successfull";
            infoBar.Message = "Your File "+ filename + " was opened correctly ";
            infoBar.IsOpen= true;

            Canvas.SetLeft(infoBar, 450);
            Canvas.SetTop(infoBar, 890);


            Canvas canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Colors.Transparent);
            canvas.PointerMoved += drawingTable_PointerMoved;
            canvas.PointerPressed += drawingTable_PointerPressed;
            canvas.DoubleTapped += drawingTable_DoubleTapped;            

            usedcanvas = canvas;


            // Heading 
            TextBlock Title = new TextBlock();
            Title.Text = filename;
            Title.FontSize = 30;

            Canvas.SetLeft(Title, 50);
            Canvas.SetTop(Title, 100);


            // hotbar 

            var hotbar = new CommandBar();
            
            Canvas.SetLeft(hotbar, 150);
            Canvas.SetTop(hotbar, 100);



            #region HotbarItems

            var wirebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Edit),
                Label = "wire mode"
            };
            wirebutton.Click += wiremode_Click;

            var simbutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Calculator),
                Label = "simulate"
            };
            simbutton.Click += Simbutton_Click;

            var placmentbutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Add),
                Label = "add Parts"
            };
            placmentbutton.Click += Placmentbutton_Click;

            var rotatebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Rotate),
                Label = "rotate Parts"
            };
            rotatebutton.Click += rotate_Click;

            var savebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Save),
                Label = "save"
            };
            savebutton.Click += save_Click;

            var changetotop = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Switch),
                Label = "use top bar"
            };

            hotbar.PrimaryCommands.Add(wirebutton);
            hotbar.PrimaryCommands.Add(simbutton);
            hotbar.PrimaryCommands.Add(placmentbutton);
            hotbar.PrimaryCommands.Add(rotatebutton);
            hotbar.PrimaryCommands.Add(savebutton);
            hotbar.SecondaryCommands.Add(changetotop);

            #endregion

            canvas.Children.Add(infoBar);
            canvas.Children.Add(Title);
            canvas.Children.Add(hotbar);

            currentframe.Content = canvas;

        }

        public void displayMainPage(Frame currentframe, Window window, NavigationView nv)
        {
            usedwindow = window;
            nvham = nv;

            Canvas mainCanva = new Canvas();
            mainCanva.Height = 2000;
            mainCanva.Width = 2000;

            usedcanvas= mainCanva;

            TextBlock Title = new TextBlock();
            Title.Text = "Welcome to LTGarlic";
            Title.FontSize = 30;

            Button createbutton = new Button();
            createbutton.Content = "Create a new File";
            createbutton.Click += createButtononClick;


            Button openbutton = new Button();
            openbutton.Content = "Open a File";
            openbutton.Background = accent;
            openbutton.Click += Openbutton_Click;

            Button openlib = new Button();
            openlib.Content = "Open a Libary";


            // Moving Objects inside the Canvas
            Canvas.SetLeft(Title, 50);
            Canvas.SetTop(Title, 100);

            Canvas.SetLeft(createbutton, 50);
            Canvas.SetTop(createbutton, 200);

            Canvas.SetLeft(openbutton, 190);
            Canvas.SetTop(openbutton, 200);

            Canvas.SetLeft(openlib, 295);
            Canvas.SetTop(openlib, 200);

            mainCanva.Children.Add(createbutton);
            mainCanva.Children.Add(openbutton);
            mainCanva.Children.Add(openlib);
            mainCanva.Children.Add(Title);

            currentframe.Content = mainCanva;

        }

        

        TextBox defaultPathbox = new TextBox();
        public void displaySettings(Frame currentframe)
        {


            StackPanel stacksettings = new StackPanel();

            TextBlock Title = new TextBlock();
            Title.Text = "Settings";
            Title.FontSize = 30;
            Title.Padding = new Thickness(50,100,0,0);

            #region Info1 - Theme

            TextBlock info1 = new TextBlock();
            info1.Text = "Change Theme";
            info1.FontSize = 15;
            info1.FontWeight = FontWeights.Bold;
            info1.Padding = new Thickness(50,30,0,0);

            StackPanel themepanel = new StackPanel();
            themepanel.Padding = new Thickness(50,10,0,0);

            RadioButton darkstate = new RadioButton();
            darkstate.Content = "Dark Mode";
            if (theme == "Dark")
            {
                darkstate.IsChecked = true;
            }
            else
            {
                darkstate.IsChecked = false;
            }
            darkstate.Click += Darkstate_Click;

            RadioButton lightstate = new RadioButton();
            lightstate.Content = "Light Mode";
            if (theme == "Light")
            {
                lightstate.IsChecked = true;
            }
            else
            {
                lightstate.IsChecked = false;
            }
            lightstate.Click += Lightstate_Click;

            RadioButton defaultstate = new RadioButton();    
            defaultstate.Content = "Default";
            if (theme == "Default")
            {
                defaultstate.IsChecked= true;
            }
            else
            {
                defaultstate.IsChecked= false;
            }
            defaultstate.Click += Defaultstate_Click;

            themepanel.Children.Add(darkstate);
            themepanel.Children.Add(lightstate);
            themepanel.Children.Add(defaultstate);

            #endregion


            stacksettings.Children.Add(Title);
            stacksettings.Children.Add(info1);
            stacksettings.Children.Add(themepanel);

            #region Info2 - DefaultPath

            TextBlock info2 = new TextBlock();
            info2.Text = "Default Saving Path";
            info2.FontSize = 15;
            info2.FontWeight = FontWeights.Bold;
            info2.Padding = new Thickness(50, 30, 0, 0);

            defaultPathbox.Text = defaultsavinglocation;


            Button saveconfigbutton = new Button();
            saveconfigbutton.Content = "Save Configuration";
            saveconfigbutton.Click += savecoClick;

           


            stacksettings.Children.Add(info2);
            stacksettings.Children.Add(defaultPathbox);
            stacksettings.Children.Add(saveconfigbutton);
#endregion


            currentframe.Content = stacksettings;
        }



        #endregion

        #region Events

        #region Settings Buttons Events 

        private void savecoClick(object sender, RoutedEventArgs e)
        {
            defaultsavinglocation = defaultPathbox.Text;
        }

        private void Defaultstate_Click(object sender, RoutedEventArgs e)
        {
            theme = "Default";

        }

        private void Lightstate_Click(object sender, RoutedEventArgs e)
        {
            theme = "Light";
        }

        private void Darkstate_Click(object sender, RoutedEventArgs e)
        {
            theme = "Dark";
        }
        #endregion

        #region main Events 

        async void createButtononClick(object sender, RoutedEventArgs args)
        {
            FileSavePicker fp = new FileSavePicker();

            fp.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            fp.FileTypeChoices.Add("Spice Circuit", new List<string>() { ".asc" });

            Random rnd = new Random();
            int num = 0;
            for (int j = 0; j < 1; j++)
            {
                num = rnd.Next(10);
            }

            string filename = "Circuit" + num;

            fp.SuggestedFileName = filename;

            WinRT.Interop.InitializeWithWindow.Initialize(fp, hwnd);

            var file = await fp.PickSaveFileAsync();

            if (file != null)
            {
                TeachingTip titi = new TeachingTip();
                titi.Content = "Your File " + file + " was created";
                buffersavingloc = file.Path;
            }
            else
            {
                Debug.WriteLine("open file abort");
            }
            NavigationViewItem newproject = new NavigationViewItem();
            newproject.Content = file.Name;
            newproject.Tag = "addedPage";
            nvham.MenuItems.Add(newproject);
        
        } 

        private async void Openbutton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker op = new FileOpenPicker();

            op.ViewMode = PickerViewMode.List;
            op.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            op.FileTypeFilter.Add(".asc");

            WinRT.Interop.InitializeWithWindow.Initialize(op, hwnd);

            var file = await op.PickSingleFileAsync();

            if (file != null)
            {
               TeachingTip titi = new TeachingTip();
                titi.Content= "Your File " + file + " was opened";
                buffersavingloc = file.Path;
            }
            else
            {
                Debug.WriteLine("open file abort");
            }
        }

        #endregion

        #region Editing Pages Events

        private async void Placmentbutton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog filenamedig = new ContentDialog();

            filenamedig.XamlRoot = a.XamlRoot;
            filenamedig.Title = "Add Parts";
            filenamedig.PrimaryButtonText = "Place";
            filenamedig.CloseButtonText = "Discard";
            filenamedig.DefaultButton = ContentDialogButton.Primary;

            StackPanel ff = new StackPanel();

            TextBlock info = new TextBlock();
            info.Text = "Select Parts:";
            info.Margin = new Thickness(0, 10, 0, 10);

            ListView partsview = new ListView();
            List<string> partslist = new List<string>();
            partslist.Add("Resistor");
            partslist.Add("Capacitor");
            partslist.Add("Inductance");
            partslist.Add("Diode");

            partsview.ItemsSource = partslist;

            ff.Children.Add(info);
            ff.Children.Add(partsview);



            filenamedig.Content = ff;

            try
            {
                ContentDialogResult result = await filenamedig.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // look which item is selected
                    // !! ADD PARTS HERE

                    if (partsview.SelectedItem == "Resistor")
                    {
                        components.Add(new resistor(usedcanvas));
                    }
                    if (partsview.SelectedItem == "Inductance")
                    {
                        components.Add(new inductance(usedcanvas));
                    }
                    if (partsview.SelectedItem == "Capacitor")
                    {
                        components.Add(new capacitor(usedcanvas));
                    }
                    if (partsview.SelectedItem == "Diode")
                    {
                        components.Add(new diode(usedcanvas));
                    }

                    rotation = 0;
                    placeComponentSelected = true;
                    firstAccessComponent = true;

                }
                else if (result == ContentDialogResult.Secondary)
                {
                    // The user pressed the Cancel button
                }
            }
            catch (ArgumentException ex)
            {
                // Handle the exception here
            }
        }

        private void Simbutton_Click(object sender, RoutedEventArgs e)
        {
        }
        public void wiremode_Click(object sender, RoutedEventArgs args)
        { 
            MainWindow.wireMode = !MainWindow.wireMode;
            if (MainWindow.wireMode == false)
            {
                wireStart = false;
                wireClickCnt = 0;

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
        }
        public void rotate_Click(object sender, RoutedEventArgs args)
        {

        }

        public async void save_Click(object sender, RoutedEventArgs args)
        {
            ContentDialog filenamedig = new ContentDialog();

            filenamedig.XamlRoot = a.XamlRoot;
            filenamedig.Title = "Save File";
            filenamedig.PrimaryButtonText = "Save";
            filenamedig.CloseButtonText = "Discard";
            filenamedig.DefaultButton = ContentDialogButton.Primary;

            StackPanel ff = new StackPanel();
            
            TextBlock info = new TextBlock();
            info.Text = "Do you want to save the Project? ";
            info.Margin = new Thickness(0, 10, 0, 10);
           

            ff.Children.Add(info);

            
            filenamedig.Content = ff;



            try
            {
                ContentDialogResult result = await filenamedig.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // The user pressed the OK button
                    spiceConverter.encodeFile(defaultsavinglocation);
                    
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    // The user pressed the Cancel button
                }
            }
            catch (ArgumentException ex)
            {
                // Handle the exception here
            }
        }

        private void drawingTable_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            if (e.GetCurrentPoint(usedcurrentframe).Properties.IsLeftButtonPressed)
            {
                #region placeComponent
                if (placeComponentSelected)
                    placeComponentSelected = false;
                #endregion

                #region wireMode

                //wireMode is enabled, if w has been pressed. This means, by leftclicking, a wire can be drawn.
                else if (MainWindow.wireMode)
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
                                                usedcanvas.Children.Add(connection);
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
                                                usedcanvas.Children.Add(connection);
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
                                                usedcanvas.Children.Add(connection);
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
                                                usedcanvas.Children.Add(connection);
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
                                                usedcanvas.Children.Add(connection);
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
                                                usedcanvas.Children.Add(connection);
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

            if (e.GetCurrentPoint(usedcurrentframe).Properties.IsRightButtonPressed && MainWindow.wireMode)
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
            if (MainWindow.wireMode)
            {
                MainWindow.wireMode = false;
                wireStart = false;
                wireClickCnt = 0;
                allWires[allWires.Count - 1].deleteWire();
                allWires[allWires.Count - 2].deleteWire();
            }
            #endregion
        }

        private void drawingTable_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            actualMousePos = e.GetCurrentPoint(usedcanvas).Position;

            #region grid Size
            gridMousePos.X = actualMousePos.X + (gridSize - (actualMousePos.X % gridSize)) - gridSize / 2;
            gridMousePos.Y = actualMousePos.Y + (gridSize - (actualMousePos.Y % gridSize)) - gridSize / 2;
            #endregion

            #region move components
            if (theme == "Dark")
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

            else if (theme == "Light")
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
            else if (theme == "Default")
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
            if (MainWindow.wireMode && wireStart)
            {
                Debug.WriteLine("main hubbsi");
                if (startPoint != gridMousePos)
                {
                    Debug.WriteLine("main nigga");
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
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                            oneLineUsed = true;
                        }
                        else if (startPoint.X == gridMousePos.X)//vertical
                        {
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                            oneLineUsed = true;
                        }
                        else
                        {
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, new Point(gridMousePos.X, startPoint.Y), accent);
                            allWires.Add(new wire(usedcanvas));
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
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                            oneLineUsed = true;
                        }
                        else if (startPoint.X == gridMousePos.X)//vertical
                        {
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, gridMousePos, accent);
                            oneLineUsed = true;
                        }
                        else
                        {
                            allWires.Add(new wire(usedcanvas));
                            allWires.Add(new wire(usedcanvas));
                            allWires[allWires.Count - 1].drawWire(startPoint, new Point(startPoint.X, gridMousePos.Y), accent);
                            allWires[allWires.Count - 2].drawWire(new Point(startPoint.X, gridMousePos.Y), gridMousePos, accent);
                            oneLineUsed = false;
                        }
                    }
                }

            }
            #endregion
        }

        #endregion

        #endregion

        #region Helpers

        public void transferhwnd(IntPtr ahwnd)
        {
            hwnd = ahwnd;
        }

        #endregion
    }


}
