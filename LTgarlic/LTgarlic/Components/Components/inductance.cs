﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.UI;
using LTgarlic.Components.Miscellaneous;
using Path = Microsoft.UI.Xaml.Shapes.Path;
using LTgarlic.Views;

namespace components.Components;

public class inductance : component
{
    private readonly int height = 2400;
    private readonly int width = 1200;
    private readonly int pinlength = 300;
    private int sizeDiv = 20;

    public readonly string name = "ind";
    private readonly Canvas drawingTable;
    public Point location = new();
    public int rotation;

    public override List<Point> pins { get; set; }
    public override List<Ellipse> pads { get; set; }


    public inductance(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private readonly Path myPath = new();
    public override void drawComponent(Point location, int rotation, SolidColorBrush color)
    {
        this.location = location;
        this.rotation = rotation;

        Point pin1 = location; //overhaul needed (no rotation)
        Point pin2 = new Point(location.X, location.Y + 2 * pinlength / sizeDiv + width / sizeDiv);

        pins indPins = new pins(location, sizeDiv, width, height, pinlength, rotation, pin1, pin2);

        myPath.Stroke = color;
        myPath.StrokeThickness = 3;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;
        myPath.Fill = color;

        pads = indPins.getPads();

        LineGeometry pinLine1 = new LineGeometry
        {
            StartPoint = location,
            EndPoint = new Point(location.X, location.Y + pinlength / sizeDiv)
        };
        LineGeometry pinLine2 = new LineGeometry
        {
            StartPoint = new Point(location.X, location.Y + 2 * pinlength / sizeDiv + height / sizeDiv),
            EndPoint = new Point(location.X, location.Y + pinlength / sizeDiv)
        };
        var rect = new RectangleGeometry
        {
            Rect = new Rect(location.X - width / sizeDiv / 2, location.Y + pinlength / sizeDiv, width / sizeDiv, height / sizeDiv)
        };

        var indGroup = new GeometryGroup();
        indGroup.Children.Add(rect);
        indGroup.Children.Add(pinLine1);
        indGroup.Children.Add(pinLine2);

        myPath.Data = indGroup;

        var center = new RotateTransform();
        center.Angle = rotation;
        center.CenterX = location.X + width / 2 / sizeDiv;
        center.CenterY = location.Y + height / 2 / sizeDiv;

        myPath.RenderTransform = center;

        drawingTable.Children.Add(myPath);
        drawingTable.Children.Add(pads[0]);
        drawingTable.Children.Add(pads[1]);

        foreach (Ellipse pad in pads)
        {
            pad.PointerEntered += Pad_PointerEntered;
            pad.PointerExited += Pad_PointerExited;
            pad.PointerPressed += Pad_PointerPressed;
        }

        var pins = new List<Point> { indPins.pin1, indPins.pin2 };
        this.pins = pins;
    }

    private void Pad_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (ShellPage.wireMode)
        {
            EditingPage.startPoint = new Point(Canvas.GetLeft((Ellipse)sender), Canvas.GetTop((Ellipse)sender));
        }
    }

    private void Pad_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        ((Ellipse)drawingTable.Children[drawingTable.Children.IndexOf((Ellipse)sender)]).Fill = new SolidColorBrush(Colors.Transparent);
        ((Ellipse)drawingTable.Children[drawingTable.Children.IndexOf((Ellipse)sender)]).Stroke = new SolidColorBrush(Colors.Transparent);
    }

    private void Pad_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        ((Ellipse)drawingTable.Children[drawingTable.Children.IndexOf((Ellipse)sender)]).Fill = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
        ((Ellipse)drawingTable.Children[drawingTable.Children.IndexOf((Ellipse)sender)]).Stroke = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
    }

    public override void deleteComponent()
    {
        count--;

        drawingTable.Children.Remove(myPath);
        drawingTable.Children.Remove(pads[0]);
        drawingTable.Children.Remove(pads[1]);
    }

    public override void moveComponent(Point location, int rotation, SolidColorBrush color)
    {
        deleteComponent();
        drawComponent(location, rotation, color);
    }

}
