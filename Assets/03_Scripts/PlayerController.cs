using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    Animator animator;
    [SerializeField]
    public CinemachineFreeLook vCam;

    private Vector3 playerVelocity;
    public Transform cam;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity = 0.1f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private float playerSpeed = 3f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeInput(InputData inputData) {
        Vector3 direction = inputData.Movement.normalized;
        MoveCharacter(direction);
    } 

    public void ToggleCamera(bool enabled) 
    {
        vCam.enabled = enabled;
    }


    private void MoveCharacter(Vector3 direction) {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f) {
            velocity.y = -2f;
        }
 
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        // if (Input.GetButtonDown("Jump") && isGrounded) {
        //     velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        // }
        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
