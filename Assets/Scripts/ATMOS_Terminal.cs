using UnityEngine;

public class ATMOS_Terminal : MonoBehaviour
{
    [Header("Position camera changes to when switching direction.")]
    public Transform CamPos;

    public void Interact(Transform cam){
        cam.position = CamPos.position; // [TODO] replace with a nicer transition
    }
}
