using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Target;
    public Vector3 Offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.transform.position + Offset;
        
    }
}
