using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnLocation;

    public string SpawnLocation => spawnLocation;
    public Vector3 Position => transform.position;
    public Vector3 Rotation => transform.eulerAngles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<ISpawnable>(out ISpawnable spawnable))
            {
                spawnable.SetSpawnPosition(transform.position, transform.rotation.eulerAngles);
            }
        }
    }
}
