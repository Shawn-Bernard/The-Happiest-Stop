using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
public class SanityManager : MonoBehaviour
{
    public float currentSanity;
    public float maxSanity;
    public float sanityDrainMultiplier;
    public float defaultSanityDrainMultiplier;
    public UnityEvent highSanity;
    public UnityEvent lowSanity;
    public Animation blinkAnim;
    public bool playerAlive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAlive = true;
        currentSanity = maxSanity;  
        sanityDrainMultiplier = defaultSanityDrainMultiplier;
        StartCoroutine(CheckSanity());
    }

    public void AddSanity(float sanity)
    {
        currentSanity += sanity;
        if(currentSanity > maxSanity)
        {
            currentSanity = maxSanity;
        }
    }
    public void ResetSanityDrain()
    {
        sanityDrainMultiplier = defaultSanityDrainMultiplier;
    }

    public void ChangeSanityDrain(float sanityDrain)
    {
        sanityDrainMultiplier = sanityDrain;
    }

    // Update is called once per frame
    void Update()
    {
        currentSanity -= sanityDrainMultiplier * Time.deltaTime;
    }

    public IEnumerator CheckSanity()
    {
        while(playerAlive == true)
        {
            yield return new WaitForSeconds(10);
            blinkAnim.Play();
            yield return new WaitForSeconds(0.21f);
            if (currentSanity < maxSanity / 2)
            {
                lowSanity.Invoke();
                Debug.Log("low sanity");
            }
            if (currentSanity > maxSanity / 2)
            {
                highSanity.Invoke();
                Debug.Log("high sanity");
            }

        }
    }
}
