using System.Collections;
using System.Collections.Generic;
using LoggerSystem;
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
            StartCoroutine(LoadResearchFactoryLightmap());
        }
    }

    private IEnumerator LoadResearchFactoryLightmap()
    {
        rflHandle = Addressables.LoadAssetAsync<Texture2D>(WithResearchFactoryLightmap);
        yield return rflHandle;

        if (rflHandle.Status == AsyncOperationStatus.Succeeded)
        {
            List<LightmapData> lightmapData = new List<LightmapData>();

            LightmapData lightmapData1 = new();
            lightmapData1.lightmapColor = rflHandle.Result;

            lightmapData.Add(lightmapData1);

            LightmapSettings.lightmaps = lightmapData.ToArray();
        }
        else if (rflHandle.Status == AsyncOperationStatus.Failed)
        {
            LogSystem.Log("Failed to load lightmap.", LogTypes.Error);
        }

        yield return null;
    }

    public void UnloadLightmaps()
    {
        if (rflHandle.IsValid())
        {
            Addressables.Release(rflHandle);
        }
    }
}