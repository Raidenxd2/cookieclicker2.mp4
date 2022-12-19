using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class Update : MonoBehaviour
{

    string newVersion;
    public string currentVersion;
    public GameObject UpdateScreen;
    public GameObject UpdateDownloadingScreen;
    public GameObject UpdateDownloadedScreen;
    public GameObject UpdateErrorScreen;
    public TMP_Text UpdateErrorInfo;
    public TMP_Text progressText;
    public Slider slider;
    public Game game;
    private Process process;
    private StreamWriter messageStream;

    void Start()
    {
        CheckForUpdatesGet();
    }

    public void GotoDownload()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Application.OpenURL("https://raidenxd2.itch.io/cookieclicker2mp4#download");
        }
        else
        {
            Application.OpenURL("https://raidenxd2.itch.io/cookieclicker2mp4#download");
            //DownloadUpdate();
        }
    }

    public void DownloadUpdate()
    {
        UpdateDownloadingScreen.SetActive(true);
        game.SavePlayer();
        StartCoroutine(GetText());
    }

    public void CheckForUpdatesGet()
    {
        StartCoroutine(GetRequest("https://itch.io/api/1/x/wharf/latest?target=raidenxd2/cookieclicker2mp4&channel_name=win-32"));
    }

    public void InstallUpdate()
    {
        // process = new Process();
        // process.EnableRaisingEvents = false;
        // process.StartInfo.FileName = Application.streamingAssetsPath + "/Updater.bat";
        // process.StartInfo.UseShellExecute = false;
        //process.StartInfo.RedirectStandardOutput = true;
        //process.StartInfo.RedirectStandardInput = true;
        //process.StartInfo.RedirectStandardError = true;
        // process.Start();
        //process.BeginOutputReadLine();
        //messageStream = process.StandardInput;
        System.Diagnostics.Process.Start(Application.streamingAssetsPath + "/Updater.bat");
        Application.Quit();
    }

    void CheckForUpdates()
    {
        UnityEngine.Debug.Log("Old Version: " + currentVersion);
        UnityEngine.Debug.Log("New Version: " + newVersion);
        if (currentVersion != newVersion)
        {
            UpdateScreen.SetActive(true);
        }
    }

    IEnumerator GetText()
    {
        string url = "https://www.googleapis.com/drive/v3/files/1MuanUdc7d91tu-h-l4mdAZwsFJPQAYoC?alt=media&key=AIzaSyAWPHSBM-NDHRnGNvOaw09Y_zmzW2Qizxk";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            AsyncOperation operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                yield return null;
                slider.value = progress;
                progressText.text = progress * 100f + "%";
                UnityEngine.Debug.Log(progress);
            }
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                UpdateErrorScreen.SetActive(true);
                UpdateErrorInfo.text = www.error;
                UnityEngine.Debug.LogError(www.error);
            }
            else
            {
                string savePath = Application.streamingAssetsPath + "/Update.7z";        
                System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
                UpdateDownloadedScreen.SetActive(true);
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    UnityEngine.Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    UnityEngine.Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    newVersion = webRequest.downloadHandler.text;
                    CheckForUpdates();
                    break;
            }
        }
    }
}
