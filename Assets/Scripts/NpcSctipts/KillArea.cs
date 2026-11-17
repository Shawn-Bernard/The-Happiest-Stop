using UnityEngine;

public class KillArea : MonoBehaviour
{
    private CustomerAI customerAI;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().Death();
            customerAI.SetCanMove(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customerAI = GetComponentInParent<CustomerAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
