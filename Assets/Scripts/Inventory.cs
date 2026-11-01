using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
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
    }

    public void RemoveItem(Interactable item)
    {
        Debug.Log("Item Removed");
        inventory.Remove(item);
    }

    public bool HasItem(Interactable item)
    {
        return inventory.Contains(item);
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
