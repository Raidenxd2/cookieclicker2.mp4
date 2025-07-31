using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    public GameObject DDOL;

    [SerializeField] private AssetReference VRPrefab;
    [SerializeField] private AssetReference DefaultThemeScene;

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
            LoadVRData().Forget();
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
        AddressableHandles.instance.gameSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.gameSceneRef, LoadSceneMode.Additive);
        await AddressableHandles.instance.gameSceneHandle;

        await UniTask.WaitForEndOfFrame();
        await ThemeManager.instance.SelectTheme(DefaultThemeScene);

        SceneManager.SetActiveScene(AddressableHandles.instance.gameSceneHandle.Result.Scene);

        await Addressables.UnloadSceneAsync(AddressableHandles.instance.initSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        Game.instance.PlayInitialFadeOut();
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void SetHasLoaded()
    {
        HasLoaded = false;
    }
#endif
}