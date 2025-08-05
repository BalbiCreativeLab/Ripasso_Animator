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

    //public float currentSpeed, targetSpeed, speedVelocity;
    public Vector3 currentDir, dirVelocity;

    SmoothFloat smoothSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        groundSensor = GetComponentInChildren<GroundSensor>();

        smoothSpeed = new SmoothFloat(0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Applygravity();
        RotateCharacter();

        if (isJumping)
        {
            characterController.Move(Vector3.up * Time.deltaTime * 20);
        }
    }
    void Movement()
    {
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;

        if (isSprinting)
        {
            //targetSpeed = direction.magnitude * 2;
            animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude*2));
        }
        else
        {
            //targetSpeed = direction.magnitude;
            animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude));
        }

        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, 0.2f);
        //animator.SetFloat("Speed", smoothSpeed.MoveTowards(currentSpeed));
    }

    void Applygravity()
    {
        characterController.Move(Vector3.down * 10 * Time.deltaTime);
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
            StartCoroutine(JumpCoroutine());
        }
    }

    IEnumerator JumpCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        isJumping = false;
    }
}