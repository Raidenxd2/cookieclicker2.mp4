using System;
using LoggerSystem;
using UnityEngine;

public class IsGPGPC : MonoBehaviour
{
    public static IsGPGPC instance;

    public bool isPC;
    
    #if UNITY_EDITOR
    public bool EnableInEditor;
    #endif

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        LogSystem.Log("Checking if game is running on Google Play Games PC...");

        try
        {
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            isPC = packageManager.Call<bool>("hasSystemFeature", "com.google.android.play.feature.HPE_EXPERIENCE");
        }
        catch (Exception ex)
        {
            LogSystem.Log("Failed to check if game was on Google Play Games PC, not GPGPC?\n" + ex.ToString(), LogTypes.Exception);
        }
        

        #if UNITY_EDITOR
        LogSystem.Log("EDITOR: Setting isPC to EnableInEditor\n" + EnableInEditor);
        isPC = EnableInEditor;
        LogSystem.Log(isPC.ToString());
        #endif

        LogSystem.Log("isPC: " + isPC.ToString());
    }
}
