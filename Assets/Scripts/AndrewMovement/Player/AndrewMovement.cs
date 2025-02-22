using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndrewMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public float Speed;
    public float JumpHeight;

    [Header("Gravity Properties")]
    public bool UsePhysicsGravity;
    public float GravityForce;
    public float SettlingForce;

    [Header("Unity Set Up")]
    public Transform GroundCheck;
    public LayerMask standableMask;
    public float GroundCheckRadius;
    public float GroundedGracePeriod;

    // input system
    private InputSystem_Actions actions;
    private InputAction move;
    private InputAction jump;

    private PlayerManager manager;
    private DeckTargetClamp deck;

    private float yVelocity;
    private bool isGrounded;
    private bool checkForGround;

    private void Awake()
    {
        actions = new InputSystem_Actions();

        manager = GetComponent<PlayerManager>();

        if(UsePhysicsGravity){
            GravityForce = Physics.gravity.magnitude;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        deck = DeckTargetClamp.instance;
    }

    private void Update()
    {
        // checking for ground
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, standableMask);

        if(manager.IsOnBoat){
            BoatMove();
        }else{
            Move();
        }
        
        Gravity();
    }

    private void BoatMove(){
        Vector2 readMove = move.ReadValue<Vector2>();

        Vector3 moveVec = deck.deckTarget.forward * readMove.y + deck.deckTarget.right * readMove.x;
        moveVec *= Time.deltaTime * Speed;

        deck.deckTarget.position += moveVec;
        moveVec = deck.deckTarget.position - transform.position;
        moveVec.y = 0;
        manager.Move(moveVec);
    }

    private void Move(){
        Vector2 readMove = move.ReadValue<Vector2>();

        Vector3 moveVec = transform.forward * readMove.y + transform.right * readMove.x;
        moveVec *= Time.deltaTime * Speed;

        manager.Move(moveVec);
    }

    private void Gravity(){
        if(isGrounded && checkForGround){
            yVelocity = SettlingForce;
        }else{
            yVelocity -= GravityForce * Time.deltaTime;
        }

        manager.Move(Time.deltaTime * yVelocity * Vector3.up);
    }

    private void Jump(InputAction.CallbackContext context){
        if(!isGrounded){
            return;
        }
        yVelocity = Mathf.Sqrt(JumpHeight * 2 * GravityForce);
        checkForGround = false;
        StartCoroutine(nameof(JumpGracePeriod));
    }

    private IEnumerator JumpGracePeriod(){
        yield return new WaitForSeconds(GroundedGracePeriod);
        checkForGround = true;
    }

    void OnEnable()
    {
        // input system boilerplate
        move = actions.Player.Move;
        move.Enable();

        jump = actions.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        manager.enabled = true;
    }

    void OnDisable()
    {
        // input system boilerplate
        move.Disable();
        jump.Disable();

        manager.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
    }

    void OnTriggerEnter(Collider other)
    {
        switch(other.tag){
            case "BoatTrigger":
                deck.SetPos(transform.position);
                break;
        }
    }
}
