using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(UniversalAdditionalCameraData))]
public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerModel;

    public bool canMoveCamera = true;
#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private float xRotation;
    private float yRotation;
#endif

    [SerializeField] private PlayerInput playerControls;

#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = Game.instance.ad.PostProcessing;

        if (playerControls.devices[0].displayName.Contains("Keyboard") || playerControls.devices[0].displayName.Contains("Mouse"))
        {
            sensX = 5 * BetterPrefs.GetInt("KeyboardMouseSettings_MouseSensitivity", 1);
            sensY = 5 * BetterPrefs.GetInt("KeyboardMouseSettings_MouseSensitivity", 1);
        }
    }

    private void LateUpdate()
    {
        Vector2 rotateDirection = playerControls.actions["Camera"].ReadValue<Vector2>();

        yRotation += rotateDirection.x * sensX * Time.fixedDeltaTime;
        xRotation -= rotateDirection.y * sensY * Time.fixedDeltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }
#endif
}