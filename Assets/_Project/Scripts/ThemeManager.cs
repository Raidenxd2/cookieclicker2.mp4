using Cysharp.Threading.Tasks;
using LoggerSystem;
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

    [SerializeField] private GameObject DefaultTheme;

    [SerializeField] private Notification notification;

    private AsyncOperationHandle<SceneInstance> CurrentThemeSceneHandle;

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
    
    public async UniTaskVoid SelectTheme(AssetReference ThemePrefabRef)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        if (CurrentThemeSceneHandle.IsValid())
        {
            try
            {
                await Addressables.UnloadSceneAsync(CurrentThemeSceneHandle);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        if (!ThemePrefabRef.RuntimeKeyIsValid())
        {
            LogSystem.Log("Default theme selected.");

            DefaultTheme.SetActive(true);
            ContentLoading.SetActive(false);
            return;
        }

        DefaultTheme.SetActive(false);
        CurrentThemeSceneHandle = Addressables.LoadSceneAsync(ThemePrefabRef, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        await CurrentThemeSceneHandle;

        if (CurrentThemeSceneHandle.Status == AsyncOperationStatus.Failed)
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
            DefaultTheme.SetActive(true);
        }

        ContentLoading.SetActive(false);
    }
}