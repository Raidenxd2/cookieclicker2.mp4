using UnityEngine;
using TMPro;
using LoggerSystem;

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
            LogSystem.Log("Android Theme");
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Android.assets");
        }

        if (Application.platform == RuntimePlatform.WindowsPlayer && windows_support == "true")
        {
            LogSystem.Log("Windows Theme");
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Windows.assets");
        }

        if (Application.platform == RuntimePlatform.WindowsEditor && windows_support == "true")
        {
            LogSystem.Log("Windows Theme");
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Windows.assets");
        }

        if (Application.platform == RuntimePlatform.LinuxPlayer && linux_support == "true")
        {
            LogSystem.Log("Linux Theme");
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Linux.assets");
        }

        if (Application.platform == RuntimePlatform.OSXPlayer && mac_support == "true")
        {
            LogSystem.Log("OSX Theme");
            myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customTheme_Mac.assets");
        }
        
        if (myLoadedAssetBundle == null)
        {
            LogSystem.Log("Failed to load AssetBundle!", LogTypes.Error);
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("---customTheme---.prefab");
        Instantiate(prefab);
        LogSystem.Log(ThemeSkybox);
        LogSystem.Log(ThemeSkyboxName);
        if (ThemeSkybox == "true" || ThemeSkybox == "True")
        {
            var customSkybox = myLoadedAssetBundle.LoadAsset<Material>(ThemeSkyboxName);
            RenderSettings.skybox = customSkybox;
        }

        myLoadedAssetBundle.Unload(false);
    }
}