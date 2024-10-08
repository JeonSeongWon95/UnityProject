using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Transform Parenttransform = collision.gameObject.transform.parent;
        if (Parenttransform == null)
            return;
        PlayerScript PlayerScr = Parenttransform.gameObject.GetComponent<PlayerScript>();
        if (PlayerScr == null)
            return;

        PlayerScr.CanMove = false;
    }
    void OnCollisionExit(Collision collision)
    {
        Transform Parenttransform = collision.gameObject.transform.parent;
        if (Parenttransform == null)
            return;
        PlayerScript PlayerScr = Parenttransform.gameObject.GetComponent<PlayerScript>();
        if (PlayerScr == null)
            return;

        PlayerScr.CanMove = true;
    }
}
