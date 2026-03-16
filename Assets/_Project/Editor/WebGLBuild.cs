using Cysharp.Threading.Tasks;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class WebGLBuild : Editor
{
    [MenuItem("Tools/Prepare WebGL Build")]
    public static void MakeWebGLBuild()
    {
        if (!EditorUtility.DisplayDialog("", "Are you sure you want to prepare a WebGL build? This will:\n\nRemove all VR and XR packages\nRemove XR Interaction Toolkit Samples\nRemove Input System package\nRemove _Project/Resources\nRemove Normal Map textures\nRemove Plaster001 textures\nRemove Reflection Probe textures\n\nThis operation is destructive.", "Yes", "No"))
        {
            return;
        }
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            EditorUtility.DisplayDialog("", "Please switch to the WebGL build target!", "OK");
            return;
        }

        EditorUtility.DisplayProgressBar("", "Preparing...", 0);

        if (Directory.Exists(Application.dataPath + "/Samples/XR Interaction Toolkit"))
        {
            Directory.Delete(Application.dataPath + "/Samples/XR Interaction Toolkit", true);
            File.Delete(Application.dataPath + "/Samples/XR Interaction Toolkit.meta");
        }

        if (Directory.Exists(Application.dataPath + "/../Packages/com.unity.xr.openxr"))
        {
            Directory.Delete(Application.dataPath + "/../Packages/com.unity.xr.openxr", true);
        }

        if (Directory.Exists(Application.dataPath + "/_Project/Resources"))
        {
            AssetDatabase.DeleteAsset("Assets/_Project/Resources");
        }

        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Textures/Plaster001/Plaster001_2K-PNG_Color.png");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Textures/Plaster001/Plaster001_NDR.png");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Themes/content/SpaceTheme/Textures/_Normal.png");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Themes/content/ForestTheme/Textures/ForestThemeAtlasNormal.png");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Themes/content/ForestTheme/ForestTheme_Scene/ReflectionProbe-0.exr");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Themes/content/SpaceTheme/SpaceTheme_Scene/ReflectionProbe-0.exr");
        CheckIfFileExistsAndDelete(Application.dataPath + "/_Project/Scenes/BossCookies-Environment/ReflectionProbe-0.exr");

        if (!File.Exists(Application.dataPath + "/WebGL_PackagesRemoved"))
        {
            string[] packages = new[] { "com.unity.xr.interaction.toolkit", "com.unity.xr.management", "com.unity.xr.openxr", "com.unity.inputsystem", "com.unity.modules.screencapture" };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RemovePackagesAsync(packages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            File.Create(Application.dataPath + "/WebGL_PackagesRemoved");

            EditorUtility.ClearProgressBar();
            return;
        }
    }

    private static void CheckIfFileExistsAndDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            if (File.Exists(path + ".meta"))
            {
                File.Delete(path + ".meta");
            }
        }
    }

    private static async UniTaskVoid RemovePackagesAsync(string[] packages)
    {
        var request = Client.AddAndRemove(null, packages);
        while (request.IsCompleted)
        {
            await UniTask.Yield();
        }
    }
}