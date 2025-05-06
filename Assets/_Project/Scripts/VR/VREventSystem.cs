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