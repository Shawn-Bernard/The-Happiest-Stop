using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Inventory Event",menuName = "Events/Inventory Event")]
public class InventoryEvent : ScriptableObject
{
    public UnityAction<IItem> onAddItem;
    public UnityAction<IItem> onRemoveItem;
    public Func<IItem, bool> onHasItem;
    public UnityAction onInventoryChanged;

    public void AddItem(IItem item)
    {
        onAddItem?.Invoke(item);
    }

    public void RemoveItem(IItem item)
    {
        onRemoveItem?.Invoke(item);
    }

    public bool HasItem(IItem item) => onHasItem?.Invoke(item) ?? false;

    public void InventoryChanged() => onInventoryChanged?.Invoke();
}