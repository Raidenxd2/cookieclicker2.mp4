using Cysharp.Threading.Tasks;
using LoggerSystem;
using SimpleFileBrowser;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public GameObject DDOL;

    public static bool HasLoaded;
    public static bool HasLoadedSharedData;
    public static bool HasLoadedAndroidVRSVC;
    public static bool HasLoadedPCNonVRSVC;
    public static bool HasLoadedPCVRSVC;

    [SerializeField] private ThemeSO DefaultTheme;

    private void Start()
    {
        if (!HasLoaded)
        {
            HasLoaded = true;
            DontDestroyOnLoad(DDOL);
        }

        Resources.UnloadUnusedAssets();

        if (!HasLoadedSharedData)
        {
            LogSystem.Log("Loading AssetBundle shareddata");
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Bundles/shareddata");
            HasLoadedSharedData = true;
        }

        if (VRManager.instance.VREnabled)
        {
            LoadVRData().Forget();
        }

        LoadGameSceneAsync().Forget();
    }

    private async UniTask LoadVRData()
    {
        Instantiate(await Resources.LoadAsync("InitScene_VRPrefab") as GameObject);
    }

    private async UniTaskVoid LoadGameSceneAsync()
    {
        if (!string.IsNullOrEmpty(Game.importPath))
        {
            File.Delete(Application.persistentDataPath + "/Saves/Default.cookie");
            File.WriteAllText(Application.persistentDataPath + "/Saves/Default.cookie", FileBrowserHelpers.ReadTextFromFile(Game.importPath));
            Game.importPath = null;
        }

#if UNITY_ANDROID
        if (!HasLoadedAndroidVRSVC)
        {
            LogSystem.Log("Loading AndroidVRShaderVariants");
            AssetBundle avrsvc = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/svc-androidvr");

            ShaderVariantCollection svc = await avrsvc.LoadAssetAsync("AndroidVRShaderVariants") as ShaderVariantCollection;
            svc.WarmUp();

            await avrsvc.UnloadAsync(true);

            HasLoadedAndroidVRSVC = true;
        }
//#else
//        if (!HasLoadedPCNonVRSVC && !VRManager.instance.VREnabled)
//        {
//            LogSystem.Log("Loading PCNonVRShaderVariants");
//            AssetBundle avrsvc = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/svc-pcnonvr");

//            ShaderVariantCollection svc = await avrsvc.LoadAssetAsync("PCNonVRShaderVariants") as ShaderVariantCollection;
//            svc.WarmUp();

//            await avrsvc.UnloadAsync(true);

//            HasLoadedPCNonVRSVC = true;
//        }

//        if (!HasLoadedPCVRSVC && VRManager.instance.VREnabled)
//        {
//            LogSystem.Log("Loading PCVRShaderVariants");
//            AssetBundle avrsvc = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/svc-pcvr");

//            ShaderVariantCollection svc = await avrsvc.LoadAssetAsync("PCVRShaderVariants") as ShaderVariantCollection;
//            svc.WarmUp();

//            await avrsvc.UnloadAsync(true);

//            HasLoadedPCVRSVC = true;
//        }
#endif

        await SceneManager.LoadSceneAsync(AddressableHandles.gameSceneRef, LoadSceneMode.Additive);

        await UniTask.WaitForEndOfFrame();
        await ThemeManager.instance.SelectTheme(DefaultTheme.ThemeAssetBundleName, DefaultTheme.ThemeSceneName, DefaultTheme.ThemeSceneFullPath);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.gameSceneRef));

        if (VRManager.instance.VREnabled)
        {
            await Game.instance.InitVR();
        }

        await SceneManager.UnloadSceneAsync(AddressableHandles.initSceneRef, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        Game.instance.PlayInitialFadeOut();
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void SetHasLoaded()
    {
        HasLoaded = false;
        HasLoadedSharedData = false;
        HasLoadedAndroidVRSVC = false;
        HasLoadedPCNonVRSVC = false;
        HasLoadedPCVRSVC = false;
    }
#endif
}