using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float WalkSpeed = 20;
    private Vector3 BaseWind = new Vector3(-5, 0, 5);
    private Vector3 CameraOffset;
    public static bool InCutScene = false;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField]
    private Camera cam;
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
    public const float JumpConstant = 2500;
    private Vector3 MoveDirection;

    private Quaternion CharacterRotation;
    private Vector3 DefaultTargetCameraOffset = new Vector3(0,2,-10);
    private Vector3 AimDownCameraOffset = new Vector3(3f, 0, -8);
    private Vector3 InventoryCameraOffset = new Vector3(3f, 0.3f, -10f);

    private Vector3 TargetCameraOffset;
    public bool aiming;
    public bool OpeningInventory;
    public bool IsGrounded;
    public bool IsFlying;
    private float TargetX = 0;
    private Vector3 height = new Vector3(0,3,0);
    private PlayerMain player;

    public bool IsCrouching = false;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        TargetCameraOffset = DefaultTargetCameraOffset;
        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMain>();    
    }
    private void StateUpdate()
    {
        if (Input.GetKeyDown(KeyMap.CrouchKey))
        {
            IsCrouching = true;
        }
        if(Input.GetKeyUp(KeyMap.CrouchKey))
        {
            IsCrouching = false ;
        }
        float targety = 0;
        if (IsFlying) targety = -1;
        if (IsCrouching) targety = 1;
        animator.SetFloat("Y", Mathf.MoveTowards(animator.GetFloat("Y"),targety,0.05f));
    }
    public void AimDownSight()
    {
        aiming = true;
        TargetCameraOffset = AimDownCameraOffset;
    }
    public void AimUpSight()
    {
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
       
        if(!InCutScene)
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
        if(Physics.Raycast(transform.position - height,transform.forward,1))
        {
            rb.AddForce(height,ForceMode.Force);
        }
    }
    private Vector3 CameraRayExtension = new Vector3(0, 0, 50);
    private Vector3 twodmask = new Vector3(1,0,1);

    private Quaternion AimingRotation;

    private void PlayerMovement()
    {
        

        inputs.x =Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        Vector3 localVelocity = ((inputs) * (rb.velocity.magnitude/WalkSpeed)).normalized;
        //Vector3 localVelocity = (transform.rotation * rb.velocity)/WalkSpeed;
        TargetX = localVelocity.magnitude;
        if(IsFlying)
        {
            
            TargetX = 1 - TargetX;
        }

        animator.SetFloat("X", Mathf.MoveTowards(animator.GetFloat("X"), TargetX, 0.1f));
        MoveDirection = cam.transform.forward * inputs.y + cam.transform.right * inputs.x;
        MoveDirection.y = 0;
        //transform.RotateAround(transform.position,transform.up, Vector3.SignedAngle(transform.forward, MoveDirection, transform.position));
        if(aiming)
        {
            transform.localRotation = Quaternion.Euler(0, yrot, 0);
            //AimingRotation.y = 0;
            rb.AddForce(MoveDirection * WalkSpeed * 10, ForceMode.Force);

        }
        else if(MoveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Scale(MoveDirection, twodmask);

            rb.AddForce(transform.forward * MoveDirection.magnitude * WalkSpeed * 10, ForceMode.Force);
            
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, CharacterRotation, Time.deltaTime * Quaternion.Angle(transform.rotation, CharacterRotation) * 60);

        }
        //transform.rotation = CharacterRotation;

    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyMap.JumpKey) && IsGrounded && !player.InBattle)
        {

            IsGrounded = false;
            rb.AddForce(Vector3.up * JumpConstant, ForceMode.Force);
        }
    }
    
    private void fly()
    {
        if (player.InBattle)
        {
            IsFlying = true;
        } else
        {
            if (!IsGrounded && Input.GetKeyDown(KeyMap.JumpKey))
            {
                IsFlying = true;
                IsCrouching = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                WalkSpeed = 60f;

            }
            if (Input.GetKeyUp(KeyMap.JumpKey))
            {
                IsFlying = false;
                WalkSpeed = 20f;

            }
            if (IsFlying)
            {
                rb.AddForce(Vector3.up * JumpConstant * 0.1f, ForceMode.Force);

            }
        }
        
    }
    private void Aiming()
    {
        if(Input.GetKeyDown(KeyMap.Aim))
        {
            aiming = !aiming;
            if(aiming)
            {
                AimDownSight();

            } else
            {
                AimUpSight();
            }
        }
    }
    private void Update()
    {
        PlayerMovement();
        CameraMovement();
        SpeedControl();
        StepMovement();
        StateUpdate();
        fly();
        Jump();
        Aiming();
        if(CameraOffset != TargetCameraOffset)
        {
            CameraOffset = Vector3.MoveTowards(CameraOffset, TargetCameraOffset, 30 * Time.deltaTime);

        }
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == GameInformation.BuildingLayer)
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
        if(Mathf.Sqrt(Mathf.Pow(rb.velocity.x,2) + Mathf.Pow(rb.velocity.z,2)) > WalkSpeed)
        {
            rb.velocity = new Vector3(dir.x * WalkSpeed, rb.velocity.y, dir.z * WalkSpeed);
        }
        if(Mathf.Abs(rb.velocity.y) > 100f)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-100,100), rb.velocity.z);
        }
    }
    private Vector3 bodyrotation = new Vector3(0,0,0);
    private Vector3 HeadRotation = Vector3.zero;
    private void LateUpdate()
    {
        if(aiming && !player.CurrentWeapon.Weaponized)
        {
            Head.transform.rotation = Quaternion.Euler(xrot, yrot, 0);




        }
    }
}
