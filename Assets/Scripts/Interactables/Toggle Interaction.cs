using UnityEngine;
using UnityEngine.Events;

public class ToggleInteraction : MonoBehaviour // I didn't add this to normal interactables so it wouldn't be locked to 1 interacble
{
    [SerializeField] private UnityEvent firstInteraction;
    [SerializeField] private UnityEvent secondInteraction;

    private bool toggle;

    /// <summary>
    /// Being called will flip from first to second interaction
    /// </summary>
    public void HandleToggle() // call this on interact or 
    {
        if (!toggle)
        { 
            firstInteraction?.Invoke(); 
        }
        else
        {
            secondInteraction?.Invoke(); 
        }

        toggle = !toggle;
    }
}
