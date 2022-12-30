using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace components.Miscellaneous;
public class wire
{
    private readonly Canvas drawingTable;
    private readonly List<Line> lines = new();
    private readonly List<wire> connectedTo = new();

    public Point startPoint { get; set; }
    public Point endPoint { get; set; }
    public SolidColorBrush color { get; set; }

    public static bool wiringType = true;

    public static List<wire> wires = new();

    public wire(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private Line horizontal = new();
    private Line vertical = new();
    public void drawWire(Point startPoint, Point endPoint, SolidColorBrush color)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.color = color;

        if (wiringType)
        {
            Line horizontal = new Line()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = startPoint.Y,
                Stroke = color,
                StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round
            };
            Line vertical = new Line()
            {
                X1 = endPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = color,
                StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round
            };
            drawingTable.Children.Add(horizontal);
            drawingTable.Children.Add(vertical);

            lines.Add(horizontal);
            lines.Add(vertical);

            this.horizontal = horizontal;
            this.vertical = vertical;
        }

        else
        {
            Line vertical = new Line()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = startPoint.X,
                Y2 = endPoint.Y,
                Stroke = color,
                StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round
            };

            Line horizontal = new Line()
            {
                X1 = startPoint.X,
                Y1 = endPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Stroke = color,
                StrokeThickness = 3,
                StrokeStartLineCap = PenLineCap.Round
            };

            drawingTable.Children.Add(vertical);
            drawingTable.Children.Add(horizontal);

            lines.Add(horizontal);
            lines.Add(vertical);

            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }

    public void deleteWire()
    {
        drawingTable.Children.Remove(horizontal);
        drawingTable.Children.Remove(vertical);

        lines.Remove(horizontal);
        lines.Remove(vertical);
    }

    public void addConnection(wire wire)
    {
        connectedTo.Add(wire);
    }

    public List<wire> getConnections(wire wire)
    {
        return connectedTo;
    }

    public void redrawWire(wire wire)
    {
        wire.deleteWire();
        wire.drawWire(wire.startPoint, wire.endPoint, wire.color);
    }

    public static void changeWiringType()
    {
        wiringType = !wiringType;
    }
}
