// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using System.Windows;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
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
using Microsoft.UI;
using Windows.UI.ApplicationSettings;
using components;
using LTgarlic.Components.Miscellaneous;
using components.Components;
using Windows.System;
using Windows.UI;
using Microsoft.Win32;
using System.Formats.Asn1;
using System.Collections;

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
        public MainWindow()
        {

            this.InitializeComponent();
            
            // Custom TitleBar

            MainLTWindow.ExtendsContentIntoTitleBar = true;
            MainLTWindow.SetTitleBar(null);

            newpage.displayMainPage(contentFrame).Click += creatButtononClick; //Createbutton Handler
            

            nvHamburgerleft.SelectionChanged += NvSample_SelectionChanged; //SelectionChanged Handler
        }

        #region switch pages 
        void NvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;
            
            

            if (item.Tag != null && item.Tag.Equals("MainItem"))
            { 
                newpage.displayMainPage(contentFrame).Click += creatButtononClick;
            }
            else
            {
                string name = item.Content.ToString();
                newpage.displayFilePage(contentFrame, name,mainLtGrid,MainLTWindow);

            }            
            
        }

        #endregion

        async void addbutton_click(object sender, RoutedEventArgs args)
        {
            
        }

        async void wirebutton_click(object sender, RoutedEventArgs args)
        {

        }
        async void savebutton_lick(object sender, RoutedEventArgs args)
        {

        }

        async void  creatButtononClick(object sender, RoutedEventArgs args)
        {
            TeachingTip errortip = new TeachingTip();

            ContentDialog filenamedig = new ContentDialog();

            filenamedig.XamlRoot = mainLtGrid.XamlRoot;
            filenamedig.Title = "Create a File";
            filenamedig.PrimaryButtonText = "Create";
            filenamedig.CloseButtonText = "Discard";
            filenamedig.DefaultButton = ContentDialogButton.Primary;

            TextBox filnamebox = new TextBox();          

            filenamedig.Content = filnamebox;

            try
            {
                ContentDialogResult result = await filenamedig.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // The user pressed the OK button
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


            NavigationViewItem newproject = new NavigationViewItem();
            newproject.Content = filnamebox.Text;  // to be replaced with file name
            nvHamburgerleft.MenuItems.Add(newproject);
        }
    }
}
