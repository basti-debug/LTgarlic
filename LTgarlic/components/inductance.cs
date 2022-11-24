using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace components
{
    internal class inductance : component
    {
        string name;
        Point location;
        int rotation;

        public inductance(string name, Point location, int rotation) 
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
