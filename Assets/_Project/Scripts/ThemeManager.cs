using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private ThemeSO[] Themes;
    
    [SerializeField] private GameObject ThemeButton;
    [SerializeField] private Transform ThemeButtonParent;

    [SerializeField] private WindowAnimations ThemesScreen;
    [SerializeField] private WindowAnimations GlobalDark;

    [SerializeField] private GameObject ContentLoading;

    [SerializeField] private Notification notification;
    [SerializeField] private AddressableLightmaps al;

    public ThemeObject CurrentTheme;

    public ThemeObject FallbackTheme;

    private AssetBundle CurrentThemeBundle;
    private string CurrentSceneName;

#if UNITY_EDITOR
    [SerializeField] private bool LoadAssetBundlesInEditor;
#endif

    public static ThemeManager instance;

    private void Awake()
    {
        CurrentTheme = FallbackTheme;
        instance = this;
    }

    private void Start()
    {
        foreach (var theme in Themes)
        {
            GameObject go = Instantiate(ThemeButton, ThemeButtonParent);
            ThemeButton tb = go.GetComponent<ThemeButton>();

            tb.ThemeAssetBundleName = theme.ThemeAssetBundleName;
            tb.ThemeSceneName = theme.ThemeSceneName;

            tb.ThemeButtonText.text = theme.ThemeName;

            tb.button.onClick.AddListener(() => SelectTheme(theme.ThemeAssetBundleName, theme.ThemeSceneName, theme.ThemeSceneFullPath).Forget());
        }
    }
    
    public async UniTask SelectTheme(string AssetBundleName, string SceneName, string FullAssetPath)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        CurrentTheme = FallbackTheme;

        if (!string.IsNullOrEmpty(CurrentSceneName))
        {
            try
            {
#if UNITY_EDITOR
                if (LoadAssetBundlesInEditor)
                {
                    LogSystem.Log("Unloading Scene " + CurrentSceneName + " and bundle " + CurrentThemeBundle.name);
                }
                else
                {
#endif
                    LogSystem.Log("Unloading Scene " + CurrentSceneName);
#if UNITY_EDITOR
                }
#endif
                
                al.UnloadLightmaps();
                al.RemoveLightmaps();
                await SceneManager.UnloadSceneAsync(CurrentSceneName);

#if UNITY_EDITOR
                if (LoadAssetBundlesInEditor)
                {
#endif
                    await CurrentThemeBundle.UnloadAsync(true);
#if UNITY_EDITOR
                }
#endif
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        try
        {
            LogSystem.Log("Loading AssetBundle " + AssetBundleName + " and scene " + SceneName);

#if UNITY_EDITOR
            if (LoadAssetBundlesInEditor)
            {
#endif
                CurrentThemeBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + AssetBundleName);
                CurrentSceneName = SceneName;
                await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
#if UNITY_EDITOR
            }
            else
            {
                CurrentSceneName = SceneName;
                await EditorSceneManager.LoadSceneAsyncInPlayMode(FullAssetPath, new(LoadSceneMode.Additive));
            }
#endif
        }
        catch
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
            ContentLoading.SetActive(false);

            return;
        }

        CurrentTheme = GameObject.Find("ThemeScene").GetComponent<ThemeObject>();

        Game.instance.CheckResearchFactory();
        Game.instance.CheckDrill();

        al.InitAddressableLightmaps();

        ContentLoading.SetActive(false);
    }

    public async UniTask UnloadTheme()
    {
        try
        {
            LogSystem.Log("Unloading Scene " + CurrentSceneName + " and bundle " + CurrentThemeBundle.name);
            al.UnloadLightmaps();
            al.RemoveLightmaps();
            await SceneManager.UnloadSceneAsync(CurrentSceneName);

#if UNITY_EDITOR
            if (LoadAssetBundlesInEditor)
            {
#endif
                await CurrentThemeBundle.UnloadAsync(true);
#if UNITY_EDITOR
            }
#endif
        }
        catch
        {

        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}