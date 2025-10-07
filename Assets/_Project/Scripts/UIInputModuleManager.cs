using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIInputModuleManager : MonoBehaviour
{
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
}