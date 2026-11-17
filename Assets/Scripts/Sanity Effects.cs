using UnityEngine;
using UnityEngine.Events;

public class SanityEffects : MonoBehaviour // I plan to use this for effects, mainly post processing or radio sounds
{
    [SerializeField] private SanityEvent sanityEvent;

    [SerializeField] private UnityEvent highSanity;
    [SerializeField] private UnityEvent lowSanity;
    [SerializeField] private UnityEvent lowerSanity;
    [SerializeField] private UnityEvent lowestSanity;

    public void HighSanity() => highSanity?.Invoke();
    public void LowSanity() => lowSanity?.Invoke();
    public void LowerSanity() => lowerSanity?.Invoke();
    public void LowestSanity() => lowestSanity?.Invoke();

    private void OnEnable()
    {
        sanityEvent.onHighSanity += HighSanity;
        sanityEvent.onLowSanity += LowSanity;
        sanityEvent.onLowerSanity += LowerSanity;
        sanityEvent.onLowestSanity += LowestSanity;
    }

    private void OnDisable()
    {
        sanityEvent.onHighSanity -= HighSanity;
        sanityEvent.onLowSanity -= LowSanity;
        sanityEvent.onLowerSanity -= LowerSanity;
        sanityEvent.onLowestSanity -= LowestSanity;
    }
}
