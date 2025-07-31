using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    private AsyncOperationHandle<SceneInstance> CurrentThemeSceneHandle;

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

            tb.ThemePrefabRef = theme.ThemePrefabRef;

            tb.ThemeButtonText.text = theme.ThemeName;

            tb.button.onClick.AddListener(() => SelectTheme(theme.ThemePrefabRef).Forget());
        }
    }
    
    public async UniTask SelectTheme(AssetReference ThemePrefabRef)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        CurrentTheme = FallbackTheme;

        if (CurrentThemeSceneHandle.IsValid())
        {
            try
            {
                al.UnloadLightmaps();
                al.RemoveLightmaps();
                await Addressables.UnloadSceneAsync(CurrentThemeSceneHandle);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        try
        {
            CurrentThemeSceneHandle = Addressables.LoadSceneAsync(ThemePrefabRef, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            await CurrentThemeSceneHandle;
        }
        catch
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
            ContentLoading.SetActive(false);
        }

        if (CurrentThemeSceneHandle.Status == AsyncOperationStatus.Failed)
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
        }
        else
        {
            CurrentTheme = GameObject.Find("ThemeScene").GetComponent<ThemeObject>();

            Game.instance.CheckResearchFactory();
            Game.instance.CheckDrill();

            al.InitAddressableLightmaps();
        }

        ContentLoading.SetActive(false);
    }

    public async UniTask UnloadTheme()
    {
        try
        {
            al.UnloadLightmaps();
            al.RemoveLightmaps();
            await Addressables.UnloadSceneAsync(CurrentThemeSceneHandle);
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