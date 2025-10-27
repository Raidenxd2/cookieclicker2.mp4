using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class PreInitScene : MonoBehaviour
{
    public static bool SetJapaneseLanguage;

#if UNITY_STANDALONE_WIN
    [DllImport("BeanShootoutNative_DarkMode", EntryPoint = "DllMain")]
    private static extern void _();

    private void Awake()
    {
        if (!Application.isEditor)
        {
            try
            {
                _();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
#endif

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

        if (PlayerPrefs.GetString("selected-locale", "en") == "ja")
        {
            SetJapaneseLanguage = true;
            PlayerPrefs.SetString("selected-locale", "en");
            PlayerPrefs.Save();
        }

        await LocalizationSettings.InitializationOperation;

        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        SceneManager.LoadScene(AddressableHandles.initSceneRef);
    }
}