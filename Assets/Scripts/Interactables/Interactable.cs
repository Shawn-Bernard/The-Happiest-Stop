using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool activeOnStart;
    [SerializeField] protected UnityEvent<GameObject> onInteract;
    
    [SerializeField] private bool canInteract;
    [SerializeField] protected string interactableName;

    public virtual void Start()
    {
        canInteract = activeOnStart;
    }

    public virtual void Interact(GameObject interactor)
    {
        //Debug.Log($"Interact has happend on {gameObject.name}");
        onInteract?.Invoke(interactor);
    }

    public void SetActive(bool state)
    {
        canInteract = state;
    }

    public bool GetInteractableStatus()
    {
        return canInteract;
    }

    public string GetInteractableName() => interactableName;

    public void SetInteractableName(string newName)
    {
        interactableName = newName;
    }
}
