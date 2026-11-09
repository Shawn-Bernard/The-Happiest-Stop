using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Sanity Event", menuName = "Events/Sanity Event")]
public class SanityEvent : ScriptableObject
{
    public UnityEvent onStartSanity;

    public void StartSanity() => onStartSanity?.Invoke();
}
