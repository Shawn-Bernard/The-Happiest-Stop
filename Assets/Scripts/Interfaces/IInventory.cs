using UnityEngine;

public interface IInventory 
{
    public void AddItem(IItem item);

    public void RemoveItem(IItem item);

    public bool HasItem(IItem item);

    public void HoldItem(IPickUp pickUp);

    public void DropItem();
}
