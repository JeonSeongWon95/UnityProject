/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float WalkSpeed = 15.0f;
    public float RunSpeed = 50.0f;
    public float RotationSpeed = 10.0f;

    private Vector3 MoveVector;
    private Rigidbody PlayerRigidbody;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        if(HorizontalInput == 0 && VerticalInput == 0) 
        {
            anim.SetBool("Walk", false);
            return;
        }

        anim.SetBool("Walk", true);

        MoveVector = new Vector3(VerticalInput, 0, -HorizontalInput);
        MoveVector.Normalize();

        Vector3 NewPosition = PlayerRigidbody.position + MoveVector * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed);
        PlayerRigidbody.MovePosition(NewPosition);

        if (MoveVector != Vector3.zero)
        {
            Quaternion TargetRoatation = Quaternion.LookRotation(MoveVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRoatation, RotationSpeed * Time.deltaTime);
        }

    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float WalkSpeed = 15.0f;
    public float RunSpeed = 50.0f;
    public float RotationSpeed = 10.0f;
    public bool IsJumping;

    private Vector3 MoveVector;
    private bool IsJump;
    private bool IsRun;
    private Rigidbody PlayerRigidbody;
    private Animator anim;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateInput();
    }

    void FixedUpdate()
    {
        if (MoveVector != Vector3.zero)
        {
            float CurrentSpeed = IsRun ? RunSpeed : WalkSpeed;
            Vector3 NewPosition = PlayerRigidbody.position + MoveVector * Time.fixedDeltaTime * CurrentSpeed;
            PlayerRigidbody.MovePosition(NewPosition);

            Quaternion TargetRotation = Quaternion.LookRotation(MoveVector);
            PlayerRigidbody.rotation = Quaternion.Slerp(PlayerRigidbody.rotation, TargetRotation, RotationSpeed * Time.fixedDeltaTime);
        }

        if (IsJump && !IsJumping) 
        {
            IsJumping = true;
            PlayerRigidbody.AddForce(Vector3.up  * 5.0f, ForceMode.Impulse);
        }

        anim.SetBool("IsJump", IsJumping);
    }

    void UpdateInput() 
    {
        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");
        IsRun = Input.GetButton("Run");
        IsJump = Input.GetButton("Jump");

        MoveVector = new Vector3(VerticalInput, 0, -HorizontalInput).normalized;
        anim.SetBool("Walk", MoveVector != Vector3.zero);
    }
}
