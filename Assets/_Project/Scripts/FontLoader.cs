using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FontLoader : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset sspNormal;
    [SerializeField] private TMP_FontAsset sspBold;

    [SerializeField] private AssetReference jpNormalRef;
    [SerializeField] private AssetReference jpBoldRef;

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

        TMP_FontAsset jpNormalFA = await Addressables.LoadAssetAsync<TMP_FontAsset>(jpNormalRef);
        TMP_FontAsset jpBoldFA = await Addressables.LoadAssetAsync<TMP_FontAsset>(jpBoldRef);

        sspNormal.fallbackFontAssetTable.Add(jpNormalFA);
        sspBold.fallbackFontAssetTable.Add(jpBoldFA);

        HasLoadedJapaneseFont = true;

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