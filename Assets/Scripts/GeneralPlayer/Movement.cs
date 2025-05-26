using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float WalkSpeed = 15;
    private const float WalkSpeedMax = 15;
    private const float AimWalkSpeedMax = 7;
    private const float FlySpeedMax = 60;





    private Vector3 BaseWind = new Vector3(-5, 0, 5);
    private Vector3 CameraOffset;
    public static bool InCutScene = false;
    public Rigidbody rb;
    private Animator animator;
    [SerializeField]
    public Camera cam;
    public GameObject Head;

    [Header("Inputs")]
    private Vector2 inputs = new Vector2();
    private float mouseX;
    private float mouseY;
    [SerializeField]
    private float sensX = 10;
    [SerializeField]
    private float sensY = 10;
    private float xrot;
    private float yrot;
    public const float JumpConstant = 400;
    private Vector3 MoveDirection;

    private Quaternion CharacterRotation;
    private Vector3 DefaultTargetCameraOffset = new Vector3(0, 2, -12);
    private Vector3 AimDownCameraOffset = new Vector3(3f, 0, -8);
    private Vector3 InventoryCameraOffset = new Vector3(3f, 0.3f, -10f);

    public Vector3 TargetCameraOffset;
    public bool aiming;
    public bool OpeningInventory;
    public bool IsGrounded;
    public bool IsFlying;
    public float TargetX = 0;
    public float targety = 0;


    private Vector3 height = new Vector3(0, 3, 0);
    private PlayerMain player;
    public bool IsCrouching = false;
    private void OnEnable()
    {
        TargetCameraOffset = DefaultTargetCameraOffset;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMain>();
        NetworkPos = transform.position;
    }

    private void StateUpdate()
    {

        if (Input.GetKeyDown(KeyMap.CrouchKey))
        {
            IsCrouching = true;
        }
        if (Input.GetKeyUp(KeyMap.CrouchKey))
        {
            IsCrouching = false;
        }

        if (IsFlying)
        {
            targety = -1;
        }
        else if (IsCrouching)
        {
            targety = 1;
        }
        else
        {
            targety = 0;
        }
        animator.SetFloat("Y", Mathf.MoveTowards(animator.GetFloat("Y"), targety, 0.05f));
        animator.SetFloat("X", Mathf.MoveTowards(animator.GetFloat("X"), TargetX, 0.1f));




    }
    private void FixedUpdate()
    {
        if (player.IsLocal)
        {
            if (!GameInformation.instance.MainNetwork.IsServer)
            {
                PacketSend.Client_Send_Position(transform.position, Head.transform.rotation, transform.rotation);
                PacketSend.Client_Send_AnimationState(TargetX, targety);
            }
            else
            {
                PacketSend.Server_DistributeMovement(0, transform.position, Head.transform.rotation, transform.rotation);
                PacketSend.Server_DistributePlayerAnimationState(0, TargetX, targety);
            }

        }
    }
    public void AimDownSight()
    {
        WalkSpeed = AimWalkSpeedMax;
        aiming = true;
        TargetCameraOffset = AimDownCameraOffset;
    }
    public void AimUpSight()
    {
        WalkSpeed = WalkSpeedMax;

        aiming = false;
        TargetCameraOffset = DefaultTargetCameraOffset;
    }
    public void OpenInventory()
    {
        OpeningInventory = true;
        TargetCameraOffset = InventoryCameraOffset;
    }
    public void CloseInventory()
    {
        OpeningInventory = false;
        TargetCameraOffset = DefaultTargetCameraOffset;
    }
    private void CameraMovement()
    {
        if (!InCutScene)
        {

            cam.transform.rotation = Quaternion.Euler(xrot, yrot, 0);
            cam.transform.position = transform.position + cam.transform.rotation * CameraOffset;

        }
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        yrot += mouseX;
        xrot -= mouseY;
        xrot = Mathf.Clamp(xrot, -85f, 85f);
    }
    private void StepMovement()
    {
        if (Physics.Raycast(transform.position - height, transform.forward, 1))
        {
            rb.AddForce(height, ForceMode.Force);
        }
    }
    private Vector3 CameraRayExtension = new Vector3(0, 0, 50);
    private Vector3 twodmask = new Vector3(1, 0, 1);

    private Vector3 AimingRotation;

    private void PlayerMovement()
    {


        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        Vector3 localVelocity = ((inputs) * (rb.velocity.magnitude / WalkSpeed)).normalized;
        //Vector3 localVelocity = (transform.rotation * rb.velocity)/WalkSpeed;
        TargetX = localVelocity.magnitude;
        if (IsFlying)
        {

            TargetX = 1 - TargetX;
        }

        MoveDirection = (cam.transform.forward * inputs.y + cam.transform.right * inputs.x);
        MoveDirection.y = 0;
        MoveDirection = MoveDirection.normalized;
        //transform.RotateAround(transform.position,transform.up, Vector3.SignedAngle(transform.forward, MoveDirection, transform.position));
        if (aiming)
        {
            rb.AddForce(Vector3.up, ForceMode.Force);

            AimingRotation = cam.transform.forward;
            AimingRotation.y = 0;
            transform.forward = AimingRotation;
            //AimingRotation.y = 0;
            rb.AddForce(Quaternion.Euler(AimingRotation) * MoveDirection * WalkSpeedMax * 100 * Time.deltaTime, ForceMode.Force);

        }
        else
        {
            if (MoveDirection != Vector3.zero)
            {
                transform.forward = MoveDirection;

                rb.AddForce(MoveDirection.normalized * WalkSpeedMax * 100 * Time.deltaTime, ForceMode.Force);
            }
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, CharacterRotation, Time.deltaTime * Quaternion.Angle(transform.rotation, CharacterRotation) * 60);

        }

        //transform.rotation = CharacterRotation;

    }
    private void Jump()
    {


        if (Input.GetKeyDown(KeyMap.JumpKey) && IsGrounded)
        {

            IsGrounded = false;
            rb.AddForce(Vector3.up * JumpConstant, ForceMode.Force);
        }
    }

    private void fly()
    {

        if (!IsGrounded && Input.GetKeyDown(KeyMap.JumpKey) && !player.InBattle)
        {
            IsFlying = true;
            IsCrouching = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            WalkSpeed = FlySpeedMax;

        }
        if (Input.GetKeyUp(KeyMap.JumpKey))
        {
            IsFlying = false;
            WalkSpeed = WalkSpeedMax;

        }
        if (IsFlying)
        {
            rb.AddForce(Vector3.up * JumpConstant * Time.deltaTime * 10f, ForceMode.Force);

        }


    }
    private void Aiming()
    {

        if (Input.GetKeyDown(KeyMap.Aim))
        {
            aiming = !aiming;
            if (aiming)
            {
                AimDownSight();

            }
            else
            {
                AimUpSight();
            }
        }
    }
    private void Update()
    {

        if (player.IsLocal)
        {
            PlayerMovement();
            CameraMovement();
            SpeedControl();
            StepMovement();
            StateUpdate();
            fly();
            Jump();
            Aiming();
            if (CameraOffset != TargetCameraOffset)
            {
                CameraOffset = Vector3.MoveTowards(CameraOffset, TargetCameraOffset, 30 * Time.deltaTime);

            }
        }
        else
        {
            transform.rotation = NetworkBodyrot;
            transform.position = Vector3.MoveTowards(transform.position, NetworkPos, 100 * Time.deltaTime);
            animator.SetFloat("X", Mathf.MoveTowards(animator.GetFloat("X"), NetworkAnimationX, 0.1f));
            animator.SetFloat("Y", Mathf.MoveTowards(animator.GetFloat("Y"), NetworkAnimationY, 0.05f));
        }



    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == GameInformation.BuildingLayer)
        {
            IsGrounded = true;
        }
    }
    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == GameInformation.BuildingLayer)
        {
            IsGrounded = false;
        }
    }*/
    private void SpeedControl()
    {

        Vector3 dir = rb.velocity.normalized;
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) > WalkSpeed)
        {
            rb.velocity = new Vector3(dir.x * WalkSpeed, rb.velocity.y, dir.z * WalkSpeed);
        }
        if (Mathf.Abs(rb.velocity.y) > 100f)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -100, 100), rb.velocity.z);
        }
    }
    private Vector3 bodyrotation = new Vector3(0, 0, 0);
    private Vector3 HeadRotation = Vector3.zero;


    public Vector3 NetworkPos;
    public Quaternion NetworkRot = Quaternion.identity;
    public Quaternion NetworkBodyrot = Quaternion.identity;
    public float NetworkAnimationX = 0;
    public float NetworkAnimationY = 0;
    public void SetMovement(Vector3 pos, Quaternion Headrot, Quaternion bodyrot)
    {
        NetworkPos = pos;
        NetworkRot = Headrot;
        NetworkBodyrot = bodyrot;
    }
    public void SetAnimation(float x, float y)
    {
        NetworkAnimationX = x;
        NetworkAnimationY = y;
    }
    private void LateUpdate()
    {
        if (player.IsLocal)
        {
            if (aiming && !player.CurrentWeapon.Weaponized)
            {
                Head.transform.rotation = Quaternion.Euler(xrot, yrot, 0);




            }
        }
        else
        {
            Head.transform.rotation = NetworkRot;
        }

    }
}
