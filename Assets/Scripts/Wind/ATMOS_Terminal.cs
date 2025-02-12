using UnityEngine;

public class ATMOS_Terminal : MonoBehaviour
{
    public float speed;
    [Header("Unity Set Up")]
    [Tooltip("Position camera changes to when switching direction.")]
    public Transform CamPos;
    [Tooltip("UI for terminal")]
    public Transform TerminalUI;

    private WindManager windManager;
    private Transform mainCam;

    private void Awake() {
        mainCam = Camera.main.transform;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Interact();
        }
    }

    public void Interact(){
        // [TODO] lock player movement
        mainCam.SetPositionAndRotation(CamPos.position, CamPos.rotation); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        TerminalUI.gameObject.SetActive(true); // show UI
    }

    public void Cancel(){
        // [TODO] unlock player movement
        mainCam.SetLocalPositionAndRotation(new Vector3(0f,0.75f,0f), Quaternion.identity); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TerminalUI.gameObject.SetActive(false); // hide UI
    }

    // i had to do it this way for it to be recognized in the unity event for buttons
    public void SetWind(int windCode){
        WindManager.SetWind(windCode);
    }
}
