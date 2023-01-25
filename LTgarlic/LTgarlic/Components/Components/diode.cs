﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;
using LTgarlic.Components.Miscellaneous;
using Path = Microsoft.UI.Xaml.Shapes.Path;
using LTgarlic.Views;

namespace components.Components;

public class diode : component
{
    private readonly int height = 1200;
    private readonly int width = 1200;
    private readonly int pinlength = 1200;
    private int sizeDiv = 20;

    public readonly string name = "diode";
    private readonly Canvas drawingTable;

    public Point location = new();
    public int rotation;

    public override List<Point> pins { get; set; }
    public override List<Ellipse> pads { get; set; }

    public diode(Canvas drawingTable)
    {
        this.drawingTable = drawingTable;
    }

    private readonly Path myPath = new();
    public override void drawComponent(Point location, int rotation, SolidColorBrush color)
    {
        this.location = location;
        this.rotation = rotation;
        pins diodePins = new pins();
        var pinGroup = diodePins.drawPins(location, sizeDiv, width, height, pinlength, rotation);

        myPath.Stroke = color;
        myPath.StrokeThickness = 3;
        myPath.StrokeEndLineCap = PenLineCap.Round;
        myPath.StrokeStartLineCap = PenLineCap.Round;

        pads = diodePins.getPads();

        #region draw Diode
        var l1 = new LineGeometry()
        {
            StartPoint = location,
            EndPoint = new Point(location.X + width / sizeDiv, location.Y),
        };
        var l2 = new LineGeometry()
        {
            StartPoint = location,
            EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv),
        };
        var l3 = new LineGeometry()
        {
            StartPoint = new Point(location.X + width / sizeDiv, location.Y),
            EndPoint = new Point(location.X + width / 2 / sizeDiv, location.Y + height / sizeDiv),
        };
        var l4 = new LineGeometry()
        {
            StartPoint = new Point(location.X, location.Y + height / sizeDiv),
            EndPoint = new Point(location.X + width / sizeDiv, location.Y + height / sizeDiv)
        };

        var diodeGroup = new GeometryGroup();
        diodeGroup.Children.Add(l1);
        diodeGroup.Children.Add(l2);
        diodeGroup.Children.Add(l3);
        diodeGroup.Children.Add(l4);
        #endregion
        diodeGroup.Children.Add(pinGroup[0]);
        diodeGroup.Children.Add(pinGroup[1]);

        myPath.Data = diodeGroup;

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

        var pins = new List<Point> { diodePins.pin1, diodePins.pin2 };
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
