using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private AndrewMovement movement;
    private AndrewLook look;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
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
}
