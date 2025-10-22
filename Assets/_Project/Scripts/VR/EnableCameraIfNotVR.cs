using UnityEngine;

public class EnableCameraIfNotVR : MonoBehaviour
{
    private void Start()
    {
#if !CC2_REMOVE_VR_SUPPORT
        if (!VRManager.instance.VREnabled)
        {
            GetComponent<Camera>().enabled = true;
        }
#else
        GetComponent<Camera>().enabled = true;
#endif
    }
}