#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseVR : MonoBehaviour
{
    [SerializeField] private Transform Hammer;
    [SerializeField] private Transform Hammer2;
    [SerializeField] private Transform HammerNewParent;

    [SerializeField] private GameObject OldCamera;
    [SerializeField] private GameObject XRRoot;

    [SerializeField] private PlayerInput playerInput;

    private void Awake()
    {
        if (VRManager.instance.VREnabled)
        {
            XRRoot.SetActive(true);
            Hammer.parent = HammerNewParent;
            Hammer2.parent = HammerNewParent;
            Hammer.position = Vector3.zero;
            Hammer2.position = Vector3.zero;
            Hammer.localPosition = new(-0.0066f, 0.6149f, -0.0724f);
            Hammer.localScale = new(0.2f, 0.25f, 0.2f);
            Hammer2.localPosition = new(-0.0066f, 0.6149f, -0.0724f);
            Hammer2.localScale = new(0.1f, 0.1f, 0.1f);
            OldCamera.SetActive(false);
        }
        else
        {
            playerInput.enabled = true;
        }
    }
}
#endif