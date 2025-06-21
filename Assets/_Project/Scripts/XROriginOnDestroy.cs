using UnityEngine;

public class XROriginOnDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        Game.instance.LoadVRFallbackScene();
    }
}