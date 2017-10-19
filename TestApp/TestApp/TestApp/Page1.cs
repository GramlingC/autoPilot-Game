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
                //Defines the rows
                RowDefinitions =
                {
                    //The row definition defines the height of each row
                    //GridLength.Auto means it'll fit exactly the size of what's inside it
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height = GridLength.Auto},
                    //this type of grid length expands to fill in what isn't taken up by other rows
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto}
                },
                //I want several lines of text near the top, that's the Autos
                //I want a large empty space, that's the star
                //I want buttons at the bottom, that's the last Auto

                ColumnDefinitions =
                {
                    //Same here, but I want 4 buttons and space after them
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = GridLength.Auto},
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)}
                }
            };

            //Now, to add some content.
            grid.Children.Add(new Label
            {
                Text = "> First Text.",
                TextColor = Color.LightGreen
                //Here we could keep editing this and make it be different sizes,angles,etc
                //If we declared this as a global variable, we could change it with functions
            },0,5,0,1);
            //The parameters are (View,Left,Right,Top,Bottom)
            //View is anything from Labels to buttons to sliders, etc, and are adapt to platform
            //The numbers are grid positions. Left and top are the grid positions where they start
            //Top and Bottom are where they end, they will not enter these grid positions
            //for example, the grid's column index 5 doesn't exist, the last is 4
            //But it'll stop before 5, which means it'll span all the columns

            grid.Children.Add(new Label
            {
                Text = "> Second Text. This text will hopefully be so long that it'll wrap around to a second line, and this grid row will expand accordingly.",
                TextColor = Color.LightGreen
            }, 0, 5, 1, 2);//Make sure to put this in a different row

            grid.Children.Add(new Label
            {
                Text = "> Third Text.",
                TextColor = Color.LightGreen
            }, 0, 5, 2, 3);

            this.Padding = new Thickness(10, 20, 50, 50); //Some breathing room around the edges
            this.Content = grid;//Puts the grid on the page
            this.BackgroundColor = Color.Black;

            //BEFORE YOU RUN: You can right click the sub-projects to the right
            //such as TestApp.UWP and choose Set as Start Up Project to choose your platform
            //Only UWP works for me at the moment
        }
    }
}