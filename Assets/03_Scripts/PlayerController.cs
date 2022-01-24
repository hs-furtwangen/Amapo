using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    Animator animator;
    [SerializeField] public CinemachineFreeLook vCam;
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

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeInput(InputData _inputData) {
        Vector3 direction = _inputData.Movement.normalized;
        MoveCharacter(direction);
    } 

    public void ToggleCamera(bool enabled) 
    {
        vCam.enabled = enabled;
    }

    private void MoveCharacter(Vector3 _direction) {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
 
        if (_direction.magnitude >= 0.1f) {
            Vector3 moveDir = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * _direction;
            controller.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            animator.SetBool("isWalking", true);
        } else
            animator.SetBool("isWalking", false);
    
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
