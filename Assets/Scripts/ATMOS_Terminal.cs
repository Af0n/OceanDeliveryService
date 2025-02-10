using UnityEngine;
using UnityEngine.Events;

public class ATMOS_Terminal : MonoBehaviour
{
    [Header("Position camera changes to when switching direction.")]
    public Transform CamPos;
    public Transform TerminalUI;

    public void Interact(Transform cam){
        // [TODO] lock player movement
        cam.position = CamPos.position; // [TODO] replace with a nicer transition

        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        TerminalUI.gameObject.SetActive(true); // show UI
    }

    public void Cancel(Transform cam){
        cam.localPosition = new Vector3(0f,0.75f,0f); // [TODO] replace with a nicer transition and non-hardcoded values
        
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TerminalUI.gameObject.SetActive(false); // hide UI
    }
}
