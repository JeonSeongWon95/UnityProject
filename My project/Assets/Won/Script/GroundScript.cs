using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Transform Parenttransform = collision.gameObject.transform.parent;

        if (Parenttransform == null)
            return;

        PlayerScript PlayerSc = Parenttransform.gameObject.GetComponent<PlayerScript>();
        if (PlayerSc == null)
            return;

        PlayerSc.IsJumping = false;
    }
}
