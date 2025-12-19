using UnityEngine;

public class PickUpEvent : GameEvent
{

}

public class CollectibleGEV : MonoBehaviour
{
    string pickupEvent = "OnCollectiblepPickUp";

    public int collectibleValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEventSystem.AddEvent(typeof(PickUpEvent));
    }

    private void OnTriggerEnter(Collider other)
    {
        GEVDATA_CollectiblePickedUp data = new GEVDATA_CollectiblePickedUp();
        data.collectibleValue = collectibleValue;
        GameEventSystem.TriggerEvent(typeof(PickUpEvent), data);
        Destroy(gameObject);
    }
}
