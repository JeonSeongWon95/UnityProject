using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAtCameraScript : MonoBehaviour
{
    public Camera TargetCamera;
    void Start()
    {
        
    }

    void Update()
    {
        if (TargetCamera != null)
        {
            transform.LookAt(TargetCamera.transform);
        }
    }

    public void SetCamera(Camera Newcamera) 
    {
        TargetCamera = Newcamera;
    }
}
