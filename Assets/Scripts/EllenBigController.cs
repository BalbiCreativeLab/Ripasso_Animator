using UnityEngine;
using UnityEngine.InputSystem;

public class EllenBigController : MonoBehaviour
{
    Animator animator;
    InputAction move;
    PlayerInput player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        //move = InputSystem.actions.FindAction("Move");
        InputSystem.actions.FindAction("Move").performed += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        /*float speedX, speedZ;
        speedX = 0;
        speedZ = 0;
         
        if(Input.GetKey(KeyCode.W))
            speedZ = 1;
        else
            speedZ = 0;

        if (Input.GetKey(KeyCode.A))
            speedX = -1;
        else if(Input.GetKey(KeyCode.D))
            speedX = 1;*/

/*
        Vector2 input = move.ReadValue<Vector2>();
        float speedX, speedZ;
        speedX = input.x;
        speedZ = input.y;

        animator.SetFloat("SpeedX", speedX);
        animator.SetFloat("SpeedZ", speedZ);*/
    }
/*
    public void ReceiveMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        animator.SetFloat("SpeedX", input.x);
        animator.SetFloat("SpeedZ", input.y);
    }*/
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        //animator.SetFloat("SpeedX", input.x);
        animator.SetFloat("SpeedZ", input.y);
    }
}
