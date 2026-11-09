using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject currentNPC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnNewNPC();
    }
    public void SpawnNewNPC()
    {
        currentNPC = Instantiate(customerPrefab, gameObject.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
