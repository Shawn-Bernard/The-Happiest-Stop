using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Holdable : Item, IPickUp
{
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private UnityEvent onPickUp;
    [SerializeField] private UnityEvent onDropPickUp;

    private Collider itemCollider; // Used for dropping the item
    private Rigidbody rb;
    private Vector3 halfExtents; // Used for when dropping the item
    private Transform originalParent; // So it doesn't be parent less if it has a one
    private Scene originalScene; // just so it doesn't carry along with the player when changing scenes

    public override void Awake()
    {
        itemCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        halfExtents = itemCollider.bounds.extents;

        originalParent = transform.parent;
        originalScene = gameObject.scene;
        base.Awake();
    }
    public override void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent<IInventory>(out IInventory newInventory))
        {
            if (inventory != newInventory) inventory = newInventory;
        }
        onInteract?.Invoke();
    }

    /// <summary>
    /// Calls on pickup event for disabling hit box
    /// </summary>
    public void PickUp()
    {
        if (itemCollider != null) itemCollider.enabled = false;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        TakeItem();
        inventory.HoldItem(this);
    }

    public void DropPickUp()
    {
        RemoveItem();
        inventory.DropItem();
    }
    public void AttachTo(Transform parent)
    {
        onPickUp?.Invoke();
        transform.SetParent(parent, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = offsetPosition;
        transform.localRotation = Quaternion.Euler(offsetRotation);
        SetOffset();
    }

    public void Detach(Vector3 dropPosition)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        transform.SetParent(originalParent, true);
        SceneManager.MoveGameObjectToScene(gameObject, originalScene);
        transform.position = dropPosition;
        onDropPickUp?.Invoke();
        if (itemCollider != null) itemCollider.enabled = true;
        
    }
    public Vector3 GetHalfExtends() => halfExtents;

    public void SetOffset()
    {
        transform.localPosition = offsetPosition;
        transform.localRotation = Quaternion.Euler(offsetRotation);
    }

}
