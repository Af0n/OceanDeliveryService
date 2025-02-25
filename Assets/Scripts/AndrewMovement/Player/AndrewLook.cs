using UnityEngine;
using UnityEngine.InputSystem;

public class AndrewLook : MonoBehaviour
{
    [Header("Look Properties")]
    public float Sens;
    public float MinLookAngle;
    public float MaxLookAngle;

    [Header("Unity Set Up")]
    public Transform Cam;

    private float xRotation;

    private PlayerManager manager;
    private InputSystem_Actions actions;
    private InputAction look;
    
    private DeckTargetClamp deck;

    private void Awake() {
        actions = new InputSystem_Actions();
        
        manager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        deck = DeckTargetClamp.instance;
    }

    private void Update()
    {
        if(manager.IsOnBoat){
            BoatLook();
            return;
        }
        Look();
    }

    private void BoatLook()
    {
        Vector2 readLook = look.ReadValue<Vector2>();
        readLook *= Time.deltaTime * Sens;

        // rotate player body sideways
        deck.AddYRot(readLook.x);
        transform.rotation = Quaternion.Euler(0f, deck.deckTarget.eulerAngles.y, 0f);


        // rotate camera
        xRotation -= readLook.y;
        xRotation = Mathf.Clamp(xRotation, MinLookAngle, MaxLookAngle);
        Cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void Look()
    {
        Vector2 readLook = look.ReadValue<Vector2>();
        readLook *= Time.deltaTime * Sens;

        // rotate player body sideways
        transform.Rotate(Vector3.up, readLook.x);

        // rotate camera
        xRotation -= readLook.y;
        xRotation = Mathf.Clamp(xRotation, MinLookAngle, MaxLookAngle);
        Cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void OnEnable()
    {
        // input system boilerplate
        look = actions.Player.Look;
        look.Enable();
    }

    private void OnDisable()
    {
        // input system boilerplate
        look.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        switch(other.tag){
            case "BoatTrigger":
                deck.SetYRot(transform.eulerAngles.y);
                break;
        }
    }
}
