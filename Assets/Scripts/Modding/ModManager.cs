using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using ModIO;

public class ModManager : MonoBehaviour
{

    public GameObject ShopButton;
    public GameObject StatsItem;
    public GameObject ThemeButton;
    public RectTransform ShopContent;
    public RectTransform StatsContent;
    public RectTransform ThemeContent;
    public ShopButtonMod shopButtonMod;
    public StatsItemMod statsItemMod;
    public ThemeItemMod themeItemMod;

    // Start is called before the first frame update
    void Start()
    {
        GetMods();
        if (!Directory.Exists(Application.persistentDataPath + "/localmod"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/localmod");
        }
        //LoadTestAssetBundle();
    }

    public void LoadTestAssetBundle()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundles/customtheme.assets");
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("---customTheme---.prefab");
        Instantiate(prefab);

        myLoadedAssetBundle.Unload(false);
    }

    public void GetMods()
    {
        SubscribedMod[] mods = ModIOUnity.GetSubscribedMods(out Result result);

        if (result.Succeeded())
        {
            foreach(var mod in mods)
            {
                if (mod.status == SubscribedModStatus.Installed)
                {
                    string dir = mod.directory;
                    Debug.Log(dir);
                    string path = dir + "/mod.json";
                    StreamReader reader = new StreamReader(path); 
                    var modJsonData = JsonConvert.DeserializeObject<ModJsonData>(reader.ReadToEnd());
                    if (modJsonData.mod_type == "Theme")
                    {
                        LoggerSystem.Logger.Log(modJsonData.theme_customsky_enabled, LoggerSystem.LogTypes.Normal);
                        LoggerSystem.Logger.Log(modJsonData.theme_customsky_name, LoggerSystem.LogTypes.Normal);
                        themeItemMod.AssetBundlePath = dir;
                        themeItemMod.ThemeItemName = modJsonData.theme_name;
                        themeItemMod.ThemeSkyboxName = modJsonData.theme_customsky_name;
                        themeItemMod.ThemeSkybox =  modJsonData.theme_customsky_enabled;
                        themeItemMod.android_support = modJsonData.android_support;
                        themeItemMod.windows_support = modJsonData.windows_support;
                        themeItemMod.mac_support = modJsonData.mac_support;
                        themeItemMod.linux_support = modJsonData.linux_support;
                        Instantiate(ThemeButton, ThemeContent);
                    }
                    else
                    {
                        shopButtonMod.ShopItemName = modJsonData.shop_item_name;
                        shopButtonMod.ShopItemPrice = modJsonData.shop_item_price;
                        shopButtonMod.ShopItemCPS = modJsonData.shop_item_cps;
                        shopButtonMod.ShopItemCPC = modJsonData.shop_item_cpc;
                        shopButtonMod.ShopItemOldPrice = modJsonData.shop_item_price;
                        statsItemMod.ShopItemName = modJsonData.shop_item_name;
                        statsItemMod.ShopItemAmount = 0;
                        Instantiate(ShopButton, ShopContent);
                        Instantiate(StatsItem, StatsContent);
                    }
                    
                    reader.Close();
                }
            }
        }
        if (File.Exists(Application.persistentDataPath + "/localmod/mod.json"))
        {
            string dir = Application.persistentDataPath + "/localmod";
            Debug.Log(dir);
            string path = dir + "/mod.json";
            StreamReader reader = new StreamReader(path); 
            var modJsonData = JsonConvert.DeserializeObject<ModJsonData>(reader.ReadToEnd());
            if (modJsonData.mod_type == "Theme")
            {
                LoggerSystem.Logger.Log(modJsonData.theme_customsky_enabled, LoggerSystem.LogTypes.Normal);
                LoggerSystem.Logger.Log(modJsonData.theme_customsky_name, LoggerSystem.LogTypes.Normal);
                themeItemMod.AssetBundlePath = dir;
                themeItemMod.ThemeItemName = modJsonData.theme_name;
                themeItemMod.ThemeSkyboxName = modJsonData.theme_customsky_name;
                themeItemMod.ThemeSkybox = modJsonData.theme_customsky_enabled;
                themeItemMod.android_support = modJsonData.android_support;
                themeItemMod.windows_support = modJsonData.windows_support;
                themeItemMod.mac_support = modJsonData.mac_support;
                themeItemMod.linux_support = modJsonData.linux_support;
                Instantiate(ThemeButton, ThemeContent);
            }
            else
            {
                shopButtonMod.ShopItemName = modJsonData.shop_item_name;
                shopButtonMod.ShopItemPrice = modJsonData.shop_item_price;
                shopButtonMod.ShopItemCPS = modJsonData.shop_item_cps;
                shopButtonMod.ShopItemCPC = modJsonData.shop_item_cpc;
                shopButtonMod.ShopItemOldPrice = modJsonData.shop_item_price;
                statsItemMod.ShopItemName = modJsonData.shop_item_name;
                statsItemMod.ShopItemAmount = 0;
                Instantiate(ShopButton, ShopContent);
                Instantiate(StatsItem, StatsContent);
            }

            reader.Close();
        }
    }

    public void Open()
    {
        ModIOBrowser.Browser.OpenBrowser(null);
    }
}