using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private bool readyToJump;
#endif

    [Header("Camera")]
    public Camera playerCam;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Other")]
    public Transform orientation;
    public GameObject playerModel;
    public LayerMask dontRenderLayer;
    public LayerMask spinnerLayer;
    public bool canMove = true;
    public bool IsOnKeyboardMouse;
#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private Transform oldParent = null;
#endif
    [SerializeField] private PlayerInput playerControls;
    [SerializeField] private PlayerFade fade;

    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float sprintSpeed = 14;
    [SerializeField] private float groundDrag = 9;
    [SerializeField] private float jumpForce = 9;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float fovNormal = 75;
    [SerializeField] private float fovSprint = 80;

    [SerializeField] private PlayerCam playerCamComponent;

#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private CursorLockMode prevCursorLockMode;
    private bool prevCanMove;
    private bool prevCanMoveCamera;
    private Vector3 prevVel;

    private bool Respawning;
#endif

#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private void Start()
    {
        oldParent = transform.parent;

        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ResetJump();

        Debug.Log("(PlayerMovement) Controller2: " + playerControls.devices[0].displayName);
        Debug.Log(playerControls.devices[0].name);

        if (playerControls.currentControlScheme.Contains("Keyboard") || playerControls.currentControlScheme.Contains("Mouse"))
        {
            IsOnKeyboardMouse = true;
        }
    }

    private void FixedUpdate()
    {
        if (Respawning)
        {
            return;
        }

        if (transform.position.y <= -100 && !Respawning)
        {
            Respawning = true;
            Respawn().Forget();
        }

        MovePlayer();
    }

    private async UniTask Respawn()
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        fade.FadeIn();
        await UniTask.WaitForSeconds(1f);

        rb.position = Vector3.zero;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        Respawning = false;
        rb.useGravity = true;
        fade.FadeOut();
    }

    private void Update()
    {
        if (Respawning)
        {
            return;
        }

        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // Handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void MyInput()
    {
        Vector2 moveDirection;

        if (!canMove)
        {
            moveDirection = Vector2.zero;
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }

        moveDirection = playerControls.actions["Movement"].ReadValue<Vector2>();
        horizontalInput = moveDirection.x;
        verticalInput = moveDirection.y;

        // When to jump
        if (playerControls.actions["Jump"].IsPressed() && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        RaycastHit hitInfo = new();
        bool hit = Physics.Raycast(transform.position, Vector3.down, out hitInfo, playerHeight * 0.5f + 0.2f, spinnerLayer);

        if (hit)
        {
            transform.SetParent(hitInfo.transform);
        }
        else
        {
            if (oldParent != null)
            {
                transform.SetParent(oldParent);
            }
        }

        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On ground
        if (grounded)
        {
            if (playerControls.actions["Sprint"].IsPressed())
            {
                playerCam.fieldOfView = fovSprint;
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
            }
            else
            {
                playerCam.fieldOfView = fovNormal;
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        // In air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void Jump()
    {
        //Reset Y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }
#endif
}