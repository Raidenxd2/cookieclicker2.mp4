using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThemeManagerGUI : EditorWindow
{

    [MenuItem("Cookieclicker2.mp4 Theme Manager/Build...")]
    public static void ShowWindow()
    {
        GetWindow<ThemeManagerGUI>("Build...");
    }


    void OnGUI()
    {
        if (GUILayout.Button("Build AssetBundle"))
        {
            EditorApplication.ExecuteMenuItem("Cookieclicker2.mp4 Theme Manager/Build AssetBundle");
        }
        if (GUILayout.Button("Bake Prefab Lightmaps"))
        {
            EditorApplication.ExecuteMenuItem("Cookieclicker2.mp4 Theme Manager/Bake Prefab lightmap data");
        }
    }
}
