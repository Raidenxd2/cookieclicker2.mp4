#if SAVE_LOG
using System;
using System.IO;
using UnityEngine;

class BetaBuild_SaveLogsToFile
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Awake()
    {
        if (File.Exists(Application.temporaryCachePath + "/Beta_Log.txt"))
        {
            File.Move(Application.temporaryCachePath + "/Beta_Log.txt", Application.temporaryCachePath + "/Beta_Log_archive" + UnityEngine.Random.Range(0, 999999) + ".txt");
        }

        File.Create(Application.temporaryCachePath + "/Beta_Log.txt");

        Application.logMessageReceived += HandleLog;
    }

    static void HandleLog(string logString, string stackTrace, LogType type)
    {
        File.AppendAllText(Application.temporaryCachePath + "/Beta_Log.txt", type.ToString() + Environment.NewLine + logString + Environment.NewLine + stackTrace);
    }
}
#endif