using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Inventory Event",menuName = "Events/Inventory Event")]
public class InventoryEvent : ScriptableObject
{
    public UnityAction<Interactable> onAddItem;
    public UnityAction<Interactable> onRemoveItem;
    public Func<Interactable, bool> onHasItem;

    public void AddItem(Interactable item) => onAddItem?.Invoke(item);

    public void RemoveItem(Interactable item) => onRemoveItem?.Invoke(item);

    public bool HasItem(Interactable item) => onHasItem?.Invoke(item) ?? false;
}