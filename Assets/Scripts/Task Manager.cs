using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private int maxTasks = 5;
    [SerializeField] private List<Task> randomTasks; // Put in all tasks so it can be picked at random
    [SerializeField] private List<Task> startingTasks; // Tasks that will auto be added when starting
    [SerializeField] private List<Task> activeTasks; // Tasks that are active and can be completed


    [SerializeField] private TasksEvent tasksEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var task in startingTasks)
        {
            AddTask(task);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTask(Task task)
    {
        task.SetActive();
        task.OnStepChange.AddListener(UpdateTaskStep);
        task.OnTaskCompleted.AddListener(RemoveTask);
        activeTasks.Add(task);
        tasksEvent.TaskChanged();
    }

    public void RemoveTask(Task task)
    {
        if (!activeTasks.Contains(task)) return;
        task.OnStepChange.RemoveListener(UpdateTaskStep);
        task.OnTaskCompleted.RemoveListener(RemoveTask);
        activeTasks.Remove(task);
        tasksEvent.TaskChanged();
    }
    /// <summary>
    /// Picks a random task that not already in active task and add to active tasks
    /// </summary>
    public void PickRandomTask()
    {
        if (activeTasks.Count >= maxTasks) return;

        List<Task> availableTasks = new List<Task>();

        foreach (Task task in randomTasks)
        {
            if (!activeTasks.Contains(task))
            {
                availableTasks.Add(task);
            }
        }

        if (availableTasks.Count == 0) return;

        int randomIndex = Random.Range(0, availableTasks.Count);

        AddTask(availableTasks[randomIndex]);
    }
    /// <summary>
    /// For loop that refills active tasks with random tasks
    /// </summary>
    public void FillActiveTask()
    {
        for (int i = activeTasks.Count; i < maxTasks; i++)
        {
            if (activeTasks.Count >= maxTasks) return;
            PickRandomTask();
        }
    }

    /// <summary>
    /// Removes untouched tasks and refills empty slots for random tasks
    /// </summary>
    public void SoftReset()
    {
        foreach (Task task in activeTasks)
        {
            if (!task.IsTouched())
            {
                task.CancelTask();
                RemoveTask(task);
            }
        }
        FillActiveTask();
    }

    #region UI Related logic
    public string GetTaskName(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= activeTasks.Count)
            return "";

        return activeTasks[taskIndex].GetTaskName();
    }

    public string GetTaskStep(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= activeTasks.Count)
            return "";

        return activeTasks[taskIndex].GetCurrentStep();
    }

    public string GetTaskInfo(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= activeTasks.Count) return "";
        string taskInfo;
        return taskInfo = $"{GetTaskName(taskIndex)}: {GetTaskStep(taskIndex)}";
    }
    public void UpdateTaskStep(Task task)
    {
        if (!activeTasks.Contains(task)) return;

        int taskIndex = activeTasks.IndexOf(task);

        tasksEvent.UpdateTaskStep(taskIndex);
    }
    #endregion

    

    private void OnEnable()
    {
        tasksEvent.onGetTaskName += GetTaskName;
        tasksEvent.onGetTaskStep += GetTaskStep;
    }

    private void OnDisable()
    {
        tasksEvent.onGetTaskName -= GetTaskName;
        tasksEvent.onGetTaskStep -= GetTaskStep;
    }
}
