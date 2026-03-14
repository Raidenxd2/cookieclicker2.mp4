using System.IO;
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
        
        if (report.summary.platform is BuildTarget.StandaloneOSX)
        {
            // TODO: Add modified UnityEngine.UIElementsModule.dll for macOS
            
            string dir = Path.GetDirectoryName(report.summary.outputPath);
            if (!report.summary.options.HasFlag(BuildOptions.Development))
            {
                // if (File.Exists(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll"))
                // {
                //     File.Delete(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
                //     File.Copy(Application.dataPath + "/_Project/UIElementsNoInit/mac/UnityEngine.UIElementsModule.dll~", dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
                // }
            }
        }

        if (report.summary.platform is BuildTarget.StandaloneLinux64)
        {
            // TODO: Add modified UnityEngine.UIElementsModule.dll for Linux
            
            string dir = Path.GetDirectoryName(report.summary.outputPath);
            if (!report.summary.options.HasFlag(BuildOptions.Development))
            {
                // if (File.Exists(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll"))
                // {
                //     File.Delete(dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
                //     File.Copy(Application.dataPath + "/_Project/UIElementsNoInit/linux/UnityEngine.UIElementsModule.dll~", dir + "/BeanShootout_Data/Managed/UnityEngine.UIElementsModule.dll");
                // }
            }
        }
    }
}