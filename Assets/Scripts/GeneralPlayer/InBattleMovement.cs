using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class InBattleMovement : MonoBehaviour
{
    private const float WalkSpeed = 15f;
    private Vector3 BaseWind = new Vector3(-5, 0, 5);
    private Vector3 CameraOffset;
    public static bool InCutScene = false;
    private Rigidbody rb;
    private Animator animator;


    [Header("Inputs")]
    [SerializeField]
    private float sensX = 20;
    [SerializeField]
    private float sensY = 20;
    private Vector3 MoveDirection;
    private Quaternion CharacterRotation;
    private float mouseX;
    private float mouseY;
    private float xrot;
    private float yrot;
    private float HorizontalInput;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
    }
   
    private void PlayerMovement()
    {

        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        //mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yrot -= mouseY;
        xrot += mouseX;
        //xrot = Mathf.Clamp(xrot, -40f, 40f);
        yrot = Mathf.Clamp(yrot, -45f, 45f);
        transform.rotation = Quaternion.Euler(60f,xrot-90f,0);
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        //Vector3 localVelocity = (transform.rotation * rb.velocity)/WalkSpeed;
        rb.AddForce(Vector3.forward * HorizontalInput * WalkSpeed ,ForceMode.Impulse);
    }
    private void Update()
    {
        PlayerMovement();
        SpeedControl();
    }
    private void SpeedControl()
    {
        if(rb.velocity.magnitude > WalkSpeed)
        {
            rb.velocity = rb.velocity.normalized * WalkSpeed;
        }
    }
}
