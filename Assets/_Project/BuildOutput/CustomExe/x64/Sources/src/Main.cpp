// IMPORTANT: You must disable warnings as errors in order to build this.

// me vs c++ who will win
#include "PrecompiledHeader.h"
#include "..\UnityPlayerStub\Exports.h"
#include <iostream>
#include <fstream>
#include <string>
#include <atlstr.h>

using namespace std;

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

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE, LPWSTR lpCmdLine, int nShowCmd)
{
    LPCWSTR customDataFolder = nullptr;

    LPWSTR newCmd = lpCmdLine;

    try
    {
        string line;

        ifstream GraphicsApiReadFile("Cookieclicker2.mp4_Data\\GraphicsAPI");

        // Get the first line in the file as a int this is horrid
        for (int result; std::getline(GraphicsApiReadFile, line); result = std::stoi(line))
        {
        }

        GraphicsApiReadFile.close();

        // GraphicsApiSetting can be the following:
        // 0: Direct3D 11: nothing
        // 1: Direct3D 12: -force-d3d12
        // 2: Vulkan: -force-vulkan

        if (line == "1")
        {
            size_t len = wcslen(lpCmdLine) + wcslen(L" -force-d3d12") + 1;
            LPWSTR combined = new wchar_t[len];
            wcscpy(combined, lpCmdLine);
            wcscat(combined, L" -force-d3d12");

            newCmd = combined;
        }
        else if (line == "2")
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
    }

    return UnityMain2(hInstance, customDataFolder, newCmd, nShowCmd);
}