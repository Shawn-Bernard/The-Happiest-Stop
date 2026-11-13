using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class HoldInteractable : MonoBehaviour, IHoldInteractable, IInteractable
{
    [Range(.5f, 4f)]
    [SerializeField] protected float holdTime;

    [SerializeField] protected UnityEvent onHoldCompleted;

    private Coroutine holdCoroutine;

    [SerializeField] protected bool activeOnStart;
    [SerializeField] protected UnityEvent<GameObject> onInteract;

    [SerializeField] private bool canInteract;
    [SerializeField] protected string interactableName;

    public float HoldTime => holdTime;

    public virtual void Start()
    {
        canInteract = activeOnStart;
    }
    public void StartHold()
    {
        //Debug.Log("Hold Started");
        holdCoroutine = StartCoroutine(HoldInteract());
    }
    /// <summary>
    /// Called when player looks away from the interactable
    /// </summary>
    public void HoldInteractCompleted()
    {
        Debug.Log("Hold Completed");
        onHoldCompleted?.Invoke();
        holdCoroutine = null;
    }

    public void HoldInteractFailed()
    {
        if (holdCoroutine == null) return;
        Debug.Log("Hold Stopped");
        StopCoroutine(holdCoroutine);
        holdCoroutine = null;
    }

    private IEnumerator HoldInteract()
    {
        yield return new WaitForSeconds(holdTime);
        HoldInteractCompleted();
    }
    public virtual void Interact(GameObject interactor)
    {
        //Debug.Log($"Interact has happend on {gameObject.name}");
        onInteract?.Invoke(interactor);
        StartHold();
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
