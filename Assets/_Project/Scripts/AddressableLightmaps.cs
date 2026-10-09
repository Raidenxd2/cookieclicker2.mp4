using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLightmaps : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Texture2D GameLightmap;
    private Texture2D rflTexture;

    private AsyncOperationHandle<Texture2D> lightmapHandle;

    [SerializeField] private bool LoadAssetBundlesInEditor;

    public void InitAddressableLightmaps()
    {
        if (ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapRef == null)
        {
            return;
        }

        if (lightmapHandle.IsValid())
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
        BeanLogger.Log("Loading research factory lightmap", this);
        lightmapHandle = Addressables.LoadAssetAsync<Texture2D>(ThemeManager.instance.CurrentTheme.ResearchFactoryLightmapRef);
        await lightmapHandle;

        rflTexture = lightmapHandle.Result;

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
        if (lightmapHandle.IsValid())
        {
            BeanLogger.Log("Unloading research factory lightmap", this);

            rflTexture = null;
            Addressables.Release(lightmapHandle);
        }
    }
}