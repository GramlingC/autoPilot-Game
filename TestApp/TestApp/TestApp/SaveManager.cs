using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
//using System.Runtime.Serialization;

// Binary serialization seems to not be supported by Xamarin, so 
// we can use XML serialization for now until we find an alternative solution.

namespace TestApp
{
    // The SaveManager class can write objects to files and read
    // them back by the use of object serialization.
    class SaveManager
    {
        // Completed, need testing
        bool SaveObjects(string fileName, params object[] objList)
        {
            bool wasSuccessful = true;

            // If the file exists, we delete it so we can rewrite our new file.
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // The using block lets us open a FileStream safely, because when 
            // it closes, it will automatically close the stream for us. As we
            // open the filestream, we create our save file.
            using (FileStream stream = File.Create(fileName))
            {
                // Convert each object into a stream of binary data, and write it to the file.
                for (int index = 0; index < objList.Length; index++)
                {
                    // Use an XML formatter to format to xml data
                    // Note: XmlSerializer MUST be instantiated with a type in the constructor,
                    // which is why it is created inside the for loop.
                    XmlSerializer serializer = new XmlSerializer(objList[index].GetType());

                    try
                    {
                        serializer.Serialize(stream, objList[index]);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error serializing objects: " + e.Message);
                        wasSuccessful = false;
                    }
                }
            }

            return wasSuccessful;
        }
        bool LoadObjects(string fileName, params object[] objList)
        {
            bool wasSuccessful = true;
            
            if (File.Exists(fileName))
            {
                using (FileStream stream = File.OpenRead(fileName))
                {
                    for (int index = 0; index < objList.Length; index++)
                    {
                        // Use an XML formatter to format xml data
                        XmlSerializer serializer = new XmlSerializer(objList[index].GetType());

                        try
                        {
                            objList[index] = serializer.Deserialize(stream);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Error deserializing objects: " + e.Message);
                            wasSuccessful = false;
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine("Error Loading Objects: File does not exist.");
                wasSuccessful = false;
            }

            return wasSuccessful;
        }

        // Incomplete, and may not be needed.
        /*
        bool SaveVariables(string fileName, params object[] atrList)
        {
            // We will assume everything goes well.  If not, we will return false through this.
            bool wasSuccessful = true;

            using (TextWriter writer = new StreamWriter(fileName))
            {
                foreach (object o in atrList)
                {
                    try
                    {
                        writer.WriteLine(o);
                    }
                    catch (SecurityException e)
                    {
                        Console.Write("Error serializing attributes: " + e.Message);
                        wasSuccessful = false;
                    }
                }
            }

            return wasSuccessful;
        }
        bool LoadVariables(string fileName, params object[] atrList)
        {
            // We will assume everything goes well.  If not, we will return false through this.
            bool wasSuccessful = true;

            using (TextReader reader = new StreamReader(fileName))
            {
                string input;
                foreach (object o in atrList)
                {
                    // Note - Not finished, does not read into objects
                    try
                    {
                        //if (o.GetType())
                        input = reader.ReadLine();
                        //o = Convert.
                    }
                    catch (SecurityException e)
                    {
                        Console.Write("Error serializing attributes: " + e.Message);
                        wasSuccessful = false;
                    }
                }
            }

            return wasSuccessful;
        }
        */
    }
}
