using System;
using System.Linq;
using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;

//To make a page like this, right click TestApp (Portable)
//Choose Add, then choose Content Page (C#)

//Make sure to change App.xaml.cs to have it run your desired page
//At MainPage = new TestApp.Page1();, you'd change "Page1" to the name of the page you want it to run

namespace TestApp
{
    public class Page1 : ContentPage
    {
        Event current;

        GameStateClass state;

        Grid grid;//Making this a global variable so we can change it dynamically through functions
        //Here is the grid documentation: https://developer.xamarin.com/api/type/Xamarin.Forms.Grid/

        int row = 0;//keep track of the next row to be used

        //There will be less rows on mobile devices, since they're smaller
        //font will also be smaller
#if __MOBILE__
        int maxrow = 7;
        double fontsize = 10;
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
#if __MOBILE__
                    new ColumnDefinition { Width = GridLength.Auto},
                    //new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
#else
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                    new ColumnDefinition { Width = GridLength.Auto},
#endif
                    //this type of grid length expands to fill in what isn't taken up by other rows
                }
            };
            for (int i = 0; i < maxrow; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                //I want several lines of text near the top, that's these Autos
            }
            
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            //I want a large empty space, that's the star 

#if __MOBILE__
            //I want four buttons at the bottom, 
            //but they need to be smaller in mobile to fit the screen
            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
#else
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
#endif
            //
            // Everything below is for saving ship to file, just examples
            //
            Ship testShip = new Ship(100, 100, 100, 0);
            SaveManager.SaveObject(@"testSave.xml", testShip);
            //testShip = (Ship)SaveManager.LoadObject(@"testSave.xml", testShip);
            //Debug.WriteLine("Hull: {0} - Fuel: {1} - Lifesigns: {2} - Empathy: {3}", testShip.HullIntegrity, testShip.Fuel, testShip.Lifesigns, testShip.EmpathyLevel);




            state = new GameStateClass();

            // These two lines generate the data and save them to files
            state.GenerateSampleData();
            state.SaveEvents();

            // This will load all event files from the working directory
            state.LoadEvents();

            // This will print all events in the game state to the console
            //state.PrintEventList();

            current = state.getCurrent();

            AddLabels();
            AddButtons(current.options);


#if !__MOBILE__
            GameButton button = new GameButton
            {
                Text = "> clearScreen()",
                Key = "clearScreen",
                TextColor = Color.LightGreen,
                //Adapt to device size

                FontSize = fontsize,

                BackgroundColor = Color.DarkSlateGray,
                buttonOption = new Option
                {
                    text = "",
                },
            };
            button.Clicked += buttonClicked;//adding the function to this button's click

            grid.Children.Add(button, 5, 6, maxrow + 3, maxrow + 4);
#endif

            this.Padding = new Thickness(10, 20, 10, 10); //Some breathing room around the edges
            this.Content = grid;//Puts the grid on the page
            this.BackgroundColor = Color.Black;

            //BEFORE YOU RUN: You can right click the sub-projects to the right
            //such as TestApp.UWP and choose Set as Start Up Project to choose your platform


        }

        void AddLabels()
        {
            foreach (string s in current.text)
            {
                AddLabel(s);
            }
        }

        void AddLabel(string text)
        {
            grid.Children.Add(new Label
            {
                Text = "> " + text,
                TextColor = Color.LightGreen,
                FontSize = fontsize,

                //Here we could keep editing this and make it be different sizes,angles,etc
                //If we declared this as a global variable, we could change it with functions
            }, 0, 6, row, ++row);
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

        void AddButtons(List<Option> op)
        {
            List<View> removable = new List<View>();
            foreach (GameButton b in grid.Children.OfType<GameButton>())
            {
                if (b.Key != "clearScreen")
                {
                    removable.Add(b);
                }
            }
            foreach (GameButton b in removable)
                grid.Children.Remove(b);

            GameButton button;

            for (int i = 0; i < 4; i++)
            {
                button = new GameButton
                {
                    Text = "> " + op[i].text,
                    Key = "option" + i,
                    TextColor = Color.LightGreen,
                    HorizontalOptions = LayoutOptions.StartAndExpand,

                    FontSize = fontsize,

                    BackgroundColor = Color.DarkSlateGray,

                    buttonOption = op[i],
                    
                };
                button.Clicked += buttonClicked;

#if __MOBILE__
                grid.Children.Add(button, 0, 1, maxrow + i + 1, maxrow + i + 2);
#else
                grid.Children.Add(button, i, i+1, maxrow + 3, maxrow + 4);
#endif
                //make sure to put this in a different column each loop
            }
        }

        void buttonClicked(object sender, EventArgs e)
        {
            GameButton button = (GameButton)sender;
            AddLabel(button.buttonOption.text);
            
            // this may be done later by retreiving text from a database (?)
            switch (button.Key)
            {
                case "option1":
                    state.goTo(current.options[0].nextEventNumber);
                    break;
                case "option2":
                    state.goTo(current.options[1].nextEventNumber);
                    break;
                case "option3":
                    state.goTo(current.options[2].nextEventNumber);
                    break;
                case "option4":
                    state.goTo(current.options[3].nextEventNumber);
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
                    return;
            }
            current = state.getCurrent();
            AddLabels();
            AddButtons(current.options);
        }
    }
}