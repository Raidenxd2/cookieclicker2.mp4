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
        Android_ARMv7,
        Android_ARM64,
        itch_io_Windows_Release,
        Windows64Build,
        itch_io_Linux_Release,
        itch_io_macOS_Release,
        Android_x86,
        Android_x86_64,
        Android_Universal,
    }

    public static class BuildConstants
    {
        public static readonly DateTime buildDate = new DateTime(638759360715249631);
        public const string version = "1.3.0-Beta2-OS_11023";
        public const ReleaseType releaseType = ReleaseType.Release;
        public const Platform platform = Platform.PC;
        public const ScriptingBackend scriptingBackend = ScriptingBackend.Mono;
        public const Architecture architecture = Architecture.Windows_x64;
        public const Distribution distribution = Distribution.itch_io_Windows_Release;
    }
}

