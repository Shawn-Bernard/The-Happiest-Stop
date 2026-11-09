using UnityEngine;

public interface ISpawnable
{
    /// <summary>
    /// Setting the new spawn position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void SetSpawnPosition(Vector3 position, Vector3 rotation);

    /// <summary>
    /// Setting the start position
    /// </summary>
    public void SetStartPosition();

    /// <summary>
    /// Moves to spawn position
    /// </summary>
    public void ResetToSpawn();

    /// <summary>
    /// Moves to the starting position
    /// </summary>
    public void ResetToStart();
}
