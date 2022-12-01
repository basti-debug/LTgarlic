using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace components
{
    public class resistor : component
    {
        private int height = 200;
        private int width = 100;
        int sizeDiv = 2;

        string name = "res";
        Point location;
        int rotation;
        Canvas drawingTable;
        int pinlength = 100;

        public resistor(Point location, int rotation, Canvas drawingTable)
        {
            this.location = location;
            this.rotation = rotation;
            this.drawingTable = drawingTable;
        }

        public override List<Point> drawComponent()
        {
            Point pin1 = new Point(location.X + width / 2 / sizeDiv, location.Y - pinlength / sizeDiv);
            Point pin2 = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv + pinlength / sizeDiv);

            Path myPath = new Path();
            myPath.Stroke = new SolidColorBrush(Colors.Black);
            myPath.StrokeThickness = 3;
            myPath.StrokeEndLineCap = PenLineCap.Round;
            myPath.StrokeStartLineCap = PenLineCap.Round;

            RectangleGeometry rect = new RectangleGeometry
            {
                Rect = new Rect(location.X, location.Y, width / sizeDiv, height / sizeDiv)
            };
            LineGeometry pinline1 = new LineGeometry()
            {
                StartPoint = pin1,
                EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y)
            };
            LineGeometry pinline2 = new LineGeometry()
            {
                StartPoint = pin2,
                EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv)
            };

            GeometryGroup resistor = new GeometryGroup();
            resistor.Children.Add(rect);
            resistor.Children.Add(pinline1);
            resistor.Children.Add(pinline2);

            myPath.Data = resistor;

            RotateTransform center = new RotateTransform();
            center.Angle = rotation;
            center.CenterX = location.X + width / 2 / sizeDiv;
            center.CenterY = location.Y + height / 2 / sizeDiv;

            drawingTable.Children.Add(myPath);

            List<Point> Pins = new List<Point> { pin1, pin2 };

            return Pins;
        }
    }
}
