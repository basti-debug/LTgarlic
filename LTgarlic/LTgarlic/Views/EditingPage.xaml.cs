using LTgarlic.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace LTgarlic.Views;

public sealed partial class EditingPage : Page
{
    public EditingViewModel ViewModel
    {
        get;
    }

    public EditingPage()
    {
        ViewModel = App.GetService<EditingViewModel>();
        InitializeComponent();
    }
}
