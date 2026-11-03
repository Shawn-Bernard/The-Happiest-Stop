using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemRequirement : HoldInteractable
{
    [Header("Single Item")]
    [SerializeField] private Interactable requiredItem;// Checks if this item is picked up or in inventory

    [Header("Grouped Items")]
    [SerializeField] private bool groupedItem;// Mark this as true when using mutiple items
    [SerializeField] private Transform groupedItems;// Should be a empty gameobject with required items as child of this

    [Header("Player Inventory Event")]
    [SerializeField] private InventoryEvent inventoryEvent; // Used to check inventory

    private Interactable[] requiredItems;
    [SerializeField] private int itemCount;
    [SerializeField] private int highestItemCount;
    private int totalItems;

    private bool isCompleted;

    [Header("Events")]
    [SerializeField] private UnityEvent onRequirementsMet;
    [SerializeField] private UnityEvent onMissingItem;

    

    [Header("Task Settings")]
    [SerializeField] string taskStep;
    [SerializeField] int taskStepIndex;
    [SerializeField] private UnityEvent<string, int> onItemObtained;

    private void Awake()
    {
        if (groupedItem && groupedItems != null) requiredItems = groupedItems.GetComponentsInChildren<Interactable>();

        if (groupedItem)
        {
            totalItems = requiredItems.Length;
        }
        else
        {
            totalItems = 1;
        }
    }

    public void CheckForItem()
    {
        // For mutiple items
        if (highestItemCount == totalItems && groupedItem)
        {
            onRequirementsMet?.Invoke();
            return;
        }

        // For single item
        if (itemCount == totalItems && !groupedItem)
        {
            onRequirementsMet?.Invoke();
            return;
        }

        Debug.Log("Player missing items");
        onMissingItem?.Invoke();
    }

    public void UpdateCount()
    {
        itemCount = 0;

        if (groupedItem)
        {
            foreach (Interactable item in requiredItems)
            {
                if (inventoryEvent.HasItem(item))
                    itemCount++;
            }
        }
        else if (inventoryEvent.HasItem(requiredItem))
        {
            itemCount++;
        }

        if (itemCount >= highestItemCount)
        {
            highestItemCount = itemCount;
        }

        onItemObtained?.Invoke($"{taskStep} {itemCount}/{totalItems}", taskStepIndex);
        CheckForItem();
    }

    public void RemoveItems()
    {
        if (itemCount == totalItems)
        {
            foreach (Interactable item in requiredItems)
            {
                inventoryEvent.RemoveItem(item);
            }
        }
    }

    public void SetInteractables(bool state)
    {
        if (groupedItem)
        {
            Debug.Log($"Items were set {state}");
            foreach (Interactable item in requiredItems)
            {
                item.gameObject.SetActive(state);
                Debug.Log($"{item.isActiveAndEnabled}");
            }
        }
    }



    public override void OnEnable()
    {
        base.OnEnable();
        inventoryEvent.onInventoryChanged += UpdateCount;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        inventoryEvent.onInventoryChanged -= UpdateCount;
    }
}
