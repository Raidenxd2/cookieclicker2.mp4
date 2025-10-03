using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class CustomExeBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get {return 0;} }

    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_STANDALONE_WIN
        if (report.summary.platform == BuildTarget.StandaloneWindows64 && UnityEditor.WindowsStandalone.UserBuildSettings.architecture == OSArchitecture.x64 && EditorPrefs.GetBool("CC2_CustomExeBuild", false))
        {
            string outputPath = Path.GetDirectoryName(report.summary.outputPath);
            File.Copy(Application.dataPath + "/_Project/BuildOutput/CustomExe/x64/Cookieclicker2.mp4.exe", outputPath + "/Cookieclicker2.mp4.exe", true);
            Directory.CreateDirectory(outputPath + "/Cookieclicker2.mp4_Data/ExecutableSourcesAndSymbols");
            CopyFilesRecursively(Application.dataPath + "/_Project/BuildOutput/CustomExe/x64/ExecutableSourcesAndSymbols", outputPath + "/Cookieclicker2.mp4_Data/ExecutableSourcesAndSymbols");
        }
        else if (report.summary.platform == BuildTarget.StandaloneWindows && UnityEditor.WindowsStandalone.UserBuildSettings.architecture == OSArchitecture.x86 && EditorPrefs.GetBool("CC2_CustomExeBuild", false))
        {
            string outputPath = Path.GetDirectoryName(report.summary.outputPath);
            File.Copy(Application.dataPath + "/_Project/BuildOutput/CustomExe/x86/Cookieclicker2.mp4.exe", outputPath + "/Cookieclicker2.mp4.exe", true);
            Directory.CreateDirectory(outputPath + "/Cookieclicker2.mp4_Data/ExeeutableSourcesAndSymbols");
            CopyFilesRecursively(Application.dataPath + "/_Project/BuildOutput/CustomExe/x86/ExecutableSourcesAndSymbols", outputPath + "/Cookieclicker2.mp4_Data/ExecutableSourcesAndSymbols");
        }
        else if (report.summary.platform == BuildTarget.StandaloneWindows64 && UnityEditor.WindowsStandalone.UserBuildSettings.architecture == OSArchitecture.ARM64 && EditorPrefs.GetBool("CC2_CustomExeBuild", false))
        {
            string outputPath = Path.GetDirectoryName(report.summary.outputPath);
            File.Copy(Application.dataPath + "/_Project/BuildOutput/CustomExe/ARM64/Cookieclicker2.mp4.exe", outputPath + "/Cookieclicker2.mp4.exe", true);
            Directory.CreateDirectory(outputPath + "/Cookieclicker2.mp4_Data/ExeeutableSourcesAndSymbols");
            CopyFilesRecursively(Application.dataPath + "/_Project/BuildOutput/CustomExe/ARM64/ExecutableSourcesAndSymbols", outputPath + "/Cookieclicker2.mp4_Data/ExecutableSourcesAndSymbols");
        }
#endif
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            if (newPath.Contains(".meta"))
            {
                continue;
            }
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }

    [MenuItem("Cookieclicker2.mp4/Enable Custom Exe Build")]
    public static void EnableCustomExeBuild()
    {
        EditorPrefs.SetBool("CC2_CustomExeBuild", true);
    }

    [MenuItem("Cookieclicker2.mp4/Disable Custom Exe Build")]
    public static void DisableCustomExeBuild()
    {
        EditorPrefs.SetBool("CC2_CustomExeBuild", false);
    }
}