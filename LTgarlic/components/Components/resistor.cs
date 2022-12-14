﻿using System;
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
using components.Miscellaneous;

namespace components.Components;

public class resistor : component
{
    private readonly int height = 200;
    private readonly int width = 100;
    private readonly int pinlength = 100;
    private readonly int sizeDiv = 2;

    public readonly string name = "res";
    private readonly Canvas drawingTable;

    public resistor(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private readonly Path myPath = new();
    private List<Ellipse> pads = new();
    public override List<Point> drawComponent(Point location, int rotation)
    {
        pins resPins = new pins(location, sizeDiv, width, height, pinlength);
        var pinGroup = resPins.drawPins();

        myPath.Stroke = new SolidColorBrush(Colors.Black);
        myPath.StrokeThickness = 3;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;

        pads = resPins.getPads();   

        var rect = new RectangleGeometry
        {
            Rect = new Rect(location.X, location.Y, width / sizeDiv, height / sizeDiv)
        };

        var resGroup = new GeometryGroup();
        resGroup.Children.Add(rect);
        resGroup.Children.Add(pinGroup[0]);
        resGroup.Children.Add(pinGroup[1]);

        myPath.Data = resGroup;

        var center = new RotateTransform();
        center.Angle = rotation;
        center.CenterX = location.X + width / 2 / sizeDiv;
        center.CenterY = location.Y + height / 2 / sizeDiv;

        myPath.RenderTransform = center;
        pads[0].RenderTransform = center;
        pads[1].RenderTransform = center;

        count++;

        drawingTable.Children.Add(myPath);
        drawingTable.Children.Add(pads[0]);
        drawingTable.Children.Add(pads[1]);

        List<Point> pins = new List<Point> { resPins.pin1, resPins.pin2 };

        return pins;
    }

    public override void deleteComponent()
    {
        count--;

        drawingTable.Children.Remove(myPath);
        drawingTable.Children.Remove(pads[0]);
        drawingTable.Children.Remove(pads[1]);
    }

    private List<Point> pins = new();
    public override List<Point> moveComponent(Point location, int rotation)
    {
        deleteComponent();
        pins = drawComponent(location, rotation);
        return pins;
    }
}