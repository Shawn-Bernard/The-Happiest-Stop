using UnityEngine;

public class Item : Interactable
{
    [SerializeField] InventoryEvent inventoryEvent;
    private Vector3 startPosition;
    private Vector3 startRotation;

    private void Awake()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(startRotation.x, startRotation.y, startRotation.z);
    }

    public void SetPosition()
    {
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
    }

    public void PickedUp()
    {
        inventoryEvent.AddItem(this);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ResetPosition();
        onInteract.AddListener(PickedUp);
    }

    private void OnDisable()
    {
        onInteract.RemoveListener(PickedUp);
    }
}
