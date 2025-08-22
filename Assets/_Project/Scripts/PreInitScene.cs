using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class PreInitScene : MonoBehaviour
{
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

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperationHandle initHandle = LocalizationSettings.InitializationOperation;
        await initHandle;

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        SceneManager.LoadScene(AddressableHandles.initSceneRef);
    }
}