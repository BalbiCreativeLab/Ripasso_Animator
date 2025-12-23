using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    PlayerEventSystem playerEvents;
    public GameObject interactObj;
    public IInteractable interactable;
    public bool hasKey;

    private void Start()
    {
        playerEvents = GetComponentInParent<PlayerEventSystem>();
        playerEvents.OnInteract += Interact;
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

    void Interact()
    {
        interactable?.Interact(gameObject);
    }
}
