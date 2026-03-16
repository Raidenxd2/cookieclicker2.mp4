using Cysharp.Threading.Tasks;
using LoggerSystem;
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
    
    [SerializeField] private GameObject AndroidWarningScreen;

    [SerializeField] private ThemeSO DefaultTheme;

    private void Start()
    {
        if (!HasLoaded)
        {
            HasLoaded = true;
            DontDestroyOnLoad(DDOL);
        }

#if UNITY_ANDROID
        if (PlayerPrefs.GetInt("GoogleAndroidWarningShown", 0) == 0 && !VRManager.instance.VREnabled)
        {
            PlayerPrefs.SetInt("GoogleAndroidWarningShown", 1);
            AndroidWarningScreen.SetActive(true);
            
            return;
        }
#endif
        
        ContinueLoad();
    }

    public void ContinueLoad()
    {
        Resources.UnloadUnusedAssets();

#if !UNITY_WEBGL
        if (!HasLoadedSharedData)
        {
            LogSystem.Log("Loading AssetBundle shareddata");
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Bundles/shareddata.bundle");
            HasLoadedSharedData = true;
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
        Instantiate(await Resources.LoadAsync("InitScene_VRPrefab") as GameObject);
    }
#endif

    private async UniTaskVoid LoadGameSceneAsync()
    {
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

        await SceneManager.LoadSceneAsync(AddressableHandles.gameSceneRef, LoadSceneMode.Additive);

        await UniTask.WaitForEndOfFrame();
        await ThemeManager.instance.SelectTheme(DefaultTheme.ThemeAssetBundleName, DefaultTheme.ThemeSceneName, DefaultTheme.ThemeSceneFullPath);

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