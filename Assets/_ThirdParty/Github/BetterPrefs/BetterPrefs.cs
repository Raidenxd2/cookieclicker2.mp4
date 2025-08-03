/*
BetterPrefs is a replacement for Unity's PlayerPrefs that aims to add features that PlayerPrefs is lacking, such as support for multiple saves, save import/export and even more data types, such as booleans, Vector2s and Vector3s.

Version: 3.0.0

https://github.com/Carroted/BetterPrefs

Author: amytimed
License: MIT
*/

using System.Collections.Generic;
using System.IO;

public static class BetterPrefs
{
    static Dictionary<string, object> data; // The data that will be saved. This should only ever be accessed through the Get and Set functions.
    public static string currentSave = null; // The full path and name of the save file that is currently loaded, null if no save is loaded.

    static void Write2DArray(string[,] array, BinaryWriter writer) // Used by Save
    {
        writer.Write(array.GetLength(0));
        writer.Write(array.GetLength(1));
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                writer.Write(array[i, j]);
            }
        }

        // Close the writer
        writer.Close();
    }

    static string[,] Read2DArray(BinaryReader reader) // Used by Load
    {
        int x = reader.ReadInt32();
        int y = reader.ReadInt32();
        string[,] array = new string[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                array[i, j] = reader.ReadString();
            }
        }
        // Close the reader
        reader.Close();
        return array;
    }

    public static void SetBool(string key, bool value)
    {
        if (data == null)
        {
            return;
        }
        data[key] = value;
    }

    public static void SetInt(string key, int value)
    {
        if (data == null)
        {
            return;
        }

        data[key] = value;
    }

    public static void SetFloat(string key, float value)
    {
        if (data == null)
        {
            return;
        }

        data[key] = value;
    }

    public static void SetString(string key, string value)
    {
        if (data == null)
        {
            return;
        }
        data[key] = value;
    }

    public static bool GetBool(string key, bool fallback)
    {
        if (data == null)
        {
            return false;
        }

        if (data.ContainsKey(key))
        {
            return (bool)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static int GetInt(string key, int fallback)
    {
        if (data == null)
        {
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (int)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static float GetFloat(string key, float fallback)
    {
        if (data == null)
        {
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (float)data[key];
        }
        else
        {
            return fallback;
        }
    }

    public static string GetString(string key, string fallback)
    {
        if (data == null)
        {
            return "";
        }

        if (data.ContainsKey(key))
        {
            return ((string)data[key]);
        }
        else
        {
            return fallback;
        }
    }

    // Get methods without fallback

    public static bool GetBool(string key)
    {
        if (data == null)
        {
            return false;
        }

        if (data.ContainsKey(key))
        {
            return (bool)data[key];
        }
        else
        {
            return false;
        }
    }

    public static int GetInt(string key)
    {
        if (data == null)
        {
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (int)data[key];
        }
        else
        {
            return -1;
        }
    }

    public static float GetFloat(string key)
    {
        if (data == null)
        {
            return -1;
        }

        if (data.ContainsKey(key))
        {
            return (float)data[key];
        }
        else
        {
            return -1;
        }
    }

    public static string GetString(string key)
    {
        if (data == null)
        {
            return "";
        }

        if (data.ContainsKey(key))
        {
            return ((string)data[key]);
        }
        else
        {
            return "";
        }
    }

    public static string Save(string savePath = "default")
    {
        if (data == null)
        {
            return null;
        }

        if (savePath == "default")
        {
            savePath = currentSave; // If no save path is specified, we save to where our loaded save is located. This is useful for saving over the same save file.
        }

        // Data is stored in a two-dimensional array of strings. They contain the type, key and value of each entry.
        /* Example:
        {
            { "int", "exampleInt", "2" },
            { "string", "exampleString", "Hello World!"},
            { "vector2", "exampleVector2", "1.0,2.0" },
            { "bool", "exampleBool", "true" }
        }
        */

        string[,] dataArray = new string[data.Count, 3];

        if (data.Count == 0)
        {
            // Delete save file if it exists
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            return null;
        }

        int i = 0; // Using a foreach loop with an index is actually simpler than using a for loop for this very specific case.

        foreach (KeyValuePair<string, object> pair in data)
        {
            // Add the stuff to the array

            string objectType = "unknown";
            string valueFormatted = "unparsable";

            if (pair.Value is bool)
            {
                objectType = "bool";
                valueFormatted = pair.Value.ToString().ToLower();
            }
            else if (pair.Value is int)
            {
                objectType = "int";
                valueFormatted = pair.Value.ToString();
            }
            else if (pair.Value is float)
            {
                objectType = "float";
                valueFormatted = pair.Value.ToString();
            }
            else if (pair.Value is string)
            {
                objectType = "string";
                valueFormatted = pair.Value.ToString();
            }
            else
            {
                continue;
            }

            dataArray[i, 0] = objectType;
            dataArray[i, 1] = pair.Key;
            dataArray[i, 2] = valueFormatted;

            i++;
        }

        // Save the data to the file

        // If any of the directories in savePath don't exist, create them

        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        // Use Write2DArray we defined earlier to write the data to the file

        FileStream file = File.Create(savePath);

        // Write2DArray(string[,], BinaryWriter)

        Write2DArray(dataArray, new BinaryWriter(file));

        file.Close();

        return savePath; // Return the path to the file, could be useful

    }

    public static void Load(string savePath = "default")
    {
        data = new Dictionary<string, object>();

        if (File.Exists(savePath)) // If the file doesn't exist, we know that file path is where the current save should be, so we store that and when Save is called, saves will be put there by default.
        {
            // Use Read2DArray we defined earlier to read the data from the file

            FileStream file = File.Open(savePath, FileMode.Open);

            // Read2DArray(BinaryReader)

            string[,] dataArray = Read2DArray(new BinaryReader(file));

            file.Close();

            // Add the data from the array to the dictionary

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                string objectType = dataArray[i, 0];
                string key = dataArray[i, 1];
                string valueFormatted = dataArray[i, 2];

                if (objectType == "bool")
                {
                    // This is a bool

                    bool value;

                    if (!bool.TryParse(valueFormatted, out value))
                    {
                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        data[key] = value;
                    }
                }
                else if (objectType == "string")
                {
                    // This is a string

                    string value = valueFormatted; // :D :) :D :D

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        data[key] = value;
                    }
                }
                else if (objectType == "int")
                {
                    // This is an int

                    int value;

                    if (!int.TryParse(valueFormatted, out value))
                    {
                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        data[key] = value;
                    }
                }
                else if (objectType == "float")
                {
                    // This is a float

                    float value;

                    if (!float.TryParse(valueFormatted, out value))
                    {
                        continue;
                    }

                    // Add the value to the dictionary

                    if (!data.ContainsKey(key))
                    {
                        data.Add(key, value);
                    }
                    else
                    {
                        data[key] = value;
                    }
                }
                else
                {
                    // This is not a valid type
                    continue;
                }

            }
        }

        currentSave = savePath;
    }

    public static void DeleteAll()
    {
        // Delete all the data

        if (data == null)
        {
            data = new Dictionary<string, object>();
        }

        data.Clear();
    }

    public static Dictionary<string, object> GetData() // Get the data
    {
        if (data == null)
        {
            return new Dictionary<string, object>();
        }

        return data;
    }
}
