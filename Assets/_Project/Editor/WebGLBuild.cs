using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.PackageManager;

public class WebGLBuild : Editor
{
    [MenuItem("Tools/Make WebGL Build")]
    public static void MakeWebGLBuild()
    {
        if (!EditorUtility.DisplayDialog("", "Are you sure you want to make a WebGL build? This will:\n\nRemove all VR and XR packages\nRemove XR Interaction Toolkit Samples\nPrevent VR data from being included\nMake a WebGL build\n\nThis operation is destructive.", "Yes", "No"))
        {
            return;
        }
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
        {
            EditorUtility.DisplayDialog("", "Please switch to the WebGL build target!", "OK");
            return;
        }

        EditorUtility.DisplayProgressBar("", "Preparing...", 0);

        if (!Directory.Exists("../Builds"))
        {
            Directory.CreateDirectory("../Builds");
        }

        if (!Directory.Exists("../Builds/CC2WebGL"))
        {
            Directory.CreateDirectory("../Builds/CC2WebGL");
        }

        if (Directory.Exists("Samples/XR Interaction Toolkit"))
        {
            Directory.Delete("Samples/XR Interaction Toolkit");
            File.Delete("Samples/XR Interaction Toolkit.meta");
        }

        string[] packages = new[] { "com.unity.xr.core-utils", "com.unity.xr.interaction.toolkit", "com.unity.xr.legacyinputhelpers", "com.unity.xr.management", "com.unity.xr.openxr", "com.unity.xr.meta-openxr", "com.unity.modules.vr", "com.unity.modules.xr", "com.unity.modules.subsystems" };

        Client.AddAndRemove(packagesToRemove: packages);

        AddressableAssetGroup aag1 = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>("Assets/AddressableAssetsData/AssetGroups/VRDataShared");
        aag1.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
        EditorUtility.SetDirty(aag1);
        AddressableAssetGroup aag2 = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>("Assets/AddressableAssetsData/AssetGroups/InitScene-VRData");
        aag2.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
        EditorUtility.SetDirty(aag2);
        AddressableAssetGroup aag3 = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>("Assets/AddressableAssetsData/AssetGroups/GameScene-VRData");
        aag3.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
        EditorUtility.SetDirty(aag3);
        AddressableAssetGroup aag4 = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>("Assets/AddressableAssetsData/AssetGroups/VRFallback");
        aag4.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
        EditorUtility.SetDirty(aag3);

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

        BuildPipeline.BuildPlayer(scenes.ToArray(), "../Builds/CC2WebGL/", BuildTarget.WebGL, BuildOptions.None);

        EditorUtility.DisplayProgressBar("", "Finishing...", 100);

        File.Copy("_Project/Editor/WebGLBuildOutput/cert.pem", "../Builds/CC2WebGL/cert.pem");
        File.Copy("_Project/Editor/WebGLBuildOutput/FinishBuild.bat", "../Builds/CC2WebGL/FinishBuild.bat");
        File.Copy("_Project/Editor/WebGLBuildOutput/key.pem", "../Builds/CC2WebGL/key.pem");
        File.Copy("_Project/Editor/WebGLBuildOutput/StartServer.bat", "../Builds/CC2WebGL/StartServer.bat");

        EditorUtility.ClearProgressBar();
    }
}