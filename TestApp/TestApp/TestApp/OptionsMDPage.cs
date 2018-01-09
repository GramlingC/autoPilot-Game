using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
    public class OptionsMDPage : ContentView
    {

        public ContentPage Master;
        Page Detail;
        public List<GameButton> buttons;
        public ListView listView;
        public Label header;

        public OptionsMDPage(Page1 game)
        {
            header = new Label
            {
                Text = "Options",
                FontSize = game.fontsize,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.LightGreen,
            };


            // Assemble an array of NamedColor objects.
            buttons = new List<GameButton>();

            GameButton button;

            for (int i = 0; i < game.currentEvent.options.Count; i++)
            {
                button = new GameButton
                {
                    Text = "> " + game.currentEvent.options[i].text,
                    Key = "option" + i,
                    TextColor = Color.LightGreen,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,

                    FontSize = game.fontsize,

                    BackgroundColor = Color.DarkSlateGray,

                    buttonOption = game.currentEvent.options[i],

                    IsVisible = false,
                    IsEnabled = false
                };
                button.Clicked += game.buttonClicked;


                buttons.Add(button);
            }

            // Create ListView for the master page.
            listView = new ListView
            {
                ItemsSource = buttons,
                ItemTemplate = new DataTemplate(() => 
                {
                    Label L = new Label();
                    L.SetBinding(Label.TextProperty, "Text");

                    L.SetBinding(Label.VerticalOptionsProperty, "HorizontalOptions");
#if __MOBILE__
                    L.SetBinding(Label.FontSizeProperty, "FontSize");
#else
                    L.SetBinding(Label.FontSizeProperty, "FontSize");
#endif
                    L.SetBinding(Label.TextColorProperty, "TextColor");
        
                    return new ViewCell { View = L };
                })
            };

            // Create the master page with the ListView.
            this.Master = new ContentPage
            {
                Title = header.Text,
                Content = new StackLayout
                {
                    Children =
                    {
                        header,
                        listView
                    }
                }
            };

            // Create the detail page using NamedColorPage and wrap it in a
            // navigation page to provide a NavigationBar and Toggle button
            this.Detail = new ContentPage();


            // Define a selected handler for the ListView.
            listView.ItemTapped += (sender, args) =>
            {
                OptionDetail opD = new OptionDetail((GameButton)args.Item, game);
                this.Detail = opD;

                // Show the detail page.
                Content = (Detail as ContentPage).Content;
            };

            // Initialize the ListView selection.
            //listView.SelectedItem = Pages[0];
            Content = Master.Content;

        }

        public void Update()
        {
            listView.ItemsSource = buttons;
        }

    }

}
