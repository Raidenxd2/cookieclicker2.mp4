using com.raiden.assetbundleassetreference.Runtime;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Networking;

public class FontLoader : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset sspNormal;
    [SerializeField] private TMP_FontAsset sspBold;

    [SerializeField] private AssetBundleAssetReference jpNormal;
    [SerializeField] private AssetBundleAssetReference jpBold;

    [SerializeField] private GameObject ContentLoading;

    public static FontLoader instance;
    public static bool HasLoadedJapaneseFont;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PreInitScene.SetJapaneseLanguage)
        {
#pragma warning disable CS4014
            LoadJapaneseFont();
#pragma warning restore CS4014
        }
    }

    // Loads the Japanese font assets and adds them to the ssp fallback tables
    public async UniTask LoadJapaneseFont()
    {
        if (HasLoadedJapaneseFont)
        {
            return;
        }

        ContentLoading.SetActive(true);

#if UNITY_WEBGL
        AssetBundle jpNormalBundle = DownloadHandlerAssetBundle.GetContent(await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/" + jpNormal.BundleName + ".bundle").SendWebRequest());
        AssetBundle jpBoldBundle = DownloadHandlerAssetBundle.GetContent(await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/" + jpBold.BundleName + ".bundle").SendWebRequest());
#else
        AssetBundle jpNormalBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + jpNormal.BundleName + ".bundle");
        AssetBundle jpBoldBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + jpBold.BundleName + ".bundle");
#endif

        TMP_FontAsset jpNormalFA = await jpNormalBundle.LoadAssetAsync(jpNormal.AssetName) as TMP_FontAsset;
        TMP_FontAsset jpBoldFA = await jpBoldBundle.LoadAssetAsync(jpBold.AssetName) as TMP_FontAsset;

        sspNormal.fallbackFontAssetTable.Add(jpNormalFA);
        sspBold.fallbackFontAssetTable.Add(jpBoldFA);

        await jpNormalBundle.UnloadAsync(false);
        await jpBoldBundle.UnloadAsync(false);

        HasLoadedJapaneseFont = true;
        PreInitScene.SetJapaneseLanguage = false;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];

        ContentLoading.SetActive(false);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}