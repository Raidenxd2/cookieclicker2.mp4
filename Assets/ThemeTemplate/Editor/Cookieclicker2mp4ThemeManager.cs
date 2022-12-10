using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class Cookieclicker2mp4ThemeManager
{
    [MenuItem("Cookieclicker2.mp4 Theme Manager/Create Theme")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            // DirectoryInfo d = new DirectoryInfo(assetBundleDirectory);
            // File.Delete("Assets/AssetBundles/AssetBundles");
            // foreach (var file in d.GetFiles("*.manifest"))
            // {
            //     file.Delete();
            // }
            // foreach (var file in d.GetFiles("*.manifest.meta"))
            // {
            //     file.Delete();
            // }
            AssetDatabase.Refresh();
            Debug.Log("<b>✔️ SUCCESSFULLY BUILDED ASSETBUNDLES ✔️</b>");
        }
        catch(Exception e)
        {
            Debug.LogError("AN ERROR OCCURED WHILE BUILDING THE ASSETBUNDLES!\n" + e.ToString());
        }
    }
}
