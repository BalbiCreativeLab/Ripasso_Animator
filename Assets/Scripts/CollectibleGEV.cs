using UnityEngine;

public class CollectibleGEV : MonoBehaviour
{
    string pickupEvent = "OnCollectiblepPickUp";

    public int collectibleValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventSystem.AddEvent(pickupEvent);
    }

    private void OnTriggerEnter(Collider other)
    {
        GEVDATA_CollectiblePickedUp data = new GEVDATA_CollectiblePickedUp();
        data.collectibleValue = collectibleValue;
        GameEventSystem.TriggerEvent(pickupEvent, data);
        Destroy(gameObject);
    }
}
