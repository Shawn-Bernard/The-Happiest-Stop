using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform canvasUI;

    [Header("Menu Events")]
    [SerializeField] private UnityEvent enableMainMenu;
    [SerializeField] private UnityEvent enableGameplay;
    [SerializeField] private UnityEvent enablePause;
    [SerializeField] private UnityEvent enableOptions;
    [SerializeField] private UnityEvent enableGameOver;
    [SerializeField] private UnityEvent enableWinner;

    [SerializeField] private List<Button> buttonList;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private void Awake()
    {
        // If null will get first child in UI Manager
        canvasUI ??= transform.GetChild(0);
        buttonList = GetComponentsInChildren<Button>(true).ToList();
    }

    public void EnableMainMenu()
    {
        DisableAllMenus();
        enableMainMenu?.Invoke();
    }

    public void EnableGameplay()
    {
        DisableAllMenus();
        enableGameplay?.Invoke();
    }

    public void EnablePauseMenu()
    {
        DisableAllMenus();
        enablePause?.Invoke();
    }
    public void EnableOptionsMenu()
    {
        DisableAllMenus();
        enableOptions?.Invoke();
    }

    public void EnableGameOverMenu()
    {
        DisableAllMenus();
        enableGameOver?.Invoke();
    }
    public void EnableWinnerMenu()
    {
        DisableAllMenus();
        enableWinner?.Invoke();
    }

    public void DisableAllMenus()
    {
        canvasUI ??= transform.GetChild(0);

        foreach (Transform UI in canvasUI)
        {
            UI.gameObject.SetActive(false);
        }
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
    private void OnEnable()
    {
        foreach (var item in buttonList)
        {
            item.onClick.AddListener(PlayClickSound);
        }
    }

    private void OnDisable()
    {
        foreach (var item in buttonList)
        {
            item.onClick.RemoveListener(PlayClickSound);
        }
    }
}

