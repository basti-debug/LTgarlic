using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace components
{
    internal class diode : component
    {
        

        string name;
        Point location;
        int rotation;

        public diode(string name, Point location, int rotation)
        {
            this.name = name;
            this.location = location;
            this.rotation = rotation;
        }

        public override void drawComponent(Canvas drawingTable)
        {
            Polygon triangle = new Polygon
            {
                Points = "",
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                Fill = new SolidColorBrush(Colors.Transparent)
            }
        }
    }
}
