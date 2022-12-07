using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI.Xaml.Controls;

namespace components;

public abstract class component
{
    public abstract List<Point> drawComponent(Point location, int rotation);
    public abstract void deleteComponent();
    public abstract List<Point> moveComponent(Point location, int rotation);
}
