using UnityEngine;
using UnityEngine.InputSystem.UI;
#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine.XR.Interaction.Toolkit.UI;
#endif

public class UIInputModuleManager : MonoBehaviour
{
#if !CC2_REMOVE_VR_SUPPORT
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            GetComponent<XRUIInputModule>().enabled = true;
        }

        if (!VRManager.instance.VREnabled)
        {
            GetComponent<InputSystemUIInputModule>().enabled = true;
        }
    }
#else
    private void Start()
    {
        GetComponent<InputSystemUIInputModule>().enabled = true;
    }
#endif
}