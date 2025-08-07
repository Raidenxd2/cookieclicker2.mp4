// IMPORTANT: You must disable warnings as errors in order to build this.

// me vs c++ who will win
#include "PrecompiledHeader.h"
#include "..\UnityPlayerStub\Exports.h"
#include <iostream>
#include <fstream>
#include <atlstr.h>
#include "Registry.hpp"

using namespace std;

using namespace m4x1m1l14n;

// Hint that the discrete gpu should be enabled on optimus/enduro systems
// NVIDIA docs: http://developer.download.nvidia.com/devzone/devcenter/gamegraphics/files/OptimusRenderingPolicies.pdf
// AMD forum post: http://devgurus.amd.com/thread/169965
extern "C"
{
    __declspec(dllexport) DWORD NvOptimusEnablement = 0x00000001;
    __declspec(dllexport) int AmdPowerXpressRequestHighPerformance = 1;
    __declspec(dllexport) extern const UINT D3D12SDKVersion = 611;
    __declspec(dllexport) extern const char* D3D12SDKPath = u8".\\D3D12\\";
}

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nShowCmd)
{
    LPWSTR newCmd = lpCmdLine;

    try
    {
        // Open the key HKEY_CURRENT_USER\Software\raiden\Cookieclicker2.mp4
        auto key = Registry::CurrentUser->Open(L"Software\\raiden\\Cookieclicker2.mp4");

        // Gets the value of the DWORD GraphicsAPI_h1886732360
        // 0: Direct3D 11: nothing
        // 1: Direct3D 12: -force-d3d12
        // 2: Vulkan: -force-vulkan
        long GraphicsAPI = key->GetInt32(L"GraphicsAPI_h1886732360");

        if (GraphicsAPI == 0)
        {

        }
        else if (GraphicsAPI == 1)
        {
            size_t len = wcslen(lpCmdLine) + wcslen(L" -force-d3d12") + 1;
            LPWSTR combined = new wchar_t[len];
            wcscpy(combined, lpCmdLine);
            wcscat(combined, L" -force-d3d12");

            newCmd = combined;
        }
        else if (GraphicsAPI == 2)
        {
            size_t len = wcslen(lpCmdLine) + wcslen(L" -force-vulkan") + 1;
            LPWSTR combined = new wchar_t[len];
            wcscpy(combined, lpCmdLine);
            wcscat(combined, L" -force-vulkan");

            newCmd = combined;
        }
    }
    catch (const std::exception& e)
    {
        // Create the file CC2PreInitLog.txt where the game is located and log the exception
        std::ofstream outfile("CC2PreInitLog.txt");
        outfile << "(wWinMain) Failed to read registry (HKEY_CURRENT_USER\\Software\\raiden\\Cookieclicker2.mp4\\GraphicsAPI_h1186732360) or failed to start the game with an argument\nException info:\n";
        outfile << e.what();
        outfile.close();
    }

    // Start the game
    return UnityMain(hInstance, hPrevInstance, newCmd, nShowCmd);
}

namespace m4x1m1l14n
{
    namespace Registry
    {
        RegistryKey_ptr ClassesRoot(new RegistryKey(HKEY_CLASSES_ROOT));
        RegistryKey_ptr CurrentUser(new RegistryKey(HKEY_CURRENT_USER));
        RegistryKey_ptr LocalMachine(new RegistryKey(HKEY_LOCAL_MACHINE));
        RegistryKey_ptr Users(new RegistryKey(HKEY_USERS));
        RegistryKey_ptr CurrentConfig(new RegistryKey(HKEY_CURRENT_CONFIG));
    }
}