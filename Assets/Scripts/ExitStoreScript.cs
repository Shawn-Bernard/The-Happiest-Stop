using UnityEngine;

public class ExitStoreScript : MonoBehaviour
{
    public float outDoorSanityDrain;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<SanityManager>().ChangeSanityDrain(outDoorSanityDrain);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(-outDoorSanityDrain);
            Debug.Log("Inside Store");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<SanityManager>().ChangeSanityDrain(outDoorSanityDrain);
            Debug.Log("OutsideStore");
        }
    }
}
