using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int Event", menuName = "Events/Int Event")]
public class IntEvent : ScriptableObject
{
    public UnityAction<int> onRaiseInt;

    public void Raise(int value) => onRaiseInt?.Invoke(value);
}
