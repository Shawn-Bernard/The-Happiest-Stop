using UnityEngine;
using UnityEngine.Events;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] GameObject TaskList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleTaskUI()
    {
        TaskList.SetActive(!TaskList.activeInHierarchy);
    }

    private void OnEnable()
    {
        playerInputs.onTaskStarted += HandleTaskUI;
    }

    private void OnDisable()
    {
        playerInputs.onTaskStarted -= HandleTaskUI;
    }
}
