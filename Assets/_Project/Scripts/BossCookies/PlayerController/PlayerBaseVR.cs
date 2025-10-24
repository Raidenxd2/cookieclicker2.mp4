#if !CC2_REMOVE_VR_SUPPORT && UNITY_EDITOR
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseVR : MonoBehaviour
{
    [SerializeField] private Transform Hammer;
    [SerializeField] private Transform HammerNewParent;

    [SerializeField] private GameObject OldCamera;
    [SerializeField] private GameObject XRRoot;

    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            XRRoot.SetActive(true);
            Hammer.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Hammer.localScale = new(0.02f, 0.02f, 0.02f);
            Hammer.parent = HammerNewParent;
            OldCamera.SetActive(false);

            playerInput.defaultControlScheme = "XR";
            playerInput.enabled = false;
            playerInput.enabled = true;
            playerInput.neverAutoSwitchControlSchemes = true;
        }
    }
}
#endif