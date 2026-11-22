using UnityEngine;
using UnityEngine.SceneManagement;
public class KillArea : MonoBehaviour
{
    private CustomerAI customerAI;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().Death();
            //;SceneManager.LoadScene(1);
            customerAI.SetCanMove(false);
            other.GetComponent<SanityManager>().ResetSanity();
            
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
