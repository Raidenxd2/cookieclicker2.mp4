using System;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
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
#if UNITY_STANDALONE_WIN
        try
        {
            int value = 0x01;
            WindowsAPI.DwmSetWindowAttribute(WindowsAPI.GetWindowHandle(), DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, Marshal.SizeOf(typeof(int)));
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
#endif
        
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