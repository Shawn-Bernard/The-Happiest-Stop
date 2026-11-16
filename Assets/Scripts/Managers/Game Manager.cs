using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameStateManager gameStateManager;
    public GameStateManager GameStateManager => instance.gameStateManager;

    [SerializeField] LevelManager levelManager;
    public LevelManager LevelManager => instance.levelManager;

    [SerializeField] SoundManager soundManger;
    public SoundManager SoundManger => instance.soundManger;

    [SerializeField] PlayerInputs playerInputs;
    public PlayerInputs PlayerInputs => instance.playerInputs;

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
        Debug.Log("Pause was called");
        gameStateManager.HandlePause();
        //instance.onRequestPause?.Invoke();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log(Time.timeScale);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
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
        playerInputs.onPauseStarted += RequestPause;
    }

    private void OnDisable()
    {
        playerInputs.onPauseStarted -= RequestPause;
    }
}
