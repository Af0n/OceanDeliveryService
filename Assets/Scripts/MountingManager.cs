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
        
        boatInput.enabled = true;
        boatCamera.SetActive(true);
    }

    public void DismountTheBoat()
    {
        playerInput.enabled = true;
        playerCamera.SetActive(true);
        playerInput.gameObject.GetComponent<PlayerMovement>().DeactivateWheel();
        
        boatInput.enabled = false;
        boatCamera.SetActive(false);
    }
    
}
