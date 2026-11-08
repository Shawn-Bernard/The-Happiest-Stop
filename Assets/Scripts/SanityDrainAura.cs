using UnityEngine;

public class SanityDrainAura : MonoBehaviour
{
    public float sanityDrain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(sanityDrain);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(-sanityDrain);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
