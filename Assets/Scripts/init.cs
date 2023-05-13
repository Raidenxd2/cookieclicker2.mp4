using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using GoogleMobileAds.Api;

public class init : MonoBehaviour
{
    public GameObject DDOL;
    public SceneLoader sceneLoader;
    public Slider slider;
    public TMP_Text progressText;
    
    void OnApplicationPause(bool isPaused) 
    {                 

    }

    void Start()
    {
#if DEVELOPMENT_BUILD
        StartCoroutine(GetText("https://raiden.netlify.app/cookieclicker2.mp4/expire/true.txt"));
#endif

    if (Application.platform == RuntimePlatform.WindowsEditor)
    {
        StartCoroutine(GetText("https://raiden.netlify.app/cookieclicker2.mp4/expire/102-dev2.txt"));
    }

#if UNITY_ANDROID
        MobileAds.Initialize(initStatus => { });
#endif
        Resources.UnloadUnusedAssets();
        DontDestroyOnLoad(DDOL);
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
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                UnityEngine.Debug.LogError(www.error);
            }
            else
            {
                var test = www.downloadHandler.text;
                if (test == "true")
                {
                    SceneManager.LoadScene("devexpire");
                }
            }
        }
    }
}
