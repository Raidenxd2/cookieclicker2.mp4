using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
 
public class SwitchRendererFeature : MonoBehaviour
{
    public ScriptableRendererFeature feature;
    public ScriptableRendererFeature feature2;

    public void set(bool toggle)
    {
        feature.SetActive(toggle);
        feature2.SetActive(toggle);
    }
}
 