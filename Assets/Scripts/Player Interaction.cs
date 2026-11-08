using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private IPickUp currentPickUp;

    [SerializeField] private UnityEvent<IInteractable> onInteractionFound; // Used for ui interactable type and string action
    [SerializeField] private UnityEvent<IHoldInteractable> onHoldStarted; // Used for ui feedback when holding interactable

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera ??= Camera.main;

        onInteractionFound?.Invoke(null); // So interactable ui disables on start
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
        if (currentInteractable is IPickUp pickUp)
        {
            currentInteractable.Interact(gameObject);
            HandlePickUp(pickUp);
            return;
        }

        // Looking at another interactable
        if (currentInteractable != null)
        {
            currentInteractable.Interact(gameObject);
            if (currentInteractable is IHoldInteractable holdInteractable)
            {
                onHoldStarted?.Invoke(holdInteractable);
            }
            return; // Returns so it doesnt drop item
        }

        // Interacting with nothing while holding something
        if (currentPickUp != null)
        {
            currentPickUp.DropPickUp();
            currentPickUp = null;
            return;
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
        if (currentInteractable is IPickUp pickUp)
        {
            currentPickUp = pickUp;
        }
    }

    private void HandlePickUp(IPickUp pickUp)
    {
        // Drop the current pickup
        if (currentPickUp == pickUp)
        {
            currentPickUp.DropPickUp();
            currentPickUp = null;
            return;
        }

        // Drop previous pickup
        if (currentPickUp != null)
        {
            currentPickUp.DropPickUp();
            currentPickUp = null;
        }

        // Pick up new pickup
        currentPickUp = pickUp;
        currentPickUp.PickUp();
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
