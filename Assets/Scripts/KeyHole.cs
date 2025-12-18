using UnityEngine;

public class KeyHole : MonoBehaviour, IInteractable
{
    bool state;

    public GameObject door;
    public GameObject key;
    public bool Interact(GameObject instigator)
    {
        if (instigator.TryGetComponent(out PlayerInteractor playerInteractor))
        {
            if(state)
            {
                IInteractable itr = door.GetComponentInChildren<IInteractable>();

                if (itr != null)
                {
                    itr.Interact(gameObject);
                    return true;
                }
            }
            else if(playerInteractor.hasKey)
            {
                playerInteractor.hasKey = false;
                key.SetActive(true);
                state = true;
                return true;
            }
        }
        return false;
    }

    public void Toggle()
    {
        state = !state;
    }

    public bool GetState()
    {
        return state;
    }
}