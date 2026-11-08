using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject sanityDrainAura;
    public GameObject sanityDrainAnnoyingAura;
    public GameObject player; 
    public SanityManager sanityManager;
    public MasterNode master;
    private UnityAction turnToMonster;
    private UnityAction returnToNormal;
    public bool fallowing;
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
        turnToMonster += TurnIntoMonster;
        returnToNormal += ReturnToNormal;
        player = GameObject.FindGameObjectWithTag("Player");
        sanityManager = player.GetComponent<SanityManager>();
        sanityManager.lowestSanity.AddListener(turnToMonster);
        sanityManager.lowerSanity.AddListener(returnToNormal);
        sanityManager.lowSanity.AddListener(returnToNormal);
        sanityManager.highSanity.AddListener(returnToNormal); 
        sanityDrainAnnoyingAura.SetActive(false);
        fallowing = false;
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
            case state.wanderStore:
                {
                    StartCoroutine(WanderStore());break;
                }
            case state.fallowPlayer:
                {
                    fallowing = true; break;
                }
        }
    }
    public void TurnIntoMonster()
    {
        Setstate(state.fallowPlayer);
    }
    public void ReturnToNormal()
    {
        if(currentState == state.fallowPlayer)
        {
            Setstate(previousState);
            fallowing = false;
        }
    }

    public IEnumerator Decide()
    {
        agent.destination = agent.gameObject.transform.position;
        yield return new WaitForSeconds(Random.Range(0.1f,2));
        int choice = Random.Range(1, 3);
        if (choice <= 1)
        {
            Setstate(state.waitAtCounter);
        }
        if (choice > 1)
        {
            Setstate(state.wanderStore);
        }

    }
    public IEnumerator WanderStore()
    {
        agent.destination = master.shelfANode.transform.position;
        yield return new WaitForSeconds(5);
        agent.destination = master.shelfBNode.transform.position;
        yield return new WaitForSeconds(7);
        Setstate(state.waitAtCounter);
    }
    // Update is called once per frame
    void Update()
    {
        if (fallowing == true)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}
