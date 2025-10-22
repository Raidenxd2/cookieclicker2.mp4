using UnityEngine;

public class VRFadeCanvas : MonoBehaviour
{
#if !CC2_REMOVE_VR_SUPPORT
    public void InitVR()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas.planeDistance = 0.1f;
    }
#endif
}