using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Development,
        Release,
        Release_with_logs,
    }

    public enum Platform
    {
        None,
        Android,
        PC,
        Linux,
        macOS,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
        Mono,
    }

    public enum Architecture
    {
        None,
        Android,
        Windows_x86,
        Windows_x64,
        Linux_x64,
        macOS,
    }

    public enum Distribution
    {
        None,
        Android_Google_Release,
        Android_Development,
        itch_io_Windows_Release,
        Windows64Build,
        itch_io_Linux_Release,
        itch_io_macOS_Release,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638615227011488193);
        public const string version = "1.3.0-Beta1_OS";
        public const ReleaseType releaseType = ReleaseType.Release_with_logs;
        public const Platform platform = Platform.Android;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Architecture architecture = Architecture.Android;
        public const Distribution distribution = Distribution.Android_Development;
    }
}

