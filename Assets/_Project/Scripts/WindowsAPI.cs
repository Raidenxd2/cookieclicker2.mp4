#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;

public static class WindowsAPI
{
    public delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);
    
    [DllImport("shell32.dll")]
    public static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);
    
    [DllImport("user32.dll")]
    public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

    [DllImport("kernel32.dll")]
    public static extern int GetCurrentThreadId();

    public static IntPtr GetWindowHandle()
    {
        IntPtr returnHwnd = IntPtr.Zero;
        var threadId = GetCurrentThreadId();
        EnumThreadWindows(threadId,
            (hWnd, lParam) => {
                if(returnHwnd == IntPtr.Zero) returnHwnd = hWnd;
                return true;
            }, IntPtr.Zero);
        return returnHwnd;
    }

    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
}

/// <summary>
/// https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid
/// </summary>
public static class KnownFolder
{
    public static readonly Guid Downloads = new Guid( "374DE290-123F-4565-9164-39C4925E467B" );
}

/// <summary>
/// https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
/// </summary>
[Flags]
public enum DwmWindowAttribute : uint
{
    DWMWA_USE_IMMERSIVE_DARK_MODE = 20
}
#endif