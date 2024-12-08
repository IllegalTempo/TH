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
    private Vector3 MoveDirection;
    private Vector3 altMoveDirection;

    private Quaternion CharacterRotation;
    private Vector3 DefaultTargetCameraOffset = new Vector3(0,2,-10);
    private Vector3 AimDownCameraOffset = new Vector3(3f, 0, -6);
    private Vector3 InventoryCameraOffset = new Vector3(3f, 0.3f, -10f);

    private Vector3 TargetCameraOffset;
    public bool aiming;
    public bool OpeningInventory;
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
    private Vector3 AimingRotation;
    private void PlayerMovement()
    {
        

        inputs.x =Input.GetAxisRaw("Horizontal");
        inputs.y = Input.GetAxisRaw("Vertical");
        Vector3 localVelocity = (inputs) * (rb.velocity.magnitude/WalkSpeed);
        //Vector3 localVelocity = (transform.rotation * rb.velocity)/WalkSpeed;
        TargetX += (localVelocity.magnitude - TargetX) * Time.deltaTime * WalkSpeed;
        animator.SetFloat("X", TargetX);
        MoveDirection = transform.forward * inputs.y + transform.right * inputs.x;
        altMoveDirection = -transform.forward * inputs.x + transform.right * inputs.y;

        CharacterRotation = Quaternion.Euler(Vector3.up * (yrot + Vector3.SignedAngle(transform.forward, MoveDirection, transform.position)));
        //transform.RotateAround(transform.position,transform.up, Vector3.SignedAngle(transform.forward, MoveDirection, transform.position));
        if(aiming)
        {
            AimingRotation = (transform.position - cam.transform.position + cam.transform.rotation * CameraRayExtension);
            //AimingRotation.y = 0;
            transform.forward = AimingRotation;
            rb.AddForce(MoveDirection * WalkSpeed * 10, ForceMode.Force);

        }
        else if(MoveDirection != Vector3.zero)
        {
            rb.AddForce(transform.forward * MoveDirection.magnitude * WalkSpeed * 10, ForceMode.Force);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, CharacterRotation, Time.deltaTime * Quaternion.Angle(transform.rotation, CharacterRotation) * 60);

        }
        //transform.rotation = CharacterRotation;

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
    }
    private void SpeedControl()
    {
        if(rb.velocity.magnitude > WalkSpeed)
        {
            rb.velocity = rb.velocity.normalized * WalkSpeed;
        }
    }
}
