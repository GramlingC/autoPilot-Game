using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace TestApp
{
    // The SaveManager class can write objects to files and read
    // them back by the use of object serialization.
    class SaveManager
    {
        bool SaveObjects(string fileName, params object[] objList)
        {
            // We will assume everything goes well.  If not, we will return false through this.
            bool wasSuccessful = true;

            // If the file exists, we delete it so we can rewrite our new file.
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // We need a binary formatter to convert our objects into binary data.
            BinaryFormatter formatter = new BinaryFormatter();

            // The using block lets us open a FileStream safely, because when 
            // it closes, it will automatically close the stream for us. As we
            // open the filestream, we create our save file.
            using (FileStream stream = File.Create(fileName))
            {
                // Convert each object into a stream of binary data, and write it to the file.
                foreach (object o in objList)
                {
                    try
                    {
                        formatter.Serialize(stream, o);
                    }
                    catch (SecurityException e)
                    {
                        Console.Write("Error serializing objects: " + e.Message);
                        wasSuccessful = false;
                    }
                }
            }

            return wasSuccessful;
        }
    }
}
