using UnityEngine;

public interface ITask
{
    public void StartedTask();

    public void CompleteTask();

    public void CancelTask();

    public bool IsTouched();

    public void ResetTask();
}
