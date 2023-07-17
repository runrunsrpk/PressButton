using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public float ZoomSpeed => zoomSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private List<float> zoomSize;
    [SerializeField] private Vector2 refResolution;
    
    public void ZoomIn(float value, float duration)
    {
        float targetSize = mainCamera.orthographicSize + value;
        mainCamera.DOOrthoSize(targetSize, duration);
    }

    public void ZoomOut(int value, float duration) 
    {
        float targetSize = mainCamera.orthographicSize - value;
        mainCamera.DOOrthoSize(targetSize, duration);
    }

    public void Zoom(int level)
    {
        int actualLevel = Mathf.Clamp(level, 0, zoomSize.Count - 1);

        //Debug.Log($"Zoom Level: {level} | {actualLevel}");
        //Debug.Log($"Zoom X: {(int)(baseResolution.x * zoomSize[actualLevel])} | Y: {(int)(baseResolution.y * zoomSize[actualLevel])}");

        PixelPerfectCamera pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();

        if (pixelPerfectCamera != null)
        {
            pixelPerfectCamera.refResolutionX = (int)(refResolution.x * zoomSize[actualLevel]);
            pixelPerfectCamera.refResolutionY = (int)(refResolution.y * zoomSize[actualLevel]);
        }
    }

}
