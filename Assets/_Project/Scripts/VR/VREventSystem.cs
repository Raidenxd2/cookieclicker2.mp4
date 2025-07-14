#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class VREventSystem : MonoBehaviour
{
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            GetComponent<InputSystemUIInputModule>().enabled = false;
            GetComponent<XRUIInputModule>().enabled = true;
        }
    }
}
#endif