using UnityEngine;

public class BlackOutArea : MonoBehaviour
{
    public float blackoutSanityDrain;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(blackoutSanityDrain);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(-blackoutSanityDrain);
        }
    }
}

