using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using LTgarlic.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using LTgarlic;
using LTgarlic.Helpers;
using Microsoft.UI.Xaml.Printing;
using LTgarlic.Views;
using System.Net;

namespace LTgarlic.Views;


public sealed partial class MainHomePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }


    public MainHomePage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }  

    public async void createbutton_Click(object sender, RoutedEventArgs e)
    {
        var hwnd = App.MainWindow.GetWindowHandle();

        var picker = new FileSavePicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeChoices.Add("Spice Circuit", new List<string>() { ".asc" });
        picker.SuggestedFileName = "cicuit";
        var file = await picker.PickSaveFileAsync();

        var drawcanvas = new Canvas();
        var hotbar = new CommandBar();

        var wirebutton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Edit),
            Label = "wire mode"
        };

        var simbutton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Calculator),
            Label = "simulate"
        };

        hotbar.PrimaryCommands.Add(wirebutton);
        hotbar.SecondaryCommands.Add(simbutton);

        

        //App.MainWindow.Content = hotbar;
        //App.MainWindow.Close();


        //NavigationViewItem newItem = new NavigationViewItem();
        //newItem.Content = "New Item";
        //newItem.Icon = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE74C" };
        //newItem.AddHandler(NavigationViewItem.PointerPressedEvent, new PointerEventHandler(OnNewItemClicked), true);
        //NavigationViewControl.MenuItems.Add(newItem);

        //private void OnNewItemClicked(object sender, PointerRoutedEventArgs e)
        //{
        //    NavigationViewItem selectedItem = sender as NavigationViewItem;
        //    if (selectedItem != null)
        //    {
        //        // Navigate to the corresponding view and view model for the new item
        //    }
        //}


        //LTgarlic.Views.ShellPage.Navigation
        //NavigationViewControl.Items.Add(newItem);

        //NavigationHelper.SetNavigateTo(newItem, typeof(MainViewModel).FullName);
    }




    private async void openbutton_Click(object sender, RoutedEventArgs e)
    {
        var hwnd = App.MainWindow.GetWindowHandle();

        var picker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeFilter.Add(".asc");
        var file = await picker.PickSingleFileAsync();
    }

    private async void importlbrbutton_Click(object sender, RoutedEventArgs e)
    {
        var hwnd = App.MainWindow.GetWindowHandle();

        var picker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.SuggestedStartLocation = PickerLocationId.Downloads;
        picker.FileTypeFilter.Add(".asy");
        picker.FileTypeFilter.Add(".lib");
        picker.FileTypeFilter.Add(".sub");
        var file = await picker.PickSingleFileAsync();
    }
}
