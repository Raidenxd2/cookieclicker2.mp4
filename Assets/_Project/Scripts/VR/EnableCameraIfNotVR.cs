using UnityEngine;

public class EnableCameraIfNotVR : MonoBehaviour
{
    private void Start()
    {
#if !CC2_REMOVE_VR_SUPPORT
#if UNITY_EDITOR
        if (VRManager.instance.VREnabled && VRManager.instance.FakeVR)
        {
            GetComponent<Camera>().enabled = true;
            return;
        }
#endif
        if (!VRManager.instance.VREnabled)
        {
            GetComponent<Camera>().enabled = true;
        }
#else
        GetComponent<Camera>().enabled = true;
#endif
    }
}