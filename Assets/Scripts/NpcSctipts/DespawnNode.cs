using UnityEngine;

public class DespawnNode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            Debug.Log("kill");
            if (other.gameObject.GetComponent<CustomerAI>().currentState == CustomerAI.state.exitStore)
            {
                Destroy(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
