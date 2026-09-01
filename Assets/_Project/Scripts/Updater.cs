using System.IO;
using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Updater : MonoBehaviour
{
    [SerializeField] private GameObject UpdateAvailabeScreen;
    [SerializeField] private GameObject GlobalDark;

    [SerializeField] private Toggle UpdateCheckerToggle;

#if !UNITY_WEBGL
    private void Start()
    {
        UpdateCheckerToggle.isOn = !BetterPrefs.GetBool("DisableUpdateChecker", false);
        
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

        BeanLogger.Log("Latest version: " + version, this);
        BeanLogger.Log("Local version: " + localVersion, this);

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