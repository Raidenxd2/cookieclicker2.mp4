using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLightmaps : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private AssetReferenceTexture2D WithResearchFactoryLightmap;
    private AsyncOperationHandle<Texture2D> rflHandle;

    public void InitAddressableLightmaps()
    {
        if (rflHandle.IsValid())
        {
            Addressables.Release(rflHandle);
        }

        if (game.ResearchFactory)
        {
            LoadResearchFactoryLightmap().Forget();
        }
    }

    private async UniTaskVoid LoadResearchFactoryLightmap()
    {
        rflHandle = Addressables.LoadAssetAsync<Texture2D>(WithResearchFactoryLightmap);
        await rflHandle;

        List<LightmapData> lightmapData = new List<LightmapData>();

        LightmapData lightmapData1 = new();
        lightmapData1.lightmapColor = rflHandle.Result;

        lightmapData.Add(lightmapData1);

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