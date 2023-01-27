// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using LTgarlic.Components.Miscellaneous;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;

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
        PageBuilder newpage = new PageBuilder();

        public static bool wireMode = false;

        public static List<wire> allWires = new();
        public List<Ellipse> connections = new();

        public static int wireClickCnt = 0;
        public static bool wireStart = false;
        public static Point startPoint = new();
        public static Point endPoint = new();

        public static bool oneLineUsed;

        public IntPtr hwnd;

        public MainWindow()
        {

            this.InitializeComponent();
            
            // Custom TitleBar

            MainLTWindow.ExtendsContentIntoTitleBar = true;
            MainLTWindow.SetTitleBar(null);

            // Transfer Handle to PageBuilder (needed for fileopendialog)
            hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this); 
            newpage.transferhwnd(hwnd);

            //create a new page 
            newpage.displayMainPage(contentFrame,MainLTWindow,nvHamburgerleft);

            //SelectionChanged Handler
            nvHamburgerleft.SelectionChanged += NvSample_SelectionChanged; 

            contentFrame.KeyDown += ContentFrame_KeyDown;

            

        }

        private void ContentFrame_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine("main giga");
            if (e.Key == (VirtualKey)0x57)
            {
                Debug.WriteLine("main nigga");
                wireMode = !wireMode;
                if (wireMode == false)
                {
                    Debug.WriteLine("main false");
                    wireStart = false;
                    wireClickCnt = 0;

                    if (oneLineUsed)
                    {
                        PageBuilder.allWires[PageBuilder.allWires.Count - 1].deleteWire();
                        PageBuilder.allWires.Remove(PageBuilder.allWires[PageBuilder.allWires.Count - 1]);
                    }
                    else
                    {
                        Debug.WriteLine("main delete");
                        PageBuilder.allWires[PageBuilder.allWires.Count - 1].deleteWire();
                        PageBuilder.allWires[PageBuilder.allWires.Count - 2].deleteWire();
                        PageBuilder.allWires.Remove(PageBuilder.allWires[PageBuilder.allWires.Count - 1]);
                        PageBuilder.allWires.Remove(PageBuilder.allWires[PageBuilder.allWires.Count - 1]);
                    }
                }
            }
        }


        #region switch pages 
        void NvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;
            
            

            if (item.Tag != null && item.Tag.Equals("MainItem"))
            {
                newpage.displayMainPage(contentFrame,MainLTWindow,nvHamburgerleft);
            }
            if (item.Tag != null && item.Tag.Equals("Settings"))
            {
                newpage.displaySettings(contentFrame);
            }
            if (item.Tag != null && item.Tag.Equals("addedPage"))
            {
                string name = item.Content.ToString();
                newpage.displayFilePage(contentFrame, name,mainLtGrid,MainLTWindow);

            }            
            
        }

        #endregion

       

       
    }
}
