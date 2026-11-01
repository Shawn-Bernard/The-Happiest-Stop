using UnityEngine;

public interface IHoldInteractable : IInteractable
{
    void HoldInteractCompleted();

    void HoldInteractFailed();
}