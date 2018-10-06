using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWidthCamera : MonoBehaviour {

    public float screenWidth = 600;

    private Camera _camera;

    private float size;
    private float ratio;
    private float screenHeight;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        UpdateCamera();
    }

    void UpdateCamera()
    {
        ratio = (float)Screen.height / (float)Screen.width;
        screenHeight = screenWidth * ratio;
        size = screenHeight / 200;
        _camera.orthographicSize = size;
    }
}