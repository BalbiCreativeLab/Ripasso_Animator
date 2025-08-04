using Unity.VisualScripting;
using UnityEngine;

public class T_GroundSensor : MonoBehaviour
{
    public float radius = 0.06f;
    public LayerMask layer;
    public bool isGrounded = false;
    public bool enableDebug = true;

    void Update()
    {
        isGrounded = Physics.OverlapSphere(transform.position, radius, layer).Length > 0;
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