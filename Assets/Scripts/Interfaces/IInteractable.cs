using UnityEngine;

public interface IInteractable 
{
    public void Interact();

    public bool GetInteractableStatus();

    public void SetActive(bool state);

    public string GetInteractableName();
}
