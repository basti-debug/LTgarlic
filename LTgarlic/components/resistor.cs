using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace components
{
    internal class resistor : component
    {
        private int resWidth = 100;
        private int resHeight = 50;

        string name;
        Point location;
        int rotation;

        public resistor(string name, Point location, int rotation)
        {
            this.name = name;
            this.location = location;
            this.rotation = rotation;
        }

        public override void drawComponent(Canvas drawingTable)
        {
            Rectangle resistor = new Rectangle
            {
                Width = resWidth,
                Height = resHeight,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3,
                Fill = new SolidColorBrush(Colors.Transparent)
            };
            Canvas.SetBottom(resistor, location.Y);
            Canvas.SetLeft(resistor, location.X);
            resistor.RenderTransform = new RotateTransform(rotation);

            drawingTable.Children.Add(resistor);
        }
    }
}
