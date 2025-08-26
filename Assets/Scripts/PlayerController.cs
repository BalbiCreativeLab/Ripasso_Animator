using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public Vector2 direction;
    public bool isSprinting = false;
    public bool isJumping = false;

    Animator animator;
    CharacterController characterController;
    GroundSensor groundSensor;
    [SerializeField] Camera cam;

    Vector3 correctedDir;
    Vector3 initialPosition;
    Vector3 currentVelocity = Vector3.zero;
    Vector3 targetVelocity;

    public Vector3 currentDir, dirVelocity;

    SmoothFloat smoothSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        groundSensor = GetComponentInChildren<GroundSensor>();

        smoothSpeed = new SmoothFloat(0.2f);
        currentVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        initialPosition = transform.position;

        Movement();
        ApplyJump();
        ApplyGravity();
        RotateCharacter();

        animator.SetBool("IsGrounded", groundSensor.isGrounded);
        animator.SetFloat("VerticalSpeed", currentVelocity.y);
    }

    private void LateUpdate()
    {
        currentVelocity = (transform.position - initialPosition) / Time.deltaTime ;
    }

    void Movement()
    {
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;

        if (isSprinting)
        {
            animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude*2));
        }
        else
        {
            animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude));
        }
    }

    void ApplyGravity()
    {
        //if(!groundSensor.isGrounded)
        //characterController.Move(Vector3.up * ((currentVelocity.y) * Time.deltaTime));
    }

    void RotateCharacter()
    {
        currentDir = Vector3.Slerp(currentDir, correctedDir, Time.deltaTime * 5f);

        if(direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(currentDir, Vector3.up);
    }

    public void Jump()
    {
        if(groundSensor.isGrounded)
        {
            isJumping = true;
            //StartCoroutine(JumpCoroutine());
            animator.SetTrigger("Jump");
            currentVelocity.y = 10;
        }
    }
    private void ApplyJump()
    {
        if (isJumping)
        {
            //characterController.Move(Vector3.up * Time.deltaTime * 10);
        }
    }

    IEnumerator JumpCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        isJumping = false;
    }
}