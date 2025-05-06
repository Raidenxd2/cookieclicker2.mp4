using UnityEngine;

public class VRFadeCanvas : MonoBehaviour
{
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            canvas.planeDistance = 0.1f;
        }
    }
}