using UnityEngine;
using UnityEngine.Events;

public class Item : Interactable,IItem, ISpawnable
{
    [SerializeField] protected bool resetOnRemove;

    private Vector3 startPosition;
    private Vector3 startRotation;

    private Vector3 spawnPosition;
    private Vector3 spawnRotation;
    protected IInventory inventory;

    public virtual void Awake()
    {
        SetStartPosition();
    }

    public override void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent<IInventory>(out IInventory newInventory))
        {
            if (inventory != newInventory) inventory = newInventory;
        }
        TakeItem();
        gameObject.SetActive(false);
        base.Interact(interactor);
    }
    public virtual void TakeItem()
    {
        if (inventory == null) return;
        inventory.AddItem(this);
    }

    public virtual void RemoveItem()
    {
        if (inventory == null) return;
        inventory.RemoveItem(this);
        if (resetOnRemove) ResetToSpawn();
    }

    public void SetSpawnPosition(Vector3 position, Vector3 rotation)
    {
        spawnPosition = position;
        spawnRotation = rotation;
    }

    public void SetStartPosition()
    {
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
        SetSpawnPosition(startPosition, startRotation);
    }

    public void ResetToSpawn()
    {
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(spawnRotation);
    }

    public void ResetToStart()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation);
    }

    public virtual void OnEnable()
    {
        ResetToSpawn();
    }
}
