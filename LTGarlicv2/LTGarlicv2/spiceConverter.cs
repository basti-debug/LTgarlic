using components.Components;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using LTgarlic.Components.Miscellaneous;
using Windows.Foundation;

namespace LTGarlicv2
{
    internal class spiceConverter
    {
        public static void encodeFile(string filelocation)
        {
            string spiceString = "";
            string fileName = filelocation;

            spiceString += "Version 4\n";

            foreach (wire wire in PageBuilder.allWires)
            {
                spiceString += "WIRE " + ((wire.startPoint.X + 15) / 30 * 16) + " " + ((wire.startPoint.Y + 15) / 30 * 16) + " " + ((wire.endPoint.X + 15) / 30 * 16) + " " + ((wire.endPoint.Y + 15) / 30 * 16) + "\n";
            }

            foreach (component component in PageBuilder.components)
            {
                if (component is resistor)
                {
                    switch (((resistor)component).rotation)
                    {
                        case 0:
                            spiceString += "SYMBOL " + ((resistor)component).name + " " + ((((resistor)component).location.X - 15) / 30 * 16) + " " + ((((resistor)component).location.Y - 15) / 30 * 16) + " R" + ((resistor)component).rotation + "\n";
                            break;
                        case 90:
                            spiceString += "SYMBOL " + ((resistor)component).name + " " + ((((resistor)component).location.X + 135) / 30 * 16) + " " + ((((resistor)component).location.Y + 15) / 30 * 16) + " R" + ((resistor)component).rotation + "\n";
                            break;
                        case 180:
                            spiceString += "SYMBOL " + ((resistor)component).name + " " + ((((resistor)component).location.X + 105) / 30 * 16) + " " + ((((resistor)component).location.Y + 165) / 30 * 16) + " R" + ((resistor)component).rotation + "\n";
                            break;
                        case 270:
                            spiceString += "SYMBOL " + ((resistor)component).name + " " + ((((resistor)component).location.X - 45) / 30 * 16) + " " + ((((resistor)component).location.Y + 135) / 30 * 16) + " R" + ((resistor)component).rotation + "\n";
                            break;
                    }
                    spiceString += "SYMATTR InstName R" + PageBuilder.components.IndexOf(component) + "\n";
                }

                if (component is inductance)
                {
                    switch (((inductance)component).rotation)
                    {
                        case 0:
                            spiceString += "SYMBOL " + ((inductance)component).name + " " + ((((inductance)component).location.X - 15) / 30 * 16) + " " + ((((inductance)component).location.Y + -15) / 30 * 16) + " R" + ((inductance)component).rotation + "\n";
                            break;
                        case 90:
                            spiceString += "SYMBOL " + ((inductance)component).name + " " + ((((inductance)component).location.X + 135) / 30 * 16) + " " + ((((inductance)component).location.Y + 15) / 30 * 16) + " R" + ((inductance)component).rotation + "\n";
                            break;
                        case 180:
                            spiceString += "SYMBOL " + ((inductance)component).name + " " + ((((inductance)component).location.X + 105) / 30 * 16) + " " + ((((inductance)component).location.Y + 165) / 30 * 16) + " R" + ((inductance)component).rotation + "\n";
                            break;
                        case 270:
                            spiceString += "SYMBOL " + ((inductance)component).name + " " + ((((inductance)component).location.X - 45) / 30 * 16) + " " + ((((inductance)component).location.Y + 135) / 30 * 16) + " R" + ((inductance)component).rotation + "\n";
                            break;
                    }
                    spiceString += "SYMATTR InstName L" + PageBuilder.components.IndexOf(component) + "\n";
                }

                if (component is capacitor)
                {
                    switch (((capacitor)component).rotation)
                    {
                        case 0:
                            spiceString += "SYMBOL " + ((capacitor)component).name + " " + ((((capacitor)component).location.X - 15) / 30 * 16) + " " + ((((capacitor)component).location.Y + 15) / 30 * 16) + " R" + ((capacitor)component).rotation + "\n";
                            break;
                        case 90:
                            spiceString += "SYMBOL " + ((capacitor)component).name + " " + ((((capacitor)component).location.X + 75) / 30 * 16) + " " + ((((capacitor)component).location.Y - 15) / 30 * 16) + " R" + ((capacitor)component).rotation + "\n";
                            break;
                        case 180:
                            spiceString += "SYMBOL " + ((capacitor)component).name + " " + ((((capacitor)component).location.X + 105) / 30 * 16) + " " + ((((capacitor)component).location.Y + 75) / 30 * 16) + " R" + ((capacitor)component).rotation + "\n";
                            break;
                        case 270:
                            spiceString += "SYMBOL " + ((capacitor)component).name + " " + ((((capacitor)component).location.X + 15) / 30 * 16) + " " + ((((capacitor)component).location.Y + 105) / 30 * 16) + " R" + ((capacitor)component).rotation + "\n";
                            break;
                    }
                    spiceString += "SYMATTR InstName C" + PageBuilder.components.IndexOf(component) + "\n";
                }

                if (component is diode)
                {
                    switch (((diode)component).rotation)
                    {
                        case 0:
                            spiceString += "SYMBOL " + ((diode)component).name + " " + ((((diode)component).location.X - 15) / 30 * 16) + " " + ((((diode)component).location.Y + 15) / 30 * 16) + " R" + ((diode)component).rotation + "\n";
                            break;
                        case 90:
                            spiceString += "SYMBOL " + ((diode)component).name + " " + ((((diode)component).location.X + 75) / 30 * 16) + " " + ((((diode)component).location.Y - 15) / 30 * 16) + " R" + ((diode)component).rotation + "\n";
                            break;
                        case 180:
                            spiceString += "SYMBOL " + ((diode)component).name + " " + ((((diode)component).location.X + 105) / 30 * 16) + " " + ((((diode)component).location.Y + 75) / 30 * 16) + " R" + ((diode)component).rotation + "\n";
                            break;
                        case 270:
                            spiceString += "SYMBOL " + ((diode)component).name + " " + ((((diode)component).location.X + 15) / 30 * 16) + " " + ((((diode)component).location.Y + 105) / 30 * 16) + " R" + ((diode)component).rotation + "\n";
                            break;
                    }
                    spiceString += "SYMATTR InstName D" + PageBuilder.components.IndexOf(component) + "\n";
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(spiceString);
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        public static void decodeFile(Canvas canvas, string path)
        {
            string[] data = File.ReadAllLines(path);
            bool containsSheet = false;
            if (data[1].Contains("SHEET"))
                containsSheet = true;

            Point location = new Point();
            int rotation;

            if (containsSheet)
            {
                for (var i = 2; i < data.Length; i++)
                {
                    if (data[i].Contains("res"))
                    {
                        resistor res = new resistor(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            res.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            res.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("cap"))
                    {
                        capacitor cap = new capacitor(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            cap.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            cap.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("diode"))
                    {
                        diode dio = new diode(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            dio.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            dio.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("ind"))
                    {
                        inductance ind = new inductance(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            ind.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            ind.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                }
            }
            else
            {
                for (var i = 1; i < data.Length; i++)
                {
                    if (data[i].Contains("res"))
                    {
                        resistor res = new resistor(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            res.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            res.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("cap"))
                    {
                        capacitor cap = new capacitor(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            cap.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            cap.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("diode"))
                    {
                        diode dio = new diode(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            dio.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            dio.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                    if (data[i].Contains("ind"))
                    {
                        inductance ind = new inductance(canvas);
                        string[] substrings = data[i].Split(' ');
                        location.X = Convert.ToDouble(substrings[2]);
                        location.Y = Convert.ToDouble(substrings[3]);
                        rotation = Convert.ToInt32(substrings[4].Substring(1));

                        if (PageBuilder.theme == "Dark")
                        {
                            ind.drawComponent(location, rotation, new SolidColorBrush(Colors.White));
                        }
                        else if (PageBuilder.theme == "Light")
                        {
                            ind.drawComponent(location, rotation, new SolidColorBrush(Colors.Black));
                        }
                    }
                }
            }

        }
    }
}
