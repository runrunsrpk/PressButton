using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isActivate;

    public void InitButton()
    {
        isActivate = false;

        spriteRenderer.color = Color.gray;
    }

    public void ActivateButton()
    {
        isActivate = true;

        spriteRenderer.color = Color.green;
    }

    public void DeactivateButton() 
    { 
        isActivate= false;

        spriteRenderer.color = Color.red;
    }

    public void PressButton()
    {
        GameplayManager gameplayManager = GameManager.Instance.GameplayManager;

        if (isActivate == true) 
        { 
            DeactivateButton();
            gameplayManager.OnPressedButton?.Invoke(true);
        }
        else
        {
            gameplayManager.OnPressedButton?.Invoke(false);
        }
    }
}
