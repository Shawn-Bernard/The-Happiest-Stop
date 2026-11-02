using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private InventoryEvent inventoryEvent;
    [SerializeField] private List<Interactable> inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new List<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Interactable item)
    {
        Debug.Log("Item Added");
        if (!inventory.Contains(item)) inventory.Add(item);
        inventoryEvent.InventoryChanged();
    }

    public void RemoveItem(Interactable item)
    {
        Debug.Log("Item Removed");
        inventory.Remove(item);
        inventoryEvent.InventoryChanged();
    }

    public bool HasItem(Interactable item)
    {
        return inventory.Contains(item);
    }

    public void HoldItem(PickUp pickUp)
    {
        if (pickUpTransform.childCount > 0) return;
        AddItem(pickUp);
        pickUp.transform.SetParent(pickUpTransform, false);
        pickUp.transform.localPosition = Vector3.zero;
        pickUp.transform.localRotation = Quaternion.identity;
        pickUp.transform.localScale = Vector3.one;
        pickUp.OnPickUp();
    }

    public void DropItem(PickUp pickUp)
    {
        if (pickUpTransform.childCount == 0) return;
        RemoveItem(pickUp);
        pickUpTransform.GetChild(0).SetParent(null, true);
    }

    private void OnEnable()
    {
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
