using Unity.VisualScripting;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public float radius = 0.06f;
    public bool isGrounded = false;
    public bool enableDebug = true;

    void Update()
    {
        isGrounded = Physics.OverlapSphere(transform.position, radius).Length > 0;
    }

    private void OnDrawGizmos()
    {
        if (enableDebug == false)
            return;

        Gizmos.color = Color.red;
        if( isGrounded )
        { 
            Gizmos.color = Color.green;
        }

        Gizmos.DrawSphere(transform.position, radius);
    }
}   