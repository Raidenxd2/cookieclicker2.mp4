// using UnityEngine;

// public class PlatformCheck
// {
//     #if UNITY_ANDROID && !UNITY_EDITOR
//     private static AndroidJavaObject PackageManager
//     {
//         get
//         {
//             var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//             var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
//             return currentActivity.Call<AndroidJavaObject>("getPackageManager");
//         }
//     }

//     public static bool IsChromeOS => PackageManager.Call<bool>("hasSystemFeature", "org.chromium.arc");

//     public static bool IsGooglePlayGames =>
//         PackageManager.Call<bool>("hasSystemFeature", "com.google.android.play.feature.HPE_EXPERIENCE");

//     public static bool HasKeyboard
//     {
//         get
//         {
//             var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//             var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
//             var resources = currentActivity.Call<AndroidJavaObject>("getResources");
//             var configuration = resources.Call<AndroidJavaObject>("getConfiguration");
//             var keyboard = configuration.Get<int>("keyboard");
//             return keyboard == 2; // Configuration.KEYBOARD_QWERTY
//         }
//     }
// #else
//     public static bool IsChromeOS => false;
//     public static bool IsGooglePlayGames => false;
//     public static bool HasKeyboard => true;
// #endif
// }