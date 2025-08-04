using UnityEngine;

public class RBTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(Vector3.right, Vector3.up));
    }
}
