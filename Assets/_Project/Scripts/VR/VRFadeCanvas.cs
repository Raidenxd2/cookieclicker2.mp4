using UnityEngine;

public class VRFadeCanvas : MonoBehaviour
{
    public void InitVR()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas.planeDistance = 0.1f;
    }
}