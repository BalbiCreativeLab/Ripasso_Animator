using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    public GameObject interactObj;
    public IInteractable interactable;
    public bool hasKey;
    InputAction interact;

    private void Start()
    {
        interact = InputSystem.actions.FindAction("Interact");
        interact.performed += Interact;
    }
    private void OnTriggerEnter(Collider other)
    {
        IInteractable itr = other.gameObject.GetComponentInChildren<IInteractable>();
        if (itr != null)
        {
            interactObj = other.gameObject;
            interactable = itr;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable itr = other.gameObject.GetComponentInChildren<IInteractable>();

        if (itr == interactable)
        {
            interactObj = null;
            interactable = null;
        }
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        interactable?.Interact(gameObject);
    }
}
