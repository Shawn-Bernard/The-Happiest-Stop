using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Tasks Event", menuName = "Events/Tasks Event")]
public class TasksEvent : ScriptableObject
{
    public Func<int, string> onGetTaskName;
    public Func<int, string> onGetTaskStep;
    public UnityEvent<int> onUpdateTaskStep;
    public UnityEvent onTaskChanged = new UnityEvent();

    public string GetTaskName(int taskIndex) => onGetTaskName?.Invoke(taskIndex);

    public string GetTaskStep(int taskIndex) => onGetTaskStep?.Invoke(taskIndex);

    public void UpdateTaskStep(int taskIndex) => onUpdateTaskStep?.Invoke(taskIndex);

    public void TaskChanged() => onTaskChanged?.Invoke();
}
