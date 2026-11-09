using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterState;

    [SerializeField] private UnityEvent onExitState;
    public void EnterState() => onEnterState?.Invoke();

    public void ExitState() => onExitState?.Invoke();
}
