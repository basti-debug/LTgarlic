using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using ABI.Windows.UI;
using components.Components;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel.Store;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Security.Cryptography.Core;
using Windows.UI;

namespace LTgarlic.Components.Miscellaneous;
public class pins
{
    #region properties
    public Point location
    {
        get; set;
    }
    public int sizeDiv
    {
        get; set;
    }
    public int width
    {
        get; set;
    }
    public int height
    {
        get; set;
    }
    public int pinlength
    {
        get; set;
    }
    public int rotation
    {
        get; set;
    }

    public Point pin1
    {
        get; set;
    }
    public Point pin2
    {
        get; set;
    }

    #endregion

    #region constructor
    public pins(Point location, int sizeDiv, int width, int height, int pinlength, int rotation, Point pin1, Point pin2)
    {
        this.location = location;
        this.sizeDiv = sizeDiv;
        this.width = width;
        this.height = height;
        this.pinlength = pinlength;
        this.rotation = rotation;
        this.pin1 = pin1;
        this.pin2 = pin2;
    }
    #endregion

    #region getPads
    public Ellipse pad1 = new();
    public Ellipse pad2 = new();
    public List<Ellipse> getPads()
    {
        pad1.Width = 300 / sizeDiv;
        pad1.Height = 300 / sizeDiv;
        pad1.Stroke = new SolidColorBrush(Colors.Transparent);
        pad1.StrokeThickness = 1;
        pad1.Fill = new SolidColorBrush(Colors.Transparent);

        pad2.Width = 300 / sizeDiv;
        pad2.Height = 300 / sizeDiv;
        pad2.Stroke = new SolidColorBrush(Colors.Transparent);
        pad2.StrokeThickness = 1;
        pad2.Fill = new SolidColorBrush(Colors.Transparent);

        Canvas.SetTop(pad1, location.Y - pinlength / sizeDiv - pad1.Height / 2);
        Canvas.SetLeft(pad1, location.X + width / 2 / sizeDiv - pad1.Width / 2);

        Canvas.SetTop(pad2, location.Y + height / sizeDiv + pinlength / sizeDiv - pad2.Height / 2);
        Canvas.SetLeft(pad2, location.X + width / 2 / sizeDiv - pad2.Width / 2);

        var center1 = new RotateTransform();
        center1.Angle = rotation;
        center1.CenterX = pad1.Width / 2;
        center1.CenterY = pinlength / sizeDiv + height / 2 / sizeDiv + pad1.Height / 2;

        var center2 = new RotateTransform();
        center2.Angle = rotation;
        center2.CenterX = pad2.Width / 2;
        center2.CenterY = -pinlength / sizeDiv - height / 2 / sizeDiv + pad2.Height / 2;

        pad1.RenderTransform = center1;
        pad2.RenderTransform = center2;

        var padGroup = new List<Ellipse> { pad1, pad2 };

        return padGroup;
    }
    #endregion

}
