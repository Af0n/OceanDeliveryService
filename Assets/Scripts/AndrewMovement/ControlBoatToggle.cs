using UnityEngine;

[RequireComponent(typeof(BoatControl))]

public class ControlBoatToggle : MonoBehaviour
{
    private AndrewMovement playerMove;
    private CharacterController playerController;
    private BoatControl boatControl;
    private bool isMounted;

    void Awake()
    {
        playerMove = GameObject.FindWithTag("Player").GetComponent<AndrewMovement>();
        playerController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        
        // default to not mounted
        Mount(false);
    }

    void Update()
    {
        // [TODO] replace with proper interaction
        if (Input.GetKeyDown(KeyCode.B))
        {
            isMounted = !isMounted;
            Mount();
        }
    }

    private void Mount()
    {
        playerMove.enabled = !isMounted;
        playerController.enabled = !isMounted;

        boatControl.enabled = isMounted;
    }

    private void Mount(bool b)
    {
        isMounted = b;
        Mount();
    }
}
