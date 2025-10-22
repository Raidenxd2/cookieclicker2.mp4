using Cysharp.Threading.Tasks;
using LoggerSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AddressableLightmaps : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Texture2D GameLightmap;
    private Texture2D rflTexture;

    private AssetBundle lightmapBundle;

    [SerializeField] private bool LoadAssetBundlesInEditor;

    public void InitAddressableLightmaps()
    {
        if (string.IsNullOrEmpty(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapABName))
        {
            return;
        }

        if (lightmapBundle != null)
        {
            UnloadLightmaps();
        }

        if (game.ResearchFactory)
        {
            LoadResearchFactoryLightmap().Forget();
        }
    }

    public void RemoveLightmaps()
    {
        List<LightmapData> lightmapData = new();

        LightmapData lightmapData1 = new();
        lightmapData1.lightmapColor = GameLightmap;

        lightmapData.Add(lightmapData1);

        LightmapSettings.lightmaps = lightmapData.ToArray();
    }

    private async UniTaskVoid LoadResearchFactoryLightmap()
    {
        LogSystem.Log("Loading AssetBundle " + ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapABName + " and asset " + ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapAssetName);
#if UNITY_WEBGL
        lightmapBundle = DownloadHandlerAssetBundle.GetContent(await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/" + ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapABName + ".bundle").SendWebRequest());
#else
        lightmapBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapABName + ".bundle");
#endif
        rflTexture = await lightmapBundle.LoadAssetAsync(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapAssetName) as Texture2D;

        List<LightmapData> lightmapData = new();

        LightmapData lightmapData1 = new();
        lightmapData1.lightmapColor = GameLightmap;

        LightmapData lightmapData2 = new();
        lightmapData2.lightmapColor = rflTexture;

        lightmapData.Add(lightmapData1);
        lightmapData.Add(lightmapData2);

        LightmapSettings.lightmaps = lightmapData.ToArray();
    }

    public void UnloadLightmaps()
    {
        if (lightmapBundle != null)
        {
            LogSystem.Log("Unloading AssetBundle " + lightmapBundle.name);

            rflTexture = null;
            lightmapBundle.Unload(true);
            lightmapBundle = null;
        }
    }
}