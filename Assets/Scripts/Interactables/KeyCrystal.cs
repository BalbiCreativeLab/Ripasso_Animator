using System.Collections;
using UnityEngine;

public class KeyCrystal : MonoBehaviour, IInteractable
{
    private void OnEnable()
    {
        StartCoroutine(Delay());    
    }

    public bool GetState()
    {
        return false;
    }

    public bool Interact(GameObject instigator)
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

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<ConstantForce>().enabled = false;
    }
}
