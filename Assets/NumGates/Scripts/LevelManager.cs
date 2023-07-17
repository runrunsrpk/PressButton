using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Action OnCorrectPressed;
    public Action OnIncorrectPressed;

    public Action<int> OnUpdateLevel;

    private int level;
    private int tempLevel;
    private int wave;

    private int maxActiveAmount;
    private int activeButtonAmount;

    private SpawnManager spawnManager;
    private GameplayManager gameplayManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SkipLevel();
        }
    }

    public void Initialize()
    {
        spawnManager = GameManager.Instance.SpawnManager;
        gameplayManager = GameManager.Instance.GameplayManager;

        EnableAction();
    }

    public void Terminate()
    {
        DisableAction();
    }

    private void EnableAction()
    {
        OnCorrectPressed += CorrectPressed;
        OnIncorrectPressed += IncorrectPressed;

        gameplayManager.OnStartGame += StartGame;
        gameplayManager.OnPlayGame += PlayGame;
        gameplayManager.OnEndGame += EndGame;
        gameplayManager.OnPauseGame += PauseGame;
        gameplayManager.OnResumeGame += ResumeGame;
        gameplayManager.OnRestartGame += RestartGame;
        gameplayManager.OnExitGame += ExitGame;
    }

    private void DisableAction()
    {
        OnCorrectPressed -= CorrectPressed;
        OnIncorrectPressed -= IncorrectPressed;

        gameplayManager.OnStartGame -= StartGame;
        gameplayManager.OnPlayGame -= PlayGame;
        gameplayManager.OnEndGame -= EndGame;
        gameplayManager.OnPauseGame -= PauseGame;
        gameplayManager.OnResumeGame -= ResumeGame;
        gameplayManager.OnRestartGame -= RestartGame;
        gameplayManager.OnExitGame -= ExitGame;
    }

    #region Helper
    private void SkipLevel()
    {
        if (!IsMaxWave())
        {
            wave++;
            GenerateWave();
        }
        else
        {
            GenerateLevel();
        }
    }

    private bool IsMaxWave()
    {
        return wave + 1 >= spawnManager.GetMaxWave();
    }

    private bool IsMaxLevel()
    {
        return tempLevel + 1 >= spawnManager.GetMaxLevel();
    }

    private bool IsButtonsActivated()
    {
        return activeButtonAmount - 1 > 0;
    }
    #endregion

    #region Level Actions
    private void CorrectPressed()
    {
        if(IsButtonsActivated())
        {
            activeButtonAmount--;
        }
        else
        {
            if(!IsMaxWave())
            {
                wave++;
                GenerateWave();
            }
            else
            {
                GenerateLevel();
            }
        }
    }

    private void IncorrectPressed()
    {
        // Show incorect condition
        // - Restart wave
        // - Restart game
        GenerateWave();
    }
    #endregion

    #region Level Handler
    public void InitLevel()
    {
        List<int> buttons = new List<int> { 0 };

        maxActiveAmount = spawnManager.GetRow();

        Debug.LogWarning($"Level: {level} | Wave: {wave} | TempLevel: {tempLevel} | Max: {maxActiveAmount}");

        spawnManager.ActivateButtons(buttons);
    }

    public void GenerateLevel()
    {
        if(level < GameManager.Instance.GetMaxGameLevel())
        {
            Debug.Log($"Temp: {tempLevel} | Max: {spawnManager.GetMaxLevel()}");
            //tempLevel = IsMaxLevel() ? 0 : tempLevel + 1;
            level++;
            wave = 0;

            SaveRelaxMode();

            if (IsMaxLevel())
            {
                tempLevel = 0;
                spawnManager.SpawnNextLevel();
                maxActiveAmount = spawnManager.GetRow();
            }
            else
            {
                tempLevel += 1;
                maxActiveAmount = (tempLevel % spawnManager.GetRow() == 0) ? maxActiveAmount + 1 : maxActiveAmount;
                GenerateWave();
            }

            OnUpdateLevel?.Invoke(level);
        }
        else
        {
            maxActiveAmount = UnityEngine.Random.Range(spawnManager.GetRow(), spawnManager.GetMaxActivateAmount());
            wave = spawnManager.GetMaxWave();
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        StartCoroutine(OnGenerateWave());
    }

    private IEnumerator OnGenerateWave()
    {
        SaveRelaxMode();

        spawnManager.DeactivateButtons();

        yield return new WaitForSecondsRealtime(0.25f);

        int maxButton = spawnManager.GetButtonAmount();

        List<int> maxButtons = new List<int>();

        for (int i = 0; i < maxButton; i++)
        {
            maxButtons.Add(i);
        }

        activeButtonAmount = maxActiveAmount;

        List<int> buttons = new List<int>();

        for (int i = 0; i < maxActiveAmount; i++)
        {
            int index = UnityEngine.Random.Range(0, maxButtons.Count);
            buttons.Add(maxButtons[index]);
            maxButtons.RemoveAt(index);
        }

        Debug.LogWarning($"Level: {level} | Wave: {wave} | TempLevel: {tempLevel} | Max: {maxActiveAmount}");

        spawnManager.ActivateButtons(buttons);
    }
    #endregion

    #region Helper
    private void SaveRelaxMode()
    {
        RelaxModeData data = new RelaxModeData()
        {
            actualLevel = level,
            tempLevel = tempLevel,
            wave = wave,
            row = spawnManager.GetRow(),
            col = spawnManager.GetColumn(),
        };

        SaveManager.SaveRelaxModeData(data);
    }

    #endregion
    #region Gameplay Action
    private void StartGame()
    {
        RelaxModeData data = SaveManager.LoadRelaxModeData();

        level = data.actualLevel;
        tempLevel = data.tempLevel;
        wave = data.wave;

        activeButtonAmount = 0;
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
    }
    #endregion
}
