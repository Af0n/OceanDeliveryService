using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    private AndrewMovement movement;
    private AndrewLook look;
    private PlayerInteract interaction;
    private CharacterController controller;
    
    public bool IsOnBoat;
    public float onSurfaceDepth = -1f;
    public GameObject playerFloater;
    public switchCam cameraSwitcher;
    public GameObject inventorySystem;
    public float inventoryVisibility = 1f;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
        interaction = GetComponent<PlayerInteract>();
        controller = GetComponent<CharacterController>();
    }
    
    private void FixedUpdate()
    {
        if (playerFloater.transform.position.y > onSurfaceDepth && playerFloater.transform.position.y < 0)
        {
            movement.isSwimming = false;
            movement.isFloating = true;
            playerFloater.SetActive(true);
        } else if (playerFloater.transform.position.y < onSurfaceDepth)
        {
            movement.isSwimming = true;
            movement.isFloating = false;
            playerFloater.SetActive(false);
        }
        else if (playerFloater.transform.position.y > 0)
        {
            movement.isSwimming = false;
            movement.isFloating = false;
            playerFloater.SetActive(false);
        }
    }

    public void SetMovement(bool b)
    {
        movement.enabled = b;
    }

    public void SetLook(bool b)
    {
        look.enabled = b;
    }

    public void SetMoveLook(bool b)
    {
        SetMovement(b);
        SetLook(b);
    }

    public void SetInteraction(bool b)
    {
        interaction.enabled = b;
    }

    public void SetAll(bool b){
        SetMoveLook(b);
        SetInteraction(b);
    }

    public void Move(Vector3 vec){
        controller.Move(vec);
    }

    void OnTriggerEnter(Collider other)
    {
        switch(other.tag){
            case "BoatTrigger":
                IsOnBoat = true;
                break;
            case "DeliveryZone":
                SetLook(false);
                Debug.Log("Entered Delivery Zone");
                
                cameraSwitcher.ChangeCamera();
                cameraSwitcher.ManagerCamera();
                // inventorySystem.SetActive(true);
                StartCoroutine(DelayedInventory());
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch(other.tag){
            case "BoatTrigger":
                IsOnBoat = false;
                break;
            case "DeliveryZone":
                Debug.Log("Exited Delivery Zone");
                SetLook(true);
                
                cameraSwitcher.ChangeCamera();
                cameraSwitcher.ManagerCamera();
                // inventorySystem.SetActive(false);
                StartCoroutine(DelayedInventory());
                break;
        }
    }
    private IEnumerator DelayedInventory()
    {
        yield return new WaitForSeconds(inventoryVisibility);
        if (inventorySystem.activeSelf == true){
            inventorySystem.SetActive(false);
        }else{
            inventorySystem.SetActive(true);
        }
    }
    
    public void ToggleSurface(float surfaced)
    {
        // onSurfaceDepth = surfaced;
    }

    void Start()
    {
        PlayerFloater.OnSurfacedPlayer += ToggleSurface;
    }

    void OnDestroy()
    {
        PlayerFloater.OnSurfacedPlayer -= ToggleSurface;
    }
}
