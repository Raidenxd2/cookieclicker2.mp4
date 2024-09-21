using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildManager : Editor
{
    [MenuItem("Cookieclicker2.mp4/Enable beta build")]
    public static void EnableBetaBuild()
    {
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, "PROBUILDER_EXPERIMENTAL_FEATURES;PRIME_TWEEN_DOTWEEN_ADAPTER;ENABLE_LOG;SAVE_LOG");

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }

    [MenuItem("Cookieclicker2.mp4/Disable beta build")]
    public static void DisableBetaBuild()
    {
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Android, "PROBUILDER_EXPERIMENTAL_FEATURES;PRIME_TWEEN_DOTWEEN_ADAPTER");

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    }
}