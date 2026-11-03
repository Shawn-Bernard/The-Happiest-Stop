using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onMainMenu;
    [SerializeField] private UnityEvent onGameplay;
    private static LevelManager instance;
    public static LevelManager Instance => instance;

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
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "Boot Loader") // Will probably remove once i get game states going and call it in boot loader
        {
            LoadMainMenu();
        }
        else if (activeScene == "Main Menu")
        {
            onMainMenu?.Invoke();
        }
        else if (sceneIndex > 1)// This would be gameplay levels index
        {
            onGameplay?.Invoke();
        }


        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadMainMenu()
    {
        LoadScene("Main Menu");
    }

    public void LoadGameplay()
    {
        onGameplay?.Invoke();
        LoadScene("Gameplay");
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
        else
        {
            LoadMainMenu();
        }
    }

    public void LoadLastLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex - 1 >= 0)
        {
            SceneManager.LoadScene(sceneIndex - 1);
        }
        else
        {
            LoadMainMenu();
        }
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

[DefaultExecutionOrder(-1000)]
public static class BootLoad
{
    const string bootSceneName = "Boot Loader";
    // Loads before scene is loadeded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        // When not in boot loader scene add boot loader additive 
        if (activeScene != bootSceneName)
        {
            SceneManager.LoadScene(bootSceneName, LoadSceneMode.Additive);
        }
        // When in boot loader scene, scene will switch to main menu from Level Manager
    }

}
