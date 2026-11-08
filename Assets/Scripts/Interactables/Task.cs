using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour, ITask
{
    [SerializeField] private string taskName;

    private IInteractable[] interactablesInvolved;
    [SerializeField] private string[] steps;
    [Header("Events")]

    [SerializeField] private UnityEvent onTaskStarted;

    [SerializeField] private UnityEvent<Task> onTaskCompleted;
    public UnityEvent<Task> OnTaskCompleted => onTaskCompleted; // for updaing in ui. task already auto assigned

    [SerializeField] private UnityEvent onTaskReset;
    [SerializeField] private UnityEvent<Task> onStepChange;
    public UnityEvent<Task> OnStepChange => onStepChange; // for updaing in ui. task already auto assigned

    [Header("Debug things")]
    private int currentStepIndex;
    [SerializeField] private string currentStep;
    private bool isCompleted;
    private bool isTouched; // For whenever player moves a step
    private bool isActive;

    public void Start()
    {
        currentStepIndex = 0;
        currentStep = steps[currentStepIndex];
        GetInteractables();
    }

    private void GetInteractables()
    {
        interactablesInvolved = GetComponentsInChildren<IInteractable>();
        //Debug.Log($"{taskName} has {interactablesInvolved.Length} interactables");
    }

    #region Steps
    public void NextStep()
    {
        if (isCompleted) return;
        if (!isTouched) isTouched = true;
        //Debug.Log("Next step asked");

        currentStepIndex++;

        if (currentStepIndex >= steps.Length)
        {
            CompleteTask();
            return;
        }

        currentStep = steps[currentStepIndex];
        onStepChange?.Invoke(this);
    }
    /// <summary>
    /// Checks if the task can advance to the next step
    /// the current index can't proceed if its + 1 of the step index
    /// example : current index[2] can only go to the next step if its step Index [2] + 1 = 3 
    /// </summary>
    /// <param name="stepIndex"></param>
    public void NextStep(int stepIndex)
    {
        if (isCompleted) return;
        if (currentStepIndex == stepIndex + 1) return;

        NextStep();
    }

    public void LastStep()
    {
        if (currentStepIndex == 0) return;
        if (!isTouched) isTouched = true;
        currentStepIndex--;
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length) return;
        currentStep = steps[currentStepIndex];
        onStepChange?.Invoke(this);
    }
    /// <summary>
    /// Checks if the task can go back to the last step
    /// the current index can go back if it's + 1 of the step index
    /// example: current index [2] can go back to step index [1] because [1] + 1 = 2
    /// </summary>
    /// <param name="stepIndex"></param>
    public void LastStep(int stepIndex)
    {
        if (isCompleted) return; 
        if (currentStepIndex != stepIndex + 1) return;

        LastStep();
    }

    public void UpdateStep(string stepToAdd, int stepIndex)
    {
        if (isCompleted) return;
        if (stepIndex < 0 || stepIndex >= steps.Length) return;
        if (!isTouched) isTouched = true;

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
        //Debug.Log($"{taskName} completed");
        isCompleted = true;
        onTaskCompleted?.Invoke(this);
        foreach (var interactable in interactablesInvolved)
        {
            interactable.SetActive(false);
        }
    }

    public bool IsTouched() => isTouched;

    public void ResetTask()
    {
        isTouched = false;
        onTaskReset?.Invoke();
    }

    public void SetActive(bool state)
    {
        isActive = state;
        if (isActive)
        {
            isCompleted = false;
            StartedTask();
            ResetTask();
        }
        else
        {
            foreach (var interactable in interactablesInvolved)
            {
                interactable.SetActive(false);
            }
        }
        
    }

    public void StartedTask()
    {
        onTaskStarted?.Invoke();
        foreach (var interactable in interactablesInvolved)
        {
            interactable.SetActive(true);
        }
    }

    public string GetTaskName() => taskName;
    #endregion


}
