using UnityEngine;
using UnityEngine.AI;

public class testNPCMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject testNode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = testNode.transform.position;
    }
}
