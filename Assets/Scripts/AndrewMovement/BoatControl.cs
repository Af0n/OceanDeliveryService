using UnityEngine;
using UnityEngine.InputSystem;

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
    public float WheelTurnAmount{
        get{ return wheelAngle / WheelTurnTime; }
    }

    // value between [-1, 1]
    public float SailTurnAmount{
        get{ return sailAngle / SailTurnTime; }
    }

    // value between [0, 1]
    public float SailFurlAmount{
        get{ return sailAmount / SailFurlTime; }
    }

    private float wheelAngle;
    private float sailAngle;
    private float sailAmount;

    // input system
    private InputSystem_Actions actions;
    private InputAction wheelTurn;
    private InputAction sailTurn;
    private InputAction sailLower;

    private void Awake() {
        actions = new InputSystem_Actions();
    }

    void Update()
    {
        TurnWheel();

        TurnSail();

        FurlSail();
    }

    private void TurnWheel(){
        wheelAngle += wheelTurn.ReadValue<float>() * Time.deltaTime;
        wheelAngle = Mathf.Clamp(wheelAngle, -WheelTurnTime, WheelTurnTime);
    }

    private void TurnSail(){
        sailAngle += sailTurn.ReadValue<float>() * Time.deltaTime;
        sailAngle = Mathf.Clamp(sailAngle, -SailTurnTime, SailTurnTime);
    }

    private void FurlSail(){
        sailAmount += sailLower.ReadValue<float>() * Time.deltaTime;
        sailAmount = Mathf.Clamp(sailAmount, 0, SailFurlTime);
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
    }

    void OnDisable()
    {
        // input system boilerplate
        wheelTurn.Disable();
        sailTurn.Disable();
        sailLower.Disable();
    }
}
