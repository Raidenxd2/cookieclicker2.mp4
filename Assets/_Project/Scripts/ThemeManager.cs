using System;
using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

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

    private AsyncOperationHandle<SceneInstance> CurrentThemeHandle;
    public string CurrentSceneName;

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

            tb.ThemeButtonText.text = theme.ThemeName;

            tb.button.onClick.AddListener(() => SelectTheme(theme.ThemeRef, theme.ThemeSceneName).Forget());
        }
    }
    
    public async UniTask SelectTheme(AssetReference ThemeRef, string SceneName)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        CurrentTheme = FallbackTheme;

        if (!string.IsNullOrEmpty(CurrentSceneName))
        {
            try
            {
                BeanLogger.Log("Unloading Scene " + CurrentSceneName, this);
                
                al.UnloadLightmaps();
                al.RemoveLightmaps();
                await Addressables.UnloadSceneAsync(CurrentThemeHandle);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        try
        {
            BeanLogger.Log("Loading Scene " + SceneName, this);

            CurrentThemeHandle = Addressables.LoadSceneAsync(ThemeRef, LoadSceneMode.Additive);
            await CurrentThemeHandle;

            CurrentSceneName = SceneName;
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
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
            BeanLogger.Log("Unloading Scene " + CurrentSceneName, this);
            
            al.UnloadLightmaps();
            al.RemoveLightmaps();
            await Addressables.UnloadSceneAsync(CurrentThemeHandle);
        }
        catch (Exception ex)
        {
            BeanLogger.LogError("Failed to unload current theme.", this);
            Debug.LogException(ex);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}