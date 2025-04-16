using System;
using System.Collections;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndrewMovement : MonoBehaviour
{
    public AudioManager audioManager;
    public Animator animator;

    [Header("Movement Properties")]
    public float Speed;
    public float JumpHeight;
    [HideInInspector] public bool canSwim;
    public bool isSwimming;
    public bool isFloating;
    public float verticalWaterSpeed;
    private bool isOnLand;

    [Header("Gravity Properties")]
    public bool UsePhysicsGravity;
    public float GravityForce;
    public float SettlingForce;

    [Header("Unity Set Up")]
    public Transform Cam;
    public Transform CamTarget;
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
        animator = GetComponent<Animator>();

        if (audioManager == null)
        {
            GameObject audioGameObject = GameObject.Find("AudioManager");
            audioManager = audioGameObject.GetComponent<AudioManager>();
        }
        if (UsePhysicsGravity)
        {
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
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Jump", !isGrounded);

        if (!wasGroundedLastFrame && isGrounded)
        {
            audioManager.Landing();
        }
        wasGrounded = isGrounded;

        if (isSwimming || isFloating)
        {
            animator.SetBool("Water", true);
        }
        else
        {
            animator.SetBool("Water", false);
        }

        if (isSwimming)
        {
            audioManager.PlayMusic(false);
        }
        else
        {
            audioManager.Vertical(false);
            audioManager.PlayMusic(true);
        }

        if (manager.IsOnBoat)
        {
            BoatMove();
        }
        else
        {
            Move();
        }

        Gravity();

    }

    private void BoatMove()
    {
        Vector2 readMove = move.ReadValue<Vector2>();
        if (isGrounded)
        {
            if (readMove.magnitude > 0)
            {
                audioManager.StartWalking(true);
            }
            else
            {
                audioManager.StopWalking(true);
            }
        }
        else
        {
            audioManager.StopWalking(true);
        }

        Vector3 moveVec;
        if (manager.IsThirdPerson)
        {
            Vector3 temp = CalculateCameraForward();
            CamTarget.forward = temp;
            moveVec = CamTarget.forward * readMove.y + CamTarget.right * readMove.x;
        }
        else
        {
            moveVec = deck.deckTarget.forward * readMove.y + deck.deckTarget.right * readMove.x;
        }

        moveVec *= Time.deltaTime * Speed;

        deck.deckTarget.position += moveVec;
        moveVec = deck.deckTarget.position - transform.position;
        moveVec.y = 0;
        animator.SetFloat("Speed", readMove.magnitude * Speed);
        animator.SetFloat("MotionSpeed", 1);
        manager.Move(moveVec);
    }

    private void Move()
    {
        Vector2 readMove = move.ReadValue<Vector2>();
        animator.SetFloat("Speed", readMove.magnitude * Speed);
        animator.SetFloat("MotionSpeed", 1);
        Vector3 moveVec;
        if (manager.IsThirdPerson)
        {
            Vector3 temp = CalculateCameraForward();
            CamTarget.forward = temp;
            moveVec = CamTarget.forward * readMove.y + CamTarget.right * readMove.x;
        }
        else
        {
            moveVec = transform.forward * readMove.y + transform.right * readMove.x;
        }

        if (canSwim)
        {
            if (isSwimming)
            {
                if (readMove.magnitude > 0)
                {
                    audioManager.StartSwimming(false);
                }
                else
                {
                    audioManager.StopSwimming(false);
                }
                WaterVertical();
                moveVec *= Time.deltaTime * upgradeManager.swimSpeedUpgrade;

                manager.Move(moveVec);
            }
            else if (isFloating)
            {
                if (readMove.magnitude > 0)
                {
                    audioManager.StartSwimming(true);
                }
                else
                {
                    audioManager.StopSwimming(true);
                }
                WaterVertical();
                moveVec *= Time.deltaTime * upgradeManager.swimSpeedUpgrade;

                manager.Move(moveVec);
            }
            else
            {
                moveVec *= Time.deltaTime * Speed;

                if (isGrounded)
                {
                    if (readMove.magnitude > 0)
                    {
                        audioManager.StartWalking(false);
                    }
                    else
                    {
                        audioManager.StopWalking(false);
                    }
                }
                else
                {
                    audioManager.StopWalking(false);
                }

                manager.Move(moveVec);
            }
        }
        else
        {
            moveVec *= Time.deltaTime * Speed;

            if (isGrounded)
            {
                if (readMove.magnitude > 0)
                {
                    audioManager.StartWalking(false);
                }
                else
                {
                    audioManager.StopWalking(false);
                }
            }
            else
            {
                audioManager.StopWalking(false);
            }

            manager.Move(moveVec);
        }
    }

    public Vector3 CalculateCameraForward()
    {
        Vector3 temp = Cam.forward;
        temp.y = 0;
        temp.Normalize();
        return temp;
    }

    private void Gravity()
    {
        if (isSwimming || isFloating)
        {
            return;
        }
        if (isGrounded && checkForGround)
        {
            yVelocity = SettlingForce;
        }
        else
        {
            yVelocity -= GravityForce * Time.deltaTime;
        }

        manager.Move(Time.deltaTime * yVelocity * Vector3.up);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isSwimming || isFloating)
        {
            return;
        }
        if (!isGrounded)
        {
            return;
        }
        yVelocity = Mathf.Sqrt(JumpHeight * 2 * GravityForce);
        checkForGround = false;
        audioManager.Jump();
        animator.SetBool("Jump", true);
        StartCoroutine(nameof(JumpGracePeriod));
    }
    private void WaterVertical()
    {
        float direction = waterVertical.ReadValue<float>();
        if (isSwimming)
        {
            if (direction != 0)
            {
                audioManager.Vertical(true);
            }
            else
            {
                audioManager.Vertical(false);
            }
            Vector3 moveVec = new Vector3(0, direction, 0);
            moveVec *= Time.deltaTime * verticalWaterSpeed;
            manager.Move(moveVec);
        }
        if (isFloating)
        {
            if (direction == -1)
            {
                audioManager.Submerge();
                manager.playerFloater.SetActive(false);
                Vector3 moveVec = new Vector3(0, direction, 0);
                moveVec *= Time.deltaTime * verticalWaterSpeed;
                manager.Move(moveVec);
                isFloating = false;
                isSwimming = true;
            }
        }
    }

    private IEnumerator JumpGracePeriod()
    {
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
        switch (other.tag)
        {
            case "BoatTrigger":
                deck.SetPos(transform.position);
                break;
        }
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        //Leave here so the errors are not happening
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        //Leave here so the errors are not happening
    }
}
