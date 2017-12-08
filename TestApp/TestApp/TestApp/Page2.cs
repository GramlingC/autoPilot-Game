using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Xamarin.Forms;

namespace TestApp
{
    public class Page2 : ContentPage
    {
        Page1 MainPageReference;

        public Page2(GameStateClass state)
        {
            //Debug.WriteLine(state.ToString());

            //Adding used text to the string to be put in the class
            string text = "";
            foreach(string s in state.usedText)
            {
                text += s + "\n";
            }

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

            Content = new StackLayout()
            {
                Children =
                {
                    new Label()
                    {
                        FontAttributes = FontAttributes.Bold,
                        Text = "Captain's Log",
                        TextColor = Color.YellowGreen,
                        HorizontalOptions = LayoutOptions.Center
                    },

                    new ScrollView()
                    {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Content =
                        new Label()
                        {
                            Text = text,
                            TextColor = Color.OrangeRed
                        }
                    },

                    backButton
                }
            };

            //There is also a PushAsync option rather than PushModalAsync
            void buttonClicked(object sender, EventArgs e)
            {
                Navigation.PopModalAsync();

                if (MainPageReference != null)
                    MainPageReference.ResumeText();
            }


        }

        public void AddMainPage(Page1 reference)
        {
            MainPageReference = reference;
        }
    }
}