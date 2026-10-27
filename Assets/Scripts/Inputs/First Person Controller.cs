using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    [Header("Movement Settings")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float moveAcceleration;
    private Vector3 moveInput;
    private Vector2 moveDirection;
    private Rigidbody rb;

    [Header("Head Settings")]
    [SerializeField] Transform cameraRoot;
    [SerializeField] private float headSpeed;
    [Range(40,90)]
    [SerializeField] private int upLookLimit = 90;
    [Range(40, 90)]
    [SerializeField] private int downLookLimit = 90;
    private Vector2 lookDirection;
    private float xRotation;
    private float yRotation;

    [Header("Ground Settings")]
    [SerializeField] Transform groundCheck; // Where should the gound check start
    [SerializeField] float groundDrag; // Controls how quickly the player slows down while grounded
    [SerializeField] float groundCheckDistance;
    private bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchToWalk();
        playerInputs.EnableAllInputs();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLook();

        HandleGroundCheck();
        
        ControlSpeed();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// Handles the player movement
    /// </summary>
    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        moveDirection = transform.TransformDirection(moveDirection); // Moves from local to world space
        moveDirection.y = 0f;
        moveDirection.Normalize();

        rb.AddForce(moveDirection * moveSpeed * moveAcceleration, ForceMode.Force);
    }

    /// <summary>
    /// Handles the player Velocity to match the current move speed
    /// </summary>
    private void ControlSpeed()
    {
        Vector3 floatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (floatVelocity.magnitude > moveSpeed)
        {
            Vector3 limtedVelocity = floatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limtedVelocity.x, rb.linearVelocity.y,limtedVelocity.z);
        }
    }

    public void SwitchToWalk()
    {
        moveSpeed = walkSpeed;
    }

    public void SwitchToSprint()
    {
        moveSpeed = sprintSpeed;
    }

    /// <summary>
    /// Shoots a raycast to the ground and applies ground drag to stop sliding
    /// </summary>
    private void HandleGroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.transform.position, Vector3.down, groundCheckDistance);

        if (isGrounded) rb.linearDamping = groundDrag;
        else rb.linearDamping = 0f;
    }

    /// <summary>
    /// Handles the camera by moving y and then moves the player on x
    /// </summary>
    private void HandleLook()
    {
        float lookX = lookDirection.x * headSpeed;// * Time.deltaTime;
        float lookY = lookDirection.y * headSpeed;// * Time.deltaTime;

        yRotation += lookX;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation,-upLookLimit, downLookLimit); // Setting the upper and lower look limits

        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);// Rotates the camera root up and down
        transform.rotation = Quaternion.Euler(0, yRotation, 0);// Rotates the player side to side
    }

    private void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void SetLookInput(Vector2 lookInput)
    {
        lookDirection = lookInput.normalized;
    }

    private void OnEnable()
    {
        playerInputs.onMove += SetMoveInput;
        playerInputs.onLook += SetLookInput;
        playerInputs.onSprintStarted += SwitchToSprint;
        playerInputs.onSprintCanceled += SwitchToWalk;
    }
    private void OnDisable()
    {
        playerInputs.onMove -= SetMoveInput;
        playerInputs.onLook -= SetLookInput;
        playerInputs.onSprintStarted -= SwitchToSprint;
        playerInputs.onSprintCanceled -= SwitchToWalk;

    }
}
