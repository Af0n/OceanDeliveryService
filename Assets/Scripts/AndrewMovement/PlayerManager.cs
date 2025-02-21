using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private AndrewMovement movement;
    private AndrewLook look;
    private PlayerInteract interaction;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
        interaction = GetComponent<PlayerInteract>();
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
}
