using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI.Xaml.Controls;
using components.Miscellaneous;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace components.Components;

public abstract class component
{
    public static readonly List<int> indexes = new();
    public abstract List<Point> pins { get; set; }
    public abstract List<Ellipse> pads { get; set; }
    public static int count { get; set; }

    public abstract List<Point> drawComponent(Point location, int rotation, SolidColorBrush color);
    public abstract void deleteComponent();
    public abstract List<Point> moveComponent(Point location, int rotation, SolidColorBrush color);
}
