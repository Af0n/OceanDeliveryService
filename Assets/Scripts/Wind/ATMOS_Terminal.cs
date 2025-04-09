using UnityEngine;
using UnityEngine.InputSystem;

public class ATMOS_Terminal : Interactable
{
    public float speed;
    [Header("Unity Set Up")]
    [Tooltip("Position camera changes to when switching direction.")]
    public Transform CamPos;
    [Tooltip("UI for terminal")]
    public Transform TerminalUI;

    private PlayerManager playerMan;
    private Transform mainCam;
    private Vector3 defaultCam;

    private InputSystem_Actions actions;
    private InputAction pause;

    private void Awake()
    {
        // input system boilerplate
        actions = new InputSystem_Actions();

        playerMan = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        mainCam = Camera.main.transform;
    }

    public override void Interact()
    {
        // save this for later;
        defaultCam = mainCam.transform.localPosition;
        Debug.Log(defaultCam);

        playerMan.SetAll(false);

        mainCam.SetPositionAndRotation(CamPos.position, CamPos.rotation); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        TerminalUI.gameObject.SetActive(true); // show UI
    }

    public void Cancel()
    {
        playerMan.SetAll(true);
        mainCam.SetLocalPositionAndRotation(defaultCam, Quaternion.identity); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TerminalUI.gameObject.SetActive(false); // hide UI
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        // test if player NOT in UI
        if(!TerminalUI.gameObject.activeSelf){
            return;
        }

        playerMan.SetAll(true);
        mainCam.SetLocalPositionAndRotation(defaultCam, Quaternion.identity); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TerminalUI.gameObject.SetActive(false); // hide UI
    }

    // i had to do it this way for it to be recognized in the unity event for buttons
    public void SetWind(int windCode)
    {
        WindManager.SetWind(windCode);
    }

    private void OnEnable()
    {
        // input system boilerplate
        pause = actions.UI.Pause;
        pause.Enable();
        pause.performed += Cancel;
    }

    private void OnDisable()
    {
        // input system boilerplate
        pause.Disable();
    }
}
