using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesScriipt : MonoBehaviour
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
        Transform PlayerTransform = collision.gameObject.transform;
        //Transform ParentTransform = collision.gameObject.transform.parent;
        if (PlayerTransform == null)
            return;
        Rigidbody PlayerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        //Rigidbody ParentRigidbody = ParentTransform.gameObject.GetComponent<Rigidbody>();
        if (PlayerRigidbody == null)
            return;

        Vector3 Direction = PlayerTransform.position - transform.position;
        Direction.y = 0;
        Direction.Normalize();

        PlayerRigidbody.AddForce(Direction * 5.0f, ForceMode.Impulse);
    }

}
