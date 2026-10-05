using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Experimental;
using UnityEngine;

public class CC2Build : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        var connectSettingsRes = EditorResources.Load<Object>("ProjectSettings/UnityConnectSettings.asset");
        var connectSettingsObj = new SerializedObject(connectSettingsRes);
        connectSettingsObj.FindProperty("m_Enabled").boolValue = false;
        connectSettingsObj.FindProperty("UnityAnalyticsSettings").FindPropertyRelative("m_Enabled").boolValue = false;
        connectSettingsObj.ApplyModifiedProperties();
        
        AssetDatabase.SaveAssets();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        // For Windows, macOS, and Linux: Copy modified UnityEngine.UIElementsModule that skips initialization.
        if (report.summary.platform is BuildTarget.StandaloneWindows or BuildTarget.StandaloneWindows64)
        {
            string dir = Path.GetDirectoryName(report.summary.outputPath);

            if (!report.summary.options.HasFlag(BuildOptions.Development))
            {
                if (File.Exists(dir + "/Cookieclicker2.mp4_Data/Managed/UnityEngine.UIElementsModule.dll"))
                {
                    File.Delete(dir + "/Cookieclicker2.mp4_Data/Managed/UnityEngine.UIElementsModule.dll");
                    File.Copy(Application.dataPath + "/_Project/UIElementsNoInit/win/UnityEngine.UIElementsModule.dll~", dir + "/Cookieclicker2.mp4_Data/Managed/UnityEngine.UIElementsModule.dll");
                }
            }
        }

        if (report.summary.platform is BuildTarget.StandaloneWindows or BuildTarget.StandaloneWindows64 or BuildTarget.StandaloneLinux64)
        {
            string dir = Path.GetDirectoryName(report.summary.outputPath);
            
            Debug.Log("Removing xrsdk-pre-init-library from " + dir + "/Cookieclicker2.mp4_Data/boot.config");
                
            var oldLines = File.ReadAllLines(dir + "/Cookieclicker2.mp4_Data/boot.config");
            var newLines = oldLines.Where(line => !line.Contains("xrsdk-pre-init-library"));
                
            File.WriteAllLines(dir + "/Cookieclicker2.mp4_Data/boot.config", newLines);
            
            if (Directory.Exists(dir + "/Cookieclicker2.mp4_Data/StreamingAssets/aa/AddressablesLink"))
            {
                Directory.Delete(dir + "/Cookieclicker2.mp4_Data/StreamingAssets/aa/AddressablesLink", true);
            }

            if (File.Exists(dir + "/Cookieclicker2.mp4_Data/Plugins/no_plugins_were_generated.txt"))
            {
                File.Delete(dir + "/Cookieclicker2.mp4_Data/Plugins/no_plugins_were_generated.txt");
            }
            
            File.Copy("Assets/Plugins/Newtonsoft.Json.pdb", dir + "/Cookieclicker2.mp4_Data/Managed/Newtonsoft.Json.pdb", true);
        }

        if (report.summary.platform is BuildTarget.StandaloneOSX)
        {
            string dir = Path.GetDirectoryName(report.summary.outputPath);
            
            Debug.Log("Removing xrsdk-pre-init-library from " + dir + "/Contents/Resources/Data/boot.config");
                
            var oldLines = File.ReadAllLines(dir + "/Contents/Resources/Data/boot.config");
            var newLines = oldLines.Where(line => !line.Contains("xrsdk-pre-init-library"));
                
            File.WriteAllLines(dir + "/Contents/Resources/Data/boot.config", newLines);

            if (Directory.Exists(dir + "/Contents/Resources/Data/StreamingAssets/aa/AddressablesLink"))
            {
                Directory.Delete(dir + "/Contents/Resources/Data/StreamingAssets/aa/AddressablesLink", true);
            }
            
            File.Copy("Assets/Plugins/Newtonsoft.Json.pdb", dir + "/Contents/Resources/Data/Managed/Newtonsoft.Json.pdb", true);
        }
        
        // if (report.summary.platform is BuildTarget.StandaloneOSX)
        // {
        //     // TODO: Add modified UnityEngine.UIElementsModule.dll for macOS
        //     
        //     string dir = Path.GetDirectoryName(report.summary.outputPath);
        //     if (!report.summary.options.HasFlag(BuildOptions.Development))
        //     {
        //         // if (File.Exists(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll"))
        //         // {
        //         //     File.Delete(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
        //         //     File.Copy(Application.dataPath + "/_Project/UIElementsNoInit/mac/UnityEngine.UIElementsModule.dll~", dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
        //         // }
        //     }
        // }
        //
        // if (report.summary.platform is BuildTarget.StandaloneLinux64)
        // {
        //     // TODO: Add modified UnityEngine.UIElementsModule.dll for Linux
        //     
        //     string dir = Path.GetDirectoryName(report.summary.outputPath);
        //     if (!report.summary.options.HasFlag(BuildOptions.Development))
        //     {
        //         // if (File.Exists(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll"))
        //         // {
        //         //     File.Delete(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
        //         //     File.Copy(Application.dataPath + "/_Project/UIElementsNoInit/linux/UnityEngine.UIElementsModule.dll~", dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
        //         // }
        //     }
        // }
    }
}