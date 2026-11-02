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

    [Header("Events")]
    [SerializeField] private UnityEvent onRequirementsMet;
    [SerializeField] private UnityEvent onMissingItem;

    [SerializeField] private UnityEvent<string,int> onItemObtained;

    [Header("Task Settings")]
    [SerializeField] string taskStep;
    [SerializeField] int taskStepIndex;

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
            onRequirementsMet?.Invoke();
        }
        else
        {
            onMissingItem?.Invoke();
        }
    }

    private void CheckGroupedItems()
    {
        foreach (Interactable item in requiredItems)
        {
            if (!inventoryEvent.HasItem(item))
            {
                Debug.Log($"Missing item {item.name}");
                onMissingItem?.Invoke();
                return;
            }
        }
        onRequirementsMet?.Invoke();
        Debug.Log("All required trash collected yippeee!");
    }

    public void UpdateCount()
    {
        Debug.Log("Updated");
        int count = 0;
        int total;

        if (groupedItem)
        {
            total = requiredItems.Length;
        }
        else
        {
            total = 1;
        }

        if (groupedItem)
        {
            foreach (Interactable item in requiredItems)
            {
                if (inventoryEvent.HasItem(item))
                    count++;
            }
        }
        else if (inventoryEvent.HasItem(requiredItem))
        {
            count++;
        }

        onItemObtained?.Invoke($"{taskStep} {count}/{total}", taskStepIndex);
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
