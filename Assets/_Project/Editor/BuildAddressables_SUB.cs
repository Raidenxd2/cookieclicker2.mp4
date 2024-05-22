using System;
using SuperUnityBuild.BuildTool;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

public class BuildAddressables_SUB : BuildAction, IPreBuildPerPlatformAction
{
    public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform, BuildArchitecture architecture, BuildScriptingBackend scriptingBackend, BuildDistribution distribution, DateTime buildTime, ref BuildOptions options, string configKey, string buildPath)
    {
        AddressableAssetSettings.BuildPlayerContent();
    }
}