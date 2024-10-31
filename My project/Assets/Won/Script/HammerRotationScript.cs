using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRotationScript : MonoBehaviour
{
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.AngleAxis(-10.0f, Vector3.up);
        transform.rotation *= rotation;
    }
}
