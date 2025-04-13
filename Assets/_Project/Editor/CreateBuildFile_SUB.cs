using System;
using SuperUnityBuild.BuildTool;
using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateBuildFile_SUB : BuildAction, IPreBuildAction
{
    public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform, BuildArchitecture architecture, BuildScriptingBackend scriptingBackend, BuildDistribution distribution, DateTime buildTime, ref BuildOptions options, string configKey, string buildPath)
    {
        File.Delete(Application.streamingAssetsPath + "/build");

        int build = int.Parse(PlayerSettings.bundleVersion);
        build--;

        File.WriteAllText(Application.streamingAssetsPath + "/build", build.ToString());
    }
}