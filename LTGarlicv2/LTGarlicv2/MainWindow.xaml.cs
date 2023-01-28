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
using Windows.Storage;
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

#region variables wiring

        public static bool wireMode = false;

        public static List<wire> allWires = new();
        public List<Ellipse> connections = new();

        public static int wireClickCnt = 0;
        public static bool wireStart = false;
        public static Point startPoint = new();
        public static Point endPoint = new();

        public static bool oneLineUsed;

#endregion


        // variables grid frame and handler
        public static Grid mmgrid = null;
        public static Frame ffFrame = null;
        public static Canvas mmcanvas = null;
        public IntPtr hwnd;

        // variables for file 

        public static StorageFile mcurrentfile = null;



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
            mmgrid = mainLtGrid;
            ffFrame = contentFrame;
        }

        #region helpers 

        public static Grid getwindowdata()
        {
            return mmgrid;
        }

        public static Frame GetFramedata()
        {
            return ffFrame;
        }

#endregion  



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
                
                //spiceConverter.decodeFile(mmcanvas, mcurrentfile.Path); // TOBE CHANGED @GABRIEL
            }            
            
        }

        #endregion

       

       
    }
}
