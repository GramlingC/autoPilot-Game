using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace TestApp
{
	public class OptionDetail : ContentPage
	{
		public OptionDetail (GameButton button, Page1 game)
		{

            
            //button.Clicked += game.buttonClicked;


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
            backButton.Clicked += ((object sender, EventArgs e) =>
            {
                game.buttonArea.Content = game.buttonArea.Master.Content;

                game.AddButtons(game.currentEvent.options);
            }
            );

            Content = new StackLayout {
				Children =
                {
                    backButton,
					new Label
                    {
                        Text = button.buttonOption.optionSummary,
                        FontSize = game.fontsize,
                        TextColor = Color.LightGreen
                    },
                    button,
                    new StackLayout
                    {
                        Opacity = 1,
                        BackgroundColor = Color.Black,
                        Children =
                        {
                            new Label
                            {
                                Text = "Hull Integrity = " + game.state.ship.HullIntegrity,
                                TextColor = Color.LightSeaGreen,
                                FontSize = game.fontsize,
                            },
                            new Label
                            {
                                Text = "Crew Lifesigns = " + game.state.ship.Lifesigns,
                                TextColor = Color.LightSeaGreen,
                                FontSize = game.fontsize,
                            },
                            new Label
                            {
                                Text = "Fuel = " + game.state.ship.Fuel,
                                TextColor = Color.LightSeaGreen,
                                FontSize = game.fontsize,
                            },
                        }
                    }
                }
			};
            BackgroundColor = Color.Black;
		}
	}
}