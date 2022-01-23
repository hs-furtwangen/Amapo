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
    [SerializeField]
    public float playerSpeed = 3f;
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
        //Debug.Log("is executed");
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0, vertical);
        //MoveCharacter(direction);
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
        //Debug.Log("test");
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SitDown();
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    SitUp();
        //}

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f) {
            velocity.y = -2f;
        }
 
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        if (direction.magnitude >= 0.1f) {
            //float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y) / 1.3f;
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 moveDir = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * direction;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void SitDown()
    {
        animator.SetBool("sitDown", true);
    }

    public void SitUp()
    {
        animator.SetBool("sitDown", false);
    }
}
