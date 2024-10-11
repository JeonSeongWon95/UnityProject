using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerScript : MonoBehaviour
{
    public Transform CharacterBody;
    public Transform CameraBody;
    public float WalkSpeed = 5.0f;
    public float RunSpeed = 10.0f;
    public float RotationSpeed = 10.0f;
    public bool IsJumping;
    public bool CanMove = true;
    public Vector3 CameraOffset;
    public LookAtCameraScript CameraScript;
    public Camera CharacterCamera;

    private Vector3 MoveVector;
    private bool IsJump;
    private bool IsRun;
    private Rigidbody PlayerRigidbody;
    private Animator PlayerAnimator;
    private PlaySceneGameManagerScript GameManagerScr;

    void Start()
    {
        Transform ChildrenTransform = transform.Find("Character");
        PlayerRigidbody = ChildrenTransform.GetComponent<Rigidbody>();
        PlayerAnimator = ChildrenTransform.GetComponent<Animator>();
        GameManagerScr = GameObject.Find("GameManager").GetComponent<PlaySceneGameManagerScript>();
    }

    void Update()
    {
        if (!GameManagerScr.IsGameEnd)
        {
            LookAround();
            Move();
            SetCameraLocation();
        }
    }
    void Move()
    {
        Vector2 CharcterMove = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        bool IsMove = (CharcterMove.magnitude != 0);
        PlayerAnimator.SetBool("Walk", IsMove);

        IsRun = Input.GetButton("Run");
        IsJump = Input.GetButton("Jump");

        if (IsMove) 
        {
            Vector3 LookForward = new Vector3(CameraBody.forward.x, 0.0f, CameraBody.forward.z).normalized;
            Vector3 LookRight = new Vector3(CameraBody.right.x, 0.0f, CameraBody.right.z).normalized;
            Vector3 MoveDirection = (LookForward * CharcterMove.x) + (LookRight * CharcterMove.y);

            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
            CharacterBody.rotation = Quaternion.Slerp(CharacterBody.rotation, targetRotation, RotationSpeed * Time.deltaTime);

            if (CanMove)
            {
                transform.position += MoveDirection * Time.deltaTime * (IsRun ? RunSpeed : WalkSpeed);
            }
        }

    }

    void LookAround() 
    {
        Vector2 CameraMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 CamAngle = CameraBody.rotation.eulerAngles;

        float LimitX = CamAngle.x - CameraMove.y;

        if(LimitX < 180.0f) 
        {
            LimitX = Mathf.Clamp(LimitX, -1.0f, 70.0f);
        }
        else
        {
            LimitX = Mathf.Clamp(LimitX, 335.0f, 361.0f);
        }

        CameraBody.rotation = Quaternion.Euler(LimitX, CamAngle.y + CameraMove.x, CamAngle.z);
    }


    void FixedUpdate()
    {
        if (IsJump && !IsJumping)
        {
            IsJumping = true;
            PlayerRigidbody.AddForce(Vector3.up * 8.0f, ForceMode.Impulse);
        }

        PlayerAnimator.SetBool("IsJump", IsJumping);
    }

    void SetCameraLocation() 
    {
        Transform PlayerTransform = transform.Find("Character");
        Transform CameraTransform = transform.Find("CameraArm");

        CameraTransform.position = PlayerTransform.position + CameraOffset;
    }

    public void LocalPlayerSet() 
    {
        Transform PlayerTransform = transform.Find("Character");
        Transform CameraTransform = transform.Find("CameraArm");
        CameraTransform.gameObject.SetActive(true);

        Transform PlayerMarkerTransform = PlayerTransform.Find("PlayerMarker");
        Transform PlayerArrowTransform = PlayerMarkerTransform.Find("Arrow");
        PlayerArrowTransform.gameObject.SetActive(true);
    }
    
    public void SetCameraScript()
    {
        CameraScript.SetCamera(CharacterCamera);
    }
}
