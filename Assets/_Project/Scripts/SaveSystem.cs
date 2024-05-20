using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LoggerSystem;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer (Game ga, OfflineManager om, AdvancedQualitySettings ad, ResearchFactory rf)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/cookie2";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new(ga, om, ad, rf);

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
            LogSystem.Log(new FileLoadException().ToString(), LogTypes.Exception);
            return null;
        }
    }
}