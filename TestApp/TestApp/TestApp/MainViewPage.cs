using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
    class MainViewPage : MasterDetailPage
    {
        public MainViewPage()
        {
            Page1 game = new Page1();

            Label header = new Label
            {
                Text = "autoPilot()",
                FontSize = game.fontsize,
                HorizontalOptions = LayoutOptions.Center
            };
            

            // Assemble an array of NamedColor objects.
            String[] Pages =
                {
                    "Main Game",
                    "Log",
                    "Stats"
                };

            // Create ListView for the master page.
            ListView listView = new ListView
            {
                ItemsSource = Pages
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
            this.Detail = new NavigationPage(new ContentPage());

            // For Windows Phone, provide a way to get back to the master page.
            if (Device.OS == TargetPlatform.WinPhone)
            {
                (this.Detail as ContentPage).Content.GestureRecognizers.Add(
                    new TapGestureRecognizer((view) =>
                    {
                        this.IsPresented = true;
                    }));
            }

            // Define a selected handler for the ListView.
            listView.ItemSelected += (sender, args) =>
            {
                if (args.SelectedItem == "Main Game")
                {
                    this.Detail = game;
                }
                else if (args.SelectedItem == "Log")
                {
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    // Request for game to pause any typing labels
                    game.PauseText(tcs);

                    //Navigation.PushModalAsync(logPage);
                    Page2 log = new Page2(game.state, tcs);
                    this.Detail = log;
                }
                else if (args.SelectedItem == "Stats")
                {
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    // Request for game to pause any typing labels
                    game.PauseText(tcs);

                    //Navigation.PushModalAsync(logPage);
                    Page3 log = new Page3(game, tcs);
                    this.Detail = log;
                }
                // Show the detail page.
                this.IsPresented = false;
            };

            // Initialize the ListView selection.
            listView.SelectedItem = Pages[0];


        }
    }

}
