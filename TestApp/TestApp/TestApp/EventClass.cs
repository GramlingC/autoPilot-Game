using System;
using System.Collections.Generic;

//Contains Event class and Options class. 
namespace TestApp
{
    public class Event
    {
        private LinkedList<int> events = new LinkedList<int>();
        private LinkedListNode<int> current;

        public Event()
        {
            //Read a line from the file with the total amount of events
            //Subsitute 30 for n which will be retrieved
            for (int x = 1; x <= 30; x++)
            {
                events.AddLast(x);
            }
            current = events.First;
        }

        public int GetCurrentEvent()
        {
            return current.Value;
        }

        public void SetCurrentEvent(int newEvent)
        {
            current = events.Find(newEvent); //error when newEvent not in the an event linked list
        }

        public void NextEvent()
        {
            current = current.Next;
        }

        //returns list of options depending on the current event. ex. [1, 2, 3, 4]
        public List<int> GetOptions(Options o)
        {
            int eventNumber = this.GetCurrentEvent();
            List<int> optionList = o.GetOptions()[eventNumber][1];
            return optionList;
        }

        //returns list of next events depending on the current event and the options. ex. [2, 3, 4, 5]
        public List<int> GetNextEventsList(Options o)
        {
            int eventNumber = this.GetCurrentEvent();
            List<int> eventList = o.GetOptions()[eventNumber][2];
            return eventList;
        }

        //Returns an int which refers to the next event according to the option chosen. Returns 0 is the chosen option is not in the option list
        public int GetNextEvent(Options o, int chosenOption)
        {
            int eventNumber = this.GetCurrentEvent();
            List<int> optionList = GetOptions(o);
            for (int i = 0; i < optionList.Count; i++)
                if (optionList[i] == chosenOption)
                    return GetNextEventsList(o)[i];
            return 0;
        }
    }




    public class Options
    {
        private List<List<List<int>>> options = new List<List<List<int>>>();
        private int eventCount = 0;

        public Options(int eventCount)
        {
            this.eventCount = eventCount;
        }

        public void CreateOption()
        {
            //Things being added to Temp will change (as in how they are entered) depending on how we read from the file
            for (int i = 1; i <= eventCount; i++)
            {
                List<List<int>> Temp = new List<List<int>>();
                Temp.Add(new List<int> { i }); //Event#
                Temp.Add(new List<int> { i, i + 1, i + 2, i + 3 }); //Options
                Temp.Add(new List<int> { i + 4, i + 5, i + 6, i + 7 }); //NextEvent (dependent on option it corresponds to
                options.Add(Temp);
            }
        }
        public List<List<List<int>>> GetOptions()
        {
            return options;
        }

        public int GetEventCount()
        {
            return eventCount;
        }


    }
}
/*
public class Test
{
 public static void Main()
 {
     Options o = new Options(30);
     o.CreateOption();
     Event e = new Event();
     e.SetCurrentEvent(7);
     List<int> z = e.GetOptions(o);
     for (int x = 0; x < z.Count; x++)
	Console.WriteLine(z[x]);
    Console.WriteLine(e.GetNextEvent(o, 9));
 }
}
*/

//Format of Options list: [[[event#],[boolOption1, ...], [nextOption1, ...]]]
//Ex: [[[1],[op1,op2,op3,op4],[0, 0, 0, 0], [5,6,7,8]],[[2],[op5, op6, op7, op8],[0, 0, 0, 0], [12,3,5,75]]]

//What we expect to read from the file:
//Event 1: "Text A", Options[1,2,3,4]
//...
//Option 1: "Text1", NextEvent = 5
//Option 2: "Text2", NextEvent = 6