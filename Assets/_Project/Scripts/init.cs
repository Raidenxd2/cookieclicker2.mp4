using Cysharp.Threading.Tasks;
using SimpleFileBrowser;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public GameObject DDOL;

    private static bool HasLoaded;
    private static bool HasLoadedSharedData;
    
    [SerializeField] private ThemeSO DefaultTheme;

    private void Start()
    {
        if (!HasLoaded)
        {
            HasLoaded = true;
            DontDestroyOnLoad(DDOL);
        }

        StartAsync().Forget();
    }

    private async UniTaskVoid StartAsync()
    {
        await Resources.UnloadUnusedAssets();

#if !UNITY_WEBGL
        if (!HasLoadedSharedData)
        {
            await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/shareddata.bundle");
            HasLoadedSharedData = true;
        }
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            Instantiate(await Resources.LoadAsync("InitScene_VRPrefab") as GameObject);
        }
#endif
        
        if (PlayerPrefs.GetInt("BeanLocalization_CurrentLanguage", 0) == 3)
        {
            await FontLoader.instance.LoadJapaneseFont();
        }

        await BeanLocalization.Init("MainLocalization");
        
        if (!string.IsNullOrEmpty(Game.importPath))
        {
            File.Delete(Application.persistentDataPath + "/Saves/Default.cookie");
            await File.WriteAllTextAsync(Application.persistentDataPath + "/Saves/Default.cookie", FileBrowserHelpers.ReadTextFromFile(Game.importPath));
            Game.importPath = null;
        }

#if UNITY_WEBGL
        if (!HasLoadedSharedData)
        {
            var sharedDataWWW = (await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/shareddata.bundle").SendWebRequest());
            DownloadHandlerAssetBundle.GetContent(sharedDataWWW);
            HasLoadedSharedData = true;
        }
#endif

        await SceneManager.LoadSceneAsync(SceneNames.gameSceneRef, LoadSceneMode.Additive);

        await UniTask.WaitForEndOfFrame();
        await ThemeManager.instance.SelectTheme(DefaultTheme.ThemeAssetBundleName, DefaultTheme.ThemeSceneName, DefaultTheme.ThemeSceneFullPath);

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            await Game.instance.InitVR();
        }
#endif

        await SceneManager.UnloadSceneAsync(SceneNames.initSceneRef, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

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