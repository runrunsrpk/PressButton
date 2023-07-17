using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeButton : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Button relaxButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button freeButton;
    [SerializeField] private Button backButton;

    [Header("Other UI")]
    [SerializeField] private UIHomeButton uiHomeButton;

    private GameplayManager gameplayManager;

    private void OnEnable()
    {
        gameplayManager = GameManager.Instance.GameplayManager;

        relaxButton.onClick.AddListener(OnClickRelaxMode);
        speedButton.onClick.AddListener(OnClickSpeedMode);
        freeButton.onClick.AddListener(OnClickFreeMode);
        backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDisable()
    {
        relaxButton.onClick.RemoveListener(OnClickRelaxMode);
        speedButton.onClick.RemoveListener(OnClickSpeedMode);
        freeButton.onClick.RemoveListener(OnClickFreeMode);
        backButton.onClick.RemoveListener(OnClickBack);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    private void OnClickRelaxMode()
    {
        // Enter relax mode
        gameplayManager.OnChangeGameState?.Invoke(GameState.StartGame);
    }

    private void OnClickSpeedMode()
    {
        // Show deficulty
    }

    private void OnClickFreeMode()
    {
        // Enter free mode
    }

    private void OnClickBack()
    {
        uiHomeButton.Show();
        Hide();
    }
}
