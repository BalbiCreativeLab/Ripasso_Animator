using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(T_PlayerController))]
public class T_PlayerInput : MonoBehaviour
{
    T_PlayerController controller;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;

    void Start()
    {
        controller = GetComponent<T_PlayerController>();

        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += UpdateDirection;
        moveAction.canceled += UpdateDirection;

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += (ctx) => controller.Jump();

        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.performed += (ctx) => controller.isSprinting = true;
        sprintAction.canceled += (ctx) => controller.isSprinting = false;
    }

    void UpdateDirection(InputAction.CallbackContext context)
    {
        controller.direction = context.ReadValue<Vector2>();
    }
}
