using UnityEngine;
using UnityEngine.InputSystem;

// Questo script gestisce solo gli input del giocatore e li rimanda al player controller,
// non gestisce nessuna logica

[RequireComponent(typeof(PlayerController))]

public class PlayerInput : MonoBehaviour
{
    PlayerController controller;
    InputAction moveAction, sprintAction, jumpAction;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += UpdateDirection;
        moveAction.canceled += UpdateDirection;

        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.performed += SetSprint;
        sprintAction.canceled += SetSprint;

        sprintAction = InputSystem.actions.FindAction("Jump");
        sprintAction.performed += Jump;
    }

    void UpdateDirection(InputAction.CallbackContext context)
    {
        controller.direction = context.ReadValue<Vector2>();
    }

    void SetSprint(InputAction.CallbackContext context)
    {
        controller.isSprinting = context.ReadValueAsButton();
    }

    void Jump(InputAction.CallbackContext context)
    {
        controller.Jump();
    }
}