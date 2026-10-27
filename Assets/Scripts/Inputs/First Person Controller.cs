using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] Transform cameraRoot;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector2 lookDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float headSpeed;

    private float xRotation;
    private float yRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputs.EnableAllInputs();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        float lookX = lookDirection.x * headSpeed * Time.deltaTime;
        float lookY = lookDirection.y * headSpeed * Time.deltaTime;

        yRotation += lookX;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void SetMoveInput(Vector2 moveInput)
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    private void SetLookInput(Vector2 lookInput)
    {
        lookDirection = lookInput.normalized;
    }

    private void OnEnable()
    {
        playerInputs.onMove += SetMoveInput;
        playerInputs.onLook += SetLookInput;
    }
    private void OnDisable()
    {
        playerInputs.onMove -= SetMoveInput;
        playerInputs.onLook -= SetLookInput;

    }
}
