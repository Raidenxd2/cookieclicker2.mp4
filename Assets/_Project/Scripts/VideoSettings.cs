#if UNITY_STANDALONE
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset URPAsset;

    [SerializeField] private TMP_Dropdown GraphicsAPIDropdown;
    [SerializeField] private TMP_Dropdown WindowModeDropdown;
    [SerializeField] private TMP_Dropdown ResolutionDropdown;
    [SerializeField] private TMP_Dropdown AADropdown;
    [SerializeField] private Toggle VSyncToggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private int currentResolutionIndex = 0;

    [SerializeField] private GameObject GameRestartRequired;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            GraphicsAPIDropdown.gameObject.SetActive(true);
        }

        AddOptionsToDropdown();

        if (!BetterPrefs.GetBool("VideoSettings_Setup", false))
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
        BetterPrefs.SetBool("VideoSettings_Setup", true);
        BetterPrefs.SetInt("WindowMode", 1);
        BetterPrefs.SetInt("GRAPHICS_AA", 1);
        BetterPrefs.SetBool("GRAPHICS_VSync", true);
        PlayerPrefs.SetInt("GraphicsAPI", 0);
    }

    private void AddOptionsToDropdown()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        ResolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutions.Length; i++)
        {
            filteredResolutions.Add(resolutions[i]);
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

        if (!BetterPrefs.GetBool("VideoSettings_Setup", false))
        {
            BetterPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        }

        if (BetterPrefs.GetInt("ResolutionIndex", 1) >= options.Count)
        {
            BetterPrefs.SetInt("ResolutionIndex", options.Count);
        }
        if (BetterPrefs.GetInt("ResolutionIndex", 1) < 0)
        {
            BetterPrefs.SetInt("ResolutionIndex", options.Count);
        }

        ResolutionDropdown.RefreshShownValue();
    }

    private void LoadSettings()
    {
        ResolutionDropdown.value = BetterPrefs.GetInt("ResolutionIndex", 0);
        WindowModeDropdown.value = BetterPrefs.GetInt("WindowMode", 1);
        GraphicsAPIDropdown.value = PlayerPrefs.GetInt("GraphicsAPI", 0);
        AADropdown.value = BetterPrefs.GetInt("GRAPHICS_AA", 1);
        VSyncToggle.isOn = BetterPrefs.GetBool("GRAPHICS_VSync", true);
        SetResolution(BetterPrefs.GetInt("ResolutionIndex", 1));
        ChangeWindowMode(BetterPrefs.GetInt("WindowMode", 1));
        ChangeAntiAliasing(BetterPrefs.GetInt("GRAPHICS_AA", 1));
        ChangeVSync(BetterPrefs.GetBool("GRAPHICS_VSync", true));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];

        BetterPrefs.SetInt("ResolutionIndex", resolutionIndex);

        switch (BetterPrefs.GetInt("WindowMode", 1))
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
        BetterPrefs.SetInt("WindowMode", val);

        SetResolution(BetterPrefs.GetInt("ResolutionIndex", 1));
    }

    public void ChangeVSync(bool val)
    {
        BetterPrefs.SetBool("GRAPHICS_VSync", val);

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
        BetterPrefs.SetInt("GRAPHICS_AA", val);

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

    public void ChangeGraphicsAPI(int val)
    {
        PlayerPrefs.SetInt("GraphicsAPI", val);

        GameRestartRequired.SetActive(true);
    }
}
#endif