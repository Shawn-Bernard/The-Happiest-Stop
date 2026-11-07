using UnityEngine;
using UnityEngine.Events;

public class PickUp : Interactable, IPickUp
{
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private UnityEvent onPickUp;
    [SerializeField] private UnityEvent onDropPickUp;

    /// <summary>
    /// Calls on pickup event for disabling hit box
    /// </summary>
    public void OnPickUp()
    {
        onPickUp?.Invoke();
        SetOffset();
    }

    public void OnDropPickUp()
    {
        onDropPickUp?.Invoke();
    }

    public void SetOffset()
    {
        transform.localPosition = offsetPosition;
        transform.localRotation = Quaternion.Euler(offsetRotation);
    }
}
