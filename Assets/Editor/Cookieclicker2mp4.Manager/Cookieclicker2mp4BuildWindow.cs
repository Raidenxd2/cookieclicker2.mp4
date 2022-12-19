using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cookieclicker2mp4BuildWindow : EditorWindow
{

    bool il2cppbuild;
    bool debugBuild;

    [MenuItem("Cookieclicker2.mp4/Build Game...")]
    public static void ShowWindow()
    {
        GetWindow<Cookieclicker2mp4BuildWindow>("Build Game");
    }


    void OnGUI()
    {
        debugBuild = EditorGUILayout.Toggle("Development Build", debugBuild);
        il2cppbuild = EditorGUILayout.Toggle("IL2CPP Build", il2cppbuild);
        if (il2cppbuild)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone | BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        }
        else
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone | BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Switch to Windows"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        }
        if (GUILayout.Button("Switch to MacOS"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
        }
        if (GUILayout.Button("Switch to Linux"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64);
        }
        if (GUILayout.Button("Switch to Android"))
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Build"))
        {
            var show = GameObject.FindGameObjectsWithTag("ShowInBuild");
            var hide = GameObject.FindGameObjectsWithTag("HideInBuild");
            foreach (var item in show)
            {
                item.SetActive(true);
            }
            foreach (var item in hide)
            {
                item.SetActive(false);
            }
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
            string[] levels = new string[] {"Assets/Scenes/Init.unity", "Assets/Scenes/Game.unity"};

            if (debugBuild)
            {
                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.DetailedBuildReport | BuildOptions.ShowBuiltPlayer | BuildOptions.Development);
            }
            else
            {
                BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.DetailedBuildReport | BuildOptions.ShowBuiltPlayer);
            }
            
            foreach (var item in show)
            {
                item.SetActive(false);
            }
            foreach (var item in hide)
            {
                item.SetActive(true);
            }
        }

        if (GUILayout.Button("Debug"))
        {
            Debug.Log(debugBuild);
            Debug.Log(il2cppbuild);
        }
    }
}
