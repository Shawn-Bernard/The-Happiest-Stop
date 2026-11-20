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

    [SerializeField] GameObject playerPrefab;
    public GameObject Player => playerPrefab;

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
        GameStateManager.HandlePause();
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

    public void GameplayCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NormalCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        PlayerInputs.onPauseStarted += RequestPause;
    }

    private void OnDisable()
    {
        PlayerInputs.onPauseStarted -= RequestPause;
    }
}
