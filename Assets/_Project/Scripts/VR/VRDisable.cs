using UnityEngine;

public class VRDisable : MonoBehaviour
{
#if !CC2_REMOVE_VR_SUPPORT
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            gameObject.SetActive(false);
        }
    }
#endif
}