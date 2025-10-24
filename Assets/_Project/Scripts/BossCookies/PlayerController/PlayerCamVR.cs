using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerCamVR : MonoBehaviour
{
    public float sensY;

    public Transform Root;
    public Transform orientation;
    public Transform playerModel;

    [SerializeField] private PlayerInput playerControls;

    private float yRotation;

    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = Game.instance.ad.PostProcessing;
    }

    private void LateUpdate()
    {
        Vector2 rotateDirection = playerControls.actions["Camera"].ReadValue<Vector2>();

        yRotation += rotateDirection.x * sensY * Time.fixedDeltaTime;

        //Rotate camera and orientation
        Root.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}