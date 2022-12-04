using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace LTgarlic.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
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
        FolderPicker openPicker= new FolderPicker();
        openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        openPicker.FileTypeFilter.Add(".asc");

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

        var file = await openPicker.PickSingleFolderAsync();

        if(file != null )
        {
            // FILE PICKED
            Debug.WriteLine("haspicked a file");
        }
        
    }

    private void openbutton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void importlbrbutton_Click(object sender, RoutedEventArgs e)
    {

    }
}
