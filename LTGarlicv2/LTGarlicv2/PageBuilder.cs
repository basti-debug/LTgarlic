using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTGarlicv2
{
    public class PageBuilder
    {
        public void displayFilePage(Frame currentframe, TextBlock Title, string filename)
        {
            Title.Text = filename;
            Canvas canvas = new Canvas();
            var hotbar = new CommandBar();

            var wirebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Edit),
                Label = "wire mode"
            };

            var simbutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Calculator),
                Label = "simulate"
            };

            hotbar.PrimaryCommands.Add(wirebutton);
            hotbar.SecondaryCommands.Add(simbutton);

            canvas.Children.Add(hotbar);

            currentframe.Content = canvas;

        }
    }
}
