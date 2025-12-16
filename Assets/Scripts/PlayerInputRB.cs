using UnityEngine;
using UnityEngine.InputSystem;

// Questo script gestisce solo gli input del giocatore e li rimanda al player controller,
// non gestisce nessuna logica

[RequireComponent(typeof(PlayerControllerRB))]

public class PlayerInputRB : MonoBehaviour
{
    PlayerControllerRB controller;
    InputAction moveAction, sprintAction, jumpAction;

    void Start()
    {
        controller = GetComponent<PlayerControllerRB>();

        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += UpdateDirection;
        moveAction.canceled += UpdateDirection;

        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.performed += SetSprint;
        sprintAction.canceled += SetSprint;

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += Jump;
    }

    void UpdateDirection(InputAction.CallbackContext context)
    {
        controller.direction = context.ReadValue<Vector2>();
    }

    void SetSprint(InputAction.CallbackContext context)
    {
        controller.requestSprinting = context.ReadValueAsButton();
    }

    void Jump(InputAction.CallbackContext context)
    {
        controller.requestJumping = true;
    }
}