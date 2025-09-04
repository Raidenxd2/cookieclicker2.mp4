using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIInputModuleManager : MonoBehaviour
{
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            GetComponent<XRUIInputModule>().enabled = true;
        }
#if ENABLE_INPUT_SYSTEM

        if (!VRManager.instance.VREnabled)
        {
            GetComponent<InputSystemUIInputModule>().enabled = true;
        }
#endif
    }
}