using UnityEngine;
using UnityEngine.InputSystem;
public class MountingManager : MonoBehaviour
{
    public GameObject boat;
    public GameObject playerCamera;

    public GameObject boatCamera;
    
    public PlayerInput playerInput;
    public PlayerInput boatInput;
    

    void Start()
    {
        PlayerMovement.mountBoat += MountTheBoat;
        BoatMovement.dismountBoat += DismountTheBoat;
    }
    
    void OnDestroy(){
        PlayerMovement.mountBoat -= MountTheBoat;
        BoatMovement.dismountBoat -= DismountTheBoat;
    }

    public void MountTheBoat()
    {
        playerInput.enabled = false;
        playerCamera.SetActive(false);
        playerInput.gameObject.GetComponent<CharacterController>().enabled = false;
        playerInput.gameObject.GetComponent<PlayerMovement>().enabled = false;
        playerInput.gameObject.transform.SetParent(boatInput.gameObject.transform, true);
        
        boatInput.enabled = true;
        boatCamera.SetActive(true);
    }

    public void DismountTheBoat()
    {
        playerInput.enabled = true;
        playerCamera.SetActive(true);
        playerInput.gameObject.GetComponent<PlayerMovement>().enabled = true;
        playerInput.gameObject.GetComponent<CharacterController>().enabled = true;
        playerInput.gameObject.transform.SetParent(null, true);

        boatInput.enabled = false;
        boatCamera.SetActive(false);
    }
    
}
