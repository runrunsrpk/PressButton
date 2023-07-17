using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIHome UIHome => uiHome;
    [SerializeField] private UIHome uiHome;
    public UIGameplay UIGameplay => uiGameplay;
    [SerializeField] private UIGameplay uiGameplay;

    private GameplayManager gameplayManager;
    private LevelManager levelManager;

    public void Initialize()
    {
        gameplayManager = GameManager.Instance.GameplayManager;
        levelManager = GameManager.Instance.LevelManager;

        EnableAction();
    }

    public void Terminate()
    {
        DisableAction();
    }

    private void EnableAction()
    {
        gameplayManager.OnStartGame += StartGame;
        gameplayManager.OnPlayGame += PlayGame;
        gameplayManager.OnEndGame += EndGame;
        gameplayManager.OnPauseGame += PauseGame;
        gameplayManager.OnResumeGame += ResumeGame;
        gameplayManager.OnRestartGame += RestartGame;
        gameplayManager.OnExitGame += ExitGame;

        levelManager.OnUpdateLevel += UpdateLevel;
    }

    private void DisableAction()
    {
        gameplayManager.OnStartGame -= StartGame;
        gameplayManager.OnPlayGame -= PlayGame;
        gameplayManager.OnEndGame -= EndGame;
        gameplayManager.OnPauseGame -= PauseGame;
        gameplayManager.OnResumeGame -= ResumeGame;
        gameplayManager.OnRestartGame -= RestartGame;
        gameplayManager.OnExitGame -= ExitGame;

        levelManager.OnUpdateLevel -= UpdateLevel;
    }

    #region Gameplay Action
    private void StartGame()
    {
        // 'All' : load data if any
        // 'Relax / Speed' : update level text
        // 'Free' : update text
        uiHome.Hide();
        uiGameplay.Show();

        RelaxModeData data = SaveManager.LoadRelaxModeData();

        uiGameplay.SetLevelText(data.actualLevel);
    }

    private void PlayGame()
    {
        // 'Speed Mode' : update timer text
    }

    private void EndGame()
    {
        // 'Speed Mode' : show result
    }

    private void PauseGame()
    {
        // 'Speed Mode' : stop timer
    }

    private void ResumeGame()
    {
        // 'Spped Mode' : countdown and continue timer
    }

    private void RestartGame()
    {
        // 'Speed Mode' : 
        // 'Relax / Free' : hide restart button
    }

    private void ExitGame()
    {
        // 'Relax' : Save lasted level
        // 'Speed' : Save lasted level and timer
        uiGameplay.Hide();
        uiHome.Show();
    }
    #endregion

    #region Level Action
    private void UpdateLevel(int level)
    {
        uiGameplay.SetLevelText(level);
    }
    #endregion
}
