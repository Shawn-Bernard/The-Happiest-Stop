using UnityEngine;
using UnityEngine.Events;

public class RequiredItem : MonoBehaviour
{
    [Header("Single Item")]
    [SerializeField] private Interactable requiredItem;// Checks if this item is picked up or in inventory

    [Header("Grouped Items")]
    [SerializeField] private bool usingGroup; // Mark this as true when using mutiple items
    [SerializeField] private Transform groupedItems;// Should be a empty gameobject with required items as child of this

    [Header("Player Inventory Event")]
    [SerializeField] private InventoryEvent inventoryEvent; // Used to check inventory

    private Interactable[] requiredItems;
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
        if (usingGroup && groupedItems != null) requiredItems = groupedItems.GetComponentsInChildren<Interactable>();

        if (usingGroup)
        {
            totalItems = requiredItems.Length;
        }
        else
        {
            totalItems = 1;
        }
        onItemObtained?.Invoke($"{taskStep} {itemCount}/{totalItems}", taskStepIndex);
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
            foreach (Interactable item in requiredItems)
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
        if (usingGroup)
        {
            Debug.Log($"Items were set {state}");
            foreach (Interactable item in requiredItems)
            {
                item.gameObject.SetActive(state);
                Debug.Log($"{item.isActiveAndEnabled}");
            }
        }
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
