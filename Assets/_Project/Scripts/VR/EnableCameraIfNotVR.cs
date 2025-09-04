using UnityEngine;

public class EnableCameraIfNotVR : MonoBehaviour
{
    private void Start()
    {
        if (!VRManager.instance.VREnabled)
        {
            GetComponent<Camera>().enabled = true;
        }
    }
}