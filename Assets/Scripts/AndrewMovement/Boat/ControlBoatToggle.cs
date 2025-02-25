using UnityEngine;

[RequireComponent(typeof(BoatControl))]

public class ControlBoatToggle : MonoBehaviour
{
    
    public Transform MountPos;

    private Transform player;
    private PlayerManager playerMan;
    private BoatControl boatControl;

    private bool isMounted;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMan = player.GetComponent<PlayerManager>();

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
        playerMan.SetInteraction(!isMounted);

        boatControl.enabled = isMounted;

        if(isMounted){
            TeleportMount();
            return;
        }

        Dismount();
    }

    private void Mount(bool b)
    {
        isMounted = b;
        Mount();
    }

    private void TeleportMount(){
        player.parent = MountPos;
        player.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void Dismount(){
        player.parent = null;
    }
}
