using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace LTgarlic;
public class neweditingpage : Page
{
    public neweditingpage()
    {
        var drawcanvas = new Canvas();
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

        this.Content = drawcanvas;
    }
}