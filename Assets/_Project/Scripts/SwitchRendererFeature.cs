using UnityEngine;
using UnityEngine.Rendering.Universal;
 
public class SwitchRendererFeature : MonoBehaviour
{
    public ScriptableRendererFeature feature2;

    public void set(bool toggle)
    {
        feature2.SetActive(toggle);
    }
}