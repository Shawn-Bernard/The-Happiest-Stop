using UnityEngine;

public class TrashCan : HoldInteractable
{
    [SerializeField] private InventoryEvent inventoryEvent;
    [SerializeField] private Transform requiredTrashPile; // A empty game object with all the trash thats required as the child in the pile

    private Interactable[] requiredItems;

    private void Awake()
    {
        requiredItems = requiredTrashPile.GetComponentsInChildren<Interactable>();
    }

    public void CheckForItem()
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
