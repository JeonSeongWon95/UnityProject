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
        switch (Dir)
        {
            case 0:
                dir = Direction.Left;
                break;
            case 1:
                dir = Direction.Right;
                break;
        }
    }


    void Update()
    {
        if (dir == Direction.Left)
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        else if (dir == Direction.Right)
        {
            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && dir == Direction.Right)
        {
            dir = Direction.Left;
        }
        else if (collision.gameObject.tag == "Obstacle" && dir == Direction.Left)
        {
            dir = Direction.Right;
        }
    }
}

