using UnityEngine;

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

    private void Awake()
    {
        control = GetComponent<BoatControl>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        ForwardMovement();

        Turn();
    }

    private void Turn(){
        float turnForce = control.WheelTurnAmount * TurnEffectiveness;
        Vector3 force = transform.up * turnForce;
        rb.AddTorque(force);
    }

    private void ForwardMovement(){
        if(control.IsAnchored){
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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

        rb.AddForce(transform.forward * dotProd);
    }
}
