using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using PCLStorage;

namespace TestApp
{
    public class GameStateClass
    {
        /*
        private int[] ReadText(String file, int typeSelector, int eventNumber = -1)
        {
            //Reads text from file to return either Events or Options
            int[] ReturnArray = new int[30];
            //if tS == 0, returns Events
            if (typeSelector == 0)
            {
                //returns array of all event #'s
            }
            //if tS == 1, returns Options
            else if (typeSelector == 1)
            {
                //returns option numbers that match with event number
            }

            return ReturnArray;
        }

        private List<List<List<int>>> CreateEventList()
        {
            //Creates the Event List by utilizing ReadText to get specifications
            //int[] EventsArray = this.ReadText(file,0)
            int[] EventsArray = new int[] { 1, 2, 3, 4 };
            //EventsArray determined by ReadText(tS=0)

            List<List<List<int>>> ReturnList = new List<List<List<int>>>();



            for (int i = 0; i <= EventsArray.Length - 1; i++)
            {
                List<List<int>> Temp = new List<List<int>>();
                //This 0 is always 0 at initialization to show that this event has not happened yet
                Temp.Add(new List<int> { 0 });
                //This next List is determined by ReadText returning options
                //Temp.Add(ReadText(file,typeSelector = 1, eventNumber = x);
                ReturnList.Add(Temp);
            }


            return ReturnList;
        }

        private List<List<int>> CreateOptionList()
        {
            //Creates the Option List by utilizing ReadText to get a list of options
            //int[] OptionsArray = this.ReadText(file,1)
            int[] OptionsArray = new int[] { 1, 2, 3, 4 };
            //OptionsArray determined by ReadText(tS=1)

            List<List<int>> ReturnList = new List<List<int>>();

            for (int i = 0; i <= OptionsArray.Length - 1; i++)
            {
                List<int> Temp = new List<int>();
                //First Value in this list shows whether or not the player has chosen this choice before (Always 0 on initialization)
                Temp.Add(0);
                //Second Value is whether this option is available to the user at the current time (0 or 1 on initialization)
                Temp.Add(1);
                //This next List is determined by ReadText returning options
                //Temp.Add(ReadText(file,typeSelector = 1, eventNumber = x);
                ReturnList.Add(Temp);


                /*
                 * FINAL THOUGHTS 
                 * Options list could be triple deep list (Could have a list of next events in addition to booleans)
                 * Find out how the text is going to be formatted
                 * Find out how to import text 
                 * winState/loseState implementation
                 * Ship class in GameState?
                 * Saving/Loading 
                 * 
                 * 
                 *

            }


            return ReturnList;
        }

        int[,][] EventArray = new int[,][]
        {
            // Declares a 3D array with the first inner array being a boolean value stating whether event has happened already or not, and the second inner array being option for the Event
            { new[] { 0 }, new[] { 1,2,3,4 } },
            { new[] { 0 }, new[] { 5,6,7,8 } },
            { new[] { 0 }, new[] { 9,10,11,12 } },
            { new[] { 0 }, new[] { 13,14,15,16 } }
        };

        int[][] OptionArray = new int[][]
        {
            new int[] {1,3,5,7,9},
            new int[] {0,2,4,6},
            new int[] {11,22}
        };
        */

        private List<Event> eList = new List<Event>();
        private int currentEvent;
        public bool ready;
        public Ship ship;

        public Event getCurrent()
        {
            // Removed this line, in case events are not inserted in order
            //return eList[currentEvent];
            foreach (Event e in eList)
            {
                if (e.eventNumber == currentEvent)
                {
                    return e;
                }
            }

            return null;
        }
        public void goTo(int next)
        {
            currentEvent = next;
        }

        public void ClearEventList()
        {
            while (eList.Count > 0)
            {
                eList.RemoveAt(0);
            }
        }
        public void PrintEventList()
        {
            foreach (Event e in eList)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        // This simply fills up the List of events with dummy events.
        public void GenerateSampleData()
        {
            Option o1, o2, o3, o4;
            Event e;
            List<String> t;

            o1 = new Option(0, 1, false, "doOne()", "This is a summary for choice one", new List<string> {"This is the result of choice one.", "And here is another line of the result."});
            o2 = new Option(1, 3, false, "doTwo()", "This is a summary for choice two", new List<string> { "This is the result of choice two.", "And here is another line of the result." });
            o3 = new Option(2, 2, false, "doThree()", "This is a summary for choice three", new List<string> { "This is the result of choice three.", "And here is another line of the result." });
            o4 = new Option(3, 4, false, "doFour()", "This is a summary for choice four", new List<string> { "This is the result of choice four.", "And here is another line of the result." });
            t = new List<string>()
            {
                "This is the first event!",
                "Feel free to read this at your own disposal",
                "Multiple lines of text in one Event right here!"
            };
            e = new Event(0, "Initial Event", t, new List<Option> { o1, o2, o3, o4 });

            eList.Add(e);
            
            o1 = new Option(0, 0, false, "escogerUno()", "This is a summary for choice one", new List<string> { "This is the result of choice one.", "And here is another line of the result." });
            o2 = new Option(1, 2, false, "escogerDos()", "This is a summary for choice two", new List<string> { "This is the result of choice two.", "And here is another line of the result." });
            o3 = new Option(2, 3, false, "escogerTres()", "This is a summary for choice three", new List<string> { "This is the result of choice three.", "And here is another line of the result." });
            o4 = new Option(3, 4, false, "escogerCuatro()", "This is a summary for choice four", new List<string> { "This is the result of choice four.", "And here is another line of the result." });
            t = new List<string>()
            {
                "Welcome to event number 2",
                "Or, as I like to say, numero dos",
                "If you couldn't already tell, this event was| based on Kloss's test class in EventClass.cs!"
            };
            e = new Event(1, "Event Two", t, new List<Option> { o1, o2, o3, o4 });

            eList.Add(e);

            o1 = new Option(0, 0, false, "gotoEvent(1)", "This is a summary for choice one", new List<string> { "This is the result of choice one.", "And here is another line of the result." });
            o2 = new Option(1, 1, false, "gotoEvent(2)", "This is a summary for choice two", new List<string> { "This is the result of choice two.", "And here is another line of the result." });
            o3 = new Option(2, 3, false, "gotoEvent(4)", "This is a summary for choice three", new List<string> { "This is the result of choice three.", "And here is another line of the result." });
            o4 = new Option(3, 4, false, "gotoEvent(5)", "This is a summary for choice four", new List<string> { "This is the result of choice four.", "And here is another line of the result." });
            t = new List<string>()
            {
                "We are now at the third event",
                "This one only has two lines!!!"
            };
            e = new Event(2, "Third Event", t, new List<Option> { o1, o2, o3, o4 });

            eList.Add(e);

            o1 = new Option(0, 0, false, "toBeginning()", "This is a summary for choice one", new List<string> { "This is the result of choice one.", "And here is another line of the result." });
            o2 = new Option(1, 2, false, "toPrevious()", "This is a summary for choice two", new List<string> { "This is the result of choice two.", "And here is another line of the result." });
            o3 = new Option(2, 4, false, "toNext()", "This is a summary for choice three", new List<string> { "This is the result of choice three.", "And here is another line of the result." });
            o4 = new Option(3, 4, false, "toEnd()", "This is a summary for choice four", new List<string> { "This is the result of choice four.", "And here is another line of the result." });
            t = new List<string>()
            {
                "The next event is the last one.",
                "It's the most realistic event in this sample| data.",
                "However, it is still silly.",
                "Enjoy."
            };
            e = new Event(3, "#4", t, new List<Option> { o1, o2, o3, o4 });

            eList.Add(e);

            o1 = new Option(0, 0, false, "releaseCargo('BoxOfSupplies')", "This is a summary for choice one", new List<string> { "This is the result of choice one.", "And here is another line of the result." });
            o2 = new Option(1, 0, false, "examineLifeSigns()", "This is a summary for choice two", new List<string> { "This is the result of choice two.", "And here is another line of the result." });
            o3 = new Option(2, 0, false, "probePlanet()", "This is a summary for choice three", new List<string> { "This is the result of choice three.", "And here is another line of the result." });
            o4 = new Option(3, 0, false, "selfDestruct()", "This is a summary for choice four", new List<string> { "This is the result of choice four.", "And here is another line of the result." });
            t = new List<string>()
            {
                "A distress signal is coming in from the| nearby planet",
                "The coordinates of the signal seem to be| coming from beneath the surface of the| only ocean of the planet, which covers| less than 20% of its surface",
                "The signal asks for a box of supplies, but| stresses not to intervene in any other way",
                "This is the first event!",
                "Feel free to read this at your own disposal",
                "Multiple lines of text in one Event right here!",
                "The next event is the last one.",
                "It's the most realistic event in this sample| data.",
                "However, it is still silly.",
                "Enjoy.",
                "Welcome to event number 2",
                "Or, as I like to say, numero dos",
                "A distress signal is coming in from the| nearby planet",
                "The coordinates of the signal seem to be| coming from beneath the surface of the| only ocean of the planet, which covers| less than 20% of its surface",
                "The signal asks for a box of supplies, but| stresses not to intervene in any other way",
                "This is the first event!",
                "Feel free to read this at your own disposal",
                "Multiple lines of text in one Event right here!",
                "The next event is the last one.",
                "It's the most realistic event in this sample| data.",
                "However, it is still silly.",
                "Enjoy.",
                "Welcome to event number 2",
                "Or, as I like to say, numero dos",
                "If you couldn't already tell, this event was| based on Kloss's test class in EventClass.cs!"
            };
            e = new Event(4, "Underwater Investigation", t, new List<Option> { o1, o2, o3, o4 });

            eList.Add(e);
        }

        public void SaveEvents()
        {
            foreach (Event e in eList)
            {
                int id = e.eventNumber;

                SaveManager.SaveObject(@"testEvent"+id+"Save.xml",e);
            }
        }
        public void LoadEvents()
        {
            if (eList.Count == 0)
            {
                eList = SaveManager.LoadAll();
            }
        }
    }
}
