using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = Quaternion.AngleAxis(-10.0f, Vector3.up);
        transform.rotation *= rotation;
    }
}
