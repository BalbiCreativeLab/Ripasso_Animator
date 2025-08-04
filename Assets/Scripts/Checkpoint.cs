using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public EventSystem eventSystem;

    private void OnTriggerEnter(Collider other)
    {
        eventSystem.SaveGame?.Invoke();
        Debug.Log("saving game");
    }
}
