using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustom : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private Transform lockedParent;
    [SerializeField] private GameObject uiLockedPrefab;

    private GameManager gameManager;
    private SpawnManager spawnManager;
    private CameraManager cameraManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        spawnManager = gameManager.SpawnManager;
        cameraManager = gameManager.CameraManager;

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

    public void InitUILockedButton()
    {
        //Transform parent = spawnManager.GetSpawnerParent();

        //Debug.Log($"Init Custom: {parent.childCount}");

        //foreach(Transform child in parent)
        //{
        //    Vector3 position = cameraManager.MainCamera.ViewportToScreenPoint(child.position);

        //    UILockedButton uiLockedButton = Instantiate(uiLockedPrefab, lockedParent).GetComponent<UILockedButton>();
        //    uiLockedButton.transform.localPosition = position;
        //}
    }

    public void CreateUILockedButton(Vector3 position, Vector3 parentPosition)
    {
        float refScale = transform.localScale.x;
        Vector3 buttonPosition = cameraManager.MainCamera.WorldToScreenPoint(position) / refScale;
        Vector3 offsetPosition = cameraManager.MainCamera.WorldToScreenPoint(parentPosition) / refScale;

        Vector2 refResolution = new Vector2(Screen.width, Screen.height) / refScale;

        Vector3 actualOffset = new Vector3(refResolution.x - offsetPosition.x, refResolution.y - offsetPosition.y, offsetPosition.z);
        Vector3 spawnPosition = (buttonPosition - actualOffset);

        UILockedButton uiLockedButton = Instantiate(uiLockedPrefab, lockedParent).GetComponent<UILockedButton>();
        uiLockedButton.transform.localPosition = spawnPosition;
    }

    public void DestroyUILockedButton()
    {
        foreach (Transform child in lockedParent)
        {
            Destroy(child.gameObject);
        }
    }
}
