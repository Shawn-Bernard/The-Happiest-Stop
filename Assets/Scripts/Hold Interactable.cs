using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class HoldInteractable : Interactable, IHoldInteractable
{
    [Range(.5f, 4f)]
    [SerializeField] protected float holdTime;

    [SerializeField] protected UnityEvent onHoldCompleted;

    private Coroutine holdCoroutine;

    public void StartHold()
    {
        Debug.Log("Hold Started");
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

    public virtual void OnEnable()
    {
        onInteract.AddListener(StartHold);
    }

    public virtual void OnDisable()
    {
        onInteract.RemoveListener(StartHold);
    }

}
