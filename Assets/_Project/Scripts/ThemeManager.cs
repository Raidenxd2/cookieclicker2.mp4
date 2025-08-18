using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            tb.button.onClick.AddListener(() => SelectTheme(theme.ThemeAssetBundleName, theme.ThemeSceneName).Forget());
        }
    }
    
    public async UniTask SelectTheme(string AssetBundleName, string SceneName)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        CurrentTheme = FallbackTheme;

        if (!string.IsNullOrEmpty(CurrentSceneName))
        {
            try
            {
                LogSystem.Log("Unloading Scene " + CurrentSceneName + " and bundle " + CurrentThemeBundle.name);
                al.UnloadLightmaps();
                al.RemoveLightmaps();
                await SceneManager.UnloadSceneAsync(CurrentSceneName);
                await CurrentThemeBundle.UnloadAsync(true);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        try
        {
            LogSystem.Log("Loading AssetBundle " + AssetBundleName + " and scene " + SceneName);
            CurrentThemeBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + AssetBundleName);
            string[] scenes = CurrentThemeBundle.GetAllScenePaths();

            CurrentSceneName = SceneName;
            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
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
            await CurrentThemeBundle.UnloadAsync(true);
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