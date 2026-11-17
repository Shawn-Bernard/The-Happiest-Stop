using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour, IInventory
{
    [SerializeField] private PlayerInputs playerInputs;
    [Header("Smoke related")]
    [Range(1f, 20f)]
    [SerializeField] private float sanityRestored = 10;
    [SerializeField] private int maxCharges = 5;
    [SerializeField] private float smokeCooldown = 1f;
    private int currentCharges;
    private bool canSmoke;
    [SerializeField] private bool SmokesEquipped;
    [SerializeField] private UnityEvent onEquip;
    [SerializeField] private UnityEvent onUnequip;
    [SerializeField] private UnityEvent<float> onSmokeUsed;

    [Header("Holdables & Items related")]
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private InventoryEvent inventoryEvent;
    [SerializeField] private List<IItem> inventory;
    private IPickUp currentPickUp;
    [SerializeField] private float dropDistance = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = new List<IItem>();
        currentCharges = maxCharges;
        canSmoke = true;
    }

    private void ToggleSmoking()
    {
        if (SmokesEquipped)
        {
            UnequipSmokes();
            return;
        }

        if (currentPickUp != null) currentPickUp.DropPickUp();

        EquipSmokes();
    }

    private void UnequipSmokes()
    {
        SmokesEquipped = false;
        onUnequip?.Invoke();
    }

    private void EquipSmokes()
    {
        SmokesEquipped = true;
        onEquip?.Invoke();
    }

    public void UseSmoke()
    {
        if (currentCharges <= 0 || !canSmoke || !SmokesEquipped) return;
        currentCharges--;
        onSmokeUsed?.Invoke(sanityRestored);
        StartCoroutine(SmokeCooldown());

    }
    private IEnumerator SmokeCooldown()
    {
        canSmoke = false;

        yield return new WaitForSeconds(smokeCooldown);

        canSmoke = true;
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
        if (inventory.Contains(item)) inventory.Remove(item);

        inventoryEvent.InventoryChanged();
    }

    public bool HasItem(IItem item)
    {
        return inventory.Contains(item);
    }

    public void HoldItem(IPickUp pickUp)
    {
        // Drop the last held item if there is one and if its not the same  
        if (currentPickUp != null && currentPickUp != pickUp) currentPickUp.DropPickUp();
        if (SmokesEquipped) UnequipSmokes();

        currentPickUp = pickUp;

        currentPickUp.AttachTo(pickUpTransform);
    }
    /// <summary>
    /// Drops the current item and shoots a box raycast to check for wall, so it doesn't go through
    /// </summary>
    public void DropItem()
    {
        if (currentPickUp == null) return;

        if (currentPickUp is IItem item) RemoveItem(item);
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
    public void HandlePickUp(IPickUp pickUp)
    {
        // If looking at nothing and interacting drop current held item
        if (pickUp == null && currentPickUp != null)
        {
            currentPickUp.DropPickUp();
            return;
        }

        // If looking at the same pick up then drop
        if (currentPickUp == pickUp && currentPickUp != null)
        {
            currentPickUp.DropPickUp();
            return;
        }

        // If holding something already drop current item
        if (currentPickUp != null)
        {
            currentPickUp.DropPickUp();
        }

        if (pickUp == null) return;
        // Then pickup new holdable
        currentPickUp = pickUp;
        currentPickUp.PickUp();
    }

    public void ClearInventory()
    {
        if (inventory == null || inventory.Count == 0) return;
        currentCharges = maxCharges;
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

        playerInputs.onCigarettesStarted += ToggleSmoking;
        playerInputs.onUseStarted += UseSmoke;
    }

    private void OnDisable()
    {
        inventoryEvent.onAddItem -= AddItem;
        inventoryEvent.onRemoveItem -= RemoveItem;
        inventoryEvent.onHasItem -= HasItem;

        playerInputs.onCigarettesStarted -= ToggleSmoking;
        playerInputs.onUseStarted -= UseSmoke;
    }
}
