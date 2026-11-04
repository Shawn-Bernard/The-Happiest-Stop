using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject sanityDrainAura;
    public GameObject sanityDrainAnnoyingAura;

    public MasterNode master;
    public state currentState { get; private set; }
    public state previousState {  get; private set; }
    public enum state
    {
        none,
        init,
        enteringStore,
        wanderStore,
        waitAtCounter,
        fallowPlayer,
        exitStore
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sanityDrainAnnoyingAura.SetActive(false);

        Setstate(state.init);
    }

    public void Setstate(state newstate)
    {
        if(newstate == currentState)
        {
            return;
        }
        previousState = currentState;
        currentState = newstate;
        OnStateChange(currentState);
    }
    public void OnStateChange(state newState)
    {
        switch(newState)
        {
            case state.init:
                {
                    Setstate(state.enteringStore); break;
                }
            case state.enteringStore:
                {
                    agent.SetDestination(master.enteredStoreNode.transform.position); break;
                }
            case state.waitAtCounter:
                {
                    sanityDrainAnnoyingAura.SetActive(true);
                    agent.SetDestination(master.counterNode.transform.position); break;
                }
            case state.exitStore:
                {
                    agent.SetDestination(master.spawnNode.transform.position); break;
                }
        }
    }

    public IEnumerator Decide()
    {
        agent.destination = agent.gameObject.transform.position;
        yield return new WaitForSeconds(Random.Range(0.1f,2));
        //int choice = Random.Range(1, 3);
        int choice = 1;
        if (choice >= 1)
        {
            Setstate(state.waitAtCounter);
        }
        if (choice < 1)
        {
            Setstate(state.wanderStore);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
