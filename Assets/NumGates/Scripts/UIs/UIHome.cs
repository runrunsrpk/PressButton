using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Transform logoTransform;

    [Header("Other UI")]
    [SerializeField] private UIHomeButton uiHomeButton;
    [SerializeField] private UIModeButton uiModeButton;
    
    public void Show()
    {
        gameObject.SetActive(true);

        uiHomeButton.Show();
        uiModeButton.Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
