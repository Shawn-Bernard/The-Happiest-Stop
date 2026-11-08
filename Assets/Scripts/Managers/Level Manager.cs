using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onMainMenu;
    [SerializeField] private UnityEvent onGameplay;
    [SerializeField] private UnityEvent<Vector3,Vector3> onSpawnPointFound;
    private string currentSpawn;

    public void LoadSceneWithSpawn(string sceneName, string spawnLocation)
    {
        currentSpawn = spawnLocation;

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Looks for a spawn point on scene loaded
    /// </summary>
    public void HandleSpawn()
    {
        SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

        if (spawnPoints.Length == 0) return;

        foreach (SpawnPoint point in spawnPoints)
        {
            // If current spawn is null or is the location then spawn
            if (string.IsNullOrEmpty(currentSpawn) || point.SpawnLocation == currentSpawn)
            {
                Debug.Log("a spawn was found");
                onSpawnPointFound?.Invoke(point.Position, point.Rotation);
                return;
            }
            else
            {
                Debug.Log("no spawn was found");
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;
        string activeScene = scene.name;

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
            HandleSpawn();
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
        LoadSceneWithSpawn("Gameplay", "Default");
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
