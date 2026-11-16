using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NPCSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public GameObject currentNPC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnNpc());
    }
    public void SpawnNewNPC()
    {
        if (currentNPC == null)
        {
            currentNPC = Instantiate(customerPrefab, gameObject.transform);
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
