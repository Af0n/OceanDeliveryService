using UnityEngine;

public class WheelInteract : Interactable
{
    public ControlBoatToggle boatToggle;
    public Transform WheelPos;

    public override void Interact()
    {
        boatToggle.MountToggle();
    }

    private void Update() {
        transform.position = WheelPos.position;
        transform.rotation = WheelPos.rotation;
    }
}
