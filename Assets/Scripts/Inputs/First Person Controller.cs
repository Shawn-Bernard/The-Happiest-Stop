using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour,ISpawnable
{
    [SerializeField] PlayerInputs playerInputs;
    private Vector3 startPosition;
    private Vector3 startRotation;

    private Vector3 spawnPosition;
    private Vector3 spawnRotation;

    [Header("Movement Settings")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    private Vector3 moveInput;
    private Vector2 moveDirection;
    [SerializeField] private CharacterController characterController;

    [Header("Head Settings")]
    [SerializeField] Transform cameraRoot;
    [Range(0.1f, 5)]
    [SerializeField] private float sensitivity = 1;
    [SerializeField] private bool useRotationSmoothing;
    [Range(10f, 30f)]
    [SerializeField] private float rotationSmoothing = 30;
    [Range(40, 90)]
    [SerializeField] private int upLookLimit = 90;
    [Range(40, 90)]
    [SerializeField] private int downLookLimit = 90;
    private Vector2 lookDirection;
    private float xRotation;
    private float yRotation;
    private Vector3 velocity;

    [Header("Physics Settings")]
    [SerializeField] private float pushForce = 5f; // How much force is added to object when colliding
    [SerializeField] private float groundStickVelocity = -1f; // Small amount of force to keep the player grounded

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetStartPosition();
        SwitchToWalk();
        playerInputs.EnableAllInputs();
        characterController ??= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleLook();
    }

    /// <summary>
    /// Handles the player movement
    /// </summary>
    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection.Normalize();
        moveDirection = transform.TransformDirection(moveDirection);

        if (characterController.isGrounded)
        {
            velocity.y = groundStickVelocity;
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 finalMovement = moveDirection * moveSpeed;
        finalMovement.y = velocity.y;

        characterController.Move(finalMovement * Time.deltaTime);
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
    /// Handles the camera by moving y and then moves the player on x
    /// </summary>
    private void HandleLook()
    {
        float lookX = lookDirection.x * sensitivity;// * Time.deltaTime;
        float lookY = lookDirection.y * sensitivity;// * Time.deltaTime;

        yRotation += lookX;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -upLookLimit, downLookLimit); // Setting the upper and lower look limits

        if (useRotationSmoothing) // Will make the camera less snapy when looking
        {
            cameraRoot.localRotation = Quaternion.Slerp(
            cameraRoot.localRotation,
            Quaternion.Euler(xRotation, 0f, 0f),
            rotationSmoothing * Time.deltaTime
             );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0f, yRotation, 0f),
                rotationSmoothing * Time.deltaTime
            );
        }
        else // Will make the camera more responsive
        {
            cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);// Rotates the camera root up and down
            transform.rotation = Quaternion.Euler(0, yRotation, 0);// Rotates the player side to side
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic) return;

        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);

        rb.AddForce(pushDirection * pushForce, ForceMode.Force);
    }

    private void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void SetLookInput(Vector2 lookInput)
    {
        lookDirection = lookInput;
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

    public void SetSpawnPosition(Vector3 position, Vector3 rotation)
    {
        Debug.Log("Player has spawn");
        spawnPosition = position;
        spawnRotation = rotation;
    }

    public void SetStartPosition()
    {
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
        SetSpawnPosition(startPosition, startRotation);
    }

    public void ResetToSpawn()
    {
        Debug.Log("Player has been reset to spawn");
        characterController.enabled = false;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(spawnRotation);
        characterController.enabled = true;
    }

    public void ResetToStart()
    {
        characterController.enabled = false;
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);
        characterController.enabled = true;
    }
}
