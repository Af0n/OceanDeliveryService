using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    public delegate void DismountBoat();
    public static event DismountBoat dismountBoat; 
    
    public GameObject player;
    public bool anchored = false;

    public bool hasMotorUpgrade = false;

    public GameObject sailPole;
    public GameObject sailCloth;

    public float maxPoleRotate = 45f;
    private float targetRotation;
    private float poleRotation;
    public float poleRotationSpeed = 2f;
    
    private float inputValuePole;
    private float inputValueCloth;
    private float inputValueTurn;

    public float distanceFromPlayer;
    
    private Vector3 startSailPosition = new Vector3(0.2f, 0.4f, 1f);
    private Vector3 endSailPosition = new Vector3(0.2f, 0.2f, 1f);

    private Vector3 startSailScale = new Vector3(1f, 0.1f, 6f);
    private Vector3 endSailScale = new Vector3(1f, 0.5f, 6f);
    private float sailLerpValue = 0f;
    public float slowFactor = 0.5f;
    
    public float rotationSpeed = 0f; // Current rotation speed
    public float maxRotationSpeed = 10f; // Maximum rotation speed
    public float acceleration = 2f; // How fast it speeds up
    public float deceleration = 2f; // How fast it slows down when counteracting the other direction
    public float boatSpeed = 10f;
    [HideInInspector]
    public Vector3 boatDirection = new Vector3(0f, 0f, 0f);
    
    void Update()
    {
        AdjustSailCloth();
        RotateSail();
        
        if (Vector3.Distance(transform.position, player.transform.position) > distanceFromPlayer)
        {
            anchored = true;
        }
        
        if (anchored)
        {
            if (hasMotorUpgrade)
            {
                TurnBoat();
            }
            return;
        }
        transform.Translate(Vector3.forward * boatSpeed * Time.deltaTime);
        boatDirection = Vector3.forward * boatSpeed;
        TurnBoat();
    }
    
    public void Anchor(InputAction.CallbackContext context){
        if(context.started){
            anchored = !anchored;
        }
    }

    public void SailCloth(InputAction.CallbackContext context)
    {
        inputValueCloth = context.ReadValue<float>();
    }

    public void SailPole(InputAction.CallbackContext context)
    {
        inputValuePole = context.ReadValue<float>();
    }
    
    public void AdjustSailCloth()
    {
        if (inputValueCloth != 0) 
        {
            float adjustedInput = inputValueCloth > 0 ? inputValueCloth : inputValueCloth * slowFactor;
        
            sailLerpValue += adjustedInput * Time.deltaTime;
            sailLerpValue = Mathf.Clamp01(sailLerpValue);
        }

        sailCloth.transform.localPosition = Vector3.Lerp(startSailPosition, endSailPosition, sailLerpValue);
        sailCloth.transform.localScale = Vector3.Lerp(startSailScale, endSailScale, sailLerpValue);
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        inputValueTurn = context.ReadValue<float>();
    }

    public void TurnBoat()
    {
        if (inputValueTurn != 0)
        {
            float direction = Mathf.Sign(inputValueTurn);
            if (Mathf.Sign(rotationSpeed) != direction && rotationSpeed != 0)
            {
                rotationSpeed += direction * deceleration * Time.deltaTime;
            }
            else
            {
                rotationSpeed += direction * acceleration * Time.deltaTime;
                rotationSpeed = Mathf.Clamp(rotationSpeed, -maxRotationSpeed, maxRotationSpeed);
            }
        }

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    
    public void RotateSail()
    {
        if (inputValuePole != 0)
        {
            targetRotation += inputValuePole * poleRotationSpeed * Time.deltaTime;
            targetRotation = Mathf.Clamp(targetRotation, -maxPoleRotate, maxPoleRotate);
        }

        poleRotation = Mathf.LerpAngle(poleRotation, targetRotation, Time.deltaTime * 10f); 

        sailPole.transform.localRotation = Quaternion.Euler(0f, poleRotation, 0f);
    }

    public void Dismount(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dismountBoat?.Invoke();
        }
        
    }
}
