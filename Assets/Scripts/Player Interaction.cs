using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private PickUp currentPickUp;

    [SerializeField] private UnityEvent<PickUp> onPickedItem; // To pass the pickup to inventory
    [SerializeField] private UnityEvent<PickUp> onDropPickedItem; // To call drop pickup on invenotry 
    [SerializeField] private UnityEvent<IInteractable> onInteractionFound;

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
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable)
            && interactable.GetInteractableStatus())
            {
                currentInteractable = interactable;
                onInteractionFound?.Invoke(interactable);
                return;
            }
        }

        if (currentInteractable == null) return;

        CancelInteractable();

        currentInteractable = null;

        onInteractionFound?.Invoke(null);
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
        // Looking at a pickup
        if (currentInteractable is PickUp pickUp)
        {
            HandlePickUp(pickUp);
            return;
        }

        // Looking at another interactable
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
            return; // Returns so it doesnt drop item
        }

        // Interacting with nothing while holding something
        if (currentPickUp != null)
        {
            HandleDrop();
        }
    }

    private void CancelInteractable()
    {
        if (currentInteractable == null) return ;
        if (currentInteractable is IHoldInteractable interactable)
        {
            interactable.HoldInteractFailed();
        }
    }

    private void HandlePickUp()
    {
        if (currentInteractable == null) return;
        if (currentInteractable is PickUp pickUp)
        {
            currentPickUp = pickUp;
            onPickedItem?.Invoke(currentPickUp);
        }
    }

    private void HandlePickUp(PickUp pickUp)
    {
        // Drop the current pickup
        if (currentPickUp == pickUp)
        {
            HandleDrop();
        }

        // Drop previous pickup
        if (currentPickUp != null)
        {
            HandleDrop();
        }

        // Pick up new pickup
        currentPickUp = pickUp;

        onPickedItem?.Invoke(currentPickUp);
    }

    private void HandleDrop()
    {
        Debug.Log("Dropping Pickup");
        currentPickUp.OnDropPickUp();

        onDropPickedItem?.Invoke(currentPickUp);
        currentPickUp = null;
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
