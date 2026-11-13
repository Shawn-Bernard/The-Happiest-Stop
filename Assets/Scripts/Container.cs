using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private Item itemGiven;

    /// <summary>
    /// Used to give inventory to item
    /// Gets called onInteract event that gives a gameobject
    /// Call before giving items
    /// </summary>
    /// <param name="Interactor"></param>
    public void GetInventory(GameObject Interactor)
    {
        if (itemGiven == null) return;

        itemGiven.Interact(Interactor);
    }

    /// <summary>
    /// USed to handle giving a item or holdable
    /// </summary>
    public void GiveItem()
    {
        if (itemGiven == null) return;

        if (itemGiven is Holdable holdable)
        {
            holdable.PickUp();
        }
        else
        {
            itemGiven.TakeItem();
        }
    }

    /// <summary>
    /// Used to remove item or holdable
    /// </summary>
    public void RemoveItem()
    {
        if (itemGiven == null) return;

        if (itemGiven is Holdable holdable)
        {
            holdable.DropPickUp();
        }
        else
        {
            itemGiven.RemoveItem();
        }
    }

}
