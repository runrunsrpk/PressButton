using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustom : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;

        homeButton.onClick.AddListener(OnClickHome);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnClickHome);
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
        gameManager.OnExitCustom?.Invoke();
    }
}
