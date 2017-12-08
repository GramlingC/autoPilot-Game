using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
    class GameLabel : Label
    {
        public GameLabel() : this("") { }
        public GameLabel(string text)
        {
            FullText = "> " + text;
            Completed = false;
            RequestEnd = false;
            Text = ">";
            TextColor = Color.LightGreen;
            Paused = false;

            //DisplayText();
        }

        public string FullText { get; set; } // The full text that will eventually be displayed
        public bool Completed { get; set; }  // If the text has finished displaying entirely
        public bool RequestEnd { get; set; } // If the calling function has requested to finish text displaying early
        public bool Paused { get; set; }     // If the text typing should be paused or not.

        // Loop through the text and display it.
        public async Task DisplayText()
        {
            while (!Completed && !RequestEnd && !Paused)
            {
                AdvanceText();
                await Task.Delay(20);
            }

            if (RequestEnd)
                CompleteText();

            if (!Paused)
                Completed = true;
        }

        // Advance text by a character
        public void AdvanceText()
        {
            int curLength = Text.Length;

            // Replace the text to display by itself plus one extra letter from FullText
            Text = FullText.Substring(0, curLength + 1);

            // If the text is equal to the FullText, set as completed
            Completed = FullText.Equals(Text);

            return;
        }

        // Completely show text and mark it as completed.
        public void CompleteText()
        {
            if (!FullText.Equals(Text))
            {
                Text = FullText;
                Completed = true;
            }

            return;
        }
    }
}
