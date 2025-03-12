using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ControlBoatToggle))]

public class BoatControl : MonoBehaviour
{
    [Header("Boat Limits")]
    [Tooltip("How long it takes to turn the wheel from the center to any extreme. Keep positive")]
    public float WheelTurnTime;
    [Tooltip("How long it takes to turn the sail from the center to any extreme. Keep positive")]
    public float SailTurnTime;
    [Tooltip("How long it takes to open the sail from closed. Keep positive")]
    public float SailFurlTime;
    public bool StartAnchored;
    
    // value between [-1, 1]
    public float WheelTurnAmount
    {
        get { return wheelAngle / WheelTurnTime; }
    }

    // value between [-1, 1]
    public float SailTurnAmount
    {
        get { return sailAngle / SailTurnTime; }
    }

    // value between [0, 1]
    public float SailFurlAmount
    {
        get { return sailAmount / SailFurlTime; }
    }

    public bool IsAnchored{
        get{ return isAnchored; }
    }

    private ControlBoatToggle controlBoatToggle;

    private float wheelAngle;
    private float sailAngle;
    private float sailAmount;
    private bool isAnchored;

    // input system
    private InputSystem_Actions actions;
    private InputAction wheelTurn;
    private InputAction sailTurn;
    private InputAction sailLower;
    private InputAction anchor;
    private InputAction dismount;
    private InputAction waterVertical;
    private InputAction waterForwardBackward;
    private InputAction submarine;
    
    
    
    private Vector3 moveVerticalVector;
    private Vector3 moveHorizontalVector;
    private BoatMovement boatMovement;
    private BoatUpgradeManager boatUpgradeManager;
    public AudioManager audioManager;
    

    private void Awake()
    {
        if (audioManager == null)
        {
            GameObject audioGameObject = GameObject.Find("AudioManager");
            audioManager = audioGameObject.GetComponent<AudioManager>();
        }
        actions = new InputSystem_Actions();

        controlBoatToggle = GetComponent<ControlBoatToggle>();

        isAnchored = StartAnchored;
        boatMovement = GetComponent<BoatMovement>();
        boatUpgradeManager = GetComponent<BoatUpgradeManager>();
    }

    void Update()
    {
        TurnWheel();

        TurnSail();

        FurlSail();
        
        WaterVertical();
        
        WaterForwardBackward();
    }

    private void TurnWheel()
    {
        wheelAngle += wheelTurn.ReadValue<float>() * Time.deltaTime;
        wheelAngle = Mathf.Clamp(wheelAngle, -WheelTurnTime, WheelTurnTime);
    }

    private void TurnSail()
    {
        sailAngle += sailTurn.ReadValue<float>() * Time.deltaTime;
        sailAngle = Mathf.Clamp(sailAngle, -SailTurnTime, SailTurnTime);
    }

    private void FurlSail()
    {
        if (boatMovement.isSubmarine)
        {
            return;
        }
        sailAmount += sailLower.ReadValue<float>() * Time.deltaTime;
        sailAmount = Mathf.Clamp(sailAmount, 0, SailFurlTime);
    }

    private void Dismount(InputAction.CallbackContext context)
    {
        controlBoatToggle.MountToggle();
    }

    private void TurnToSub(InputAction.CallbackContext context)
    {
        if (boatUpgradeManager.submersible)
        {
            if (boatMovement.isSubmarine && boatMovement.areFloatersActive)
            {
                audioManager.BoatMovement(false);
                boatMovement.isSubmarine = false;
                Debug.Log("The vehicle is now a Ship");
                audioManager.SwitchVehicles();
            }
            else if (!boatMovement.isSubmarine)
            {
                audioManager.BoatMovement(true);
                boatMovement.isSubmarine = true;
                isAnchored = false;
                Debug.Log("The vehicle is now a Submarine");
                audioManager.SwitchVehicles();
            }
        }
    }

    private void AnchorToggle(InputAction.CallbackContext context){
        if (boatMovement.isSubmarine)
        {
            isAnchored = false;
            return;
        }
        isAnchored = !isAnchored;
        if (isAnchored)
        {
            audioManager.BoatMovement(false);
        }
        else
        {
            audioManager.BoatMovement(true);
        }
        audioManager.Anchor();
    }
    
    private void WaterVertical(){
        if (!boatMovement.isSubmarine)
        {
            return;
        }
        float direction = waterVertical.ReadValue<float>();
        if (boatMovement.isSubmarine && direction == -1)
        {
            boatMovement.TurnOffFloaters();
        }
        moveVerticalVector = new Vector3(0, direction, 0);
    }
    
    private void WaterForwardBackward(){
        if (!boatMovement.isSubmarine)
        {
            return;
        }
        float direction = waterForwardBackward.ReadValue<float>();
        moveHorizontalVector = transform.forward * direction;
    }

    public Vector3 GetMoveVerticalVector()
    {
        return moveVerticalVector;
    }

    public Vector3 GetMoveHorizontalVector()
    {
        return moveHorizontalVector;
    }

    public void AlterAnchor()
    {
        isAnchored = false;
    }

    void OnEnable()
    {
        // input system boilerplate
        wheelTurn = actions.Boat.Wheel;
        wheelTurn.Enable();

        sailTurn = actions.Boat.SailTurn;
        sailTurn.Enable();

        sailLower = actions.Boat.SailLower;
        sailLower.Enable();

        dismount = actions.Boat.Dismount;
        dismount.Enable();
        dismount.performed += Dismount;

        anchor = actions.Boat.Anchor;
        anchor.Enable();
        anchor.performed += AnchorToggle;
        
        waterVertical = actions.Boat.WaterVertical;
        waterVertical.Enable();
        
        waterForwardBackward = actions.Boat.WaterForwardBackward;
        waterForwardBackward.Enable();
        
        submarine = actions.Boat.Submarine;
        submarine.Enable();
        submarine.performed += TurnToSub;
    }

    void OnDisable()
    {
        // input system boilerplate
        wheelTurn.Disable();
        sailTurn.Disable();
        sailLower.Disable();
        dismount.Disable();
        anchor.Disable();
    }
}
