using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float InteractDistance;
    public float ThirdPersonCheckDelay;
    public float ThirdPersonInteractionRadius;

    [Header("Unity Set Up")]
    public Transform Cam;
    public LayerMask layerMask;
    public Transform ThirdPersonInteractCenter;

    // input system
    private InputSystem_Actions actions;
    private InputAction interact;

    private PlayerManager manager;

    private RaycastHit[] detectedInteractables;
    private Coroutine checkRoutine;

    private void Awake() {
        actions = new InputSystem_Actions();
        manager = GetComponent<PlayerManager>();

        detectedInteractables = null;
    }

    public void StartThirdPersonCheck(){
        checkRoutine = StartCoroutine(nameof(DoInteractCheck));
        Debug.Log("Checking");
    }

    public void StopThirdPersonCheck(){
        if(checkRoutine == null){
            return;
        }
        StopCoroutine(checkRoutine);
    }

    private IEnumerator DoInteractCheck(){
        yield return new WaitForSeconds(ThirdPersonCheckDelay);

        detectedInteractables = Physics.SphereCastAll(ThirdPersonInteractCenter.position, ThirdPersonInteractionRadius, Vector3.forward, ThirdPersonInteractionRadius, layerMask);
    }

    private void TryInteract(InputAction.CallbackContext context){
        
        
        //Debug.Log("Trying Interaction");
        Physics.Raycast(Cam.position, Cam.forward, out RaycastHit hitInfo, InteractDistance, layerMask);
        Debug.DrawRay(Cam.position, Cam.forward * InteractDistance, Color.red, 2f);
        
        if(!hitInfo.transform){
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
