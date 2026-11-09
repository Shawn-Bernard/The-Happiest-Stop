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

    [Header("Interactables Related")]
    [SerializeField] TextMeshProUGUI interactableTypeText;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] Slider interactableSlider;
    private Coroutine holdCoroutine;

    [Header("Sanity Related")]
    [SerializeField] Slider SanitySlider;
    //[SerializeField] private List<TextMeshProUGUI> tasksInfo;
    private Dictionary<TextMeshProUGUI, TextMeshProUGUI> tasksInfo; // For tasks names and steps

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        interactableSlider.value = 0;
        tasksInfo = new Dictionary<TextMeshProUGUI, TextMeshProUGUI>();
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

        foreach (var task in tasksInfo)
        {
            task.Key.text = tasksEvent.GetTaskName(index);
            task.Value.text = tasksEvent.GetTaskStep(index);

            index++;
        }
    }

    /// <summary>
    /// Will get all child from task list object and add them to a Dictionary for task name and steps
    /// </summary>
    public void GetTasksListInfo()
    {
        for (int i = 0; i < taskList.transform.childCount; i++)
        {
            Transform task = taskList.transform.GetChild(i);

            TextMeshProUGUI[] texts = task.GetComponentsInChildren<TextMeshProUGUI>(true);

            tasksInfo.Add(texts[0], texts[1]);
        }
    }

    /// <summary>
    /// Calls when task step
    /// </summary>
    /// <param name="taskIndex"></param>
    public void UpdateTaskUIStep(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= tasksInfo.Count) return;

        var task = tasksInfo.ElementAt(taskIndex);
        task.Value.text = tasksEvent.GetTaskStep(taskIndex);
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

    private void OnEnable()
    {
        playerInputs.onTaskStarted += HandleTaskUI;
        playerInputs.onInteractCanceled += StopHoldInteractable;

        shiftTimeEvent.onRaiseInt += SetTime;

        tasksEvent.onUpdateTaskStep.AddListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.AddListener(SetTaskUIInfo);
    }

    private void OnDisable()
    {
        playerInputs.onTaskStarted -= HandleTaskUI;
        playerInputs.onInteractCanceled -= StopHoldInteractable;

        shiftTimeEvent.onRaiseInt -= SetTime;

        tasksEvent.onUpdateTaskStep.RemoveListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.RemoveListener(SetTaskUIInfo);
    }
}
