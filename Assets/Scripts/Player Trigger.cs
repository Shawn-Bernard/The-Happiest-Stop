using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;

    [SerializeField] bool IgnoreEnter;
    [SerializeField] private UnityEvent onExit;

    [SerializeField] bool IgnoreExit;

    private void OnTriggerEnter(Collider other)
    {
        if (IgnoreEnter) return;
        if (other.CompareTag("Player"))
        {
            onEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IgnoreExit) return;
        if (other.CompareTag("Player"))
        {
            onExit?.Invoke();
        }
    }
}
