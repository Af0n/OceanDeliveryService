using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float InteractDistance;

    [Header("Unity Set Up")]
    public Transform Cam;

    // input system
    private InputSystem_Actions actions;
    private InputAction interact;

    private void Awake() {
        actions = new InputSystem_Actions();
    }

    private void TryInteract(InputAction.CallbackContext context){
        //Debug.Log("Trying Interaction");
        Physics.Raycast(Cam.position, Cam.forward, out RaycastHit hitInfo, InteractDistance);
        Debug.DrawRay(Cam.position, Cam.forward * InteractDistance, Color.red, 2f);
        try
        {
            // checks tag of hit object
            if(!hitInfo.transform.CompareTag("Interactable")){
                // if not interactable, just do nothing
                return;
            }
        }
        catch (System.Exception)
        {
            // handles non-hits if hitInfo has no transform
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
