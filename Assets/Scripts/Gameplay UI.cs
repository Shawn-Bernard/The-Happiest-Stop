using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;

    [Header("Shift Time Related")]
    [SerializeField] TextMeshProUGUI shiftTimeText;
    [SerializeField] private IntEvent shiftTimeEvent;

    [Header("Tasks Related")]
    [SerializeField] private TasksEvent tasksEvent;
    [SerializeField] GameObject taskPanel;
    [SerializeField] GameObject taskList;

    [Header("Interactables")]
    [SerializeField] TextMeshProUGUI interactableTypeText;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] Slider interactableSlider;
    private Coroutine holdCoroutine;

    [SerializeField] private List<TaskSlot> taskSlots;

    [Header("Sanity Related")]
    [SerializeField] Slider SanitySlider;
    [SerializeField] SanityEvent sanityEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        interactableSlider.value = 0;
        taskSlots = new List<TaskSlot>();
        taskList ??= taskPanel.GetComponentInChildren<Canvas>().gameObject;
        GetTasksListInfo();
    }

    private void SetTime(int currentTime)
    {
        Debug.Log("Time change in ui");
        int displayedTime = currentTime % 12;

        if (displayedTime == 0) displayedTime = 12;

        /*
        string period;

        if (currentTime < 12)
        {
            period = "AM";
        }
        else
        {
            period = "PM";
        }
        */
        shiftTimeText.text = $"{displayedTime} AM";
    }

    #region Tasks Logic

    /// <summary>
    /// Toggle for task list menu
    /// </summary>
    public void HandleTaskUI()
    {
        taskPanel.SetActive(!taskPanel.activeInHierarchy);
    }

    /// <summary>
    /// Gets the task name and current step
    /// </summary>
    public void SetTaskUIInfo()
    {
        int index = 0;

        foreach (var taskSlot in taskSlots)
        {
            taskSlot.SetTaskName(tasksEvent.GetTaskName(index));
            taskSlot.SetRewardText(tasksEvent.GetTaskReward(index));
            taskSlot.SetStepText(tasksEvent.GetTaskStep(index));
            index++;
        }
    }

    /// <summary>
    /// Will get all child from task list object and add them to a Dictionary for task name and steps
    /// </summary>
    public void GetTasksListInfo()
    {
        taskSlots = taskList.GetComponentsInChildren<TaskSlot>().ToList();
    }

    /// <summary>
    /// Calls when task step
    /// </summary>
    /// <param name="taskIndex"></param>
    public void UpdateTaskUIStep(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex > taskSlots.Count) return;
        taskSlots[taskIndex].SetStepText(tasksEvent.GetTaskStep(taskIndex));
    }
    #endregion

    public void HandleInteractable(IInteractable interactable)
    {
        if (interactable == null)
        {
            interactableTypeText.gameObject.SetActive(false);
            return;
        }
        interactableTypeText.gameObject.SetActive(true);

        if (interactable is HoldInteractable)
        {
            interactableTypeText.text = $"Hold E";
            interactableSlider.gameObject.SetActive(true);
        }
        else
        {
            interactableTypeText.text = $"Tap E";
            interactableSlider.gameObject.SetActive(false);
        }

        actionText.text = interactable.GetInteractableName();
    }
    public void HandleHoldInteractable(IHoldInteractable interactable)
    {
        if (interactable == null) return;

        if (holdCoroutine != null) StopCoroutine(holdCoroutine);

        holdCoroutine = StartCoroutine(UpdateHoldSlider(interactable.HoldTime));
    }

    private IEnumerator UpdateHoldSlider(float holdTime)
    {
        float elapsedTime = 0f;
        interactableSlider.value = 0f;

        while (elapsedTime < holdTime)
        {
            elapsedTime += Time.deltaTime;

            interactableSlider.value = elapsedTime / holdTime;

            yield return null;
        }

        interactableSlider.value = 1f;

        holdCoroutine = null;
    }
    public void StopHoldInteractable()
    {
        interactableSlider.value = 0f;
        if (holdCoroutine == null) return;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }
    public void SetSanity(float sanity)
    {
        SanitySlider.value = Mathf.Clamp01(sanity / 100);
    }

    public void HideSanity()
    {
        SanitySlider.gameObject.SetActive(false);
    }

    public void ShowSanity()
    {
        SanitySlider.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        playerInputs.onTaskStarted += HandleTaskUI;
        playerInputs.onInteractCanceled += StopHoldInteractable;

        shiftTimeEvent.onRaiseInt += SetTime;

        tasksEvent.onUpdateTaskStep.AddListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.AddListener(SetTaskUIInfo);

        sanityEvent.onSanityChange += SetSanity;
        sanityEvent.onStartSanity += ShowSanity;
        sanityEvent.onEndSanity += HideSanity;
    }

    private void OnDisable()
    {
        playerInputs.onTaskStarted -= HandleTaskUI;
        playerInputs.onInteractCanceled -= StopHoldInteractable;

        shiftTimeEvent.onRaiseInt -= SetTime;

        tasksEvent.onUpdateTaskStep.RemoveListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.RemoveListener(SetTaskUIInfo);

        sanityEvent.onSanityChange -= SetSanity;
        sanityEvent.onStartSanity -= ShowSanity;
        sanityEvent.onEndSanity -= HideSanity;
    }
}
