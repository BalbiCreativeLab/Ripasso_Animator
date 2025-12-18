using UnityEngine;

public class KeyCrystal : MonoBehaviour, IInteractable
{
    public bool GetState()
    {
        return false;
    }

    public bool Interact(GameObject instigator = null)
    {
        if(instigator.TryGetComponent(out PlayerInteractor playerInteractor))
        {
            if(playerInteractor.hasKey)
            {
                return false;
            }
            playerInteractor.hasKey = true;
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Toggle()
    {
    }
}
