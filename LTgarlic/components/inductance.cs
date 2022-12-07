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
    public class inductance : component
    {
        private readonly int height = 200;
        private readonly int width = 100;
        private readonly int pinlength = 100;
        private readonly int sizeDiv = 2;

        public readonly string name = "ind";
        private Canvas drawingTable;

        private static List<int> indexes;
        private int index;
        private static int count;

        public inductance(Point location, int rotation, Canvas drawingTable) 
        {
            this.drawingTable = drawingTable;

            index = count++;
            indexes.Add(index);
        }

        public override List<Point> drawComponent(Point location, int rotation)
        {
            Point pin1 = new Point(location.X + width / 2 / sizeDiv, location.Y - pinlength / sizeDiv);
            Point pin2 = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv + pinlength / sizeDiv);

            Path myPath = new Path();
            myPath.Stroke = new SolidColorBrush(Colors.Black);
            myPath.StrokeThickness = 3;
            myPath.StrokeEndLineCap = PenLineCap.Round;
            myPath.StrokeStartLineCap = PenLineCap.Round;
            myPath.Fill = new SolidColorBrush(Colors.Black);

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

        public override void deleteComponent()
        {
            count--;
            drawingTable.Children.RemoveAt(indexes.IndexOf(index));
            indexes.RemoveAt(index);
        }

        public override List<Point> moveComponent(Point location, int rotation)
        {
            deleteComponent();
            List<Point> pins = new();
            pins = drawComponent(location, rotation);
            return pins;
        }
    }
}
