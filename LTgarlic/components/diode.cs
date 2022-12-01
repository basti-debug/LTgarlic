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
    public class diode : component
    {
        string name = "diode";
        Point location;
        int rotation;
        Canvas drawingTable;
        int sizeDiv = 2;
        int pinlength = 100;

        int width = 100;
        int height = 100;

        public diode(Point location, int rotation, Canvas drawingTable)
        {
            this.location = location;
            this.rotation = rotation;
            this.drawingTable = drawingTable;
        }

        public override List<Point> drawComponent()
        {
            Path myPath = new Path();
            myPath.Stroke = new SolidColorBrush(Colors.Black);
            myPath.StrokeThickness = 3;
            myPath.StrokeEndLineCap = PenLineCap.Round;
            myPath.StrokeStartLineCap= PenLineCap.Round;

            Point pin1 = new Point(location.X + width / 2 / sizeDiv, location.Y - pinlength / sizeDiv);
            Point pin2 = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv + pinlength / sizeDiv);

            LineGeometry l1 = new LineGeometry()
            {
                StartPoint = location,
                EndPoint = new Point(location.X + width/sizeDiv, location.Y),
            };
            LineGeometry l2 = new LineGeometry()
            {
                StartPoint = location,
                EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv),
            };
            LineGeometry l3 = new LineGeometry()
            {
                StartPoint = new Point(location.X + width / sizeDiv, location.Y),
                EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv),
            };
            LineGeometry l4 = new LineGeometry()
            {
                StartPoint = new Point(location.X, location.Y + height / sizeDiv),
                EndPoint = new Point(location.X + width / sizeDiv, location.Y + height / sizeDiv)
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

            GeometryGroup diode = new GeometryGroup();
            diode.Children.Add(l1);
            diode.Children.Add(l2);
            diode.Children.Add(l3);
            diode.Children.Add(l4);
            diode.Children.Add(pinline1);
            diode.Children.Add(pinline2);

            myPath.Data = diode;

            RotateTransform center = new RotateTransform();
            center.Angle = rotation;
            center.CenterX = location.X + width / 2 / sizeDiv;
            center.CenterY = location.Y + height / 2 / sizeDiv;

            myPath.RenderTransform = center;

            drawingTable.Children.Add(myPath);

            List<Point> Pins = new List<Point> {pin1,pin2};

            return Pins;
        }
    }
}
