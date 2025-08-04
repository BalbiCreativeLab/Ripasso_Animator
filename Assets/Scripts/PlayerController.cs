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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        groundSensor = GetComponentInChildren<GroundSensor>();
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
            animator.SetFloat("Speed", direction.magnitude * 2);
        }
        else
        {
            animator.SetFloat("Speed", direction.magnitude);
        }
    }

    void Applygravity()
    {
        characterController.Move(Vector3.down * 10 * Time.deltaTime);
    }

    void RotateCharacter()
    {
        if(direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(correctedDir, Vector3.up);
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