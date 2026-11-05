using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> allTasks; // Put in all tasks so it can be picked at random
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
