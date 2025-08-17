using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AddressableLightmaps : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Texture2D GameLightmap;
    private Texture2D rflTexture;

    public void InitAddressableLightmaps()
    {
        if (string.IsNullOrEmpty(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapLocation))
        {
            return;
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
        rflTexture = await Resources.LoadAsync(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapLocation) as Texture2D;

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
        if (rflTexture != null)
        {
            Resources.UnloadAsset(rflTexture);
        }
    }
}