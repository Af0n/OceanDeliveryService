using System;
using System.Collections;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndrewMovement : MonoBehaviour
{
    public delegate void Swimming(bool isFloating);
    public static event Swimming StartSwim;
    public static event Swimming StopSwim;
    public static event Swimming Vertical;
    public static event Swimming OnLandMusic;
    public static event Swimming WalkAbove;
    public static event Swimming StopWalkAbove;
    public delegate void Submerge();
    public static event Submerge OnSubmerge;
    public static event Submerge JumpSound;
    public static event Submerge Landing;
    
    [Header("Movement Properties")]
    public float Speed;
    public float JumpHeight;
    public bool isSwimming;
    public bool isFloating;
    public float verticalWaterSpeed;
    private bool isOnLand;

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
    private InputAction waterVertical;

    private PlayerManager manager;
    private DeckTargetClamp deck;
    private PlayerUpgradeManager upgradeManager;

    private float yVelocity;
    public bool isGrounded;
    private bool checkForGround;
    private bool wasGrounded;
    
    private Camera MainCamera;
    private bool raisingInWater;
    private bool loweringInWater;

    private void Awake()
    {
        MainCamera = Camera.main;
        actions = new InputSystem_Actions();

        manager = GetComponent<PlayerManager>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();

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
        bool wasGroundedLastFrame = wasGrounded;
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, standableMask);

        if (!wasGroundedLastFrame && isGrounded)
        {
            Landing?.Invoke();
        }
        wasGrounded = isGrounded;
        
        if (isSwimming)
        {
            OnLandMusic?.Invoke(false);
        }
        else
        {
            Vertical?.Invoke(false);
            OnLandMusic?.Invoke(true);
        }
        
        if(manager.IsOnBoat){
            BoatMove();
        }else{
            Move();
        }
        
        Gravity();
        
    }

    private void BoatMove(){
        Vector2 readMove = move.ReadValue<Vector2>();
        if (isGrounded)
        {
            if (readMove.magnitude > 0)
            {
                WalkAbove?.Invoke(true);
            }
            else
            {
                StopWalkAbove?.Invoke(true);
            }
        }
        else
        {
            StopWalkAbove?.Invoke(true);
        }
        

        Vector3 moveVec = deck.deckTarget.forward * readMove.y + deck.deckTarget.right * readMove.x;
        moveVec *= Time.deltaTime * Speed;

        deck.deckTarget.position += moveVec;
        moveVec = deck.deckTarget.position - transform.position;
        moveVec.y = 0;
        manager.Move(moveVec);
    }

    private void Move(){
        Vector2 readMove = move.ReadValue<Vector2>();

        if (isSwimming)
        {
            if (readMove.magnitude > 0)
            {
                StartSwim?.Invoke(false);
            }
            else
            {
                StopSwim?.Invoke(false);
            }
            WaterVertical();
            Vector3 moveVec = MainCamera.transform.forward * readMove.y + MainCamera.transform.right * readMove.x;
            moveVec *= Time.deltaTime * upgradeManager.swimSpeedUpgrade;

            manager.Move(moveVec);
        } 
        else if (isFloating)
        {
            if (readMove.magnitude > 0)
            {
                StartSwim?.Invoke(true);
            }
            else
            {
                StopSwim?.Invoke(true);
            }
            WaterVertical();
            Vector3 moveVec = transform.forward * readMove.y + transform.right * readMove.x;
            moveVec *= Time.deltaTime * upgradeManager.swimSpeedUpgrade;

            manager.Move(moveVec);
        }
        else
        {
            Vector3 moveVec = transform.forward * readMove.y + transform.right * readMove.x;
            moveVec *= Time.deltaTime * Speed;

            if (isGrounded)
            {
                if (readMove.magnitude > 0)
                {
                    WalkAbove?.Invoke(false);
                }
                else
                {
                    StopWalkAbove?.Invoke(false);
                }
            }
            else
            {
                StopWalkAbove?.Invoke(false);
            }

            manager.Move(moveVec);
        }
    }

    private void Gravity(){
        if (isSwimming || isFloating)
        {
            return;
        }
        if(isGrounded && checkForGround){
            yVelocity = SettlingForce;
        }else{
            yVelocity -= GravityForce * Time.deltaTime;
        }

        manager.Move(Time.deltaTime * yVelocity * Vector3.up);
    }

    private void Jump(InputAction.CallbackContext context){
        if (isSwimming || isFloating)
        {
            return;
        }
        if(!isGrounded){
            return;
        }
        yVelocity = Mathf.Sqrt(JumpHeight * 2 * GravityForce);
        checkForGround = false;
        JumpSound?.Invoke();
        StartCoroutine(nameof(JumpGracePeriod));
    }
    private void WaterVertical(){
        float direction = waterVertical.ReadValue<float>();
        if (isSwimming)
        {
            if (direction != 0)
            {
                Vertical?.Invoke(true);
            }
            else
            {
                Vertical?.Invoke(false);
            }
            Vector3 moveVec = new Vector3(0, direction, 0);
            moveVec *= Time.deltaTime * verticalWaterSpeed;
            manager.Move(moveVec);
        }
        if (isFloating)
        {
            if (direction == -1)
            {
                OnSubmerge?.Invoke();
                manager.playerFloater.SetActive(false);
                Vector3 moveVec = new Vector3(0, direction, 0);
                moveVec *= Time.deltaTime * verticalWaterSpeed;
                manager.Move(moveVec);
                isFloating = false;
                isSwimming = true;
            }
        }
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
        
        waterVertical = actions.Player.WaterVertical;
        waterVertical.Enable();

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
