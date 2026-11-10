using UnityEngine;
using UnityEngine.Events;

public class counterNode : MonoBehaviour
{
    public UnityEvent onCounterState;
    private UnityAction addCustomerTask;
    public TaskManager taskManager;
    private GameObject customer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            CustomerAI customerAi = other.GetComponent<CustomerAI>();
            if (customerAi.currentState == CustomerAI.state.waitAtCounter)
            {
                Debug.Log("entered");
                if (other.gameObject.GetComponentInChildren<Task>())
                {
                    Task task = other.gameObject.GetComponentInChildren<Task>();
                    taskManager.AddTask(task);
                }
                /*
                customer = other.gameObject;
                addCustomerTask += GetCustoerTask;
                onCounterState.AddListener(addCustomerTask);
                onCounterState.Invoke();
                */
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }

    public void GetCustoerTask()
    {
        Debug.Log("Called get task");
        taskManager.AddTask(customer.GetComponent<Task>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
