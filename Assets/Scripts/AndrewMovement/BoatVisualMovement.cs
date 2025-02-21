using UnityEngine;

[RequireComponent(typeof(BoatControl))]

public class BoatVisualMovement : MonoBehaviour
{
    [Header("Properties")]
    public float MastAngleLimit;
    public float SailMin;
    public float SailMax;
    public float WheelTurnLimit;

    
    [Header("Unity Set Up")]
    public Transform Mast;
    public Transform SailOrigin;
    public Transform Wheel;
    public Transform Anchor;

    private BoatControl movement;

    private void Awake() {
       movement = GetComponent<BoatControl>(); 
    }

    private void Update()
    {
        RotateMast();

        RotateWheel();

        FurlSail();

        AnchorShowing();
    }

    private void RotateMast(){
        float mastAngle = movement.SailTurnAmount * MastAngleLimit;
        Mast.localRotation = Quaternion.Euler(0f, mastAngle, 0f);
    }

    private void RotateWheel(){
        float wheelAngle = movement.WheelTurnAmount * WheelTurnLimit;
        Wheel.localRotation = Quaternion.Euler(wheelAngle, 0f, 0f);
    }

    private void FurlSail(){
        float sailAmount = movement.SailFurlAmount;
        sailAmount = Mathf.Clamp(sailAmount, SailMin, SailMax);
        SailOrigin.localScale = new Vector3(1f, sailAmount, 1f);
    }

    private void AnchorShowing(){
        Anchor.gameObject.SetActive(!movement.IsAnchored);
    }
}
