using Cysharp.Threading.Tasks;
using System.IO;
using SerialPackage.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreInitScene : MonoBehaviour
{
    public static bool SetJapaneseLanguage;

    [SerializeField] private GameObject FatalErrorScreen;
    [SerializeField] private TMP_Text FatalErrorText;

    private void Start()
    {
        StartAsync().Forget();
    }

    private async UniTaskVoid StartAsync()
    {
        PlayerPrefs.SetInt("unity.player_session_count", 0);
        PlayerPrefs.SetInt("unity.player_sessionid", 0);
        PlayerPrefs.SetInt("unity.cloud_userid", 0);
        PlayerPrefs.Save();

        BeanLogger.VerboseLogging = true;

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        await SceneManager.LoadSceneAsync(SceneNames.initSceneRef);
    }
}