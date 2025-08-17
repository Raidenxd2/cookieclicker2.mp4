using Cysharp.Threading.Tasks;
using System;
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

        // #if UNITY_STANDALONE_WIN didn't fucking work
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            try
            {
                Debug.Log(Application.dataPath);
                if (File.Exists(Application.dataPath + "\\..\\baselib.dll"))
                {
                    File.Delete(Application.dataPath + "\\..\\baselib.dll");
                }
                if (File.Exists(Application.dataPath + "\\..\\Cookieclicker2.mp4_x64_MasterWithLTCG_il2cpp.pdb"))
                {
                    File.Delete(Application.dataPath + "\\..\\Cookieclicker2.mp4_x64_MasterWithLTCG_il2cpp.pdb");
                }
                if (File.Exists(Application.dataPath + "\\..\\GameAssembly.dll"))
                {
                    File.Delete(Application.dataPath + "\\..\\GameAssembly.dll");
                }
                if (Directory.Exists(Application.dataPath + "\\il2cpp_data"))
                {
                    Directory.Delete(Application.dataPath + "\\il2cpp_data", true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

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