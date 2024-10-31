using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
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
