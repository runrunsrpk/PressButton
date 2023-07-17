using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeButton : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button customButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header("Other UI")]
    [SerializeField] private UIModeButton uiModeButton;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;

        playButton.onClick.AddListener(OnClickPlay);
        customButton.onClick.AddListener(OnClickCustom);
        optionsButton.onClick.AddListener(OnClickOptions);
        exitButton.onClick.AddListener(OnClickExit);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnClickPlay);
        customButton.onClick.RemoveListener(OnClickCustom);
        optionsButton.onClick.RemoveListener(OnClickOptions);
        exitButton.onClick.RemoveListener(OnClickExit);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickPlay()
    {
        Hide();
        uiModeButton.Show();
    }

    private void OnClickCustom()
    {
        // Show custome scene
        GameManager.Instance.OnEnterCustom?.Invoke();
    }

    private void OnClickOptions()
    {
        // Show option popup
    }

    private void OnClickExit()
    {
        Application.Quit();
    }
}
