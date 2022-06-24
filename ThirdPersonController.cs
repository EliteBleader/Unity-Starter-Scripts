using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public CinemachineVirtualCamera aimCam;
    public Transform shoulder;
    public Transform gun;



    [SerializeField] float speed = 6f;
    [SerializeField] float turnSensitivity = 50f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField]float jumpHeight = 3f;
    bool isSprinting;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector2 mouseTurn;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isSprinting = false;
      

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ZoomIn();
        Sprint();
       
    }

   
    void Movement()
    {
       
        mouseTurn.x += Input.GetAxisRaw("Mouse X") * turnSensitivity * Time.deltaTime;
        mouseTurn.y += Input.GetAxisRaw("Mouse Y") * turnSensitivity * Time.deltaTime;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        direction = transform.rotation * direction;



        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        mouseTurn.y = Mathf.Clamp(mouseTurn.y, -80, 40);
        
        transform.localRotation = Quaternion.Euler(0f, mouseTurn.x, 0f);
        shoulder.localRotation = Quaternion.Euler(-mouseTurn.y, 0f, 0f);
        gun.localRotation = Quaternion.Euler(-mouseTurn.y, 0f, 0f);

        

        
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        
        controller.Move(direction * speed * Time.deltaTime);

        
       
    }
    

    void ZoomIn()
    {
        if (Input.GetButton("Fire2"))
        {
            aimCam.m_Lens.FieldOfView = 30;
        }
        else
        {
            aimCam.m_Lens.FieldOfView = 60;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            speed = 10f;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
            speed = 6f;
        }
    }

}
