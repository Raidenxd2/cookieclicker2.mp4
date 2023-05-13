using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ThemeItemMod : MonoBehaviour
{

    public string ThemeItemName;
    public string AssetBundlePath;
    public string ThemeSkybox;
    public string ThemeSkyboxName;
    public TMP_Text ItemText;
    public string android_support;
    public string windows_support;
    public string mac_support;
    public string linux_support;
    public GameObject PlatformNotSupportedScreen;
    public AssetBundle myLoadedAssetBundle;

    void OnEnable()
    {
        ItemText.text = ThemeItemName + " (custom)";
    }

    public void LoadTheme()
    {
        if (Application.platform == RuntimePlatform.Android && android_support != "true" || Application.platform == RuntimePlatform.WindowsPlayer && windows_support != "true" || Application.platform == RuntimePlatform.WindowsEditor && windows_support != "true" || Application.platform == RuntimePlatform.OSXPlayer && mac_support != "true" || Application.platform == RuntimePlatform.LinuxPlayer && linux_support != "true")
        {
            PlatformNotSupportedScreen.SetActive(true);
            return;
        }

        if (Application.platform == RuntimePlatform.Android && android_support == "true")
        {
            LoggerSystem.Logger.Log("Android Theme", LoggerSystem.LogTypes.Normal);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Android.assets");
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer && windows_support == "true")
        {
            LoggerSystem.Logger.Log("Windows Theme", LoggerSystem.LogTypes.Normal);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Windows.assets");
        }

        if (Application.platform == RuntimePlatform.WindowsEditor && windows_support == "true")
        {
            LoggerSystem.Logger.Log("Windows Theme", LoggerSystem.LogTypes.Normal);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Windows.assets");
        }

        if (Application.platform == RuntimePlatform.LinuxPlayer && linux_support == "true")
        {
            LoggerSystem.Logger.Log("Linux Theme", LoggerSystem.LogTypes.Normal);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Linux.assets");
        }

        if (Application.platform == RuntimePlatform.OSXPlayer && mac_support == "true")
        {
            LoggerSystem.Logger.Log("OSX Theme", LoggerSystem.LogTypes.Normal);
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Mac.assets");
        }
        
        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("---customTheme---.prefab");
        Instantiate(prefab);
        LoggerSystem.Logger.Log(ThemeSkybox, LoggerSystem.LogTypes.Normal);
        LoggerSystem.Logger.Log(ThemeSkyboxName, LoggerSystem.LogTypes.Normal);
        if (ThemeSkybox == "true" || ThemeSkybox == "True")
        {
            var customSkybox = myLoadedAssetBundle.LoadAsset<Material>(ThemeSkyboxName);
            RenderSettings.skybox = customSkybox;
            LoggerSystem.Logger.Log("should have set custom skybox", LoggerSystem.LogTypes.Normal);
        }

        myLoadedAssetBundle.Unload(false);
    }
}
