using System.Collections;
using LoggerSystem;
using TMPro;
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

    private AssetReference CurrentThemeSceneRef;
    private AsyncOperationHandle<SceneInstance> CurrentThemeSceneHandle;

    private void Start()
    {
        foreach (var theme in Themes)
        {
            GameObject go = Instantiate(ThemeButton, ThemeButtonParent);
            ThemeButton tb = go.GetComponent<ThemeButton>();

            tb.AssetPackName = theme.AssetPackName;
            tb.ThemePrefabRef = theme.ThemePrefabRef;

            tb.ThemeButtonText.text = theme.ThemeName;

            tb.button.onClick.AddListener(() => StartCoroutine(SelectTheme(theme.AssetPackName, theme.ThemePrefabRef)));
        }
    }
    
    public IEnumerator SelectTheme(string AssetPackName, AssetReference ThemePrefabRef)
    {
        ThemesScreen.HideWindow();
        GlobalDark.HideWindow();

        ContentLoading.SetActive(true);

        if (CurrentThemeSceneHandle.IsValid())
        {
            try
            {
                Addressables.UnloadSceneAsync(CurrentThemeSceneHandle);
            }
            catch
            {
                notification.ShowNotification("Failed to unload previous theme.", "Themes");
            }
        }

        if (AssetPackName == "default")
        {
            LogSystem.Log("Default theme selected.");

            DefaultTheme.SetActive(true);
            ContentLoading.SetActive(false);
            yield break;
        }

        DefaultTheme.SetActive(false);
        CurrentThemeSceneHandle = Addressables.LoadSceneAsync(ThemePrefabRef, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        yield return CurrentThemeSceneHandle;

        if (CurrentThemeSceneHandle.Status == AsyncOperationStatus.Failed)
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
            DefaultTheme.SetActive(true);
        }

        ContentLoading.SetActive(false);
    }
}