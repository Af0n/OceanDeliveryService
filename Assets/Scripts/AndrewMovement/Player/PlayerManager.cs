using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private AndrewMovement movement;
    private AndrewLook look;
    private PlayerInteract interaction;
    private CharacterController controller;
    
    public bool IsOnBoat;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
        interaction = GetComponent<PlayerInteract>();
        controller = GetComponent<CharacterController>();
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch(other.tag){
            case "BoatTrigger":
                IsOnBoat = false;
                break;
        }
    }
}
