using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;

public class BatchBuild2 : EditorWindow
{
    private string BuildPath = "E:/Stuff/stuff/Cookieclicker2.mp4 Unity2022/Builds";
    private bool BuildAddressables;
    private bool DevBuild;

    [MenuItem("Bean Shootout/Batch Build")]
    public static void ShowWindow()
    {
        BatchBuild2 wnd = GetWindow<BatchBuild2>();
        wnd.titleContent = new GUIContent("Batch Build");
    }

    public void OnGUI()
    {
        BuildPath = EditorGUILayout.TextField("Build Path", BuildPath);
        GUILayout.Label("The builds will be saved to this folder\n" + BuildPath + "/[VERSION]");
        GUILayout.Label("The builds will be saved in this format\n" + BuildPath + "/[VERSION]/[PLATFORM]");

        BuildAddressables = EditorGUILayout.Toggle("Build Addressables", BuildAddressables);
        DevBuild = EditorGUILayout.Toggle("Dev Build", DevBuild);

        if (GUILayout.Button("Build Windows-x64"))
        {
            if (!Directory.Exists(BuildPath))
            {
                EditorUtility.DisplayDialog("Error!", "This build path doesn't exist.", "OK");
                return;
            }

            string BuildPathWithVersion = BuildPath + "/" + Application.version;

            Prep(BuildPathWithVersion);

            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            if (BuildAddressables)
            {
                AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    EditorUtility.DisplayDialog("Error while building Addressables!", "Addressables build error encountered: " + result.Error, "OK");
                    return;
                }
            }

            BuildOptions bo = BuildOptions.None;

            if (DevBuild)
            {
                bo = BuildOptions.CompressWithLz4HC | BuildOptions.Development;
            }
            else
            {
                bo = BuildOptions.CompressWithLz4HC;
            }

            BuildPipeline.BuildPlayer(GetScenePaths(), BuildPathWithVersion + "/Windows-x64/" + Application.productName + ".exe", BuildTarget.StandaloneWindows64, bo);

            EditorUtility.RevealInFinder(BuildPathWithVersion + "/Windows-x64");
        }
        if (GUILayout.Button("Build Windows-x86"))
        {
            if (!Directory.Exists(BuildPath))
            {
                EditorUtility.DisplayDialog("Error!", "This build path doesn't exist.", "OK");
                return;
            }

            string BuildPathWithVersion = BuildPath + "/" + Application.version;

            Prep(BuildPathWithVersion);

            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            if (BuildAddressables)
            {
                AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    EditorUtility.DisplayDialog("Error while building Addressables!", "Addressables build error encountered: " + result.Error, "OK");
                    return;
                }
            }

            BuildOptions bo = BuildOptions.None;

            if (DevBuild)
            {
                bo = BuildOptions.CompressWithLz4HC | BuildOptions.Development;
            }
            else
            {
                bo = BuildOptions.CompressWithLz4HC;
            }

            BuildPipeline.BuildPlayer(GetScenePaths(), BuildPathWithVersion + "/Windows-x86/" + Application.productName + ".exe", BuildTarget.StandaloneWindows, bo);

            EditorUtility.RevealInFinder(BuildPathWithVersion + "/Windows-x86");
        }
        if (GUILayout.Button("Build Linux-x64"))
        {
            if (!Directory.Exists(BuildPath))
            {
                EditorUtility.DisplayDialog("Error!", "This build path doesn't exist.", "OK");
                return;
            }

            string BuildPathWithVersion = BuildPath + "/" + Application.version;

            Prep(BuildPathWithVersion);
            
            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64);
            if (BuildAddressables)
            {
                AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    EditorUtility.DisplayDialog("Error while building Addressables!", "Addressables build error encountered: " + result.Error, "OK");
                }
            }

            BuildOptions bo = BuildOptions.None;

            if (DevBuild)
            {
                bo = BuildOptions.CompressWithLz4HC | BuildOptions.Development;
            }
            else
            {
                bo = BuildOptions.CompressWithLz4HC;
            }

            BuildPipeline.BuildPlayer(GetScenePaths(), BuildPathWithVersion + "/Linux-x64/Cookieclicker2mp4.x86_64", BuildTarget.StandaloneLinux64, bo);

            EditorUtility.RevealInFinder(BuildPathWithVersion + "/Linux-x64");
        }
        if (GUILayout.Button("Build macOS Universal"))
        {
            if (!Directory.Exists(BuildPath))
            {
                EditorUtility.DisplayDialog("Error!", "This build path doesn't exist.", "OK");
                return;
            }

            string BuildPathWithVersion = BuildPath + "/" + Application.version;

            Prep(BuildPathWithVersion);

            EditorUserBuildSettings.SwitchActiveBuildTargetAsync(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
            UnityEditor.OSXStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.x64ARM64;
            if (BuildAddressables)
            {
                AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    EditorUtility.DisplayDialog("Error while building Addressables!", "Addressables build error encountered: " + result.Error, "OK");
                }
            }

            BuildOptions bo = BuildOptions.None;

            if (DevBuild)
            {
                bo = BuildOptions.CompressWithLz4HC | BuildOptions.Development;
            }
            else
            {
                bo = BuildOptions.CompressWithLz4HC;
            }

            BuildPipeline.BuildPlayer(GetScenePaths(), BuildPathWithVersion + "/macOS-Universal/Cookieclicker2mp4.app", BuildTarget.StandaloneOSX, bo);

            EditorUtility.RevealInFinder(BuildPathWithVersion + "/macOS-Universal");
        }
        
        static string[] GetScenePaths() 
        {
		    string[] scenes = new string[EditorBuildSettings.scenes.Length];
		    for(int i = 0; i < scenes.Length; i++) 
            {
			    scenes[i] = EditorBuildSettings.scenes[i].path;
		    }
		    return scenes;
	    }

        void Prep(string BuildPathWithVersion)
        {
            if (!Directory.Exists(BuildPathWithVersion))
            {
                Directory.CreateDirectory(BuildPathWithVersion);
            }

            if (!Directory.Exists(BuildPathWithVersion + "/Windows-x64"))
            {
                Directory.CreateDirectory(BuildPathWithVersion + "/Windows-x64");
            }
            if (!Directory.Exists(BuildPathWithVersion + "/Windows-x86"))
            {
                Directory.CreateDirectory(BuildPathWithVersion + "/Windows-x86");
            }
            if (!Directory.Exists(BuildPathWithVersion + "/Linux-x64"))
            {
                Directory.CreateDirectory(BuildPathWithVersion + "/Linux-x64");
            }
            if (!Directory.Exists(BuildPathWithVersion + "/macOS-Universal"))
            {
                Directory.CreateDirectory(BuildPathWithVersion + "/macOS-Universal");
            }
        }
    }
}