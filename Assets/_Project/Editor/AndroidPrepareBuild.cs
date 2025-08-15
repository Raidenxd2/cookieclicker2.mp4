using Cysharp.Threading.Tasks;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.PackageManager;
using UnityEngine;

public class AndroidPrepareBuild : Editor
{
    [MenuItem("Tools/Prepare Non-VR Android build")]
    public static void PrepareAndroidBuild()
    {
        if (!EditorUtility.DisplayDialog("", "Are you sure want to prepare to make a Non-VR Android build? This will:\n\nRemove all VR and XR packages\nRemove XR Interaction Toolkit Samples\nPrevent VR data from being included\nRemove Input System package\nSet AudioClips to mono\n\nThis operation is destructive.", "Yes", "No"))
        {
            return;
        }
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUtility.DisplayDialog("", "Please switch to the Android build target!", "OK");
            return;
        }

        EditorUtility.DisplayProgressBar("", "Preparing...", 0);

        AndroidConfigSO androidConfig = AssetDatabase.LoadAssetAtPath<AndroidConfigSO>("Assets/AndroidConfig.asset");
        foreach (var clip in androidConfig.AudioClipsSwitchMono)
        {
            string assetPath = AssetDatabase.GetAssetPath(clip);

            AudioImporter importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;

            if (importer != null)
            {
                importer.forceToMono = true;
                AssetDatabase.ImportAsset(assetPath);
                Debug.Log("(AndroidPrepareBuild) Changed forceToMono setting of " + assetPath);
            }
        }
        foreach (var group in androidConfig.assetGroupsToDisable)
        {
            group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = false;
            EditorUtility.SetDirty(group);
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

        AssetDatabase.Refresh();

        if (!File.Exists(Application.dataPath + "/Android_PackagesRemoved"))
        {
            string[] packages = new[] { "dev.voltstro.unitycommandlineparser", "com.unity.xr.interaction.toolkit", "com.unity.xr.management", "com.unity.xr.openxr", "com.unity.xr.meta-openxr", "com.unity.inputsystem", "com.unity.modules.vr", "com.unity.modules.screencapture" };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            RemovePackagesAsync(packages);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            File.Create(Application.dataPath + "/Android_PackagesRemoved");

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