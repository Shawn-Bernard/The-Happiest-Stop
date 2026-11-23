using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RequiredItem : MonoBehaviour
{
    private IInteractable connectedInteractable;
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

    [SerializeField] private Task relatedTask;
    [SerializeField] string taskStep;
    [SerializeField] int taskStepIndex;
    [SerializeField] private UnityEvent<string, int> onItemObtained;

    private void Awake()
    {
        connectedInteractable = GetComponent<IInteractable>();
        GetGroupedItems();

        if (usingGroup)
        {
            totalItems = requiredItems.Count;
        }
        else
        {
            totalItems = 1;
        }
    }

    private void GetGroupedItems()
    {
        if (usingGroup && groupedItems != null)
        {
            requiredItems = new List<Item>();
            foreach (Transform child in groupedItems)
            {
                if (child.TryGetComponent<Item>(out var item))
                {
                    requiredItems.Add(item);
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    private void RandomizeRequiredItems()
    {
        activeRequiredItems = new List<Item>();

        int targetCount = Mathf.RoundToInt(requiredItems.Count * (itemSpawnChance / 100f));
        targetCount = Mathf.Clamp(targetCount, 1, requiredItems.Count);


        while (activeRequiredItems.Count < targetCount)
        {
            Item randomItem = requiredItems[Random.Range(0, requiredItems.Count)];

            if (!activeRequiredItems.Contains(randomItem)) activeRequiredItems.Add(randomItem);
        }
    }

    public void CheckForItem()
    {
        if (itemCount == totalItems)
        {
            //Debug.Log("Has items");
            onCompletion?.Invoke();
            return;
        }

        //Debug.Log("Player missing items");
        onMissingItem?.Invoke(taskStepIndex);
    }

    public void UpdateCount()
    {
        if (relatedTask != null)
        {
            if (!relatedTask.IsActive)
            {
                connectedInteractable?.SetActive(false);
                return;
            }
        }
        itemCount = 0;

        if (usingGroup)
        {
            foreach (var item in activeRequiredItems)
            {
                if (inventoryEvent.HasItem(item))
                {
                    itemCount++;
                }
            }
        }
        else
        {
            if (inventoryEvent.HasItem(requiredItem))
            {
                itemCount++;
            }
        }

        if (itemCount == totalItems)
        {
            onHasItem?.Invoke(taskStepIndex);
            connectedInteractable?.SetActive(true);
            Debug.Log($"{name} has item {itemCount}");
        }
        else
        {
            Debug.Log($"{name} doesn't have item {itemCount}");
            onMissingItem?.Invoke(taskStepIndex);
            connectedInteractable?.SetActive(false);
        }
        //Debug.Log($"{name} | Using Group {usingGroup}| Item Count {itemCount} / {totalItems} | Interactable status {connectedInteractable.GetInteractableStatus()}");

        onItemObtained?.Invoke($"{taskStep} {itemCount}/{totalItems}", taskStepIndex);
    }

    public void RemoveItems()
    {
        foreach (var item in activeRequiredItems)
        {
            if (item is IPickUp) continue;
            inventoryEvent.RemoveItem(item);
        }
    }


    public void Restart()
    {
        //Debug.Log($"Restart was called on {name}");

        RemoveItems();
        if (usingGroup)
        {
            foreach (Item item in requiredItems)
            {
                item.gameObject.SetActive(false);
            }

            if (useSpawnChance)
            {
                RandomizeRequiredItems();
            }
            else
            {
                activeRequiredItems = new List<Item>(requiredItems);
            }

            foreach (Item item in activeRequiredItems)
            {
                item.gameObject.SetActive(true);
                item.SetActive(true);
                //Debug.Log($"{item} has been set true : {item.gameObject.activeInHierarchy}, interactable set to true : {item.GetInteractableStatus()}");
            }
            totalItems = activeRequiredItems.Count;
            //Debug.Log(totalItems);
        }
        else
        {
            totalItems = 1;
            //if (requiredItem is IPickUp pickUp) pickUp.DropPickUp();
        }
        UpdateCount();
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
