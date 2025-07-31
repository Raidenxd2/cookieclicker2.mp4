using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLightmaps : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Texture2D GameLightmap;
    private AsyncOperationHandle<Texture2D> rflHandle;

    public void InitAddressableLightmaps()
    {
        if (ThemeManager.instance.CurrentTheme.ResearchFactoryLightmap == null)
        {
            return;
        }

        if (rflHandle.IsValid())
        {
            Addressables.Release(rflHandle);
        }

        if (game.ResearchFactory)
        {
            LoadResearchFactoryLightmap().Forget();
        }
    }

    public void RemoveLightmaps()
    {
        if (rflHandle.IsValid())
        {
            Addressables.Release(rflHandle);
        }

        List<LightmapData> lightmapData = new();

        LightmapData lightmapData1 = new();
        lightmapData1.lightmapColor = GameLightmap;

        lightmapData.Add(lightmapData1);

        LightmapSettings.lightmaps = lightmapData.ToArray();
    }

    private async UniTaskVoid LoadResearchFactoryLightmap()
    {
        rflHandle = Addressables.LoadAssetAsync<Texture2D>(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmap);
        await rflHandle;

        List<LightmapData> lightmapData = new();

        LightmapData lightmapData1 = new();
        lightmapData1.lightmapColor = GameLightmap;

        LightmapData lightmapData2 = new();
        lightmapData2.lightmapColor = rflHandle.Result;

        lightmapData.Add(lightmapData1);
        lightmapData.Add(lightmapData2);

        LightmapSettings.lightmaps = lightmapData.ToArray();
    }

    public void UnloadLightmaps()
    {
        if (rflHandle.IsValid())
        {
            Addressables.Release(rflHandle);
        }
    }
}