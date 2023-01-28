using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace LTgarlic.Components.Miscellaneous;
public class wire
{
    private readonly Canvas drawingTable;
    private readonly List<wire> connectedTo = new();
    public static List<Line> wires = new();
    public static bool wiringType = true;

    public Point startPoint
    {
        get; set;
    }
    public Point endPoint
    {
        get; set;
    }
    public SolidColorBrush color
    {
        get; set;
    }

    public wire(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    Line actualWire = new Line();

    public void drawWire(Point startPoint, Point endPoint, SolidColorBrush color)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.color = color;

        Line actualWire = new Line()
        {
            X1 = startPoint.X,
            Y1 = startPoint.Y,
            X2 = endPoint.X,
            Y2 = endPoint.Y,
            Stroke = color,
            StrokeThickness = 3,
        };

        actualWire.StrokeEndLineCap = PenLineCap.Round;
        actualWire.StrokeStartLineCap = PenLineCap.Round;

        drawingTable.Children.Add(actualWire);
        wires.Add(actualWire);
        this.actualWire = actualWire;
    }

    public void deleteWire()
    {
        drawingTable.Children.Remove(actualWire);
        wires.Remove(actualWire);
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

    public Ellipse drawConnection(Point point)
    {
        Ellipse connection = new Ellipse()
        {
            Width = 5,
            Height = 5,
            Fill = color,
            Stroke = color,
            StrokeThickness = 5
        };

        Canvas.SetLeft(connection, point.X);
        Canvas.SetTop(connection, point.Y);

        drawingTable.Children.Add(connection);
        return connection;
    }

    public void deleteConnection(Ellipse connection)
    {
        drawingTable.Children.Remove(connection);
    }
}
