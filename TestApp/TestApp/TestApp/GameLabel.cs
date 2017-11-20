using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
    class GameLabel : Label
    {
        public GameLabel()
        {
            FullText = "";
            Completed = false;
            RequestEnd = false;
            Text = "";
            TextColor = Color.LightGreen;
        }
        public GameLabel(string text)
        {
            FullText = text;
            Completed = false;
            RequestEnd = false;
            Text = "";
            TextColor = Color.LightGreen;

            //DisplayText();
        }

        public string FullText { get; set; } // The full text that will eventually be displayed
        public bool Completed { get; set; }  // If the text has finished displaying entirely
        public bool RequestEnd { get; set; } // If the calling function has requested to finish text displaying early

        // Loop through the text and display it.
        public async Task DisplayText()
        {
            while (!Completed && !RequestEnd)
            {
                AdvanceText();
                await Task.Delay(20);
            }

            if (RequestEnd)
            {
                CompleteText();
            }

            Completed = true;
        }

        // Advance text by a character
        public void AdvanceText()
        {
            int curLength = Text.Length;

            //Text.Insert(curLength, FullText.Substring(curLength, 1));
            Text = FullText.Substring(0, curLength + 1);

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
