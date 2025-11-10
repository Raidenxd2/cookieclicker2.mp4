using UnityEngine;
#if UNITY_WEBGL
using UnityEngine.EventSystems;
#elif !UNITY_WEBGL
using UnityEngine.InputSystem.UI;
#endif
#if UNITY_WEBGL && !CC2_REMOVE_VR_SUPPORT 
using UnityEngine.InputSystem.UI;
#endif    
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
#elif !UNITY_WEBGL
    private void Start()
    {
        GetComponent<InputSystemUIInputModule>().enabled = true;
    }
#elif UNITY_WEBGL
    private void Start()
    {
        GetComponent<StandaloneInputModule>().enabled = true;
    }
#endif
}