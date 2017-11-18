using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//Contains Event class and Options class. 
//Should make options first in GameState and put them into a List<Options> 
//Then in GameState, separate them based on what each event needs? (Check test at the bottom of this file)

namespace TestApp
{
    public class Event
    {
        public int eventNumber { get; set; }
        public String eventName { get; set; }
        public List<String> text { get; set; }
        public List<Option> options { get; set; }

        public Event()
        {
            eventNumber = 0;
            eventName = "";
            text = new List<String>();
            options = new List<Option>();
        }
        public Event(int eventNumber, String eventName, List<String> text, List<Option> options) 
        {
            this.eventNumber = eventNumber;
            this.eventName = eventName;
            this.text = new List<String>(text);
            this.options = new List<Option>(options);

            //for(int i = 0; i < options.Count; i++)
            //{
            //   this.options.Add(options[i]);
            //}
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(base.ToString());
            str.Append(": Event ");
            str.Append(eventNumber);
            str.Append(", ");
            str.Append(eventName);

            foreach (string s in text)
            {
                str.Append("\n\t");
                str.Append(s);
            }

            foreach (Option o in options)
            {
                str.Append("\n\t");
                str.Append(o);
            }

            str.Append("\n");

            return str.ToString();
        }
        ////get set eventNumber
        //public int GetEventNumber()
        //{
        //    return eventNumber;
        //}
        //
        //public void SetEventNumber(int n)
        //{
        //    eventNumber = n;
        //}
        //
        ////get set text
        //public String GetEventText()
        //{
        //    return text;
        //}
        //
        //public void SetEventText(String t)
        //{
        //    text = t;
        //}
        //
        ////get set options
        //public List<Option> GetOptions()
        //{
        //    return options;
        //}
        //
        //public void SetOptions(List<Option> op)
        //{
        //    options = op;
        //}
    }

    public class Option
    {
        public int optionNumber { get; set; }
        public int nextEventNumber { get; set; }
        public bool optionPicked { get; set; }
        public String text { get; set; }

        public Option()
        {
            this.optionNumber = 0;
            this.nextEventNumber = 0;
            this.optionPicked = false;
            this.text = "";
        }

        public Option(int optionNumber, int nextEventNumber, bool optionPicked, String text)
        {
            this.optionNumber = optionNumber;
            this.nextEventNumber = nextEventNumber;
            this.optionPicked = optionPicked;
            this.text = text;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(base.ToString());
            str.Append(": Option ");
            str.Append(optionNumber);
            str.Append(", Leads to event ");
            str.Append(nextEventNumber);
            str.Append(", Picked=");
            str.Append(optionPicked);
            str.Append(", '");
            str.Append(text);
            str.Append("'");

            return str.ToString();
        }

        ////get set nextEventNumber
        //public int GetNextEventNumber()
        //{
        //    return nextEventNumber;
        //}
        //
        //public void SetNextEventNumber(int i)
        //{
        //    nextEventNumber = i;
        //}
        //
        ////get set optionNumber
        //public int GetOptionNumber()
        //{
        //    return optionNumber;
        //}
        //
        //public void SetOptionNumber(int n)
        //{
        //    optionNumber = n;
        //}
        //
        ////get set optionPicked
        //public bool GetOptionPicked()
        //{
        //    return optionPicked;
        //}
        //
        //public void SetOptionPicked(bool b)
        //{
        //    optionPicked = b;
        //}
        //
        ////get set text
        //public String GetOptionText()
        //{
        //    return text;
        //}
        //
        //public void SetOptionText(String t)
        //{
        //    text = t;
        //}
    }
    
}

/*
public class Test
{
    public static void Main()
    {
        Option o1 = new Option(1, 2, false, "Option 1");
        Option o2 = new Option(2, 3, false, "Option 2");
        Option o3 = new Option(3, 4, false, "Option 3");
        List<Option> lo = new List<Option> { o1, o2, o3 };
        Event e = new Event(1, "This is event numero uno", lo);
        Console.WriteLine(e.GetEventText());
        e.GetOptions()[0].SetOptionText("Option one");
        for (int x = 0; x < e.GetOptions().Count; x++)
        {
            Console.WriteLine("Option Text: " + e.GetOptions()[x].GetOptionText());
            Console.WriteLine("Option Number: " + e.GetOptions()[x].GetOptionNumber());
            Console.WriteLine("Next Event Number: " + e.GetOptions()[x].GetNextEventNumber());
            Console.WriteLine("Option Picked?: " + e.GetOptions()[x].GetOptionPicked());
            Console.WriteLine();
        }
    }
}
*/