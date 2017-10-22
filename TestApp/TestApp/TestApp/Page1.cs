using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

//To make a page like this, right click TestApp (Portable)
//Choose Add, then choose Content Page (C#)

namespace TestApp
{
    public class Page1 : ContentPage
    {
        Grid grid;//Making this a global variable so we can change it dynamically through functions
        //Here is the grid documentation: https://developer.xamarin.com/api/type/Xamarin.Forms.Grid/

        int row = 0;//keep track of the next row to be used

        //There will be less rows on mobile devices, since they're smaller
        //font will also be smaller
#if __MOBILE__
        int maxrow = 10;
        double fontsize = 8;
#else
        int maxrow = 15;
        double fontsize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
#endif

        public Page1()
        {
            /* Stack layout is an option if you want things equidistant down the page
             * But a grid gives us more flexibility as to where things are on the page
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };
            */

            //First, we define the grid and its rows and columns
            grid = new Grid
            {

                ColumnDefinitions =
                {
                    //The Column Definition defines the width of each column
                    //GridLength.Auto means it'll fit exactly the size of what's inside it
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)}
                }
            };
            for (int i = 0; i < maxrow; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                //I want several lines of text near the top, that's the Autos
            }
            //this type of grid length expands to fill in what isn't taken up by other rows
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //I want a large empty space, that's the star
#if __MOBILE__
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
#else
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
#endif
            //I want buttons at the bottom, that's the last Auto

            //Now, to add some content.
            AddLabel("> First Text.");
            //AddLabel is a method I wrote, read it below to see how it works
            AddLabel("> This is the second text. This text will hopefully be so long that it'll wrap around to a second line, and this grid row will expand accordingly to be two lines thick.");
            AddLabel("> Third Text.");

            GameButton button;

            // Loops through creating the buttons, since they are each very similar
            for (int i = 0; i < 4; i++)
            {
                button = new GameButton
                {
                    Text = "> choose(Option" + (i+1) + ")",
                    Key = "option" + (i+1),
                    TextColor = Color.LightGreen,
                    //Adapt to device size

                    FontSize = fontsize,

                    BackgroundColor = Color.DarkSlateGray,
                };
                button.Clicked += buttonClicked;//adding the function to this button's click

#if __MOBILE__
                grid.Children.Add(button, 0, 1, maxrow + i, maxrow + i + 1);
#else
                grid.Children.Add(button, i, i+1, maxrow + 3, maxrow + 4);
#endif
                //make sure to put this in a different column each loop
            }

#if !__MOBILE__
            button = new GameButton
            {
                Text = "> clearScreen()",
                Key = "clearScreen",
                TextColor = Color.LightGreen,
                //Adapt to device size

                FontSize = fontsize,

                BackgroundColor = Color.DarkSlateGray,
            };
            button.Clicked += buttonClicked;//adding the function to this button's click

            grid.Children.Add(button, 5, 6, maxrow + 3, maxrow + 4);
#endif

            this.Padding = new Thickness(10, 20, 10, 10); //Some breathing room around the edges
            this.Content = grid;//Puts the grid on the page
            this.BackgroundColor = Color.Black;

            //BEFORE YOU RUN: You can right click the sub-projects to the right
            //such as TestApp.UWP and choose Set as Start Up Project to choose your platform
            //Only UWP works for me at the moment
        }

        void AddLabel(string text)
        {
            grid.Children.Add(new Label
            {
                Text = text,
                TextColor = Color.LightGreen,
                FontSize = fontsize,

                //Here we could keep editing this and make it be different sizes,angles,etc
                //If we declared this as a global variable, we could change it with functions
            }, 0, 5, row, ++row);
            //The parameters are (View,Left,Right,Top,Bottom)
            //View is anything from Labels to buttons to sliders, etc, and are adapt to platform
            //The numbers are grid positions. Left and top are the grid positions where they start
            //Top and Bottom are where they end, they will not enter these grid positions
            //for example, the grid's column index 5 doesn't exist, the last is 4
            //But it'll stop before 5, which means it'll span all the columns

            //The row variable is increasing each time, so each label we add will be in a new row
            //once it gets to the maxrow, we want to move everything up so it doesn't keep
            //going down to the bottom of the screen
            if (row > maxrow)
            {
                row--;//keep row at maxrow
                bool remove = false;
                Label toremove = null;

                foreach (Label l in grid.Children.OfType<Label>())
                {
                    if (Grid.GetRow(l) == 0)
                    {
                        //we want to remove the topmost thing to make room
                        remove = true;
                        toremove = l;
                        //we can't remove it while iterating
                    }
                    else
                        Grid.SetRow(l, Grid.GetRow(l) - 1);
                    //move everything up to make space for the new label
                }
                if (remove)
                {
                    //so we remove it after
                    grid.Children.Remove(toremove);
                }
            }
        }

        void buttonClicked(object sender, EventArgs e)
        {
            GameButton button = (GameButton)sender;
            AddLabel(button.Text);

            // this may be done later by retreiving text from a database (?)
            switch (button.Key)
            {
                case "option1":
                    AddLabel("> You have chosen option one.");
                    AddLabel("> Things will happen accordingly, maybe the ship will be hit by something or waste resources.Story will happen and consequences will be had, etc, etc.");
                    break;
                case "option2":
                    AddLabel("> You have chosen option two.");
                    AddLabel("> Different things will happen by this than by option one, and so on. I'm sure it'll be a good thing.");
                    break;
                case "option3":
                    AddLabel("> You have chosen option three.");
                    AddLabel("> This return text will probably be stored in a database somewhere, and we'll have code to check what to display depending on variables and whatnot.");
                    break;
                case "option4":
                    AddLabel("> You have chosen option four.");
                    AddLabel("> Ideally, this text will be displayed one character at a time.");
                    break;
                case "clearScreen":
                    int index = 0;
                    while (index < grid.Children.Count && row > 0)
                    {
                        if (grid.Children.ElementAt(index) is Label)
                        {
                            grid.Children.RemoveAt(index);
                            row--;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    break;
            }
            //adds the button's text to the grid
        }
    }
}