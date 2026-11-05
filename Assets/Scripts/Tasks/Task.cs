using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour, ITask
{
    [SerializeField] private string taskName;

    [SerializeField] private Interactable[] interactablesInvolved;
    [SerializeField] private string[] steps;
    [SerializeField] private UnityEvent<Task> onTaskCompleted;
    public UnityEvent<Task> OnTaskCompleted => onTaskCompleted; // For auto adding to task manager
    [SerializeField] private UnityEvent onTaskReset;
    [SerializeField] private UnityEvent<Task> onStepChange;
    public UnityEvent<Task> OnStepChange => onStepChange; // For auto adding to task manager

    private int currentStepIndex;
    [SerializeField] private string currentStep;
    [SerializeField] private bool isCompleted;
    private bool isTouched;

    public void Start()
    {
        currentStep = steps[currentStepIndex];
    }

    #region Steps
    public void NextStep()
    {
        if (isCompleted) return;
        if (!isTouched) isTouched = true;
        Debug.Log("Next step asked");

        currentStepIndex++;

        if (currentStepIndex >= steps.Length)
        {
            CompleteTask();
            return;
        }

        currentStep = steps[currentStepIndex];
        onStepChange?.Invoke(this);
    }

    public void LastStep()
    {
        if (currentStepIndex == 0) return;
        if (!isTouched) isTouched = true;
        currentStepIndex--;
        currentStep = steps[currentStepIndex];
        onStepChange?.Invoke(this);
    }

    public void UpdateStep(string stepToAdd, int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= steps.Length) return;

        steps[stepIndex] = stepToAdd;
        if (currentStepIndex == stepIndex) currentStep = steps[stepIndex];
        onStepChange?.Invoke(this);
    }
    public string GetCurrentStep() => currentStep;

    #endregion

    #region Task 
    public void CancelTask()
    {
        isTouched = false;
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
        onTaskCompleted?.Invoke(this);
    }

    public bool IsTouched() => isTouched;

    public void ResetTask()
    {
        isCompleted = false;
        onTaskReset?.Invoke();
    }

    public void SetActive()
    {
        isCompleted = false;
        isTouched = false;
        StartedTask();
        ResetTask();
    }

    public void StartedTask()
    {
        foreach (var interactable in interactablesInvolved)
        {
            interactable.SetActive(true);
        }
    }

    public string GetTaskName() => taskName;
    #endregion


}
