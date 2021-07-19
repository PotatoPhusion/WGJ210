using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float targetWidth = cam.orthographicSize * 16f / 9f;
        cam.orthographicSize = targetWidth / cam.aspect;
    }
}
