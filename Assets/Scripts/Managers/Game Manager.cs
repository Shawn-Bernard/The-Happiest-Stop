using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameStateManager gameStateManager;

    [SerializeField] LevelManager levelManager;
    [SerializeField] PlayerInputs playerInputs;

    [SerializeField] private UnityEvent onRequestPause;
    private static GameManager instance;

    public static GameManager Instance => instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void RequestPause()
    {
        instance.onRequestPause?.Invoke();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void MuteOnPause()
    {
        AudioListener.pause = true;
    }

    public void UnmuteOnPause()
    {
        AudioListener.pause = false;
    }

    private void OnEnable()
    {
        instance.playerInputs.onPauseStarted += RequestPause;
    }

    private void OnDisable()
    {
        instance.playerInputs.onPauseStarted -= RequestPause;
    }
}
