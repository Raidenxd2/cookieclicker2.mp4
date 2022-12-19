using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ThemeItemMod : MonoBehaviour
{

    public string ThemeItemName;
    public string AssetBundlePath;
    public TMP_Text ItemText;

    void OnEnable()
    {
        ItemText.text = ThemeItemName + " (custom)";
    }

    public void LoadTheme()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(AssetBundlePath + "/customtheme.assets");
        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("---customTheme---.prefab");
        Instantiate(prefab);

        myLoadedAssetBundle.Unload(false);
    }
}
