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

    PlayerEventSystem playerEvents;
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
        playerEvents = GetComponent<PlayerEventSystem>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        groundSensor = GetComponentInChildren<GroundSensor>();

        smoothSpeed = new SmoothFloat(0.2f);
        currentState = CharacterState.Idle;
        playerEvents.OnMove += UpdateDirection;
        playerEvents.OnJumpRequested += UpdateJumpRequest;
        playerEvents.OnSprintRequest += UpdateSprintRequest;
    }

    private void OnDisable()
    {
        playerEvents.OnMove -= UpdateDirection;
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
    // In questo caso applichiamo lo spostamento dato dall'animator al rigidbody e poi calcoliamo la velocita', usata in seguito a inizio Jump
    private void OnAnimatorMove()
    {
        if (animator.applyRootMotion)
        {
            Vector3 tempMove = Vector3.ProjectOnPlane(animator.deltaPosition, groundSensor.groundNormal);

            rb.MovePosition(rb.position + tempMove);
            currentVelocity = tempMove / Time.deltaTime;
        }
    }

    void UpdateDirection(Vector2 newDir)
    {
        direction = newDir;
    }

    void UpdateJumpRequest(bool request)
    {
        requestJumping = request;
    }

    void UpdateSprintRequest(bool request)
    {
        requestSprinting = request;
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
        if (CheckIsJumpRequested())
            return;

        if (CheckIsAirborne())
            return;

        // controllo se non ho input, quindi torno a idle
        if (CheckNoMovement())
            return;

        // transizione a sprint
        if (direction.magnitude > 0 && requestSprinting)
        {
            currentState = CharacterState.Sprint;
            return;
        }
        animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude));
        RotateCharacter();
    }

    void SprintState()
    {
        if (CheckIsJumpRequested())
            return;

        if (CheckIsAirborne())
            return;

        // controllo se non ho input, quindi torno a idle
        if (CheckNoMovement())
            return;
        else if (requestSprinting == false)
        {
            currentState = CharacterState.Walk;
            return;
        }
        animator.SetFloat("Speed", smoothSpeed.GetAndUpdateValue(direction.magnitude * 2));
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
        // e disattivo la rootMotion
        requestJumping = false;
        animator.applyRootMotion = false;

        //movimento giocatore in aria, serve ruotare prima il character perche'
        // ora aggiorniamo correctedDir dentro la funzione RotateCharacter()
        RotateCharacter();
        rb.AddForce(correctedDir * fallMovement, ForceMode.Acceleration);
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

    bool CheckIsJumpRequested()
    {
        if (requestJumping == true)
        {
            requestJumping = false;
            currentState = CharacterState.StartJump;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckNoMovement()
    {
        if (direction.magnitude == 0)
        {
            currentState = CharacterState.Idle;
            return true;
        }
        else
            return false;
    }
    void RotateCharacter()
    {
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;
        currentDir = Vector3.Slerp(currentDir, correctedDir, Time.deltaTime * 5f);
        rb.MoveRotation(Quaternion.LookRotation(currentDir, Vector3.up));
    }
}