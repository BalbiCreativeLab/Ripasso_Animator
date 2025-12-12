using Unity.VisualScripting;
using UnityEngine;

// Questo 'sensore' controlla se un collider e' all'interno di un certo raggio a partire dal suo centro,
// si potrebbe usare un Raycast al posto di OverlapSphere e filtrare i collider percepiti tramite layermask
public class GroundSensor : MonoBehaviour
{
    public float radius = 0.06f;
    public bool isGrounded = false;
    public bool enableDebug = true;
    public bool isTimerStarted = false;
    public float startTime = 0;
    public float delay = 3;

    private void Start()
    {
        isGrounded = Physics.OverlapSphere(transform.position, radius).Length > 0;
    }

    void Update()
    {
        // OverlapSphere controlla se entro un raggio a partire da un punto nello spazio ci sono dei collider
        // il gameobject che ha questo script non ha bisogno di avere un collider e/o un Rigidbody
        isGrounded = Physics.OverlapSphere(transform.position, radius).Length > 0;

        if (!isTimerStarted && !isGrounded)
        {
            print("START TIMER");
            isGrounded = true;
            isTimerStarted = true;
            startTime = Time.time;
        }
        else if (isTimerStarted)
        {
            if(!isGrounded)
            {
                if (Time.time - startTime > delay)
                {
                    print("TIMER ELAPSED");
                    isGrounded = false;
                }
                else
                {
                    isGrounded = true;
                }
            }
            else
            {
                print("TIMER STOPPED, back to ground");
                isTimerStarted = false;
            }
        }
    }

    // Questa funzione viene lanciata quando Unity genera i Gizmo nell'editor, qui possiamo usare Gizmos per creare
    // riferimenti visivi personalizzati

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