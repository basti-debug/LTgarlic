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
    public class capacitor : component
    {
        string name = "cap";
        Point location;
        int rotation;
        Canvas drawingTable;
        int sizeDiv = 2;
        int pinlength = 100;

        int width = 150;
        int height = 50;
        int conHeight = 15;

        public capacitor(Point location, int rotation, Canvas drawingTable) 
        {
            this.location = location;
            this.rotation = rotation;
            this.drawingTable = drawingTable;
        }

        public override List<Point> drawComponent()
        {
            Point pin1 = new Point(location.X + width / 2 / sizeDiv,location.Y - pinlength / sizeDiv);
            Point pin2 = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv + pinlength / sizeDiv);

            Path myPath = new Path();
            myPath.Stroke = new SolidColorBrush(Colors.Black);
            myPath.StrokeThickness = 3;
            myPath.Fill = new SolidColorBrush(Colors.Black);

            RectangleGeometry con1 = new RectangleGeometry()
            {
                Rect = new Rect(location.X, location.Y, width / sizeDiv, conHeight / sizeDiv)
            };
            RectangleGeometry con2 = new RectangleGeometry()
            {
                Rect = new Rect(location.X, location.Y + height / sizeDiv - conHeight / sizeDiv, width / sizeDiv, conHeight / sizeDiv)
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

            GeometryGroup capacitor = new GeometryGroup();
            capacitor.Children.Add(con1);
            capacitor.Children.Add(con2);
            capacitor.Children.Add(pinline1);
            capacitor.Children.Add(pinline2);

            myPath.Data = capacitor;

            RotateTransform center = new RotateTransform();
            center.Angle = rotation;
            center.CenterX = location.X + width / 2 / sizeDiv;
            center.CenterY = location.Y + height / 2 / sizeDiv;

            myPath.RenderTransform = center;

            drawingTable.Children.Add(myPath);

            List<Point> Pins = new List<Point>() { pin1, pin2 };

            return Pins;
        }
    }
}
