using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using components.Components;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace components.Miscellaneous;
public class pins
{
    public Point location { get; set; }
    public int sizeDiv { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int pinlength { get; set; }
    public int rotation { get; set; }

    public Point pin1 { get; set; }
    public Point pin2 { get; set; }


    public pins()
    {

    }

    public List<LineGeometry> drawPins(Point location, int sizeDiv, int width, int height, int pinlength, int rotation)
    {
        this.location = location;
        this.sizeDiv = sizeDiv;
        this.width = width;
        this.height = height;
        this.pinlength = pinlength;
        this.rotation = rotation;

        pin1 = new Point(location.X + width / 2 / sizeDiv, location.Y - pinlength / sizeDiv);
        pin2 = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv + pinlength / sizeDiv);

        var pinline1 = new LineGeometry()
        {
            StartPoint = pin1,
            EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y)
        };
        var pinline2 = new LineGeometry()
        {
            StartPoint = pin2,
            EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv)
        };

        return new List<LineGeometry> { pinline1, pinline2 };
    }

    public List<Ellipse> getPads()
    {
        Ellipse pad1 = new Ellipse()
        {
            Width = 20 / sizeDiv,
            Height = 20 / sizeDiv,
            Stroke = new SolidColorBrush(Colors.LightBlue),
            StrokeThickness = 1,
            Fill = new SolidColorBrush(Colors.LightBlue),
        };

        Ellipse pad2 = new Ellipse()
        {
            Width = 20 / sizeDiv,
            Height = 20 / sizeDiv,
            Stroke = new SolidColorBrush(Colors.LightBlue),
            StrokeThickness = 1,
            Fill = new SolidColorBrush(Colors.LightBlue)
        };

        Canvas.SetTop(pad1, location.Y - pinlength / sizeDiv - pad1.Height / 2);
        Canvas.SetLeft(pad1, location.X + width / 2 / sizeDiv - pad1.Width / 2);

        Canvas.SetTop(pad2, location.Y + height / sizeDiv + pinlength / sizeDiv - pad2.Height / 2);
        Canvas.SetLeft(pad2, location.X + width / 2 / sizeDiv - pad2.Width / 2);

        var center = new RotateTransform();
        center.Angle = rotation;
        center.CenterX = location.X + width / 2 / sizeDiv;
        center.CenterY = location.Y + height / 2 / sizeDiv;

        pad1.RenderTransform = center;
        pad2.RenderTransform = center;

        List<Ellipse> padGroup = new List<Ellipse> { pad1, pad2 };

        return padGroup;
    }
}
