using UnityEngine;

public interface ITask
{
    public void StartedTask();

    public void CompleteTask();

    public void CancelTask();

    public bool GetTaskStatus();

    public void ResetTask();
}
