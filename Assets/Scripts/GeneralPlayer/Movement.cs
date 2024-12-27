using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private const float WalkSpeed = 20;
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
    private Vector3 AimDownCameraOffset = new Vector3(3f, 0, -6);
    private Vector3 InventoryCameraOffset = new Vector3(3f, 0.3f, -10f);

    private Vector3 TargetCameraOffset;
    public bool aiming;
    public bool OpeningInventory;
    public bool IsGrounded;
    private float TargetX = 0;
    private Vector3 height = new Vector3(0,3,0);

    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        TargetCameraOffset = DefaultTargetCameraOffset;
        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
    }
    private void StateUpdate()
    {
        if(Input.GetKey(KeyMap.SprintKey))
        {

        }
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
        Vector3 localVelocity = (inputs) * (rb.velocity.magnitude/WalkSpeed);
        //Vector3 localVelocity = (transform.rotation * rb.velocity)/WalkSpeed;
        TargetX += (localVelocity.magnitude - TargetX) * Time.deltaTime * WalkSpeed;
        animator.SetFloat("X", TargetX);
        MoveDirection = cam.transform.forward * inputs.y + cam.transform.right * inputs.x;

        //transform.RotateAround(transform.position,transform.up, Vector3.SignedAngle(transform.forward, MoveDirection, transform.position));
        if(aiming)
        {
            transform.localRotation = Quaternion.Euler(0, yrot, 0);
            //AimingRotation.y = 0;
            rb.AddForce(MoveDirection * WalkSpeed * 10, ForceMode.Force);

        }
        else if(MoveDirection != Vector3.zero)
        {

            rb.AddForce(transform.forward * MoveDirection.magnitude * WalkSpeed * 10, ForceMode.Force);
            
            transform.forward = Vector3.Scale(MoveDirection,twodmask);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, CharacterRotation, Time.deltaTime * Quaternion.Angle(transform.rotation, CharacterRotation) * 60);

        }
        //transform.rotation = CharacterRotation;

    }
    private void Jump()
    {
        IsGrounded = false;
        rb.AddForce(Vector3.up * JumpConstant,ForceMode.Force);
    }
    private void Update()
    {
        PlayerMovement();
        CameraMovement();
        SpeedControl();
        StepMovement();
        if(CameraOffset != TargetCameraOffset)
        {
            CameraOffset = Vector3.MoveTowards(CameraOffset, TargetCameraOffset, 30 * Time.deltaTime);

        }
        if(Input.GetKeyDown(KeyMap.JumpKey) && IsGrounded)
        {
            Jump();
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
    }
    private Vector3 bodyrotation = new Vector3(0,0,0);
    private Vector3 HeadRotation = Vector3.zero;
    private void LateUpdate()
    {
        if(aiming)
        {
            Head.transform.rotation = Quaternion.Euler(xrot, yrot, 0);




        }
    }
}
