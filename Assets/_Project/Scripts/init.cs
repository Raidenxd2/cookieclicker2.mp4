using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    public GameObject DDOL;

    [SerializeField] private AssetReference VRPrefab;

    public static bool HasLoaded;

    private void Start()
    {
        if (!HasLoaded)
        {
            HasLoaded = true;
            DontDestroyOnLoad(DDOL);
        }

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.IsMobileVR)
        {
            UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
            urp.msaaSampleCount = 4;
        }
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LoadVRData();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
#endif

        LoadGameSceneAsync().Forget();
    }

#if !CC2_REMOVE_VR_SUPPORT
    private async UniTask LoadVRData()
    {
        Instantiate(await Addressables.LoadAssetAsync<GameObject>(VRPrefab));
    }
#endif

    private async UniTaskVoid LoadGameSceneAsync()
    {
        await Addressables.UnloadSceneAsync(AddressableHandles.instance.initSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.gameSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.gameSceneRef, LoadSceneMode.Single);
        await AddressableHandles.instance.gameSceneHandle;
    }
}