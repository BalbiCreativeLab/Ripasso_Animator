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
            // avvio il timer se non sono piu' per terra e il timer non e' gia' attivo
            isGrounded = true;
            isTimerStarted = true;
            startTime = Time.time;
        }
        else if (isTimerStarted)
        {
            //invece se il timer e' attivo...
            if(!isGrounded)
            {
                if (Time.time - startTime > delay)
                {
                    // se il timer ha superato il tempo di delay imposto effettivamente isGrounded a falso, per dire agli script esterni che
                    // sono per aria
                    isGrounded = false;
                }
                else
                {
                    // se continuo ad essere per aria e il timer non ha superato il tempo di delay, forzo isGrounded a vero
                    isGrounded = true;
                }
            }
            else
            {
                // se prima che scada il timer raggiungo il terreno, resetto il timer
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