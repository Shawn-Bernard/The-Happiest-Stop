using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int maxRandomTasks = 5;
    [SerializeField] private List<Task> randomTasks; // Put in all tasks so it can be picked at random
    [SerializeField] private List<Task> startingTasks; // Tasks that will auto be added when starting
    [SerializeField] private List<Task> activeTasks; // Tasks that are active and can be completed


    [SerializeField] private TasksEvent tasksEvent;
    [SerializeField] private SanityEvent sanityEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var task in startingTasks)
        {
            AddTask(task);
        }
    }

    public void AddTask(Task task)
    {
        task.SetActive(true);
        task.OnStepChange.AddListener(UpdateTaskStep);
        task.OnTaskCompleted.AddListener(RemoveTask);
        task.OnTaskCompleted.AddListener(RewardForCompletion);
        activeTasks.Add(task);
        tasksEvent.TaskChanged();
    }

    public void RemoveTask(Task task)
    {
        if (!activeTasks.Contains(task)) return;
        task.SetActive(false);
        task.OnStepChange.RemoveListener(UpdateTaskStep);
        task.OnTaskCompleted.RemoveListener(RemoveTask);
        task.OnTaskCompleted.RemoveListener(RewardForCompletion);
        activeTasks.Remove(task);
        tasksEvent.TaskChanged();
    }

    public void AddRandomTask(Task task)
    {
        if (randomTasks.Contains(task)) return;
        randomTasks.Add(task);
    }

    public void RemoveRandomTask(Task task)
    {
        if (!randomTasks.Contains(task)) return;
        randomTasks.Remove(task);
    }

    private void RewardForCompletion(Task task)
    {
        sanityEvent.AddSanity(task.SanityGiven);
    }
    /// <summary>
    /// Picks a random task that not already in active task and add to active tasks
    /// </summary>
    public void PickRandomTask()
    {
        if (GetActiveRandomTaskCount() >= maxRandomTasks) return;

        List<Task> availableTasks = new List<Task>();

        foreach (Task task in randomTasks)
        {
            if (!activeTasks.Contains(task))
            {
                //Debug.Log($"{task.GetTaskName()} was picked for random task pool");
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
        int currentRandomTasks = GetActiveRandomTaskCount();

        for (int i = currentRandomTasks; i < maxRandomTasks; i++)
        {
            if (GetActiveRandomTaskCount() >= maxRandomTasks) return;
            PickRandomTask();
        }
    }

    /// <summary>
    /// Removes untouched tasks and refills empty slots for random tasks
    /// </summary>
    public void SoftReset()
    {
        for (int i = activeTasks.Count - 1; i >= 0; i--)
        {
            Task task = activeTasks[i];
            if (!task.IsTouched() && randomTasks.Contains(task))
            {
                task.CancelTask();
                RemoveTask(task);
            }
        }
        FillActiveTask();
    }
    private int GetActiveRandomTaskCount()
    {
        int activeCount = 0;
        foreach (Task task in activeTasks)
        {
            if (randomTasks.Contains(task))
            {
                activeCount++;
            }
        }
        return activeCount;
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

    private string GetTaskReward(int taskIndex)
    {
        if (taskIndex < 0 || taskIndex >= activeTasks.Count)
            return "";
        if (activeTasks[taskIndex].SanityGiven == 0)
            return "";

        return $"+{activeTasks[taskIndex].SanityGiven} Sanity";
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
        tasksEvent.onGetTaskReward += GetTaskReward;

        tasksEvent.onAddTask += AddTask;
        tasksEvent.onRemoveTask += RemoveTask;
        tasksEvent.onAddRandomTask += AddRandomTask;
        tasksEvent.onRemoveRandomTask += RemoveRandomTask;
    }

    private void OnDisable()
    {
        tasksEvent.onGetTaskName -= GetTaskName;
        tasksEvent.onGetTaskStep -= GetTaskStep;
        tasksEvent.onGetTaskReward -= GetTaskReward;

        tasksEvent.onAddTask -= AddTask;
        tasksEvent.onRemoveTask -= RemoveTask;
        tasksEvent.onAddRandomTask -= AddRandomTask;
        tasksEvent.onRemoveRandomTask -= RemoveRandomTask;
    }
}
