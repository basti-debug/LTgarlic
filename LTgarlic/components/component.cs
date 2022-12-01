using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace components
{
    public abstract class component
    {
        public abstract List<Point> drawComponent();
    }
}
