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
        ChangeGameState(gameplayState);
    }

    public void SwitchToMenu()
    {
        ChangeGameState(mainMenuState);
    }

    public void SwitchToPause()
    {
        ChangeGameState(pauseState);
    }

    public void SwitchToOptions()
    {
        ChangeGameState(optionsState);
    }

    public void SwitchToGameOver()
    {
        ChangeGameState(gameOverState);
    }
    public void SwitchToWinner()
    {
        ChangeGameState(winnerState);
    }

    public void SwitchToLastState()
    {
        ChangeGameState(lastState);
    }

    public void HandlePause()
    {

        if (currentState == gameplayState)
        {
            SwitchToPause();
        }
        else if (currentState == pauseState)
        {
            SwitchToGameplay();
        }
    }

    #endregion
    
}
