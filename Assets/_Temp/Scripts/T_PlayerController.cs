using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class T_PlayerController : MonoBehaviour
{
    public Vector2 direction;

    Animator animator;
    Rigidbody rb;
    T_GroundSensor groundSensor;
    [SerializeField] Camera cam;
    Vector3 correctedDir;

    public bool isSprinting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groundSensor = GetComponentInChildren<T_GroundSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void FixedUpdate()
    {
        RotateCharacter();
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

    void RotateCharacter()
    {
        if(direction.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(correctedDir, Vector3.up);
        }
    }

    public void Jump()
    {
        if(groundSensor.isGrounded)
            rb.AddForce(Vector3.up * 6, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            if(collision.GetContact(0).normal.y > 0.5f)
            {
                transform.SetParent(collision.gameObject.transform, true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(null, true);
            collision.gameObject.TryGetComponent(out Rigidbody rigidB);
        }
    }
}