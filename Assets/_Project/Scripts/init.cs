using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    public GameObject DDOL;

    private static bool HasLoaded;
    
    [SerializeField] private ThemeSO DefaultTheme;

    [SerializeField] private AssetReference InitVRPrefabRef;
    private AsyncOperationHandle<GameObject> InitVRPrefabHandle;
    
    [SerializeField] private AssetReference GameSceneRef;

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

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            InitVRPrefabHandle = Addressables.LoadAssetAsync<GameObject>(InitVRPrefabRef);
            await InitVRPrefabHandle;
            
            Instantiate(InitVRPrefabHandle.Result);
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
            await File.WriteAllBytesAsync(Application.persistentDataPath + "/Saves/Default.cookie", await File.ReadAllBytesAsync(Game.importPath));
            Game.importPath = null;
        }

        AddressableHandles.GameHandle = Addressables.LoadSceneAsync(GameSceneRef, LoadSceneMode.Additive);
        await AddressableHandles.GameHandle;

        await ThemeManager.instance.SelectTheme(DefaultTheme.ThemeRef, DefaultTheme.ThemeSceneName);

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            await Game.instance.InitVR();
            
            Addressables.Release(InitVRPrefabHandle);
        }
#endif

        await Addressables.UnloadSceneAsync(AddressableHandles.InitHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

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