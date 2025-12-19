using UnityEngine;

public class DestructibleBox : MonoBehaviour, IInteractable
{
    public bool GetState()
    {
        return false;
    }

    public bool Interact(GameObject obj = null)
    {
        print( obj.name + " HA INTERAGITO CON CASSA!");
        return true;
    }

    public void Toggle()
    {
    }
}
