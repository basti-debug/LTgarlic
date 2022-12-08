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
using WinRT.Interop;

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
        var hwnd = App.MainWindow.GetWindowHandle();

        var picker = new FileSavePicker();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeChoices.Add("Spice Circuit", new List<string>() { ".asc" });
        picker.SuggestedFileName = "cicuit";
        var file = await picker.PickSaveFileAsync();

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