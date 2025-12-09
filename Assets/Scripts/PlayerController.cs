using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// Selettore custom utilizzato per gli stati possibili del personaggio
enum CharacterState
{
    Idle, 
    Walk,
    Sprint,
    StartJump,
    Jump,
    Airborne,
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public Vector2 direction;
    public bool requestSprinting = false;
    [Tooltip("Requesting Jump")]
    public bool requestJumping = false;

    Animator animator;
    CharacterController characterController;
    GroundSensor groundSensor;
    [SerializeField] Camera cam;

    Vector3 correctedDir;
    Vector3 initialPosition;
    Vector3 currentVelocity = Vector3.zero;
    Vector3 targetVelocity;
    Vector3 targetMove;

    // Qui verra' salvato lo stato corrente del personaggio, usando l'enum creato in precedenza
    [SerializeField] CharacterState currentState;

    public Vector3 currentDir, dirVelocity;

    SmoothFloat smoothSpeed;

    [Space(10)]
    public float gravity = 9.81f;
    [Range(0.1f, 2f)]
    public float fallMultiplier = 1.5f;
    [Range(0.1f, 10f)]
    public float inertia = 0.7f;
    public float fallMovement = 1f;

    [Space(10)]
    public float jumpDuration = 0.5f;
    public float jumpHeight = 2f;
    public float jumpHorizontalPush = 2f;
    Vector3 initialJumpVelocity;

    // Collegamento ai componenti del player in scena e setup variabili
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        groundSensor = GetComponentInChildren<GroundSensor>();

        smoothSpeed = new SmoothFloat(0.2f);
        currentVelocity = Vector3.zero;
        currentState = CharacterState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        initialPosition = transform.position;

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
        
        animator.SetFloat("VerticalSpeed", currentVelocity.y);
    }

    // Questa funzione viene richiamata da Unity dopo l'elaborazione dell'animator, serve per applicare o leggere la root motion
    // senza che lo faccia Unity in automatico
    // In questo caso in base a come viene impostato targetMove dallo stato corrente applico quello spostamento al personaggio
    private void OnAnimatorMove()
    {
        // se non stiamo saltando aggiungiamo una forza verso il basso per tenere il personaggio attaccato a terra
        if (currentState != CharacterState.Jump && currentState != CharacterState.Airborne)
        {
            targetMove += Vector3.down * 20f * Time.deltaTime;
        }

        characterController.Move(targetMove);
    }

    // LateUpdate viene richiamato dopo Update e OnAnimatorMove
    // ci serve per salvarci la velocita' del personaggio in base a quanto si e' spostato rispetto all'inizio del frame
    private void LateUpdate()
    {
        currentVelocity = (transform.position - initialPosition) / Time.deltaTime;
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
        targetMove = animator.deltaPosition;
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
        targetMove = animator.deltaPosition;
        RotateCharacter();
    }

    void StartJumpState()
    {
        StartCoroutine(JumpCoroutine());
    }

    void JumpState()
    {
        requestJumping = false;
        // Aggiungo la velocita' corrente
        targetMove = initialJumpVelocity * Time.deltaTime * jumpHorizontalPush;
        // Aggiungo l'altezza di salto
        targetMove.y = jumpHeight * Time.deltaTime / jumpDuration;
    }

    // solo dopo jumpDuration usciro' dallo stato di salto
    // e quindi non applichero' piu' la jumpHeight
    IEnumerator JumpCoroutine()
    {
        animator.SetTrigger("Jump");
        initialJumpVelocity = currentVelocity;
        currentState = CharacterState.Jump;
        yield return new WaitForSeconds(jumpDuration);
        currentState = CharacterState.Airborne;
    }

    void AirborneState()
    {
        if (groundSensor.isGrounded)
        {
            currentState = CharacterState.Idle;
            animator.SetBool("IsGrounded", true);
            return;
        }

        //nel caso fossi in aria "consumo le richieste di salto date dall'input"
        requestJumping = false;

        animator.SetBool("IsGrounded", false);
        //applico drag -> attrito aerodinamico
        currentVelocity = currentVelocity.normalized * (currentVelocity.magnitude - (inertia * Time.deltaTime));
        // caduta
        targetMove = (currentVelocity - (Vector3.up * gravity * fallMultiplier * Time.deltaTime)) * Time.deltaTime;

        //movimento giocatore in aria
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        correctedDir = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * dir;
        targetMove = targetMove + (correctedDir * fallMovement * Time.deltaTime);
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
        currentDir = Vector3.Slerp(currentDir, correctedDir, Time.deltaTime * 5f);

        if(direction.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(currentDir, Vector3.up);
    }
}