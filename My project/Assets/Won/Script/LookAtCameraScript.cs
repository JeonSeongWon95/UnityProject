using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAtCameraScript : MonoBehaviour
{
    void Update()
    {
        Vector3 Location = Camera.main.transform.position;
        transform.LookAt(Location);
    }
}
