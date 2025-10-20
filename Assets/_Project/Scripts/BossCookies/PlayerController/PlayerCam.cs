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
    private float xRotation;
    private float yRotation;

    [SerializeField] private PlayerInput playerControls;

    private static bool playerHasJoined;

    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = Game.instance.ad.PostProcessing;

        if (!playerHasJoined)
        {
            gameObject.tag = "Player1Camera";
            playerHasJoined = true;
        }

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

    public static void ChangePlayerHasJoined()
    {
        playerHasJoined = false;
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    public static void ResetValues()
    {
        playerHasJoined = false;
    }
#endif
}