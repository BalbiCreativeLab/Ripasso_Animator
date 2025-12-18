using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameState.GetCollectible();
        Destroy(gameObject);
    }
}
