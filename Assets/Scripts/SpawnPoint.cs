using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnLocation;

    public string SpawnLocation => spawnLocation;
    public Vector3 Position => transform.position;
    public Vector3 Rotation => transform.eulerAngles;

    private GameObject player;
    private void Start()
    {
        player = GameManager.Instance.Player;
        if (player.TryGetComponent<ISpawnable>(out ISpawnable spawnable))
        {
            spawnable.SetSpawnPosition(transform.position, transform.rotation.eulerAngles);
            spawnable.ResetToSpawn();
        }
    }

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
