using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    


    public static void SavePlayer (Game ga, OfflineManager om, AdvancedQualitySettings ad)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/cookie2";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(ga, om, ad);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(Game ga)
    {
        string path = Application.persistentDataPath + "/cookie2";
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();
                return data;
            }
            // catch(IOException ex)
            // {
            //     ga.SaveDataErrorIO(ex);
            //     ga.SDIE.SetActive(true);
            //     ga.ErrorText.text = "" + ex;
            //     return null;
            // }
            catch(Exception ex)
            {
                ga.SDIE.SetActive(true);
                ga.SmallErrorText.text = "" + ex.Message;
                ga.ErrorText.text = "" + ex;
                return null;
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


}
