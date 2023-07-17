using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    RelaxMode,
    SpeedMode,
    FreeMode,
}

public enum GameState
{
    StartGame,
    PlayGame,
    EndGame,
    PauseGame,
    ResumeGame,
    RestartGame,
    ExitGame
}

public class GameplayManager : MonoBehaviour
{
    public Action OnStartGame;
    public Action OnPlayGame;
    public Action OnEndGame;
    public Action OnPauseGame;
    public Action OnResumeGame;
    public Action OnRestartGame;
    public Action OnExitGame;

    public Action<bool> OnPressedButton;
    public Action<GameState> OnChangeGameState;

    private LevelManager levelManager;

    public void Initialize()
    {
        EnableAction();

        levelManager = GameManager.Instance.LevelManager;
    }

    public void Terminate()
    {
        DisableAction();
    }

    private void EnableAction()
    {
        OnPressedButton += PressedButton;
        OnChangeGameState += ChangeGameState;
    }

    private void DisableAction()
    {
        OnPressedButton -= PressedButton;
        OnChangeGameState -= ChangeGameState;
    }

    #region Action
    private void PressedButton(bool isCorrect)
    {
        if(isCorrect)
        {
            levelManager.OnCorrectPressed?.Invoke();
        }
        else
        {
            levelManager.OnIncorrectPressed?.Invoke();
        }
    }

    private void ChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.StartGame: OnStartGame?.Invoke(); break;
            case GameState.PlayGame: OnPlayGame?.Invoke(); break;
            case GameState.EndGame: OnEndGame?.Invoke(); break;
            case GameState.PauseGame: OnPauseGame?.Invoke(); break;
            case GameState.ResumeGame: OnResumeGame?.Invoke(); break;
            case GameState.RestartGame: OnRestartGame?.Invoke(); break;
            case GameState.ExitGame: OnExitGame?.Invoke(); break;
        }
    }
    #endregion

    
}
