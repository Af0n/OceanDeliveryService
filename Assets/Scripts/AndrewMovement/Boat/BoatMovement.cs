using UnityEngine;
using System.Collections.Generic;

public class BoatMovement : MonoBehaviour
{
    public float minimumForce;
    public float TurnEffectiveness;
    [Tooltip("Amount to multiply when sailing into the wind. >1 harsher into the wind. <1 less harsh. keep positive")]
    public float WrongDirPenalty;

    [Header("Unity Set Up")]
    public Transform mast;

    private BoatControl control;
    private Rigidbody rb;
    private BoatUpgradeManager upgradeManager;

    [Header("Submarine Movement")] 
    private float onSurfaceDepth;
    public bool areFloatersActive;
    public bool isSubmarine;
    public float verticalWaterSpeed;
    public float horizontalWaterSpeed;
    public float submarineTurnEffectiveness;
    private Vector3 moveVerticalVector;
    private Vector3 moveHorizontalVector;
    
    public List<GameObject> floaters = new List<GameObject>();

    private void Awake()
    {
        control = GetComponent<BoatControl>();
        rb = GetComponent<Rigidbody>();
        upgradeManager = GetComponent<BoatUpgradeManager>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y > onSurfaceDepth)
        {
            TurnOnFloaters();
        }
        if (isSubmarine)
        {
            SubMovement();
            WaterTurn();
        }
        else
        {
            ForwardMovement();
            Turn();
        }
    }

    private void Turn(){
        if (control.IsAnchored && !upgradeManager.motorUpgrade)
        {
            return;
        }
        float turnForce = control.WheelTurnAmount * upgradeManager.turnEffectiveness;
        Vector3 force = transform.up * turnForce;
        rb.AddTorque(force);
    }

    private void WaterTurn()
    {
        float turnForce = control.WheelTurnAmount * upgradeManager.turnEffectiveness;
        Vector3 force = transform.up * turnForce;
        rb.AddTorque(force);
    }

    public void TurnOffFloaters()
    {
        areFloatersActive = false;
        for (int i = 0; i < floaters.Count; i++)
        {
            floaters[i].SetActive(false);
        }
    }
    
    public void TurnOnFloaters()
    {
        areFloatersActive = true;
        for (int i = 0; i < floaters.Count; i++)
        {
            floaters[i].SetActive(true);
        }
    }

    private void ForwardMovement(){
        if(control.IsAnchored){
            // rb.linearVelocity = Vector3.zero;
            // rb.angularVelocity = Vector3.zero;
            return;
        }

        // minimum cruising force
        rb.AddForce(transform.forward * minimumForce);

        // measuring wind alignment
        float dotProd = Vector3.Dot(mast.forward, WindManager.WindDir.normalized);

        if(dotProd < 0){
            dotProd *= WrongDirPenalty;
        }

        // use dotProd to calculate wind
        dotProd *= WindManager.WindMagnitude;

        dotProd *= control.SailFurlAmount;

        dotProd *= upgradeManager.sailSpeedAndSize;

        rb.AddForce(transform.forward * dotProd);
    }
    
    private void SubMovement(){
        moveVerticalVector = control.GetMoveVerticalVector();
        rb.AddForce(moveVerticalVector * verticalWaterSpeed);
            
        moveHorizontalVector = control.GetMoveHorizontalVector();
        rb.AddForce(moveHorizontalVector * horizontalWaterSpeed);
    }

    public void ToggleSurface(float surfaced)
    {
        onSurfaceDepth = surfaced;
    }

    void Start()
    {
        Floater.OnSurfacedBoat += ToggleSurface;
    }

    void OnDestroy()
    {
        Floater.OnSurfacedBoat -= ToggleSurface;
    }
}
