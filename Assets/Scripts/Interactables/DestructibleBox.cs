using UnityEngine;

public class DestructibleBox : MonoBehaviour, IInteractable
{
    public GameObject destructibleBox;

    public bool GetState()
    {
        return false;
    }

    public bool Interact(GameObject obj = null)
    {
        gameObject.SetActive(false);
        destructibleBox?.SetActive(true);
        return true;
    }

    public void Toggle()
    {
    }
}
