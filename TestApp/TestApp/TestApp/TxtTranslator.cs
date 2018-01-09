using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;

namespace TestApp
{

    public class TxtTranslator
    {
        public static Event LoadEvent(Stream stream)
        {
            Event e = new Event();

            //IFolder rootFolder = FileSystem.Current.LocalStorage;
            //IFolder appDataFolder = rootFolder.GetFolderAsync("appData").Result;

//            if (appDataFolder.CheckExistsAsync(fileName).Result == ExistenceCheckResult.FileExists)
  //          {
    //            IFile saveFile = appDataFolder.GetFileAsync(fileName).Result;
      //          Debug.WriteLine(saveFile.Path);
        //        Stream stream = Task.Run(() => saveFile.OpenAsync(PCLStorage.FileAccess.Read).Result).Result;
                using (StreamReader reader = new System.IO.StreamReader(stream))
                {
                    int optionNumber = 0;
                    int status = 0;

                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();

                        if (currentLine == "")
                            continue;

                        if (status == 0)
                        {
                            e.eventNumber = Convert.ToInt32(currentLine);
                            status++;
                            continue;
                        }
                        if (status == 1)
                        {
                            if (!currentLine.Contains("Text:"))
                                continue;
                            else
                            {
                                status++;
                                continue;
                            }
                        }
                        if (status == 2)
                        {
                            if (currentLine.Contains("Options:"))
                            {
                                status++;
                                continue;
                            }
                            else
                            {
                                e.text.Add(currentLine);
                            }
                        }
                        if (status == 3)
                        {
                            if (currentLine.Contains("[["))
                            {
                                Option option = new Option();
                                currentLine = currentLine.Replace('[', ' ').Replace(']', ' ').Trim();
                                option.optionNumber = ++optionNumber;
                                option.text = currentLine;
                                currentLine = reader.ReadLine().Replace('[', ' ').Trim();
                                while (!currentLine.Contains("]"))
                                {
                                    if (currentLine != "")
                                        option.optionSummary += currentLine + " ";
                                    currentLine = reader.ReadLine();
                                }
                                currentLine = currentLine.Replace(']', ' ').Trim();
                                if (currentLine != "")
                                    option.optionSummary += currentLine;
                                currentLine = reader.ReadLine().Replace('[', ' ').Trim();
                                while (!currentLine.Contains("]"))
                                {
                                    if (currentLine != "")
                                        option.resultText.Add(currentLine);
                                    currentLine = reader.ReadLine();
                                }
                                currentLine = currentLine.Replace(']', ' ').Trim();
                                if (currentLine != "")
                                    option.resultText.Add(currentLine);
                                while (!currentLine.Contains("|") && !currentLine.Contains("/"))
                                    currentLine = reader.ReadLine();
                                if (currentLine.Contains("|"))
                                {
                                    foreach (string s in currentLine.Split('|'))
                                    {
                                        string[] param = s.Split(':');
                                        switch (param[0])
                                        {
                                            case "H":
                                                option.HullRequired = Convert.ToInt32(param[1]);
                                                break;
                                            case "L":
                                                option.LifeRequired = Convert.ToInt32(param[1]);
                                                break;
                                            case "F":
                                                option.FuelRequired = Convert.ToInt32(param[1]);
                                                break;
                                            case "E":
                                                option.EmpRequired = Convert.ToInt32(param[1]);
                                                break;
                                            case "W":
                                                option.WeapRequired = Convert.ToInt32(param[1]);
                                                break;
                                        }
                                    }
                                }
                                while (!currentLine.Contains("/"))
                                    currentLine = reader.ReadLine();
                                if (currentLine.Contains("/"))
                                {
                                    currentLine = currentLine.Replace('{', ' ').Replace('}', ' ').Trim();
                                    foreach (string s in currentLine.Split('/'))
                                    {
                                        string[] param = s.Split(':');
                                        switch (param[0])
                                        {
                                            case "H":
                                                option.HullChange = Convert.ToInt32(param[1]);
                                                break;
                                            case "L":
                                                option.LifeChange = Convert.ToInt32(param[1]);
                                                break;
                                            case "F":
                                                option.FuelChange = Convert.ToInt32(param[1]);
                                                break;
                                            case "E":
                                                option.EmpChange = Convert.ToInt32(param[1]);
                                                break;
                                            case "W":
                                                option.WeapChange = Convert.ToInt32(param[1]);
                                                break;
                                            case "G":
                                                option.nextEventNumber = Convert.ToInt32(param[1]);
                                                break;
                                        }
                                    }
                                }
                                e.options.Add(option);
                            }
                        }

                    }

                }
         //   }
           // else
            //{
              //  Debug.WriteLine("Error Loading Objects: File does not exist.");
            //}

            return e;
        }


        public static List<Event> LoadAllEvents()
        {
            ///////
            List<Event> e = new List<Event>();

            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder appDataFolder = rootFolder.GetFolderAsync("appData").Result;

            IList<IFile> files = appDataFolder.GetFilesAsync().Result;


            foreach (IFile saveFile in files)
            {
                if (!saveFile.Name.Contains("Text"))
                    continue;
                Debug.WriteLine("Loading " + saveFile.Path);
                using (Stream stream = Task.Run(() => saveFile.OpenAsync(PCLStorage.FileAccess.Read).Result).Result)
                {
                    e.Add(LoadEvent(stream));
                }

            }

            Debug.WriteLine("Done Loading");

            return e;
        }
    }
}
