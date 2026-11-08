using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class entereStoreNode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            CustomerAI customerAi = other.GetComponent<CustomerAI>();
            if (customerAi.currentState == CustomerAI.state.enteringStore)
            {
                customerAi.StartCoroutine(customerAi.Decide());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
