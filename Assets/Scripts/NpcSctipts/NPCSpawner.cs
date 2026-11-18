using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
public class NPCSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject currentNPC;
    [SerializeField] private UnityEvent onNewCustomer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartSpawner()
    {
        StartCoroutine(SpawnNpc());
    }
    public void StopSpawner()
    {
        StopCoroutine(SpawnNpc());
    }
    public void SpawnNewNPC()
    {
        if (currentNPC == null)
        {
            currentNPC = Instantiate(customerPrefab, gameObject.transform);
            onNewCustomer?.Invoke();
            Debug.Log("new customer");
        }
        else
        {
            Debug.Log("Failed (already a customer)");
        }
    }

    public IEnumerator SpawnNpc()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            SpawnNewNPC();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
