using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float InteractDistance;

    [Header("Unity Set Up")]
    public Transform Cam;
    public LayerMask layerMask;

    // input system
    private InputSystem_Actions actions;
    private InputAction interact;

    private void Awake() {
        actions = new InputSystem_Actions();
    }

    private void TryInteract(InputAction.CallbackContext context){
        //Debug.Log("Trying Interaction");
        Physics.Raycast(Cam.position, Cam.forward, out RaycastHit hitInfo, InteractDistance, layerMask);
        Debug.DrawRay(Cam.position, Cam.forward * InteractDistance, Color.red, 2f);
        
        if(hitInfo.transform == null){
            return;
        }

        // successful interaction
        hitInfo.transform.GetComponent<Interactable>().Interact();
    }

    void OnEnable()
    {
        // input system boilerplate
        interact = actions.Player.Interact;
        interact.Enable();
        interact.performed += TryInteract;
    }

    void OnDisable()
    {
        // input system boilerplate
        interact.Disable();
    }
}
