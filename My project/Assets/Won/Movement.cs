using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float WalkSpeed = 50;
    public float MaxWalkSpeed = 100;

    private Vector3 PlayerInput;
    private Rigidbody PlayerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        PlayerInput.Normalize();

        PlayerRigidbody.AddForce(PlayerInput * WalkSpeed * Time.deltaTime, ForceMode.Force);

    }
}
