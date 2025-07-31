using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.EventSystems;
#endif

#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine.XR.Interaction.Toolkit.UI;
#endif

public class UIInputModuleManager : MonoBehaviour
{
    private void Start()
    {
#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            GetComponent<XRUIInputModule>().enabled = true;
        }
#endif

#if ENABLE_INPUT_SYSTEM

#if !CC2_REMOVE_VR_SUPPORT
        if (!VRManager.instance.VREnabled)
        {
#endif
            GetComponent<InputSystemUIInputModule>().enabled = true;
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

#endif
        
#if ENABLE_LEGACY_INPUT_MANAGER
        GetComponent<StandaloneInputModule>().enabled = true;
#endif
    }
}