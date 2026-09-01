using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Localization", menuName = "KillItMyself/Localization", order = 0)]
public class BeanLocalizationAsset : ScriptableObject
{
    public List<BeanLocalizationKey> Keys;
}