using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BlackOutEvent : MonoBehaviour
{
    public bool eventActive;
    public GameObject blackoutArea;

    public bool eventActivated;

    public UnityEvent onBlackout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventActivated = false;
        StartCoroutine(EventDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartEvent()
    {
        if (eventActivated == false)
        {
            eventActivated = true;
            blackoutArea.SetActive(true);
            onBlackout?.Invoke();
        }

    }
    
    public void EndEvent()
    {
        blackoutArea.SetActive(false);
    }

    public IEnumerator EventStarter()
    {

        while (true)
        {
            int eventCheck = Random.Range(0, 1000);
            Debug.Log(eventCheck);
            if (eventCheck == 1)
            {
                StartEvent();
                Debug.Log("BLACKOUT TIME");
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator EventDelay()
    {
        yield return new WaitForSeconds(120f);
        StartCoroutine(EventStarter());
    }
}
