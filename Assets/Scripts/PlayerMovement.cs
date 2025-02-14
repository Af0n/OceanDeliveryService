using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public delegate void MountBoat();
    public static event MountBoat mountBoat; 
    
    private Vector2 input;
    private Vector3 direction;
    private float gravity = -9.81f;
    private float velocity;
    private CharacterController cc;
    public float jumpPower;
    public float speed;
    public float acceleration;
    public float sprintMultiplier;
    private bool isSprinting;
    private float currentSpeed;
    public Camera cam;
    private bool inWheelRange = false;
    
    public float mouseSensitivity = 20f;
    private float xRotation = 0f;
    public float yRotationLock = 45f;

    public GameObject boat;
    private GameObject Wheel;
    private bool isGrounded;
    private Vector3 parentMovement;
    private bool isOnBoat;
    private bool isOnWheel;
    private BoatMovement boatMovement;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        boatMovement = boat.GetComponent<BoatMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        ApplyMovement();
    }
    
    public void Move(InputAction.CallbackContext context){
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
    }
    
    public void Look(InputAction.CallbackContext context){
        if (!context.performed) return;

        Vector2 lookInput = context.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -yRotationLock, yRotationLock);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    public void Jump(InputAction.CallbackContext context){
        if(!context.started){
            return;
        }
        if(!IsGrounded()){
            return;
        }
        velocity = jumpPower;
    }
    
    public void Sprint(InputAction.CallbackContext context){
        if (context.started) {
            isSprinting = !isSprinting;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started) {
            if (inWheelRange)
            {
                Debug.Log("Interacting with wheel");
                //Turn Player into Boat
                transform.rotation = Wheel.transform.rotation;
                isOnWheel = true;
                mountBoat?.Invoke();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            isOnBoat = true;
        }
        if (other.gameObject.CompareTag("SteeringWheel"))
        {
            Wheel = other.gameObject;
            inWheelRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            isOnBoat = false;
            transform.parent = null;
        }
        if (other.gameObject.CompareTag("SteeringWheel"))
        {
            Debug.Log(other.gameObject.name);
            inWheelRange = false;
        }
    }
    
    private void ApplyGravity(){
        if(IsGrounded() && velocity < 0.0f){
            velocity = -1.0f;
        } else {
           
            velocity += gravity * Time.deltaTime;
        }
        direction.y = velocity;
    }

    private void ApplyMovement(){
        var targetSpeed = isSprinting ? speed * sprintMultiplier : speed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        
        Vector3 localForward = transform.forward;
        Vector3 localRight = transform.right;
        
        localForward.y = 0;
        localRight.y = 0;
        
        localForward.Normalize();
        localRight.Normalize();
        
        direction = (localForward * input.y + localRight * input.x).normalized;
        direction.y = velocity;
        
        direction *= currentSpeed;
        cc.Move(direction * Time.deltaTime);
    }

    public void DeactivateWheel()
    {
        isOnWheel = false;
    }
    public bool IsGrounded() => cc.isGrounded;
}
