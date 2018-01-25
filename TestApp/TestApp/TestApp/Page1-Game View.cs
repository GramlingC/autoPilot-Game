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
        //font will also be smaller
#if __MOBILE__
        public double fontsize = 10;
#else
        public double fontsize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
#endif

        public Event currentEvent;
        public GameStateClass state;

        // Base view of the entire page
        AbsoluteLayout pageContent;

        // Can use bar at top for ship stats/pause game button
        // (not sure if Grid is best choice, and may decide to change/remove this)
        //StackLayout topBarArea;

        // Text area where all text labels appear
        // Has a stack layout inside of it, to stack all text labels on top of each other.
        ScrollView textArea;
        StackLayout textStack;

        // Button area at bottom of screen 
        // (not sure if Grid is best choice)
        public OptionsMDPage buttonArea;
        
        public Page1()
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("regular 1 loop cut.wav");
            player.Loop = true;
            player.Play();

            
            //
            // Everything below is for saving ship to file, just examples
            //
            Ship testShip = new Ship(100, 100, 100, 0);

            state = new GameStateClass();
            state.ship = testShip;

            //Generate events from text files
            state.TranslateEvents();
            
            //state.LoadEvents();
            // These two lines generate the event data
            //state.GenerateSampleData();
            //state.SaveEvents();
            //state.LoadEvents();

            currentEvent = state.getCurrent();

            pageContent = new AbsoluteLayout();

            //topBarArea = new StackLayout();

            textStack = new StackLayout();
            textArea = new ScrollView
            {
                // We only want the view to scroll vertically, not horizontally
                Orientation = ScrollOrientation.Vertical,
                Content = textStack
            };

            buttonArea = new OptionsMDPage(this);

            // Set porportions of page layout dependent on platform
            // ADJUST MOBILE LAYOUT HERE
#if __MOBILE__
            // Set the top bar area to stretch screen width and take up small portion of top of window
            //AbsoluteLayout.SetLayoutBounds(topBarArea, new Rectangle(0, 0, 1, .1));
            //AbsoluteLayout.SetLayoutFlags(topBarArea, AbsoluteLayoutFlags.All);

            // Set the top bar area to stretch screen width and take up 50% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(textArea, new Rectangle(0, 0, 1, .5));
            AbsoluteLayout.SetLayoutFlags(textArea, AbsoluteLayoutFlags.All);

            // Set the button area to stretch screen width and take up last 40% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#else
            // Set the top bar area to stretch screen width and take up small portion of top of window
            //AbsoluteLayout.SetLayoutBounds(topBarArea, new Rectangle(0, 0, 1, .05));
            //AbsoluteLayout.SetLayoutFlags(topBarArea, AbsoluteLayoutFlags.All);

            // Set the top bar area to stretch screen width and take up 50% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(textArea, new Rectangle(0, 0, 1, .6));
            AbsoluteLayout.SetLayoutFlags(textArea, AbsoluteLayoutFlags.All);

            // Set the button area to stretch screen width and take up last 40% of window (approximately)
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .4));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#endif

            // Colors to debug layout
           // topBarArea.BackgroundColor = Color.Maroon;
            //textArea.BackgroundColor = Color.DarkBlue;
            //buttonArea.BackgroundColor = Color.DarkGray;

            // Add all page parts to the page
            //pageContent.Children.Add(topBarArea);
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

/*
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
 */
            

            AddButtons(currentEvent.options);

            //#if !__MOBILE__
            //            GameButton button = new GameButton
            //            {
            //                Text = "> clearScreen()",
            //                Key = "clearScreen",
            //                TextColor = Color.LightGreen,
            //                //Adapt to device size
            //
            //                FontSize = fontsize,
            //
            //                BackgroundColor = Color.DarkSlateGray,
            //                buttonOption = new Option
            //                {
            //                    text = "",
            //                },
            //            };
            //            button.Clicked += buttonClicked;//adding the function to this button's click
            //
            //            grid.Children.Add(button, 5, 6, maxrow + 3, maxrow + 4);
            //#endif

            //Displaying Stats:

            GameButton StatsButton = new GameButton
            {
                Text = "System.Diagnostics()",
                BackgroundColor = Color.DarkSlateGray,
                TextColor = Color.LightSeaGreen,
                FontSize = fontsize,
            };

            //logPage = new Page2(state);

            ///////////////////////////////////////////////////////////////////
            //Not sure how the buttons are being managed here with the GameButton class and how clearing
            //the page for text and the IsVisible stuff so the Log button is not currently in but the 
            //Log page works and the back button works. Also not sure on what async means so i didnt add it
            //for the button clicked
            GameButton Log = new GameButton
            {
                Text = "Log",
                BackgroundColor = Color.Black,
                TextColor = Color.Red,
                FontSize = fontsize
            };
            //grid.Children.Add(Log, 0, 0);
            Log.Clicked += goToLog;
            ///////////////////////////////////////////////////////////////////

            //topBarArea.Children.Add(Log);
            
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
            //StatsButton.Clicked += (object sender, EventArgs e) =>
            //{
            //    Text = "System.Diagnostics()",
            //    BackgroundColor = Color.DarkSlateGray,
            //    TextColor = Color.LightSeaGreen,
            //    FontSize = fontsize,
            //};

            this.Padding = new Thickness(5, 5, 5, 5); //Some breathing room around the edges
            this.Content = pageContent;//Puts the content on the page
            this.BackgroundColor = Color.Black;


            //BEFORE YOU RUN: You can right click the sub-projects to the right
            //such as TestApp.UWP and choose Set as Start Up Project to choose your platform
            //Added for Log\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            //state.AddToUsedText();
            /////////////////////////////////////////////////

            BeginGame();
        }
        
        // Separating some of this into an async function. This is called at the end of page constructor.
        async void BeginGame()
        {
            currentEvent = state.getCurrent();
            AddButtons(currentEvent.options);
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
                            state.AddToUsedText(strings[i]);
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
                        state.AddToUsedText(l);
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
            GameLabel textLabel = new GameLabel(text, textArea);
            textLabel.FontSize = fontsize;
            textStack.Children.Add(textLabel);

            //await Task.Delay(100);

            // BUG: does not scroll all the way down
            //textArea.ScrollToAsync(0, textArea.ContentSize.Height, false);

            //textArea.ScrollToAsync(textLabel, ScrollToPosition.End, false);
            await textLabel.DisplayText();

            // Add any text printed to terminal to the game state's log of text
            state.AddToUsedText("> " + text);
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
        // Displays the results of an action (option) chosen
        async Task OutputOptionResult(Option option)
        {
            foreach (string s in option.resultText)
            {
                // Remove any lingering pipes from text (for sample events mainly)
                string str = s.Replace("|", "");

                await AddLabel(str);
            }
            return;
        }
        
/*
        void continueButton(string continuing)
        {
            //First we clear the buttons, as usual
            List<View> removable = new List<View>();
            foreach (GameButton b in grid.Children.OfType<GameButton>())
            {
                if (b.Key != null && (b.Key.Contains("option") || b.Text == "> Continue"))
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
        public void AddButtons(List<Option> op)
        {
            pageContent.Children.Remove(buttonArea);
            buttonArea = new OptionsMDPage(this);
#if __MOBILE__
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .5));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#else
            AbsoluteLayout.SetLayoutBounds(buttonArea, new Rectangle(0, 1, 1, .4));
            AbsoluteLayout.SetLayoutFlags(buttonArea, AbsoluteLayoutFlags.All);
#endif
          //buttonArea.BackgroundColor = Color.DarkGray;
            pageContent.Children.Add(buttonArea);
            /* Moving this all to buttonClicked for now
            List<View> removable = new List<View>();
            foreach (GameButton b in buttonArea.buttons)
            {
                if (b.Key != null && (b.Key.Contains("option") || b.Text == "> Continue"))
                {
                    removable.Add(b);
                }
            }
            foreach (GameButton b in removable)
                buttonArea.buttons.Remove(b);

            
            GameButton button;

            for (int i = 0; i < 4; i++)
            {
                button = new GameButton
                {
                    Text = "> " + op[i].text,
                    Key = "option" + i,
                    TextColor = Color.LightGreen,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,

                    FontSize = fontsize,

                    BackgroundColor = Color.DarkSlateGray,

                    buttonOption = op[i],

                    //IsVisible = false,
                    IsEnabled = false
                };

            

                //#if __MOBILE__
                //                grid.Children.Add(button, 0, 1, maxrow - i + 4, maxrow - i + 5);
                //#else
                //                grid.Children.Add(button, i, i+1, maxrow + 3, maxrow + 4);
                //#endif
                //make sure to put this in a different column each loop
            }*/
        }
        public void ShowChoices()
        {
            foreach (GameButton gb in buttonArea.buttons)
            {
                //gb.IsVisible = true;

                // Only enable the button if the ship's attributes 
                // meet the requirements of the chosen option.
                Option o = gb.buttonOption;
                Ship s = state.ship;
                bool hull = o.HullRequired < 0 ? s.HullIntegrity < Math.Abs(o.HullRequired): s.HullIntegrity >= o.HullRequired;
                bool fuel = o.FuelRequired < 0 ? s.Fuel < Math.Abs(o.FuelRequired) : s.Fuel >= o.FuelRequired;
                bool life = o.LifeRequired < 0 ? s.Lifesigns < Math.Abs(o.LifeRequired) : s.Lifesigns >= o.LifeRequired;
                bool weap = o.WeapRequired < 0 ? s.Weapons < Math.Abs(o.WeapRequired) : s.Weapons >= o.WeapRequired;
                bool emp = o.EmpRequired < 0 ? s.EmpathyLevel < Math.Abs(o.EmpRequired) : s.EmpathyLevel >= o.EmpRequired;

                if (hull && fuel && life && weap && emp 
                    && !o.optionPicked)
                {
                    gb.IsEnabled = true;
                    gb.IsVisible = true;
                }
            }
        }
        public async void buttonClicked(object sender, EventArgs e)
        {
            // Save which button we got
            GameButton button = (GameButton)sender;

            if (!button.IsVisible)
            {
                return;
            }

            foreach (GameButton b in buttonArea.buttons)
            {
                if (b.Key != null && b.Key.Contains("option"))
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
            //
            // Print the text displayed on the button
            await AddLabel(button.buttonOption.text);

            buttonArea.Content = buttonArea.Master.Content;
            buttonArea.buttons.Clear();

            // Change the attributes of the ship based on chosen option
            // Note: Moving this to AFTER the OutputOptionResult method will print
            //       the option result, and THEN do any changes to ship variables.
            ModifyShipAttributes(button.buttonOption);
            // DEBUG: Print ship in console after changes
            //Debug.WriteLine(state.ship.ToString());

            // Print the results of the option chosen
            await OutputOptionResult(button.buttonOption);
            currentEvent.options[button.buttonOption.optionNumber-1].optionPicked = true;
            // this may be done later by retreiving text from a database (?)
            if (button.buttonOption.nextEventNumber == currentEvent.eventNumber)
            {
                AddButtons(currentEvent.options);
                ShowChoices();
                return;
            }
            state.goTo(button.buttonOption.nextEventNumber);

            currentEvent = state.getCurrent();
            
            await AddLabels();
            AddButtons(currentEvent.options);
            ShowChoices();

            //buttonArea.Update();
        }

        
        // Modifies the attributes of the ship based on a given option.
        // Calls game over state if any attribute falls below 0;
        private void ModifyShipAttributes(Option o)
        {
            Ship s = state.ship;
            // In this function we can create any notifications of
            // Ship variable changes
            if (o.HullChange != 0)
            {
                s.ChangeHullIntegrity(o.HullChange);
            }

            if (o.FuelChange != 0)
            {
                s.ChangeFuel(o.FuelChange);
            }

            if (o.LifeChange != 0)
            {
                s.ChangeLifesigns(o.LifeChange);
            }

            if (o.WeapChange != 0)
            {
                s.ChangeWeap(o.WeapChange);
            }

            if (o.EmpChange != 0)
            {
                s.ChangeEmpathyLevel(o.EmpChange);
            }

            // Calls game over state if any attribute falls below 0.
            // This can be changed if we decide on different lose states.
            if (s.HullIntegrity <= 0 ||
                s.Fuel <= 0 ||
                s.Lifesigns <= 0)
            {
                GameOver();
            }
        }

        private async void GameOver()
        {
            // Need to implement game over state in here
            await AddLabel("Game Over!");
            throw new NotImplementedException();
        }

        //for going to the log. im not sure what async does so i didnt put it here but it can be changed if needed
        void goToLog(object sender, EventArgs e)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            // Request for game to pause any typing labels
            PauseText(tcs);

            //Navigation.PushModalAsync(logPage);
            Page2 log = new Page2(state, tcs);
            Navigation.PushAsync(log);
        }

        // Pauses the text by passing a TCS (TaskCompletionSource) to each gamelabel, which
        // will pause the text until a signal is received that the log is closed.
        public void PauseText(TaskCompletionSource<bool> _tcs)
        {
            foreach (GameLabel gl in textStack.Children)
            {
                if (!gl.Completed)
                {
                    gl.PauseText(_tcs);
                }
            }
        }
    }
}