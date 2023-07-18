using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILockedButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lockedText;

    public void SetLockedText(int level)
    {
        lockedText.text = $"Unlock\nLV {level}";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
