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

        Vector3 Location = Camera.main.transform.position;
        Location.x *= -1;
        transform.LookAt(Location);
    }
}
