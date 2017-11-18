using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TestApp
{
    class GameLabel : Label
    {
        public GameLabel() { FullText = ""; Completed = false; }
        public GameLabel(string text) { FullText = text; Completed = false; }

        public string FullText { get; set; } // The full text that will eventually be displayed
        public bool Completed { get; set; }  // If the text has finished displaying entirely

        // Advance text by a character
        public void AdvanceText()
        {
            int curLength = Text.Length;

            Text.Insert(curLength, FullText.Substring(curLength, 1));

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
