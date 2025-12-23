using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
// Questo script gestisce solo gli input del giocatore e li rimanda al player controller,
// non gestisce nessuna logica

[RequireComponent(typeof(PlayerControllerRB))]
[RequireComponent(typeof(PlayerEventSystem))]

public class PlayerInputRB : MonoBehaviour
{
    PlayerControllerRB controller;
    InputAction moveAction, sprintAction, jumpAction, interactAction;
    PlayerEventSystem playerEvents;
    void Start()
    {
        controller = GetComponent<PlayerControllerRB>();
        playerEvents = GetComponent<PlayerEventSystem>();

        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += UpdateDirection;
        moveAction.canceled += UpdateDirection;

        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.performed += SetSprint;
        sprintAction.canceled += SetSprint;

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += Jump;

        interactAction = InputSystem.actions.FindAction("Interact");
        interactAction.performed += Interact;
    }

    void UpdateDirection(InputAction.CallbackContext context)
    {
        playerEvents.OnMove.Invoke(context.ReadValue<Vector2>());
    }

    void SetSprint(InputAction.CallbackContext context)
    {
        playerEvents.OnSprintRequest.Invoke(context.ReadValueAsButton());
    }

    void Jump(InputAction.CallbackContext context)
    {
        playerEvents.OnJumpRequested.Invoke(context.ReadValueAsButton());
    }

    void Interact(InputAction.CallbackContext context)
    {
        playerEvents.OnInteract.Invoke();
    }
}