#if UNITY_STANDALONE
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset URPAsset;

    [SerializeField] private TMP_Dropdown WindowModeDropdown;
    [SerializeField] private TMP_Dropdown ResolutionDropdown;
    [SerializeField] private TMP_Dropdown AADropdown;
    [SerializeField] private Toggle VSyncToggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    private System.Collections.IEnumerator Start()
    {
        AddOptionsToDropdown();

        yield return new WaitForSeconds(1f);

        if (PlayerPrefs.GetInt("VideoSettings_Setup", 0) == 0)
        {
            ResetSettings();
        }
        else
        {
            LoadSettings();
        }
    }

    private void ResetSettings()
    {
        PlayerPrefs.SetInt("VideoSettings_Setup", 1);
        PlayerPrefs.SetInt("WindowMode", 1);
        PlayerPrefs.SetInt("GRAPHICS_AA", 1);
        PlayerPrefs.SetInt("GRAPHICS_VSync", 1);
    }

    private void AddOptionsToDropdown()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        ResolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            filteredResolutions.Add(resolutions[i]);
            if (resolutions[i].refreshRateRatio.numerator == currentRefreshRate)
            {
                
            }
        }

        List<string> options = new();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resoultionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resoultionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.GetInt("VideoSettings_Setup", 0) == 0)
        {
            PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        }

        ResolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", 0);
        WindowModeDropdown.value = PlayerPrefs.GetInt("WindowMode", 1);
        AADropdown.value = PlayerPrefs.GetInt("GRAPHICS_AA", 1);
        VSyncToggle.isOn = intToBool(PlayerPrefs.GetInt("GRAPHICS_VSync", 1));
        SetResolution(PlayerPrefs.GetInt("ResolutionIndex", 1));
        ChangeWindowMode(PlayerPrefs.GetInt("WindowMode", 1));
        ChangeAntiAliasing(PlayerPrefs.GetInt("GRAPHICS_AA", 1));
        ChangeVSync(intToBool(PlayerPrefs.GetInt("GRAPHICS_VSync", 1)));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);

        switch (PlayerPrefs.GetInt("WindowMode", 1))
        {
            case 0:
                Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.ExclusiveFullScreen);
                break;

            case 1:
                Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
                break;

            case 2:
                if (!Screen.fullScreen)
                {
                    break;
                }

                Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.Windowed);
                break;
        }
    }

    public void ChangeWindowMode(int val)
    {
        PlayerPrefs.SetInt("WindowMode", val);

        SetResolution(PlayerPrefs.GetInt("ResolutionIndex", 1));
    }

    public void ChangeVSync(bool val)
    {
        PlayerPrefs.SetInt("GRAPHICS_VSync", boolToInt(val));

        if (val)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void ChangeAntiAliasing(int val)
    {
        PlayerPrefs.SetInt("GRAPHICS_AA", val);

        switch (val)
        {
            case 0:
                URPAsset.msaaSampleCount = 0;
                break;
            case 1:
                URPAsset.msaaSampleCount = 2;
                break;
            case 2:
                URPAsset.msaaSampleCount = 4;
                break;
            case 3:
                URPAsset.msaaSampleCount = 8;
                break;
        }
    }

    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else   
            return false;
    }
}
#endif