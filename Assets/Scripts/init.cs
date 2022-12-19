using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class init : MonoBehaviour
{
    public GameObject DDOL;
    public SceneLoader sceneLoader;
    public GameObject DataDownloadRequiredScreen;
    public Slider slider;
    public TMP_Text progressText;
    public GameObject DataDownloadedScreen;

    // download
    

    void Start()
    {
        Debug.Log("Time took to startup: " + Time.realtimeSinceStartup);
        Resources.UnloadUnusedAssets();
        DontDestroyOnLoad(DDOL);
        // if (!Directory.Exists(Application.persistentDataPath + "/GameData"))
        // {
        //     Directory.CreateDirectory(Application.persistentDataPath + "/GameData");
        // }
        File.WriteAllText(Application.persistentDataPath + "/launchTime.txt", "Time took to startup: " + Time.realtimeSinceStartup);
    }

    // public void DownloadData(string fileName)
    // {
    //     StartCoroutine(DownloadAsynchronously(fileName));
    // }

    // IEnumerator DownloadAsynchronously(string fileName)
    // {
    //     string key = "assetKey";
    //     Addressables.ClearDependencyCacheAsync(key);
    //     AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(key);
    //     yield return getDownloadSize;
    //     if (getDownloadSize.Result > 0)
    //     {
    //         AsyncOperationHandle downloadDependencies = Addressables.DownloadDependenciesAsync(key);
    //         yield return downloadDependencies;
    //     }

    //     string url = "https://www.googleapis.com/drive/v3/files/1MuanUdc7d91tu-h-l4mdAZwsFJPQAYoC?alt=media&key=AIzaSyAWPHSBM-NDHRnGNvOaw09Y_zmzW2Qizxk";
    //     using (UnityWebRequest www = UnityWebRequest.Get(url))
    //     {
    //         AsyncOperation operation = www.SendWebRequest();
    //         while (!operation.isDone)
    //         {
    //             float progress = Mathf.Clamp01(operation.progress / .9f);
    //             yield return null;
    //             slider.value = progress;
    //             progressText.text = progress * 100f + "%";
    //             UnityEngine.Debug.Log(progress);
    //         }
    //         if (www.result == UnityWebRequest.Result.ConnectionError)
    //         {
    //             // UpdateErrorScreen.SetActive(true);
    //             // UpdateErrorInfo.text = www.error;
    //             UnityEngine.Debug.LogError(www.error);
    //         }
    //         else
    //         {
    //             string savePath = Application.persistentDataPath + "/GameData/" + fileName;        
    //             System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
    //             DataDownloadedScreen.SetActive(true);
    //         }
    //     }
    // }
}
