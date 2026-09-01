using raiden.utils;
using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FontLoader : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset sspNormal;
    [SerializeField] private TMP_FontAsset sspBold;

    [SerializeField] private AssetBundleAssetReference jpNormal;
    [SerializeField] private AssetBundleAssetReference jpBold;

    public static FontLoader instance;
    public static bool HasLoadedJapaneseFont;

    private void Awake()
    {
        instance = this;
    }

    // Loads the Japanese font assets and adds them to the ssp fallback tables
    public async UniTask LoadJapaneseFont()
    {
        if (HasLoadedJapaneseFont)
        {
            return;
        }
        
        BeanLogger.Log("Loading Japanese font", this);

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

        PlayerPrefs.SetInt("BeanLocalization_CurrentLanguage", 3);
    }

    private void OnDestroy()
    {
        instance = null;
    }
    
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    public static void ResetValues()
    {
        HasLoadedJapaneseFont = false;
    }
#endif
}