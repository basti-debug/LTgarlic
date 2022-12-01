using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace components
{
    public abstract class component
    {
        public abstract List<Point> drawComponent();
    }
}
