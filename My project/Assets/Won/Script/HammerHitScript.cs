using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody PlayerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (PlayerRigidbody == null)
                return;

            PlayerRigidbody.AddForce(transform.forward * 100.0f, ForceMode.Impulse);
        }
    }
}
