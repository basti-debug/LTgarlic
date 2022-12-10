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
        pins indPins = new pins(location, sizeDiv, width, height, pinlength);
        var pinGroup = indPins.drawPins();

        var myPath = new Path();
        myPath.Stroke = new SolidColorBrush(Colors.Black);
        myPath.StrokeThickness = 3;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;
        myPath.Fill = new SolidColorBrush(Colors.Black);

        List<Ellipse> pads = new List<Ellipse>();
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
