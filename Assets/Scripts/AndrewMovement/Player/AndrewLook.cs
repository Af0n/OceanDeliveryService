using System.Collections;
using System.Collections.Generic;
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
    private bool wakingUp;

    private void Awake() {
        actions = new InputSystem_Actions();
        
        manager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        deck = DeckTargetClamp.instance;
        
        StartCoroutine(WakingUp());
    }

    private void Update()
    {
        if (!wakingUp)
        {
            return;
        }
        if(manager.IsThirdPerson){
            ThirdPersonBody();
            return;
        }
        
        if(manager.IsOnBoat){
            BoatLook();
            return;
        }

        Look();
    }

    public void ThirdPersonBody(){
        Vector3 dir = Cam.forward;
        dir.y = 0;
        dir.Normalize();

        transform.forward = dir;
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

    public IEnumerator WakingUp()
    {
        yield return new WaitForSeconds(10f);
        wakingUp = true;
    }
}
