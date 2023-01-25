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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LTGarlicv2
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
       
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
                Debug.WriteLine("Here we are");
                newpage.displayMainPage(contentFrame);
            }
            else
            {
                Debug.WriteLine("not correct" + currentselected+"..." + item.Content +"------"+ item.Tag);
                newpage.displayFilePage(contentFrame, TitleBlock, "");
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
            newproject.Content = "file.Name";
            nvHamburgerleft.MenuItems.Add(newproject);

        }

    }
}
