using System;
using Cysharp.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using SerialPackage.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PreInitScene : MonoBehaviour
{
    [SerializeField] private AssetReference TMPSettingsRef;
    [SerializeField] private AssetReference InitSceneRef;

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
        
        TMP_Settings.instance = await Addressables.LoadAssetAsync<TMP_Settings>(TMPSettingsRef);

        AddressableHandles.InitHandle = Addressables.LoadSceneAsync(InitSceneRef);
        await AddressableHandles.InitHandle;
    }
}