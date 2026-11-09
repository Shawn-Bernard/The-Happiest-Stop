using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private InventoryEvent inventoryEvent;
    [SerializeField] private List<IItem> inventory;
    private IPickUp currentPickUp;
    [SerializeField] private float dropDistance = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new List<IItem>();
    }

    public void AddItem(IItem item)
    {
        //Debug.Log("Item Added");
        if (!inventory.Contains(item)) inventory.Add(item);
        inventoryEvent.InventoryChanged();
    }

    public void RemoveItem(IItem item)
    {
        //Debug.Log("Item Removed");
        inventory.Remove(item);
        inventoryEvent.InventoryChanged();
    }

    public bool HasItem(IItem item)
    {
        return inventory.Contains(item);
    }

    public void HoldItem(IPickUp pickUp)
    {
        if (currentPickUp != null)
        {
            DropItem();
        }

        currentPickUp = pickUp;
        currentPickUp.AttachTo(pickUpTransform);
    }
    /// <summary>
    /// Drops the current item and shoots a box raycast to check for wall, so it doesn't go through
    /// </summary>
    public void DropItem()
    {
        if (currentPickUp == null) return;

        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0f;
        direction.Normalize();

        Vector3 dropPosition;

        Vector3 halfExtents = currentPickUp.GetHalfExtends();
        float itemRadius = halfExtents.magnitude;

        // Raycast so the item doesn't pop through walls when dropped
        if (Physics.BoxCast(Camera.main.transform.position, halfExtents, direction,
            out RaycastHit hit, Quaternion.identity, dropDistance))
        {
            dropPosition = Camera.main.transform.position - direction * (dropDistance + itemRadius);
            Debug.Log("hit something");
        }
        else
        {
            dropPosition = Camera.main.transform.position + Camera.main.transform.forward * dropDistance;
            Debug.Log("hit nothing");
        }

        currentPickUp.Detach(dropPosition);
        currentPickUp = null;
    }

    public void ClearInventory()
    {
        if (inventory == null || inventory.Count == 0) return;
        DropItem();
        inventory.Clear();
        inventoryEvent.InventoryChanged();
    }

    private void OnEnable()
    {
        ClearInventory();
        inventoryEvent.onAddItem += AddItem;
        inventoryEvent.onRemoveItem += RemoveItem;
        inventoryEvent.onHasItem += HasItem;
    }

    private void OnDisable()
    {
        inventoryEvent.onAddItem -= AddItem;
        inventoryEvent.onRemoveItem -= RemoveItem;
        inventoryEvent.onHasItem -= HasItem;
    }
}
