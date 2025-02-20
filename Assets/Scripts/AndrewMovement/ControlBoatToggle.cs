using UnityEngine;

[RequireComponent(typeof(BoatControl))]

public class ControlBoatToggle : MonoBehaviour
{
    private AndrewMovement playerMove;
    private BoatControl boatControl;

    private bool isMounted;

    void Awake()
    {
        playerMove = GameObject.FindWithTag("Player").GetComponent<AndrewMovement>();
        boatControl = GetComponent<BoatControl>();

        // default to not mounted
        Mount(false);
    }

    public void MountToggle(){
        isMounted = !isMounted;
        Mount();
    }

    private void Mount()
    {
        playerMove.enabled = !isMounted;

        boatControl.enabled = isMounted;
    }

    private void Mount(bool b)
    {
        isMounted = b;
        Mount();
    }
}
