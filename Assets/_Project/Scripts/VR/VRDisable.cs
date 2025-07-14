#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;

public class VRDisable : MonoBehaviour
{
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            gameObject.SetActive(false);
        }
    }
}
#endif