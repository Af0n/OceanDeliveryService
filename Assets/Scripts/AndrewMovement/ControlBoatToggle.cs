using UnityEngine;

[RequireComponent(typeof(BoatControl))]

public class ControlBoatToggle : MonoBehaviour
{
    private PlayerManager playerMan;
    private BoatControl boatControl;

    private bool isMounted;

    private void Awake()
    {
        playerMan = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        boatControl = GetComponent<BoatControl>();

        // default to not mounted
        Mount(false);
    }

    public void MountToggle()
    {
        isMounted = !isMounted;
        Mount();
    }

    private void Mount()
    {
        playerMan.SetMovement(!isMounted);

        boatControl.enabled = isMounted;
    }

    private void Mount(bool b)
    {
        isMounted = b;
        Mount();
    }
}
