using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera ??= Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out RaycastHit hit,
            interactionDistance,
            interactableLayer))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
        else
        {
            if (currentInteractable != null)
            {
                CancelInteractable();

                currentInteractable = null;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * interactionDistance
        );

    }

    private void Interact()
    {
        if (currentInteractable == null) return;
        currentInteractable.Interact();
    }

    private void CancelInteractable()
    {
        if (currentInteractable == null) return ;
        if (currentInteractable is IHoldInteractable interactable)
        {
            interactable.HoldInteractFailed();
        }
    }

    private void OnEnable()
    {
        playerInputs.onInteractStarted += Interact;
        playerInputs.onInteractCanceled += CancelInteractable;
    }

    private void OnDisable()
    {
        playerInputs.onInteractStarted -= Interact;
        playerInputs.onInteractCanceled -= CancelInteractable;
    }
}
