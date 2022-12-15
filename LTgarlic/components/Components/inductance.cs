using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using components.Miscellaneous;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace components.Components;

public class inductance : component
{
    private readonly int height = 200;
    private readonly int width = 100;
    private readonly int pinlength = 100;
    private readonly int sizeDiv = 2;

    public readonly string name = "ind";
    private readonly Canvas drawingTable;

    public inductance(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private readonly Path myPath = new();
    private List<Ellipse> pads = new();
    public override List<Point> drawComponent(Point location, int rotation, SolidColorBrush color)
    {
        pins indPins = new pins(location, sizeDiv, width, height, pinlength);
        var pinGroup = indPins.drawPins();

        myPath.Stroke = color;
        myPath.StrokeThickness = 3;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;
        myPath.Fill = color;

        pads = indPins.getPads();

        var rect = new RectangleGeometry
        {
            Rect = new Rect(location.X, location.Y, width / sizeDiv, height / sizeDiv)
        };

        var indGroup = new GeometryGroup();
        indGroup.Children.Add(rect);
        indGroup.Children.Add(pinGroup[0]);
        indGroup.Children.Add(pinGroup[1]);

        myPath.Data = indGroup;

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

        var Pins = new List<Point> { indPins.pin1, indPins.pin2 };

        return Pins;
    }

    public override void deleteComponent()
    {
        count--;

        drawingTable.Children.Remove(myPath);
        drawingTable.Children.Remove(pads[0]);
        drawingTable.Children.Remove(pads[1]);
    }

    private List<Point> pins = new();
    public override List<Point> moveComponent(Point location, int rotation, SolidColorBrush color)
    {
        deleteComponent();
        pins = drawComponent(location, rotation, color);
        return pins;
    }

}
