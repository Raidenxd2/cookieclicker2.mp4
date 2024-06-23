using System.Collections;
using LoggerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Android;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private ThemeSO[] Themes;
    
    [SerializeField] private GameObject ThemeButton;
    [SerializeField] private Transform ThemeButtonParent;

    [SerializeField] private WindowAnimations ThemesScreen;
    [SerializeField] private WindowAnimations GlobalDark;

    [SerializeField] private GameObject ContentLoading;

    [SerializeField] private GameObject AssetPackDownloadRequiredScreen;
    [SerializeField] private TMP_Text AssetPackSizeText;

    [SerializeField] private GameObject AssetPackDownloadingScreen;
    [SerializeField] private TMP_Text AssetPackProgressText;

    [SerializeField] private GameObject NoInternetScreen;

    [SerializeField] private GameObject DefaultTheme;

    [SerializeField] private Notification notification;

    private string CurrentAssetPackName;
    private GameObject CurrentThemePrefab;
    private AssetReference CurrentThemePrefabRef;
    private AsyncOperationHandle<GameObject> CurrentThemePrefabHandle;

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

        if (CurrentThemePrefab != null)
        {
            try
            {
                Destroy(CurrentThemePrefab);
                Addressables.Release(CurrentThemePrefabHandle);
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

        #if UNITY_ANDROID && !UNITY_EDITOR

        GetAssetPackStateAsyncOperation aps = AndroidAssetPacks.GetAssetPackStateAsync(new string[] {AssetPackName});
        yield return aps;

        CurrentAssetPackName = AssetPackName;
        CurrentThemePrefabRef = ThemePrefabRef;

        if (aps.states[0].status == AndroidAssetPackStatus.Completed)
        {
            LogSystem.Log("AssetPack " + AssetPackName + " should be downloaded.");

            DefaultTheme.SetActive(false);

            CurrentThemePrefabHandle = Addressables.LoadAssetAsync<GameObject>(ThemePrefabRef);
            yield return CurrentThemePrefabHandle;

            if (CurrentThemePrefabHandle.Status == AsyncOperationStatus.Succeeded)
            {
                CurrentThemePrefab = Instantiate(CurrentThemePrefabHandle.Result);
            }
            else
            {
                notification.ShowNotification("Failed to load theme.", "Themes");
                DefaultTheme.SetActive(true);
            }

            ContentLoading.SetActive(false);
        }
        else if (aps.states[0].status == AndroidAssetPackStatus.Unknown)
        {
            LogSystem.Log("AssetPack " + AssetPackName + " isn't available for this application.")

            notification.ShowNotification("Failed to check Asset Pack status because the app wasn't installed through Google Play.");
        }
        else if (aps.states[0].status == AndroidAssetPackStatus.NotInstalled)
        {
            LogSystem.Log("AssetPack " + AssetPackName + " isn't downloaded.");

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                LogSystem.Log("Can't download AssetPack " + AssetPackName + " because the user doesn't have internet.", LogTypes.Error);

                ContentLoading.SetActive(false);
                GlobalDark.gameObject.SetActive(true);
                NoInternetScreen.SetActive(true);

                yield break;
            }

            ContentLoading.SetActive(false);

            GlobalDark.gameObject.SetActive(true);
            AssetPackDownloadRequiredScreen.SetActive(true);
            AssetPackSizeText.text = aps.size.ToString();
        }
        #else
        LogSystem.Log("Non Android platform, loading Theme.");

        DefaultTheme.SetActive(false);
        CurrentThemePrefabHandle = Addressables.LoadAssetAsync<GameObject>(ThemePrefabRef);
        yield return CurrentThemePrefabHandle;

        if (CurrentThemePrefabHandle.Status == AsyncOperationStatus.Succeeded)
        {
            CurrentThemePrefab = Instantiate(CurrentThemePrefabHandle.Result);
        }
        else
        {
            notification.ShowNotification("Failed to load theme.", "Themes");
            DefaultTheme.SetActive(true);
        }

        ContentLoading.SetActive(false);
        #endif
    }

    public void DownloadAssetPack()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            LogSystem.Log("Can't download AssetPack " + CurrentAssetPackName + " because the user doesn't have internet.", LogTypes.Error);

            NoInternetScreen.SetActive(true);
            return;
        }

        AssetPackDownloadingScreen.SetActive(true);
        
        StartCoroutine(DownloadAssetPackAsync());
    }

    private IEnumerator DownloadAssetPackAsync()
    {
        DownloadAssetPackAsyncOperation dap = AndroidAssetPacks.DownloadAssetPackAsync(new[] {CurrentAssetPackName});

        while (!dap.isDone)
        {
            AssetPackProgressText.text = dap.progress * 100f + "%";
            yield return null;
        }

        if (dap.downloadFailedAssetPacks.Length > 0)
        {
            AssetPackDownloadingScreen.GetComponent<WindowAnimations>().HideWindow();

            notification.ShowNotification("Failed to download Asset Pack", "Themes");
            yield break;
        }

        AssetPackDownloadingScreen.GetComponent<WindowAnimations>().HideWindow();
        AssetPackDownloadRequiredScreen.GetComponent<WindowAnimations>().HideWindow();
        StartCoroutine(SelectTheme(CurrentAssetPackName, CurrentThemePrefabRef));
    }

    public void CancelAssetPackDownload()
    {
        LogSystem.Log("Attempting to cancel AssetPack download.");
        AndroidAssetPacks.CancelAssetPackDownload(new[] {CurrentAssetPackName});
    }
}