using System;
using System.Linq;
using System.Diagnostics;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

//To make a page like this, right click TestApp (Portable)
//Choose Add, then choose Content Page (C#)

//Make sure to change App.xaml.cs to have it run your desired page
//At MainPage = new TestApp.Page1();, you'd change "Page1" to the name of the page you want it to run

namespace TestApp
{
    public class Page1 : ContentPage
    {
        int fontsize = 14;
        Event currentEvent;
        GameStateClass state;

        // Base view of the entire page
        AbsoluteLayout pageContent;

        // Can use bar at top for ship stats/pause game button
        // (not sure if Grid is best choice, and may decide to change/remove this)
        Grid topBarArea;

        // Text area where all text labels appear
        // Has a stack layout inside of it, to stack all text labels on top of each other.
        ScrollView textArea;
        StackLayout textStack;

        // Button area at bottom of screen 
        // (not sure if Grid is best choice)
        Grid buttonArea;
        
        public Page1()
        {
            pageContent = new AbsoluteLayout();

            topBarArea = new Grid();

            textStack = new StackLayout();
            textArea = new ScrollView
            {
                // We only want the view to scroll vertically, not horizontally
                Orientation = ScrollOrientation.Vertical,
                Content = textStack
            };

            buttonArea = new Grid();
            buttonArea.Padding = new Thickness(0, 10, 0, 0);

            // Set porportions of page layout dependent on platform
            // ADJUST MOBILE LAYOUT HERE
#if __MOBILE__
            // Set the top bar area to stretch screen width and take up small portion of top of window
            AbsoluteLayout.SetLayoutBounds(topBarArea, new Rectangle(0, 0, 1, .05));
            AbsoluteLayout.SetLayoutFlags(topBarArea, AbsoluteLayoutFlags.All);

            // Set the top bar area to stretch screen width and take up 50% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(textArea, new Rectangle(0, .15, 1, .5));
            AbsoluteLayout.SetLayoutFlags(textArea, AbsoluteLayoutFlags.All);

            // Set the button area to stretch screen width and take up last 40% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .4));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#else
            // Set the top bar area to stretch screen width and take up small portion of top of window
            AbsoluteLayout.SetLayoutBounds(topBarArea, new Rectangle(0, 0, 1, .05));
            AbsoluteLayout.SetLayoutFlags(topBarArea, AbsoluteLayoutFlags.All);

            // Set the top bar area to stretch screen width and take up 50% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(textArea, new Rectangle(0, .15, 1, .5));
            AbsoluteLayout.SetLayoutFlags(textArea, AbsoluteLayoutFlags.All);

            // Set the button area to stretch screen width and take up last 40% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .4));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#endif

            // Colors to debug layout
            topBarArea.BackgroundColor = Color.Maroon;
            textArea.BackgroundColor = Color.DarkBlue;
            buttonArea.BackgroundColor = Color.DarkGray;

            // Add all page parts to the page
            pageContent.Children.Add(topBarArea);
            pageContent.Children.Add(textArea);
            pageContent.Children.Add(buttonArea);

            // In our grid, add a TGR to recognize generic mouse/tap input (for skipping through text primarily)
            var tgr = new TapGestureRecognizer() { NumberOfTapsRequired = 1 };
            tgr.Tapped += (s, e) =>
            {
                // Handle tap event here
                // Eventually speeds up text
                foreach (GameLabel gl in textStack.Children.OfType<GameLabel>())
                {
                    if (!gl.Completed)
                    {
                        gl.CompleteText();
                    }
                }
            };
            
            // Add the TGR to the page content, to detect mouse clicks/taps
            pageContent.GestureRecognizers.Add(tgr);


//#if __MOBILE__
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
//#else
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//#endif
//
//            for (int i = 2; i < maxrow; i++)
//            {
//                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//                //I want several lines of text near the top, that's these Autos
//            }
//
//            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            //I want a large empty space, that's the star 
//
//#if __MOBILE__
//            //I want four buttons at the bottom, 
//            //but they need to be smaller in mobile to fit the screen
//            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
//            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
//            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
//            grid.RowDefinitions.Add(new RowDefinition { Height = 35 });
//#else
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
//#endif
 
            //
            // Everything below is for saving ship to file, just examples
            //
            Ship testShip = new Ship(100, 100, 100, 0);

            state = new GameStateClass();
            state.ship = testShip;

            // These two lines generate the event data
            state.GenerateSampleData();
            currentEvent = state.getCurrent();

            AddButtons(currentEvent.options);

            //GameButton StatsButton = new GameButton
            //{
            //    Text = "System.Diagnostics()",
            //    BackgroundColor = Color.DarkSlateGray,
            //    TextColor = Color.LightSeaGreen,
            //    FontSize = fontsize,
            //};

            this.Padding = new Thickness(10, 20, 10, 10); //Some breathing room around the edges
            this.Content = pageContent;//Puts the content on the page
            this.BackgroundColor = Color.Black;

            BeginGame();

        }
        
        // Separating some of this into an async function
        async void BeginGame()
        {
            await AddLabels();
            ShowChoices();
        }

        async Task AddLabels()
        {
            foreach (string s in currentEvent.text)
            {
                // Remove any lingering pipes from text (for sample events mainly)
                string str = s.Replace("|", "");

                await AddLabel(str);
            }
        }
/*
        async Task AddLabels(string continuestring = "0")
        {
            int continuing = Convert.ToInt32(continuestring);//convert to int so we can use it 
            continuestring = Convert.ToString(continuing + 1);//update the string so that if it's used again it will have moved forward
            int lineCount = 0;
            //Text interpretation will happen here?
            foreach (string s in currentEvent.text)
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
                            await AddLabel(strings[i], i == 0 ? false : true);
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
                        await AddLabel(l); 
                    }
                    else
                    {
                        continueButton(continuestring);
                        return;
                    }   
                }
                
                
                
#endif
            }

            return;
        }

    */
        async Task AddLabel(string text)
        {
            GameLabel textLabel = new GameLabel(text);
            textLabel.FontSize = fontsize;
            textStack.Children.Add(textLabel);

            textArea.ScrollToAsync(0, textArea.ContentSize.Height, false);
            //textArea.ScrollToAsync(textLabel, ScrollToPosition.End, false);
            await textLabel.DisplayText();
        }
/*
        async Task AddLabel(string text, bool continuing = false)
        {
            GameLabel nextLabel = new GameLabel(continuing ? text : "> " + text);
            nextLabel.FontSize = fontsize;
            grid.Children.Add(nextLabel, 0, 6, row, ++row);
            //The parameters are (View,Left,Right,Top,Bottom)
            //View is anything from Labels to buttons to sliders, etc, and are adapt to platform
            //The numbers are grid positions. Left and top are the grid positions where they start
            //Top and Bottom are where they end, they will not enter these grid positions
            //for example, the grid's column index 5 doesn't exist, the last is 4
            //But it'll stop before 5, which means it'll span all the columns

            // This should start the GameLabel object to slowly display it's text.
            await nextLabel.DisplayText();

            // This line would immediately present the text on the screen
            //nextLabel.CompleteText();


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
        */
/*
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

                buttonOption = new Option() { text = "Continue" },//Since key is taken, this will be used as the identifier

            };
            continueButton.Clicked += buttonClicked;
            grid.Children.Add(continueButton, 0, maxrow + 3);//put it on the grid
        }
        */
        void AddButtons(List<Option> op)
        {
            // Moving this all to buttonClicked for now
            List<View> removable = new List<View>();
            foreach (GameButton b in buttonArea.Children.OfType<GameButton>())
            {
                if (b.Key != "clearScreen")
                {
                    removable.Add(b);
                }
            }
            foreach (GameButton b in removable)
                buttonArea.Children.Remove(b);

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

                    //IsVisible = false
                    IsEnabled = false
                };
                button.Clicked += buttonClicked;

                // Position buttons in the grid.
                buttonArea.Children.Add(button, 0, 1, i, i + 1);

//#if __MOBILE__
//                grid.Children.Add(button, 0, 1, maxrow - i + 4, maxrow - i + 5);
//#else
//                grid.Children.Add(button, i, i+1, maxrow + 3, maxrow + 4);
//#endif
                //make sure to put this in a different column each loop
            }
        }
        void ShowChoices()
        {
            foreach (GameButton gb in buttonArea.Children.OfType<GameButton>())
            {
                //gb.IsVisible = true;
                gb.IsEnabled = true;
            }
        }
        async void buttonClicked(object sender, EventArgs e)
        {
            // Save which button we got
            GameButton button = (GameButton)sender;

            if (!button.IsVisible)
            {
                return;
            }

            foreach (GameButton b in buttonArea.Children.OfType<GameButton>())
            {
                if (b.Key.Contains("option"))
                    b.IsEnabled = false;
            }
           
            // Not quite sure how the continue system works
            //if (button.buttonOption.text == "Continue")//Continue button is a special case, because it doesn't come from options
            //{
            //    AddButtons(currentEvent.options);
            //    await AddLabels(button.Key);//Using the button key to keep track of how far into the event you are.
            //    ShowChoices();
            //    return;
            //}

            // this may be done later by retreiving text from a database (?)
            switch (button.Key)
            {
                case "option0":
                    state.goTo(currentEvent.options[0].nextEventNumber);
                    break;
                case "option1":
                    state.goTo(currentEvent.options[1].nextEventNumber);
                    break;
                case "option2":
                    state.goTo(currentEvent.options[2].nextEventNumber);
                    break;
                case "option3":
                    state.goTo(currentEvent.options[3].nextEventNumber);
                    break;
                //case "clearScreen":
                //    int index = 0;
                //    while (index < grid.Children.Count && row > 0)
                //    {
                //        if (grid.Children.ElementAt(index) is Label)
                //        {
                //            grid.Children.RemoveAt(index);
                //            row--;
                //        }
                //        else
                //        {
                //            index++;
                //        }
                //    }
                //    return;
            }
            currentEvent = state.getCurrent();
            AddButtons(currentEvent.options);
            await AddLabel(button.buttonOption.text);
            await AddLabels();
            ShowChoices();
        }
    }
}