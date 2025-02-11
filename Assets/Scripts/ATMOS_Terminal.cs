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

    private void Start() {
        windManager = WindManager.Instance;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            Interact(Camera.main.transform);
        }
    }

    public void Interact(Transform cam){
        // [TODO] lock player movement
        cam.SetPositionAndRotation(CamPos.position, CamPos.rotation); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        TerminalUI.gameObject.SetActive(true); // show UI
    }

    public void Cancel(Transform cam){
        // [TODO] unlock player movement
        cam.SetLocalPositionAndRotation(new Vector3(0f,0.75f,0f), Quaternion.identity); // [TODO] smoother transition
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TerminalUI.gameObject.SetActive(false); // hide UI
    }

    // i had to do it this way for it to be recognized in the unity event for buttons
    public void SetWind(int windCode){
        windManager.SetWind(windCode);
    }
}
