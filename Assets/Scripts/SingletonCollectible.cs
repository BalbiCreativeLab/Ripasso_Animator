using UnityEngine;

public class SingletonCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameStateSingleton.Current?.AddScore(3);

        //if(GameStateSingleton.Current != null )
        //       GameStateSingleton.Current.AddScore(3);

        Destroy(gameObject);
    }
}
