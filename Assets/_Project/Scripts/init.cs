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
    }
#endif
}