using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

[CreateAssetMenu(fileName = "New WebGLConfig", menuName = "CC2/WebGLConfig", order = 0)]
public class WebGLConfigSO : ScriptableObject
{
    public AddressableAssetGroup[] assetGroupsToDisable;
}