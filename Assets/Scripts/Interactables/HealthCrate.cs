using System.Collections;
using UnityEngine;

public class HealthCrate : MonoBehaviour, IInteractable
{
    public GameObject content;
    bool isOpen = false;

    public bool GetState()
    {
        return false;
    }

    public bool Interact(GameObject obj = null)
    {
        if(isOpen == false)
        {
            GetComponentInParent<Animator>()?.SetTrigger("Open");
            StartCoroutine(Delay());
        }
        
        return true;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
        content.SetActive(true);
    }

    public void Toggle()
    {
    }
}
