using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace KillItMyself.Edito
{
    public class BuildManager : EditorWindow
    {
        private static BuildManagerSettings bms;

        [MenuItem("Bean Shootout/Build Manager")]
        public static void ShowWindow()
        {
            if (!AssetDatabase.AssetPathExists("Assets/BuildManagerSettings.asset"))
            {
                BuildManagerSettings asset = CreateInstance<BuildManagerSettings>();

                AssetDatabase.CreateAsset(asset, "Assets/BuildManagerSettings.asset");
                AssetDatabase.SaveAssets();
            }

            bms = AssetDatabase.LoadAssetAtPath<BuildManagerSettings>("Assets/BuildManagerSettings.asset");

            BuildManager wnd = GetWindow<BuildManager>();
            wnd.titleContent = new("Build Manager");
        }

        void OnGUI()
        {
            if (bms == null)
            {
                LoadBms();
            }

            bms.BuildAddressables = GUILayout.Toggle(bms.BuildAddressables, "Build Addressables");
            bms.IncrementBuildNumber = GUILayout.Toggle(bms.IncrementBuildNumber, "Increment Build Number");
            bms.BuildCount = EditorGUILayout.IntField("Build Count", bms.BuildCount);
            bms.Branch = EditorGUILayout.TextField("Branch", bms.Branch);
            bms.VersionPrefix = EditorGUILayout.TextField("Version Prefix", bms.VersionPrefix);

            if (GUILayout.Button("Save settings"))
            {
                SaveSettings();
            }

            if (GUILayout.Button("Build Windows x64"))
            {
                Build(BuildTarget.StandaloneWindows64);
            }

            if (GUILayout.Button("Build Windows x86"))
            {
                Build(BuildTarget.StandaloneWindows);
            }

            if (GUILayout.Button("Build Windows ARM64"))
            {
                Build(BuildTarget.StandaloneWindows64, true);
            }

            if (GUILayout.Button("Build macOS"))
            {
                Build(BuildTarget.StandaloneOSX);
            }

            if (GUILayout.Button("Build Linux"))
            {
                Build(BuildTarget.StandaloneLinux64);
            }

            if (GUILayout.Button("Build Android"))
            {
                Build(BuildTarget.Android);
            }
        }

        private void Build(BuildTarget bt, bool winArm64 = false)
        {
            if (bms.IncrementBuildNumber)
            {
                bms.BuildCount++;

                SaveSettings();
            }

            PlayerSettings.bundleVersion = bms.VersionPrefix + "_" + bms.BuildCount.ToString() + " (" + bms.Branch + ")";
            PlayerSettings.Android.bundleVersionCode = bms.BuildCount;
            PlayerSettings.macOS.buildNumber = bms.BuildCount.ToString();

            AssetDatabase.Refresh();

            File.Delete(Application.streamingAssetsPath + "/build");

            int build = PlayerSettings.Android.bundleVersionCode;

            File.WriteAllText(Application.streamingAssetsPath + "/build", build.ToString());

            BuildTargetGroup btg = BuildTargetGroup.Unknown;

            switch (bt)
            {
                case BuildTarget.StandaloneWindows:
                    btg = BuildTargetGroup.Standalone;
                    break;
                case BuildTarget.StandaloneWindows64:
                    btg = BuildTargetGroup.Standalone;
                    break;
                case BuildTarget.StandaloneOSX:
                    btg = BuildTargetGroup.Standalone;
                    break;
                case BuildTarget.StandaloneLinux64:
                    btg = BuildTargetGroup.Standalone;
                    break;
                case BuildTarget.Android:
                    btg = BuildTargetGroup.Android;
                    break;
            }

#if UNITY_STANDALONE_WIN
            if (winArm64)
            {
                UnityEditor.WindowsStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.ARM64;
            }
            else if (bt == BuildTarget.StandaloneWindows64)
            {
                UnityEditor.WindowsStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.x64;
            }
            else if (bt == BuildTarget.StandaloneWindows)
            {
                UnityEditor.WindowsStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.x86;
            }
#endif

#if UNITY_STANDALONE_OSX
            if (bt == BuildTarget.StandaloneOSX)
            {
                UnityEditor.OSXStandalone.UserBuildSettings.architecture = UnityEditor.Build.OSArchitecture.x64ARM64;
            }
#endif

            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(btg, bt))
            {
                return;
            }

            if (bms.BuildAddressables)
            {
                AddressableAssetSettings.BuildPlayerContent();
            }

            List<string> scenes = new();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            if (bms == null)
            {
                LoadBms();
            }

            string BuildPath = Application.dataPath + "/../Builds/" + bms.BuildCount;

            if (!Directory.Exists(BuildPath))
            {
                Directory.CreateDirectory(BuildPath);
            }

            string exeName = BuildPath + "/default.exe";
            
            switch (bt)
            {
                case BuildTarget.StandaloneWindows:
                    exeName = BuildPath + "/Cookieclicker2.mp4.exe";
                    break;
                case BuildTarget.StandaloneWindows64:
                    exeName = BuildPath + "/Cookieclicker2.mp4.exe";
                    break;
                case BuildTarget.StandaloneOSX:
                    exeName = BuildPath + "/Cookieclicker2.mp4.app";
                    break;
                case BuildTarget.StandaloneLinux64:
                    exeName = BuildPath + "/Cookieclicker2.mp4.x86_64";
                    break;
                case BuildTarget.Android:
                    exeName = BuildPath + "/Cookieclicker2mp4.apk";
                    break;
            }

            BuildPipeline.BuildPlayer(scenes.ToArray(), exeName, bt, BuildOptions.ShowBuiltPlayer);

            DirectoryInfo boDir = new("Assets/_Project/BuildOutput/win");
            FileInfo[] boInfo = boDir.GetFiles("*.*");

            foreach (FileInfo file in boInfo)
            {
                if (file.Extension == ".meta")
                {

                }
                else
                {
                    File.Copy("Assets/_Project/BuildOutput/win/" + file.Name, BuildPath + "/" + Path.GetFileName(file.Name), true);
                }
            }

            DirectoryInfo boDir2 = new("Assets/CopyToStreamingAssets");
            FileInfo[] boInfo2 = boDir2.GetFiles("*.*");

            foreach (FileInfo file in boInfo2)
            {
                if (file.Extension == ".meta")
                {

                }
                else
                {
                    File.Copy("Assets/CopyToStreamingAssets/" + file.Name, BuildPath + "/Cookieclicker2.mp4_Data/StreamingAssets/" + Path.GetFileName(file.Name), true);
                }
            }
        }

        private void SaveSettings()
        {
            EditorUtility.SetDirty(bms);
            AssetDatabase.Refresh();
        }

        private void LoadBms()
        {
            bms = AssetDatabase.LoadAssetAtPath<BuildManagerSettings>("Assets/BuildManagerSettings.asset");
        }
    }
}