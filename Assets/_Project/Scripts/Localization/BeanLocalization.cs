using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class BeanLocalization
{
    private static bool IsMainAssetLoaded;
    private static BeanLocalizationAsset MainAsset;

    private static BeanLocalizationLanguage CurrentLanguage; 
    
    public static async UniTask Init(string BaseAddressableKey)
    {
        string AddressableKey = string.Empty;
        switch (PlayerPrefs.GetInt("BeanLocalization_CurrentLanguage"))
        {
            case 0:
                AddressableKey = BaseAddressableKey + "En";
                CurrentLanguage = BeanLocalizationLanguage.en;
                break;
            case 1:
                AddressableKey = BaseAddressableKey + "Es";
                CurrentLanguage = BeanLocalizationLanguage.es;
                break;
            case 2:
                AddressableKey = BaseAddressableKey + "Fr";
                CurrentLanguage = BeanLocalizationLanguage.fr;
                break;
            case 3:
                AddressableKey = BaseAddressableKey + "Ja";
                CurrentLanguage = BeanLocalizationLanguage.ja;
                break;
        }
        
        MainAsset = await Addressables.LoadAssetAsync<BeanLocalizationAsset>(AddressableKey);
        IsMainAssetLoaded = true;
    }

    public static string GetString(string KeyName)
    {
        foreach (var key in MainAsset.Keys)
        {
            if (string.Equals(key.Name, KeyName))
            {
                switch (CurrentLanguage)
                {
                    case BeanLocalizationLanguage.en:
                        return key.EN;
                    case BeanLocalizationLanguage.es:
                        return key.ES;
                    case BeanLocalizationLanguage.fr:
                        return key.FR;
                    case BeanLocalizationLanguage.ja:
                        return key.JA;
                }
            }
        }

        return KeyName;
    }
    
#if UNITY_EDITOR
    public static void LoadAsset()
    {
        if (!IsMainAssetLoaded)
        {
            MainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<BeanLocalizationAsset>("Assets/_Project/Localization/MainLocalizationEN.asset");
            IsMainAssetLoaded = true;
        }
    }
    
    [RuntimeInitializeOnLoadMethod]
    private static void ResetValues()
    {
        IsMainAssetLoaded = false;
        MainAsset = null;
    }
#endif
}

[Serializable]
public class BeanLocalizationKey
{
    public string Name;
    public string EN;
    public string ES;
    public string FR;
    public string JA;
}

public enum BeanLocalizationLanguage
{
    en,
    es,
    fr,
    ja,
}