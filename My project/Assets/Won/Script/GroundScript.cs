using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform Parenttransform = collision.gameObject.transform.parent;
        if (Parenttransform == null)
            return;

        CameraScript PlayerScript = Parenttransform.gameObject.GetComponent<CameraScript>();
        if (PlayerScript == null)
            return;

        PlayerScript.IsJumping = false;
    }
}
