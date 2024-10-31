using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncepadScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody PlayerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            if (PlayerRigidbody == null)
                return;

            PlayerRigidbody.AddForce(Vector3.up * 40.0f, ForceMode.Impulse);
        }
    }
}
