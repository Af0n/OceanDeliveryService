using UnityEngine;

public class WheelInteract : Interactable
{
    public ControlBoatToggle boatToggle;

    public override void Interact()
    {
        boatToggle.MountToggle();
    }
}
