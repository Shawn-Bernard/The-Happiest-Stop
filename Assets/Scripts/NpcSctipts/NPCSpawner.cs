using UnityEngine;
using UnityEngine.Events;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private UnityEvent onNewCustomer;
    public GameObject customerPrefab;
    public GameObject currentNPC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpawnNewNPC();
    }
    public void SpawnNewNPC()
    {
        currentNPC = Instantiate(customerPrefab, gameObject.transform);
        onNewCustomer?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
