using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool activeOnStart;
    [SerializeField] protected UnityEvent onInteract;
    
    private bool canInteract;

    private void Start()
    {
        canInteract = activeOnStart;
    }
    public void Interact()
    {
        Debug.Log($"Interact has happend on {gameObject.name}");
        onInteract?.Invoke();
    }

    public void SetActive(bool state)
    {
        canInteract = state;
    }

    public bool GetInteractableStatus()
    {
        return canInteract;
    }
}
