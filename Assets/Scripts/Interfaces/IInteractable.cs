using UnityEngine;

public interface IInteractable 
{
    /// <summary>
    /// interactor used to get things like inventory
    /// </summary>
    /// <param name="interactor"></param>
    public void Interact(GameObject interactor);

    public bool GetInteractableStatus();

    public void SetActive(bool state);

    public string GetInteractableName();

    public void SetInteractableName(string newName);
}
