using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

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

    private CharacterController controller;
    private float yVelocity;
    private bool isGrounded;
    private bool checkForGround;

    private void Awake()
    {
        actions = new InputSystem_Actions();

        controller = GetComponent<CharacterController>();

        if(UsePhysicsGravity){
            GravityForce = Physics.gravity.magnitude;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // checking for ground
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, standableMask);

        Move();

        Gravity();
    }

    private void Move(){
        Vector2 readMove = move.ReadValue<Vector2>();

        Vector3 moveVec = transform.forward * readMove.y + transform.right * readMove.x;
        moveVec *= Time.deltaTime * Speed;

        controller.Move(moveVec);
    }

    private void Gravity(){
        if(isGrounded && checkForGround){
            yVelocity = SettlingForce;
        }else{
            yVelocity -= GravityForce * Time.deltaTime;
        }

        controller.Move(Time.deltaTime * yVelocity * Vector3.up);
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

        controller.enabled = true;
    }

    void OnDisable()
    {
        // input system boilerplate
        move.Disable();
        jump.Disable();

        controller.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
    }
}
