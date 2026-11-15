using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Sanity Event", menuName = "Events/Sanity Event")]
public class SanityEvent : ScriptableObject
{
    public UnityAction onStartSanity;
    public UnityAction onEndSanity;
    public UnityAction<float> onSanityChange;

    public UnityAction onHighSanity;
    public UnityAction onLowSanity;
    public UnityAction onLowerSanity;
    public UnityAction onLowestSanity;
    public UnityAction<float> onAddSanity;


    public void StartSanity() => onStartSanity?.Invoke();

    public void EndSanity() => onEndSanity?.Invoke();

    public void SanityChange(float newSanity) => onSanityChange?.Invoke(newSanity);

    public void HighSanity() => onHighSanity?.Invoke();

    public void LowSanity() => onLowSanity?.Invoke();

    public void LowerSanity() => onLowerSanity?.Invoke();

    public void LowestSanity() => onLowestSanity?.Invoke();

    public void AddSanity(float sanity) => onAddSanity?.Invoke(sanity);
}
