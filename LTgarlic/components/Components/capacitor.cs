using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using components.Miscellaneous;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace components.Components;

public class capacitor : component
{
    private readonly int width = 150;
    private readonly int height = 50;
    private readonly int conHeight = 15;
    private readonly int pinlength = 100;
    private readonly int sizeDiv = 2;

    public string name = "cap";
    private readonly Canvas drawingTable;

    public capacitor(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private readonly Path myPath = new();
    private List<Ellipse> pads = new();
    public override List<Point> drawComponent(Point location, int rotation, SolidColorBrush color)
    {
        pins capPins = new pins(location, sizeDiv, width, height, pinlength);
        var pinGroup = capPins.drawPins();

        myPath.Stroke = color;
        myPath.StrokeThickness = 3;
        myPath.Fill = color;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;

        pads = capPins.getPads();

        var con1 = new RectangleGeometry()
        {
            Rect = new Rect(location.X, location.Y, width / sizeDiv, conHeight / sizeDiv)
        };
        var con2 = new RectangleGeometry()
        {
            Rect = new Rect(location.X, location.Y + height / sizeDiv - conHeight / sizeDiv, width / sizeDiv, conHeight / sizeDiv)
        };

        var capGroup = new GeometryGroup();
        capGroup.Children.Add(con1);
        capGroup.Children.Add(con2);
        capGroup.Children.Add(pinGroup[0]);
        capGroup.Children.Add(pinGroup[1]);

        myPath.Data = capGroup;

        var center = new RotateTransform();
        center.Angle = rotation;
        center.CenterX = location.X + width / 2 / sizeDiv;
        center.CenterY = location.Y + height / 2 / sizeDiv;

        myPath.RenderTransform = center;
        pads[0].RenderTransform = center;
        pads[1].RenderTransform = center;

        drawingTable.Children.Add(myPath);
        drawingTable.Children.Add(pads[0]);
        drawingTable.Children.Add(pads[1]);

        var Pins = new List<Point>() { capPins.pin1, capPins.pin2 };

        return Pins;
    }

    public override void deleteComponent()
    {
        count--;

        drawingTable.Children.Remove(myPath);
        drawingTable.Children.Remove(pads[0]);
        drawingTable.Children.Remove(pads[1]);
    }

    List<Point> pins = new();
    public override List<Point> moveComponent(Point location, int rotation, SolidColorBrush color)
    {
        deleteComponent();
        pins = drawComponent(location, rotation, color);
        return pins;
    }

}
