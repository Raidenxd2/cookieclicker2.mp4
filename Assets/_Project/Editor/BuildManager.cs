using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BuildManager : Editor
{
    [MenuItem("Cookieclicker2.mp4/Enable beta build")]
    public static void EnableBetaBuild()
    {
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, "PROBUILDER_EXPERIMENTAL_FEATURES;PRIME_TWEEN_DOTWEEN_ADAPTER;ENABLE_LOG;SAVE_LOG");

        GameObject.Find("BuildConfig").GetComponent<BuildConfig>().BetaBuild = true;
        GameObject.Find("BuildConfig").GetComponent<BuildConfig>().Beta_SaveLogsToFile = true;

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }

    [MenuItem("Cookieclicker2.mp4/Disable beta build")]
    public static void DisableBetaBuild()
    {
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, "PROBUILDER_EXPERIMENTAL_FEATURES;PRIME_TWEEN_DOTWEEN_ADAPTER");

        GameObject.Find("BuildConfig").GetComponent<BuildConfig>().BetaBuild = false;
        GameObject.Find("BuildConfig").GetComponent<BuildConfig>().Beta_SaveLogsToFile = false;

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }
}