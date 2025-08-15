using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

[CreateAssetMenu(fileName = "New AndroidConfig", menuName = "CC2/AndroidConfig", order = 0)]
public class AndroidConfigSO : ScriptableObject
{
    public AudioClip[] AudioClipsSwitchMono;
    public AddressableAssetGroup[] assetGroupsToDisable;
}