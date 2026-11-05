using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] private TasksEvent tasksEvent;
    [SerializeField] GameObject taskPanel;
    [SerializeField] GameObject taskList;
    

    //[SerializeField] private List<TextMeshProUGUI> tasksInfo;
    private Dictionary<TextMeshProUGUI, TextMeshProUGUI> tasksInfo; // For tasks names and steps
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tasksInfo = new Dictionary<TextMeshProUGUI, TextMeshProUGUI>();
        taskList ??= taskPanel.GetComponentInChildren<Canvas>().gameObject;
        GetTasksListInfo();
        SetTaskUIInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void OnEnable()
    {
        playerInputs.onTaskStarted += HandleTaskUI;
        tasksEvent.onUpdateTaskStep.AddListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.AddListener(SetTaskUIInfo);
    }

    private void OnDisable()
    {
        playerInputs.onTaskStarted -= HandleTaskUI;
        tasksEvent.onUpdateTaskStep.RemoveListener(UpdateTaskUIStep);
        tasksEvent.onTaskChanged.RemoveListener(SetTaskUIInfo);
    }
}
