using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.Rendering.Universal;

public class PlayerCamVR : MonoBehaviour
{
    public float sensY;

    public Transform Root;
    public Transform orientation;
    public Transform playerModel;
    
    private InputAction vr_rightPositionInputAction = new(binding: "<XRController>{RightHand}/{Primary2DAxis}", expectedControlType: "Vector2");

#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private float yRotation;

    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = Game.instance.ad.PostProcessing;
        
        vr_rightPositionInputAction.Enable();
    }

    private void LateUpdate()
    {
        Vector2 rotateDirection = vr_rightPositionInputAction.ReadValue<Vector2>();

        yRotation += rotateDirection.x * sensY * Time.fixedDeltaTime;

        //Rotate camera and orientation
        Root.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }
#endif
}