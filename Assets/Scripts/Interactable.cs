using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] protected UnityEvent onInteract; 
    public void Interact()
    {
        Debug.Log($"Interact has happend on {gameObject.name}");
        onInteract?.Invoke();
    }
}
