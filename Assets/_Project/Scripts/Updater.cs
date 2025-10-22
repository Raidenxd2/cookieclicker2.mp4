using System.IO;
using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.Networking;

public class Updater : MonoBehaviour
{
    [SerializeField] private GameObject UpdateAvailabeScreen;
    [SerializeField] private GameObject GlobalDark;

#if !UNITY_WEBGL
    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        if (BetterPrefs.GetBool("DisableUpdateChecker", false))
        {
            return;
        }

#if !UNITY_EDITOR
        StartAsync().Forget();
#endif
    }

    private async UniTaskVoid StartAsync()
    {
        int version = int.Parse((await UnityWebRequest.Get("https://raidenxd2.github.io/cookieclicker2.mp4/build").SendWebRequest()).downloadHandler.text);

        int localVersion = int.Parse((await UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, "build")).SendWebRequest()).downloadHandler.text);

        LogSystem.Log("Latest version: " + version);
        LogSystem.Log("Local version: " + localVersion);

        if (version > localVersion)
        {
            GlobalDark.SetActive(true);
            UpdateAvailabeScreen.SetActive(true);
        }
    }
#endif

    public void OpenItchIoPage()
    {
#if !UNITY_WEBGL
        OpenURLManager.instance.OpenURL("https://raidenxd2.itch.io/cookieclicker2mp4#download");
#endif
    }
}