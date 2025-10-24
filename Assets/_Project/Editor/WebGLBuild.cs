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
        if (!EditorUtility.DisplayDialog("", "Are you sure you want to prepare a WebGL build? This will:\n\nRemove all VR and XR packages\nRemove XR Interaction Toolkit Samples\nRemove Input System package\n\nThis operation is destructive.", "Yes", "No"))
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

        if (Directory.Exists(Application.dataPath + "/../Packages/com.unity.inputsystem"))
        {
            Directory.Delete(Application.dataPath + "/../Packages/com.unity.inputsystem", true);
        }

        if (Directory.Exists(Application.dataPath + "/../Packages/com.unity.xr.openxr"))
        {
            Directory.Delete(Application.dataPath + "/../Packages/com.unity.xr.openxr", true);
        }

        if (Directory.Exists(Application.dataPath + "/../Packages/com.unity.xr.interaction.toolkit"))
        {
            Directory.Delete(Application.dataPath + "/../Packages/com.unity.xr.interaction.toolkit", true);
        }

        if (!File.Exists(Application.dataPath + "/WebGL_PackagesRemoved"))
        {
            string[] packages = new[] { "dev.voltstro.unitycommandlineparser", "com.unity.xr.interaction.toolkit", "com.unity.xr.management", "com.unity.xr.openxr", "com.unity.inputsystem", "com.unity.modules.vr", "com.unity.modules.screencapture" };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RemovePackagesAsync(packages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            File.Create(Application.dataPath + "/WebGL_PackagesRemoved");

            EditorUtility.ClearProgressBar();
            return;
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