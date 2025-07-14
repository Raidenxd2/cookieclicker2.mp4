#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;

public class XROriginOnDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        Game.instance.LoadVRFallbackScene();
    }
}
#endif