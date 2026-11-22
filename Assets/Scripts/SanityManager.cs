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
    public UnityEvent lowerSanity;
    public UnityEvent lowestSanity;
    public float timeBetweenBlinks;
    public Animation blinkAnim;
    public bool playerAlive;
    private bool isActive;
    private Coroutine sanityCheck;

    [SerializeField] private SanityEvent sanityEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSanity();
        ChangeSanityDrain(defaultSanityDrainMultiplier);

        //playerAlive = true;
        //currentSanity = maxSanity;
        //sanityDrainMultiplier = defaultSanityDrainMultiplier;
        //StartCoroutine(CheckSanity());
    }

    public void StartSanity()
    {
        RespawnPlayer();
        ResetSanityDrain();
        sanityCheck = StartCoroutine(CheckSanity());
        isActive = true;
    }

    public void EndSanity()
    {
        if (sanityCheck != null) StopCoroutine(sanityCheck);
        sanityCheck = null;
        isActive = false;
    }

    public void ResetSanity()
    {
        playerAlive = true;
        currentSanity = maxSanity;
        sanityEvent.SanityChange(currentSanity);
        //sanityDrainMultiplier = defaultSanityDrainMultiplier;
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
        sanityDrainMultiplier += sanityDrain;
    }

    public void Death()
    {
        playerAlive = false;
        GameStateManager.Instance.SwitchToGameOver();
    }
    public void RespawnPlayer()
    {
        playerAlive = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        currentSanity -= sanityDrainMultiplier * Time.deltaTime;
        sanityEvent.SanityChange(currentSanity);
    }

    public IEnumerator CheckSanity()
    {
        while(playerAlive == true)
        {
            yield return new WaitForSeconds(timeBetweenBlinks);
            blinkAnim.Play();
            yield return new WaitForSeconds(0.22f);
            if (currentSanity < maxSanity * 0.75 && currentSanity > maxSanity * 0.5)
            {
                lowSanity.Invoke();
                Debug.Log("starting to go alittle crazy");
            }
            else if (currentSanity < maxSanity * 0.5 && currentSanity > maxSanity * 0.25)
            {
                lowerSanity.Invoke();
                Debug.Log("going crazy");
            }
            else if (currentSanity <= maxSanity * 0.25)
            {
                lowestSanity.Invoke();
                Debug.Log("going very crazy");
            }

            if (currentSanity >= maxSanity * 0.75)
            {
                highSanity.Invoke();
                Debug.Log("not going crazy");
            }

        }
    }

    private void OnEnable()
    {
        highSanity.AddListener(sanityEvent.HighSanity);
        lowSanity.AddListener(sanityEvent.LowSanity);
        lowerSanity.AddListener(sanityEvent.LowerSanity);
        lowestSanity.AddListener(sanityEvent.LowestSanity);

        sanityEvent.onAddSanity += AddSanity;
        sanityEvent.onStartSanity += StartSanity;
        sanityEvent.onEndSanity += EndSanity;
    }

    private void OnDisable()
    {
        highSanity.RemoveListener(sanityEvent.HighSanity);
        lowSanity.RemoveListener(sanityEvent.LowSanity);
        lowerSanity.RemoveListener(sanityEvent.LowerSanity);
        lowestSanity.RemoveListener(sanityEvent.LowestSanity);

        sanityEvent.onAddSanity -= AddSanity;
        sanityEvent.onStartSanity -= StartSanity;
        sanityEvent.onEndSanity -= EndSanity;
    }
}
