using LTgarlic.Helpers;

namespace LTgarlic;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
<<<<<<< HEAD
        public MainWindow()
        {
            InitializeComponent();

        }
=======
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
>>>>>>> uibasic
    }
}
