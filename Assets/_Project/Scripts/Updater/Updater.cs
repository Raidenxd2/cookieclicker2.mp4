using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.Networking;

public class Updater : MonoBehaviour
{
    [SerializeField] private GameObject UpdateAvailabeScreen;
    [SerializeField] private GameObject GlobalDark;

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }

        StartAsync().Forget();
    }

    private async UniTask StartAsync()
    {
        string json = (await UnityWebRequest.Get("https://itch.io/api/1/x/wharf/latest?target=raidenxd2/cookieclicker2mp4&channel_name=updatertest-win-64").SendWebRequest()).downloadHandler.text;

        string version = JsonUtility.FromJson<LatestJSON>(json).latest;

        LogSystem.Log("itch.io version: " + version);
        LogSystem.Log("Game version: " + Application.version);

        if (version != Application.version)
        {
            GlobalDark.SetActive(true);
            UpdateAvailabeScreen.SetActive(true);
        }
    }

    public void OpenItchIoPage()
    {
        OpenURLManager.instance.OpenURL("https://raidenxd2.itch.io/cookieclicker2mp4#download");
    }
}