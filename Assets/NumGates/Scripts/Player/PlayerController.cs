using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputMaster input;

    private Vector2 inputPosition;

    private void Awake()
    {
        InitInput();
    }

    private void InitInput()
    {
        input = new InputMaster();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Click.performed += OnClick;
        input.Player.Point.performed += OnPoint;
    }

    private void OnDisable()
    {
        input.Disable();

        input.Player.Click.performed -= OnClick;
        input.Player.Point.performed -= OnPoint;
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        //Debug.Log($"Click");

        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (!hit.collider) return;

            if (hit.collider.transform.TryGetComponent(out GameButton button))
            {
                button.PressButton();
            }
        }
    }

    private void OnPoint(InputAction.CallbackContext ctx)
    {
        inputPosition = ctx.ReadValue<Vector2>();
        //Debug.Log($"Position: {inputPosition}");
    }
}
