using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacles : MonoBehaviour
{
    enum Direction
    {
        Right,
        Left
    }

    public int Dir;
    public float Speed;
    private Direction dir = Direction.Right;

    void Start()
    {
        dir = (Direction)Dir;
    }


    void FixedUpdate()
    {
        Vector3 Rot = dir == Direction.Left ? Vector3.up : Vector3.down;
        transform.Translate(Rot * Speed * Time.fixedDeltaTime);       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (dir == Direction.Right)
            {
                dir = Direction.Left;
            }
            else
            {
                dir = Direction.Right;
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            Transform PlayerTransform = collision.gameObject.transform;

            if (PlayerTransform == null)
                return;

            Rigidbody PlayerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (PlayerRigidbody == null)
                return;

            Vector3 Direction = PlayerTransform.position - transform.position;
            Direction.y = 0;
            Direction.Normalize();

            PlayerRigidbody.AddForce(Direction * 5.0f, ForceMode.Impulse);
        }
    }
}

