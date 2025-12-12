using Unity.VisualScripting;
using UnityEngine;

// Questo 'sensore' controlla se un collider e' all'interno di un certo raggio a partire dal suo centro,
// si potrebbe usare un Raycast al posto di OverlapSphere e filtrare i collider percepiti tramite layermask
public class GroundSensor : MonoBehaviour
{
    public float maxDistance = 0.06f;
    public float delay = 3;
    public bool enableDebug = true;

    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public Vector3 groundNormal;
    bool isTimerStarted = false;
    float startTime = 0;

    void Update()
    {
        // OverlapSphere controlla se entro un raggio a partire da un punto nello spazio ci sono dei collider
        // il gameobject che ha questo script non ha bisogno di avere un collider e/o un Rigidbody
        //isGrounded = Physics.OverlapSphere(transform.position, radius).Length > 0;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance))
        {
            groundNormal = hit.normal;
            isGrounded = true;
            isTimerStarted = false;
        }
        else
        {
            groundNormal = Vector3.up;

            if (!isTimerStarted)
            {
                // avvio il timer se non sono piu' per terra e il timer non e' gia' attivo
                isGrounded = true;
                isTimerStarted = true;
                startTime = Time.time;
            }
            else
            {
                if (Time.time - startTime > delay)
                {
                    // se il timer ha superato il tempo di delay imposto effettivamente isGrounded a falso, per dire agli script esterni che
                    // sono per aria
                    isGrounded = false;
                }
                else
                {
                    isGrounded = true;
                }
            }
        }
    }


    // Questa funzione viene lanciata quando Unity genera i Gizmo nell'editor, qui possiamo usare Gizmos per creare
    // riferimenti visivi personalizzati
    //private void OnDrawGizmos()
    //{
    //    if (enableDebug == false)
    //        return;

    //    Gizmos.color = Color.red;
    //    if( isGrounded )
    //    { 
    //        Gizmos.color = Color.green;
    //    }

    //    Gizmos.DrawSphere(transform.position, maxDistance);
    //}
}   