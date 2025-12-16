using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerControllerRB : MonoBehaviour
{
    public Vector2 direction;
    public bool requestSprinting = false;
    [Tooltip("Requesting Jump")]
    public bool requestJumping = false;

    Animator animator;
    Rigidbody rb;
    GroundSensor groundSensor;
    [SerializeField] Camera cam;

    Vector3 correctedDir;
    Vector3 targetMove;
    Vector3 currentVelocity;

    // Qui verra' salvato lo stato corrente del personaggio, usando l'enum creato in precedenza
    [SerializeField] CharacterState currentState;

    public Vector3 currentDir;

    SmoothFloat smoothSpeed;

    [Space(10)]
    public float gravity = 9.81f;
    [Range(0.1f, 2f)]
    public float fallMultiplier = 1.5f;
    [Range(0.1f, 10f)]
    public float inertia = 0.7f;
    public float fallMovement = 1f;

    [Space(10)]
    public float jumpHeight = 2f;

    // Collegamento ai componenti del player in scena e setup variabili
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groundSensor = GetComponentInChildren<GroundSensor>();

        smoothSpeed = new SmoothFloat(0.2f);
        currentState = CharacterState.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        // Logica principale della state machine, in base allo stato corrente lancia la funzione legata a quello stato
        switch(currentState)
        {
            case CharacterState.Idle:
                IdleState();                
                break;
            case CharacterState.Walk:
                WalkState();
                break;
            case CharacterState.Sprint:
                SprintState();
                break;
            case CharacterState.StartJump: 
                StartJumpState();
                break;
            case CharacterState.Jump:
                JumpState();
                break;
            case CharacterState.Airborne:
                AirborneState();
                break;
            default:
                Debug.LogError("STATO NON TROVATO!!!!!!!!");
                break;
        }
        
        animator.SetBool("IsGrounded", groundSensor.isGrounded);
        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }

    // Questa funzione viene richiamata da Unity dopo l'elaborazione dell'animator, serve per applicare o leggere la root motion
    // senza che lo faccia Unity in automatico
    // In questo caso in base a come viene impostato targetMove dallo stato corrente applico quello spostamento al personaggio
    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            Vector3 tempMove = Vector3.ProjectOnPlane(animator.deltaPosition, groundSensor.groundNormal);

            rb.MovePosition(rb.position + tempMove);
            currentVelocity = tempMove / Time.deltaTime;
        }
    }

    // Qui di seguito sono presenti le varie funzioni legate agli stati
    // implementano la logica che in base allo stato corrente del personaggio verra' elaborata

    #region FUNCTION_STATES
    void IdleState()
    {
        // controllo se il playerinput ha richiesto un salto
        if(requestJumping == true)
        {
            requestJumping = false;
            currentState = CharacterState.StartJump;
            return;
        }

        if (CheckIsAirborne())
            return;

        // controllo se ho ricevuto input per muovermi
        if(direction.magnitude > 0 && !requestSprinting)
        {
            currentState = CharacterState.Walk;
            return;
        }

        // transizione a sprint
        if(direction.magnitude > 0 && requestSprinting)
        {
            currentState = CharacterState.Sprint;
            return;
        }

        animator.applyRootMotion = true;
        targetMove = Vector3.zero;
        animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(0));
    }

    void WalkState()
    {
        if (requestJumping == true)
        {
            requestJumping = false;
            currentState = CharacterState.StartJump;
            return;
        }

        if (CheckIsAirborne())
            return;

        // controllo se non ho input, quindi torno a idle
        if (direction.magnitude == 0)
        {
            currentState = CharacterState.Idle;
            return;
        }

        // transizione a sprint
        if (direction.magnitude > 0 && requestSprinting)
        {
            currentState = CharacterState.Sprint;
            return;
        }

        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;
        animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude));
        targetMove = Vector3.ProjectOnPlane(animator.deltaPosition, groundSensor.groundNormal);
        RotateCharacter();
    }

    void SprintState()
    {
        if (requestJumping == true)
        {
            requestJumping = false;
            currentState = CharacterState.StartJump;
            return;
        }

        if (CheckIsAirborne())
            return;

        // controllo se non ho input, quindi torno a idle
        if (direction.magnitude == 0)
        {
            currentState = CharacterState.Idle;
            return;
        }
        else if (requestSprinting == false)
        {
            currentState = CharacterState.Walk;
            return;
        }
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;
        animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude * 2));
        targetMove = Vector3.ProjectOnPlane(animator.deltaPosition, groundSensor.groundNormal);
        RotateCharacter();
    }

    void StartJumpState()
    {
        StartCoroutine(JumpCoroutine());
    }
    IEnumerator JumpCoroutine()
    {
        rb.linearVelocity = currentVelocity;
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        animator.SetTrigger("Jump");
        animator.applyRootMotion = false;
        currentState = CharacterState.Jump;
        yield return new WaitForSeconds(groundSensor.delay+0.01f);
        currentState = CharacterState.Airborne;
    }
    void JumpState()
    {
        requestJumping = false;
    }

    void AirborneState()
    {
        if (groundSensor.isGrounded)
        {
            currentState = CharacterState.Idle;
            animator.applyRootMotion = true;
            return;
        }

        //nel caso fossi in aria "consumo le richieste di salto date dall'input"
        requestJumping = false;
        animator.applyRootMotion = false;

        //movimento giocatore in aria
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;
        rb.AddForce(correctedDir * fallMovement, ForceMode.Acceleration);

        RotateCharacter();
    }

    #endregion

    bool CheckIsAirborne()
    {
        if (groundSensor.isGrounded)
        {
            return false;
        }
        else
        {
            currentState = CharacterState.Airborne;
            return true;
        }
    }

    void RotateCharacter()
    {
        if(direction.magnitude > 0)
        {
            currentDir = Vector3.Slerp(currentDir, correctedDir, Time.deltaTime * 5f);
            rb.MoveRotation(Quaternion.LookRotation(currentDir, Vector3.up));
        }
    }
}