using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using LoggerSystem;

public class CacheFile : MonoBehaviour
{
    public static CacheFile Instance { get; private set; } 
    public string URL = "raidenxd2.github.io/cookieclicker2.mp4/CookieStore.txt";
    public static bool DownloadedFile { get; private set; }

    void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    void Start()
    {
        LogSystem.Log(" ");

        if (Directory.Exists(Application.persistentDataPath + "/Cache"))
        {
            LogSystem.Log("Old Cache folder found, deleting.");
            Directory.Delete(Application.persistentDataPath + "/Cache", true);
        }

        if (Directory.Exists(Application.temporaryCachePath + "/Cache") == false)
        {
            LogSystem.Log("Creating Cache directory...");
            Directory.CreateDirectory(Application.temporaryCachePath + "/Cache");
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            LogSystem.Log("No internet connection!", LogTypes.Error);
            if (!File.Exists(Application.temporaryCachePath + "/Cache/CookieStore.txt"))
            {
                LogSystem.Log("No cache file!", LogTypes.Error);
                return;
            }
            DownloadedFile = true;
            return;
        }
        LogSystem.Log("Creating new Cache file...");
        File.Delete(Application.temporaryCachePath + "/Cache/CookieStore.txt");
        StartCoroutine(GetText(URL));
    }

    IEnumerator GetText(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            AsyncOperation operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            var data = www.downloadHandler.text;
            LogSystem.Log("Data from server: " + data);
            
            File.WriteAllText(Application.temporaryCachePath + "/Cache/CookieStore.txt", www.downloadHandler.text);
            DownloadedFile = true;
        }
    }
}