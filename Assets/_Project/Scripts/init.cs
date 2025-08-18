using Cysharp.Threading.Tasks;
using LoggerSystem;
using SimpleFileBrowser;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    public GameObject DDOL;

    public static bool HasLoaded;
    public static bool HasLoadedSharedData;

    [SerializeField] private ThemeSO DefaultTheme;

    private void Start()
    {
        if (!HasLoaded)
        {
            HasLoaded = true;
            DontDestroyOnLoad(DDOL);
        }

        Resources.UnloadUnusedAssets();

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.IsMobileVR)
        {
            UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
            urp.msaaSampleCount = 4;
        }
#endif

        if (!HasLoadedSharedData)
        {
            LogSystem.Log("Loading AssetBundle shareddata");
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Bundles/shareddata");
            HasLoadedSharedData = true;
        }

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
        Instantiate(await Resources.LoadAsync("InitScene_VRPrefab") as GameObject);
    }
#endif

    private async UniTaskVoid LoadGameSceneAsync()
    {
        if (!string.IsNullOrEmpty(Game.importPath))
        {
            await UniTask.WaitForEndOfFrame();
            File.Delete(Application.persistentDataPath + "/Saves/Default.cookie");
            File.WriteAllText(Application.persistentDataPath + "/Saves/Default.cookie", FileBrowserHelpers.ReadTextFromFile(Game.importPath));
            Game.importPath = null;
        }

        await SceneManager.LoadSceneAsync(AddressableHandles.gameSceneRef, LoadSceneMode.Additive);

        await UniTask.WaitForEndOfFrame();
        await ThemeManager.instance.SelectTheme(DefaultTheme.ThemeAssetBundleName, DefaultTheme.ThemeSceneName);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.gameSceneRef));

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            await Game.instance.InitVR();
        }
#endif

        await SceneManager.UnloadSceneAsync(AddressableHandles.initSceneRef, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        Game.instance.PlayInitialFadeOut();
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void SetHasLoaded()
    {
        HasLoaded = false;
        HasLoadedSharedData = false;
    }
#endif
}