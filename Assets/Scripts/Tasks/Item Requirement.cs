using UnityEngine;
using UnityEngine.Events;

public class ItemRequirement : HoldInteractable
{
    [Header("Single Item")]
    [SerializeField] private Interactable requiredItem;// Checks if this item is picked up or in inventory

    [Header("Grouped Items")]
    [SerializeField] private bool groupedItem;// Mark this as true when using mutiple items
    [SerializeField] private Transform groupedItems;// Should be a empty gameobject with required items as child of this

    [SerializeField] private InventoryEvent inventoryEvent; // Used to check inventory

    private Interactable[] requiredItems;

    [SerializeField] private UnityEvent onComplete;
    [SerializeField] private UnityEvent onMissingItem;

    private void Awake()
    {
        if (groupedItem && groupedItems != null) requiredItems = groupedItems.GetComponentsInChildren<Interactable>();
    }

    public void CheckForItem()
    {
        if (groupedItem)
        {
            CheckGroupedItems();
            return;
        }

        if (inventoryEvent.HasItem(requiredItem))
        {
            Debug.Log("Required item found!");
        }
        else
        {
            Debug.Log("Missing required item!");
        }
    }

    private void CheckGroupedItems()
    {
        foreach (Interactable item in requiredItems)
        {
            if (!inventoryEvent.HasItem(item))
            {
                Debug.Log($"Missing item {item.name}");
                return;
            }
        }

        Debug.Log("All required trash collected yippeee!");
    }

}
