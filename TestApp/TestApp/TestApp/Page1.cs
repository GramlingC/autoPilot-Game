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

        int row = 2;//keep track of the next row to be used

        //There will be less rows on mobile devices, since they're smaller
        //font will also be smaller
#if __MOBILE__
        int maxrow = 10;
        double fontsize = 10;
#else
        int maxrow = 18;
        double fontsize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
#endif

        public Page1()
        {


            //First, we define the grid and its rows and columns
            grid = new Grid
            {

                ColumnDefinitions =
                {
                    //The Column Definition defines the width of each column
                    //GridLength.Auto means it'll fit exactly the size of what's inside it
#if __MOBILE__
                    new ColumnDefinition { Width = 200},
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

#if __MOBILE__
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
#else
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
#endif

            for (int i = 2; i < maxrow; i++)
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
            
            //LoadObject still not working for whatever reason
            //Debug.WriteLine(SaveManager.LoadObject(@"testSave.xml", typeof(Ship)).Result.ToString());

            state = new GameStateClass();
            state.ship = testShip;

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

            //Displaying Stats:
            
            Button StatsButton = new Button
            {
                Text = "System.Diagnostics()",
                BackgroundColor = Color.DarkSlateGray,
                TextColor = Color.LightSeaGreen,
                FontSize = fontsize,
            };

            // FIRST OPTION: Display the stats as a menu
            /*
            StackLayout stats = new StackLayout
            {
                Opacity = 1,
                BackgroundColor = Color.Black,
                Children =
                {
                    new Label
                    {
                        Text = "Hull Integrity = " + state.ship.HullIntegrity,
                        TextColor = Color.LightSeaGreen,
                        FontSize = fontsize,
                    },
                    new Label
                    {
                        Text = "Crew Lifesigns = " + state.ship.Lifesigns,
                        TextColor = Color.LightSeaGreen,
                        FontSize = fontsize,
                    },
                    new Label
                    {
                        Text = "Fuel = " + state.ship.Fuel,
                        TextColor = Color.LightSeaGreen,
                        FontSize = fontsize,
                    },
                }
            };

            Button DismissButton = new Button
            {
                Text = "Dismiss",
                TextColor = Color.LightGreen,
                FontSize = fontsize,
                BackgroundColor = Color.DarkSlateGray,
            };
              
            List<Label> HiddenLabels = new List<Label>();
            //We wanna hide the text that appears under the menu
            StatsButton.Clicked += (object sender, EventArgs e) =>
            {
                grid.Children.Add(stats, 0, 2, 0, 1);
                grid.Children.Add(DismissButton,0,2,1,2);
                grid.Children.Remove(StatsButton);
                foreach (Label l in grid.Children.OfType<Label>())
                {
                    if (Grid.GetRow(l) < 6)
                    {
                        HiddenLabels.Add(l);
                        l.IsVisible = false;
                    }
                }
            };

            DismissButton.Clicked += (object sender, EventArgs e) =>
            {
                
                grid.Children.Add(StatsButton, 0, 2, 1, 2);
                grid.Children.Remove(DismissButton);
                foreach (Label l in HiddenLabels)
                {
                    l.IsVisible = true;
                }
                HiddenLabels.Clear();
                grid.Children.Remove(stats);
                
            };
            */
            // SECOND OPTION: Display the stats as regular text:
            StatsButton.Clicked += (object sender, EventArgs e) =>
            {
                AddLabel("System.Diagnostics()");
                AddLabel("Hull Integrity = " + state.ship.HullIntegrity);
                AddLabel("Crew Lifesigns = " + state.ship.Lifesigns);
                AddLabel("Fuel = " + state.ship.Fuel);
            };
            
            grid.Children.Add(StatsButton, 0, 2, 1, 2);
            //

            this.Padding = new Thickness(10, 20, 10, 10); //Some breathing room around the edges
            this.Content = grid;//Puts the grid on the page
            this.BackgroundColor = Color.Black;

            //BEFORE YOU RUN: You can right click the sub-projects to the right
            //such as TestApp.UWP and choose Set as Start Up Project to choose your platform


        }

        void AddLabels(string continuestring = "0")
        {
            int continuing = Convert.ToInt32(continuestring);//convert to int so we can use it 
            continuestring = Convert.ToString(continuing + 1);//update the string so that if it's used again it will have moved forward
            int lineCount = 0;
            //Text interpretation will happen here?
            foreach (string s in current.text)
            {
                //will add code to check for variables and whatnot

                //string is split up so that it fits onto a phone screen
                string[] strings = s.Split('|');
#if __MOBILE__
                for (int i = 0; i < strings.Length; ++i)
                {
                    lineCount++;//Keeping track of how many lines into the event we are
                    if ((continuing * (maxrow-3)) <= lineCount)
                    {
                        if (lineCount < ((continuing + 1) * (maxrow-3)))
                            // Between these two "if"s, only maxrow-3 at a time is displayed at a time
                            // "Continuing" pushes it forward to the next maxrow-2 items
                        {
                            AddLabel(strings[i], i == 0 ? false : true);
                        }
                        else
                        {
                            //if it's past the current maxrow-3, Stop and give continue option
                            continueButton(continuestring);
                            return;
                        }
                    }// if it's under the current maxrow-3, keep going.
                    
                }
#else
                lineCount++;
                //string is put back together if it's not on a phone screen
                string l = "";
                foreach (string x in strings)
                {
                    l += x;
                }

                if ((continuing * (maxrow-3)) <= lineCount) 
                {
                    if (lineCount < ((continuing + 1)* (maxrow-3)))
                    {
                        AddLabel(l); 
                    }
                    else
                    {
                        continueButton(continuestring);
                        return;
                    }   
                }
                
                
                
#endif
            }
        }

        void continueButton(string continuing)
        {
            //First we clear the buttons, as usual
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

            GameButton continueButton = new GameButton
            {
                Text = "> Continue",
                Key = continuing,//Use the button key to keep track of how far into the event we are
                TextColor = Color.LightGreen,

                FontSize = fontsize,

                BackgroundColor = Color.DarkSlateGray,

                buttonOption = new Option() { text = "Continue"},//Since key is taken, this will be used as the identifier

            };
            continueButton.Clicked += buttonClicked;
            grid.Children.Add(continueButton, 0, maxrow + 3);//put it on the grid
        }

        void AddLabel(string text, bool continuing = false)
        {
            grid.Children.Add(new Label
            {
                Text = continuing? text :"> " + text,
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
                    if (Grid.GetRow(l) == 2)
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
                    //HorizontalOptions = LayoutOptions.StartAndExpand,

                    FontSize = fontsize,

                    BackgroundColor = Color.DarkSlateGray,

                    buttonOption = op[i],

                    
                };
                button.Clicked += buttonClicked;

#if __MOBILE__
                grid.Children.Add(button, 0, 1, maxrow - i + 4, maxrow - i + 5);
#else
                grid.Children.Add(button, i, i+1, maxrow + 3, maxrow + 4);
#endif
                //make sure to put this in a different column each loop
            }
        }

        void buttonClicked(object sender, EventArgs e)
        {
            GameButton button = (GameButton)sender;

            if (button.buttonOption.text == "Continue")//Continue button is a special case, because it doesn't come from options
            {
                AddButtons(current.options);
                AddLabels(button.Key);//Using the button key to keep track of how far into the event you are.
                return;
            }

            AddLabel(button.buttonOption.text);
            
            // this may be done later by retreiving text from a database (?)
            switch (button.Key)
            {
                case "option0":
                    state.goTo(current.options[0].nextEventNumber);
                    break;
                case "option1":
                    state.goTo(current.options[1].nextEventNumber);
                    break;
                case "option2":
                    state.goTo(current.options[2].nextEventNumber);
                    break;
                case "option3":
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
            AddButtons(current.options);
            AddLabels();
        }
    }
}