using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button homeButton;

    private GameplayManager gameplayManager;

    private void OnEnable()
    {
        gameplayManager = GameManager.Instance.GameplayManager;

        homeButton.onClick.AddListener(OnClickHome);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnClickHome);
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"LEVEL {level}";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickHome()
    {
        gameplayManager.OnExitGame?.Invoke();
    }
}
