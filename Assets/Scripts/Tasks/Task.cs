using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour, ITask
{
    [SerializeField] private string taskName;

    [SerializeField] private Interactable[] interactablesInvolved;
    [SerializeField] private string[] steps;
    [SerializeField] private UnityEvent onTaskCompleted;
    [SerializeField] private UnityEvent onTaskReset;

    private int currentStepIndex;
    [SerializeField] private string currentStep;
    [SerializeField] private bool isCompleted;
    [SerializeField] private bool isActive;
    [SerializeField] private bool hasStarted;

    public void Start()
    {
        currentStep = steps[currentStepIndex];
    }

    public void NextStep()
    {
        if (isCompleted || !isActive) return;
        Debug.Log("Next step asked");

        currentStepIndex++;

        if (currentStepIndex >= steps.Length)
        {
            CompleteTask();
            return;
        }

        currentStep = steps[currentStepIndex];
        Debug.Log($"Current step {steps[currentStepIndex]}");
    }

    public void LastStep()
    {
        if (currentStepIndex == 0) return;
        Debug.Log("last step asked");
        currentStepIndex--;
        currentStep = steps[currentStepIndex];
        Debug.Log($"Current step {steps[currentStepIndex]}");
    }

    public void UpdateStep(string stepToAdd, int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= steps.Length) return;

        steps[stepIndex] = stepToAdd;

        Debug.Log($"Step changed {steps[stepIndex]}");
    }

    public void CancelTask()
    {
        isActive = false;
        foreach (var interactable in interactablesInvolved)
        {
            interactable.SetActive(false);
        }
    }

    public void CompleteTask()
    {
        if (isCompleted) return;

        Debug.Log($"{taskName} completed");
        isCompleted = true;
        onTaskCompleted?.Invoke();
    }

    public bool GetTaskStatus()
    {
        return isActive;
    }

    public void ResetTask()
    {
        isCompleted = false;
        onTaskReset?.Invoke();
    }

    public void SetActive()
    {
        isActive = true;
        StartedTask();
        Debug.Log($"Current step {steps[currentStepIndex]}");
    }

    public void StartedTask()
    {
        foreach (var interactable in interactablesInvolved)
        {
            interactable.SetActive(true);
        }
    }
}
