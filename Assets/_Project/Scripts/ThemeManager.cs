using Cysharp.Threading.Tasks;
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

            tb.ThemeSceneName = theme.ThemeSceneName;

            tb.ThemeButtonText.text = theme.ThemeName;

            tb.button.onClick.AddListener(() => SelectTheme(theme.ThemeSceneName).Forget());
        }
    }
    
    public async UniTask SelectTheme(string SceneName)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        CurrentTheme = FallbackTheme;

        if (!string.IsNullOrEmpty(CurrentSceneName))
        {
            try
            {
                al.UnloadLightmaps();
                al.RemoveLightmaps();
                await SceneManager.UnloadSceneAsync(CurrentSceneName);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        try
        {
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
            al.UnloadLightmaps();
            al.RemoveLightmaps();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.gameSceneRef));
            await SceneManager.UnloadSceneAsync(CurrentSceneName);
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