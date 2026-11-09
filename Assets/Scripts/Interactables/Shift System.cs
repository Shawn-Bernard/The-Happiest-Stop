using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ShiftSystem : MonoBehaviour
{
    private int currentTime;
    [Range(0f, 24)]
    [SerializeField] private int endTime;
    [SerializeField] private int hourTime;

    [SerializeField] private UnityEvent onShiftStarted;
    [SerializeField] private UnityEvent onShiftFinished;
    [SerializeField] private UnityEvent<int> onTimeChange;
 
    private Coroutine shiftTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartShift()
    {
        if (shiftTime == null)
        {

            Debug.Log("Shift Started");
            shiftTime = StartCoroutine(HandleShift());
        }
    }

    public void EndShift()
    {
        Debug.Log("Shift Ended");
        StopAllCoroutines();
    }

    public IEnumerator HandleShift()
    {
        onShiftStarted?.Invoke();
        while (currentTime < endTime)
        {
            yield return new WaitForSeconds(hourTime);
            currentTime++;
            onTimeChange?.Invoke(currentTime);
        }

        onShiftFinished?.Invoke();
        shiftTime = null;
    }
}
