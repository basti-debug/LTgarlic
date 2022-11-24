using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace components
{
    public class capacitor : component
    {
        string name;
        Point location;
        int rotation;

        public capacitor(string name, Point location, int rotation) 
        {
            this.name = name;
            this.location = location;
            this.rotation = rotation;
        }

        public override void drawComponent(Canvas drawingTable)
        {
            throw new NotImplementedException();
        }
    }
}
