using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Transform PlayerTransform = collision.gameObject.transform;
            if (PlayerTransform == null)
                return;

            Rigidbody WallRigidbody = gameObject.GetComponent<Rigidbody>();
            if (WallRigidbody == null)
                return;

            Vector3 Direction = transform.position - PlayerTransform.position;
            Direction.Normalize();

            WallRigidbody.AddForce(Direction * 5.0f, ForceMode.Impulse);
        }
    }
}
