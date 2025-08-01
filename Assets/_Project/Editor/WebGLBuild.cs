using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.PackageManager;
using UnityEngine;

public class WebGLBuild : Editor
{
    [MenuItem("Tools/Make WebGL Build")]
    public static void MakeWebGLBuild()
    {
        if (!EditorUtility.DisplayDialog("", "Are you sure you want to make a WebGL build? This will:\n\nRemove all VR and XR packages\nRemove XR Interaction Toolkit Samples\nPrevent VR data from being included\nRemove Input System package\nMake a WebGL build\n\nThis operation is destructive.", "Yes", "No"))
        {
            return;
        }
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            EditorUtility.DisplayDialog("", "Please switch to the WebGL build target!", "OK");
            return;
        }

        EditorUtility.DisplayProgressBar("", "Preparing...", 0);

        if (!Directory.Exists(Application.dataPath + "/../Builds"))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Builds");
        }

        if (!Directory.Exists(Application.dataPath + "/../Builds/CC2WebGL"))
        {
            Directory.CreateDirectory(Application.dataPath + "/../Builds/CC2WebGL");
        }

        if (Directory.Exists(Application.dataPath + "/Samples/XR Interaction Toolkit"))
        {
            Directory.Delete(Application.dataPath + "/Samples/XR Interaction Toolkit", true);
            File.Delete(Application.dataPath + "/Samples/XR Interaction Toolkit.meta");
        }

        if (Directory.Exists(Application.dataPath + "/../Packages/com.unity.xr.meta-openxr"))
        {
            Directory.Delete(Application.dataPath + "/../Packages/com.unity.xr.meta-openxr", true);
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
            string[] packages = new[] { "dev.voltstro.unitycommandlineparser", "com.unity.xr.interaction.toolkit", "com.unity.xr.management", "com.unity.xr.openxr", "com.unity.xr.meta-openxr", "com.unity.inputsystem", "com.unity.modules.vr", "com.unity.modules.screencapture", "com.unity.modules.uielements" };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RemovePackagesAsync(packages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            File.Create(Application.dataPath + "/WebGL_PackagesRemoved");

            EditorUtility.DisplayDialog("", "Please run 'Tools/Make WebGL Build' again.", "OK");

            EditorUtility.ClearProgressBar();
            return;
        }

        WebGLConfigSO wglc = AssetDatabase.LoadAssetAtPath<WebGLConfigSO>("Assets/WebGLConfig.asset");
        foreach (var group in wglc.assetGroupsToDisable)
        {
            group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
            EditorUtility.SetDirty(group);
        }

        AssetDatabase.Refresh();

        AddressableAssetSettings.BuildPlayerContent();

        // Get all scenes in Build Settings/Build Profiles
        List<string> scenes = new();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }

        BuildPipeline.BuildPlayer(scenes.ToArray(), Application.dataPath + "/../Builds/CC2WebGL/", BuildTarget.WebGL, BuildOptions.ShowBuiltPlayer);

        EditorUtility.DisplayProgressBar("", "Finishing...", 100);

        File.Copy(Application.dataPath + "/_Project/Editor/WebGLBuildOutput/cert.pem", Application.dataPath + "/../Builds/CC2WebGL/cert.pem", true);
        File.Copy(Application.dataPath + "/_Project/Editor/WebGLBuildOutput/FinishBuild.bat", Application.dataPath + "/../Builds/CC2WebGL/FinishBuild.bat", true);
        File.Copy(Application.dataPath + "/_Project/Editor/WebGLBuildOutput/key.pem", Application.dataPath + "/../Builds/CC2WebGL/key.pem", true);
        File.Copy(Application.dataPath + "/_Project/Editor/WebGLBuildOutput/StartServer.bat", Application.dataPath + "/../Builds/CC2WebGL/StartServer.bat", true);

        EditorUtility.ClearProgressBar();
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