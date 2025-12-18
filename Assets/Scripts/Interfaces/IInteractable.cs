using UnityEngine;

public interface IInteractable
{
    public bool Interact(GameObject obj = null);
    public void Toggle();
    public bool GetState();
}
