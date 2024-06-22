using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Theme", menuName = "Cookieclicker2.mp4/Theme", order = 0)]
public class ThemeSO : ScriptableObject
{
    public string ThemeName;
    public string AssetPackName;

    public AssetReference ThemePrefabRef;
}