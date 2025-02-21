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

    private void Awake()
    {
        actions = new InputSystem_Actions();

        controlBoatToggle = GetComponent<ControlBoatToggle>();
    }

    void Update()
    {
        TurnWheel();

        TurnSail();

        FurlSail();
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
        sailAmount += sailLower.ReadValue<float>() * Time.deltaTime;
        sailAmount = Mathf.Clamp(sailAmount, 0, SailFurlTime);
    }

    private void Dismount(InputAction.CallbackContext context)
    {
        controlBoatToggle.MountToggle();
    }

    private void AnchorToggle(InputAction.CallbackContext context){
        isAnchored = !isAnchored;
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
