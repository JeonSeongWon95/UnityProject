using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAtCameraScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Camera.main == null)
        {
            Debug.Log("Main Camera is Null");
        }
        transform.LookAt(Camera.main.transform);
    }
}
