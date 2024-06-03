using System;

// This file is auto-generated. Do not modify or move this file.

namespace SuperUnityBuild.Generated
{
    public enum ReleaseType
    {
        None,
        Development,
        Release,
    }

    public enum Platform
    {
        None,
        Android,
        PC,
        Linux,
    }

    public enum ScriptingBackend
    {
        None,
        IL2CPP,
    }

    public enum Architecture
    {
        None,
        Android,
        Windows_x86,
        Windows_x64,
        Linux_x64,
    }

    public enum Distribution
    {
        None,
        Android_Google_Release,
        Android_Development,
        itch_io_Windows_Release,
        Windows64Build,
        itch_io_Linux_Release,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638520123076764195);
        public const string version = "1.2.4-PublicRelease";
        public const ReleaseType releaseType = ReleaseType.Release;
        public const Platform platform = Platform.Linux;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.IL2CPP;
        public const Architecture architecture = Architecture.Linux_x64;
        public const Distribution distribution = Distribution.itch_io_Linux_Release;
    }
}

