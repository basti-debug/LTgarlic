using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web.Provider;

namespace LTGarlicv2
{
    public class PageBuilder
    {
        public void displayFilePage(Frame currentframe, string filename)
        {
            InfoBar infoBar= new InfoBar();
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.Title = "Opened successfull";
            infoBar.Message = "Your File "+ filename + "was opened correctly ";
            infoBar.IsOpen= true;

            Canvas.SetLeft(infoBar, 450);
            Canvas.SetTop(infoBar, 890);


            Canvas canvas = new Canvas();
            canvas.Height = 2000;
            canvas.Width = 2000;



            // Heading 
            TextBlock Title = new TextBlock();
            Title.Text = filename;
            Title.FontSize = 30;

            Canvas.SetLeft(Title, 50);
            Canvas.SetTop(Title, 100);


            // hotbar 

            var hotbar = new CommandBar();
            Canvas.SetLeft(hotbar, 500);
            Canvas.SetTop(hotbar, 800);

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

            var placmentbutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Add),
                Label = "add Parts"
            };

            var rotatebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Rotate),
                Label = "rotate Parts"
            };

            var savebutton = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Save),
                Label = "save"
            };

            var changetotop = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Switch),
                Label = "use top bar"
            };

            hotbar.PrimaryCommands.Add(wirebutton);
            hotbar.PrimaryCommands.Add(simbutton);
            hotbar.PrimaryCommands.Add(placmentbutton);
            hotbar.PrimaryCommands.Add(rotatebutton);
            hotbar.PrimaryCommands.Add(savebutton);
            hotbar.SecondaryCommands.Add(changetotop);


            canvas.Children.Add(infoBar);
            canvas.Children.Add(Title);
            canvas.Children.Add(hotbar);

            currentframe.Content = canvas;

        }

        public Button displayMainPage(Frame currentframe)
        {
            Canvas mainCanva = new Canvas();
            mainCanva.Height = 2000;
            mainCanva.Width = 2000;

            TextBlock Title = new TextBlock();
            Title.Text = "Welcome to LTGarlic";
            Title.FontSize = 30;

            Button createbutton = new Button();
            createbutton.Content = "Create a new File";
            

            Button openbutton = new Button();
            openbutton.Content = "Open a File";
            //openbutton.Background = new SolidColorBrush(Windows.UI.Colors.Red);           !! To be replaced by accentcolor 
            
            Button openlib = new Button();
            openlib.Content = "Open a Libary";


            // Moving Objects inside the Canvas
            Canvas.SetLeft(Title, 50);
            Canvas.SetTop(Title, 100);

            Canvas.SetLeft(createbutton, 50);
            Canvas.SetTop(createbutton, 200);

            Canvas.SetLeft(openbutton, 190);
            Canvas.SetTop(openbutton, 200);

            Canvas.SetLeft(openlib,295);
            Canvas.SetTop(openlib,200);

            mainCanva.Children.Add(createbutton);
            mainCanva.Children.Add(openbutton);
            mainCanva.Children.Add(openlib);
            mainCanva.Children.Add(Title);

            currentframe.Content = mainCanva;

            return createbutton;
        }
    }

    
}
