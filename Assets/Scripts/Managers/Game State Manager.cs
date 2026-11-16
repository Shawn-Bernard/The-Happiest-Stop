using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState optionsState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState winnerState;

    [SerializeField] private GameState currentState;
    [SerializeField] private GameState lastState;


    #region Game State logic
    public void ChangeGameState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
            lastState = currentState;
        }

        currentState = newState;
        currentState.EnterState();
    }

    public void SwitchToGameplay()
    {
        Debug.Log("Switch to gameplay");
        ChangeGameState(gameplayState);
    }

    public void SwitchToMenu()
    {
        Debug.Log("Switch to main menu");
        ChangeGameState(mainMenuState);
    }

    public void SwitchToPause()
    {
        Debug.Log("Switch to pause");
        ChangeGameState(pauseState);
    }

    public void SwitchToOptions()
    {
        Debug.Log("Switch to options");
        ChangeGameState(optionsState);
    }

    public void SwitchToGameOver()
    {
        Debug.Log("Switch to gameplay");
        ChangeGameState(gameOverState);
    }
    public void SwitchToWinner()
    {
        Debug.Log("Switch to gameplay");
        ChangeGameState(winnerState);
    }

    public void SwitchToLastState()
    {
        ChangeGameState(lastState);
    }

    public void HandlePause()
    {
        Debug.Log("handle pause was called");
        if (currentState == gameplayState)
        {
            SwitchToPause();
            return;
        }
        else if (currentState == pauseState)
        {
            SwitchToGameplay();
            return;
        }
    }

    #endregion
    
}
