using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace TestApp
{
    public class Page3 : ContentPage
    {
        public Page3(GameStateClass state, TaskCompletionSource<bool> _tcs = null)
        {
            //Debug.WriteLine(state.ToString());

            //Adding used text to the string to be put in the class


            this.BackgroundColor = Color.Black;

            var backButton = new Button()
            {
                Text = "Back",
                TextColor = Color.GreenYellow,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 10,
                WidthRequest = 50,
                HeightRequest = 35,
                BorderColor = Color.GreenYellow,
                BackgroundColor = Color.Black
            };
            backButton.Clicked += buttonClicked;

            StackLayout stats = new StackLayout
            {
                Opacity = 1,
                BackgroundColor = Color.Black,
                Children =
                {
                    //backButton,
                    new Label
                    {
                        Text = "Hull Integrity = " + state.ship.HullIntegrity,
                        TextColor = Color.LightSeaGreen,
                        FontSize = 15,
                    },
                    new Label
                    {
                        Text = "Crew Lifesigns = " + state.ship.Lifesigns,
                        TextColor = Color.LightSeaGreen,
                        FontSize = 15,
                    },
                    new Label
                    {
                        Text = "Fuel = " + state.ship.Fuel,
                        TextColor = Color.LightSeaGreen,
                        FontSize = 15,
                    },
                }
            };
            //There is also a PushAsync option rather than PushModalAsync
            void buttonClicked(object sender, EventArgs e)
            {
                // Set TCS from page 1 to have a result, causing chain reaction to any paused gamelabels.
                _tcs.SetResult(true);

                // Pop the page from the stack, returning to main page.
                Navigation.PopAsync();
            }

            this.Content = stats;
        }
    }
}