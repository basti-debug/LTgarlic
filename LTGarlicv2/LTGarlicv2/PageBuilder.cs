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

        public Button displayMainPage(Frame currentframe)
        {
            Canvas mainCanva = new Canvas();
            mainCanva.Height = 1000;
            mainCanva.Width = 1000;

            TextBlock Title = new TextBlock();
            Title.Text = "Welcome to LTGarlic";
            Title.FontSize = 30;

            Button createbutton = new Button();
            createbutton.Content = "Create a new File";
            createbutton.Height = 30;
            

            Button openbutton = new Button();
            openbutton.Content = "Open a File";

            Button openlib = new Button();
            openlib.Content = "Open a Libary";


            // Moving Objects inside the Canvas
            Canvas.SetLeft(Title, -200);
            Canvas.SetTop(Title, 100);

            Canvas.SetLeft(createbutton, -200);
            Canvas.SetTop(createbutton, 200);

            mainCanva.Children.Add(createbutton);
            mainCanva.Children.Add(openbutton);
            mainCanva.Children.Add(openlib);
            mainCanva.Children.Add(Title);

            currentframe.Content = mainCanva;

            return createbutton;
        }
    }

    
}
