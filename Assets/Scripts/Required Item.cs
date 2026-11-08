using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RequiredItem : MonoBehaviour
{
    [Header("Single Item")]
    [SerializeField] private Item requiredItem;// Checks if this item is picked up or in inventory

    [Header("Grouped Items")]
    [SerializeField] private bool usingGroup; // Mark this as true when using mutiple items
    [SerializeField] private Transform groupedItems;// Should be a empty gameobject with required items as child of this
    [SerializeField] private bool useSpawnChance;
    [Range(1, 100)]
    [SerializeField] private int itemSpawnChance = 50;

    [Header("Player Inventory Event")]
    [SerializeField] private InventoryEvent inventoryEvent; // Used to check inventory

    private List<Item> requiredItems;
    [SerializeField] private List<Item> activeRequiredItems;
    [SerializeField] private int itemCount;
    private int totalItems;

    [Header("Events")]
    [SerializeField] private UnityEvent onCompletion;
    [SerializeField] private UnityEvent<int> onHasItem;
    [SerializeField] private UnityEvent<int> onMissingItem;



    [Header("Task Settings")]
    [SerializeField] string taskStep;
    [SerializeField] int taskStepIndex;
    [SerializeField] private UnityEvent<string, int> onItemObtained;

    private void Awake()
    {
        GetGroupedItems();
        Restart();
    }

    private void GetGroupedItems()
    {
        if (usingGroup && groupedItems != null) requiredItems = groupedItems.GetComponentsInChildren<Item>(true).ToList();
    }

    private bool RollSpawnChance()
    {
        return Random.Range(0, 100) < itemSpawnChance;
    }

    private void RandomizeRequiredItems()
    {
        activeRequiredItems = new List<Item>();

        foreach (Interactable item in requiredItems)
        {
            bool shouldActivate = Random.Range(0, 100) < itemSpawnChance;
            item.gameObject.SetActive(shouldActivate);
            item.SetActive(shouldActivate);

            if (shouldActivate) activeRequiredItems.Add((Item)item);
        }
    }

    public void CheckForItem()
    {
        if (itemCount == totalItems)
        {
            onCompletion?.Invoke();
            return;
        }

        Debug.Log("Player missing items");
        onMissingItem?.Invoke(taskStepIndex);
    }

    public void UpdateCount()
    {
        itemCount = 0;

        if (usingGroup)
        {
            foreach (var item in activeRequiredItems)
            {

                if (inventoryEvent.HasItem(item))
                {
                    itemCount++;
                }
                else
                {
                    onMissingItem?.Invoke(taskStepIndex);
                }
            }
        }
        else
        {
            if (inventoryEvent.HasItem(requiredItem))
            {
                itemCount++;
                onHasItem?.Invoke(taskStepIndex);
            }
            else
            {
                onMissingItem?.Invoke(taskStepIndex);
            }
        }

        if (itemCount == totalItems)
        {
            onHasItem?.Invoke(taskStepIndex);
        }

        onItemObtained?.Invoke($"{taskStep} {itemCount}/{totalItems}", taskStepIndex);
    }

    public void RemoveItems()
    {
        foreach (var item in activeRequiredItems)
        {
            inventoryEvent.RemoveItem(item);
        }
    }


    public void Restart()
    {
        itemCount = 0;

        RemoveItems();
        if (usingGroup)
        {
            if (useSpawnChance)
            {
                RandomizeRequiredItems();
            }
            else
            {
                activeRequiredItems = new List<Item>(requiredItems);

                foreach (var item in activeRequiredItems)
                {
                    item.gameObject.SetActive(true);
                    item.SetActive(true);
                }
            }

            totalItems = activeRequiredItems.Count;
        }
        else
        {
            totalItems = 1;
        }

        onItemObtained?.Invoke($"{taskStep} {itemCount}/{totalItems}", taskStepIndex);
    }


    private void OnEnable()
    {
        inventoryEvent.onInventoryChanged += UpdateCount;
    }

    private void OnDisable()
    {
        inventoryEvent.onInventoryChanged -= UpdateCount;
    }
}
