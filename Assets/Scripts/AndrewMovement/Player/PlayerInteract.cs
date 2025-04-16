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
    private InputAction interact, swap;

    private PlayerManager manager;

    private RaycastHit[] detectedInteractables;
    private int selectedInteractable;
    private Coroutine checkRoutine;

    public Transform SelectedInteractable
    {
        get
        {
            if (!manager.HasThirdPersonInteractable)
            {
                return null;
            }
            return detectedInteractables[selectedInteractable].transform;
        }
    }

    private void Awake()
    {
        actions = new InputSystem_Actions();
        manager = GetComponent<PlayerManager>();

        detectedInteractables = null;
    }

    public void StartThirdPersonCheck()
    {
        Debug.Log("Checking");
        checkRoutine = StartCoroutine(nameof(DoInteractCheck));
    }

    public void StopThirdPersonCheck()
    {
        if (checkRoutine == null)
        {
            return;
        }
        Debug.Log("No More Checking");
        StopCoroutine(checkRoutine);
    }

    private IEnumerator DoInteractCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(ThirdPersonCheckDelay);
            detectedInteractables = Physics.SphereCastAll(ThirdPersonInteractCenter.position, ThirdPersonInteractionRadius, Vector3.forward, ThirdPersonInteractionRadius, layerMask);
            manager.HasThirdPersonInteractable = detectedInteractables.Length != 0;
            manager.HasMultipleTPI = detectedInteractables.Length > 1;

            if (selectedInteractable > detectedInteractables.Length - 1)
            {
                selectedInteractable = 0;
            }

            if (selectedInteractable < 0)
            {
                selectedInteractable = detectedInteractables.Length - 1;
            }
        }

    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if(manager.IsThirdPerson){
            if(detectedInteractables.Length == 0){
                // nothing to interact with
                return;
            }
            detectedInteractables[selectedInteractable].transform.GetComponent<Interactable>().Interact();
            return;
        }
        //Debug.Log("Trying Interaction");
        Physics.Raycast(Cam.position, Cam.forward, out RaycastHit hitInfo, InteractDistance, layerMask);
        Debug.DrawRay(Cam.position, Cam.forward * InteractDistance, Color.red, 2f);

        if (!hitInfo.transform)
        {
            return;
        }

        // successful interaction
        hitInfo.transform.GetComponent<Interactable>().Interact();
    }

    private void ScrollSelected(InputAction.CallbackContext context)
    {
        // dont even try if no interactables nearby
        if (detectedInteractables.Length == 0)
        {
            return;
        }

        float value = swap.ReadValue<float>();

        if (value < 0)
        {
            selectedInteractable--;
        }
        else
        {
            selectedInteractable++;
        }

        if(selectedInteractable > detectedInteractables.Length-1){
            selectedInteractable = 0;
        }

        if(selectedInteractable < 0){
            selectedInteractable = detectedInteractables.Length-1;
        }
    }

    void OnEnable()
    {
        // input system boilerplate
        interact = actions.Player.Interact;
        interact.Enable();
        interact.performed += TryInteract;

        swap = actions.Player.ScrollInteractTarget;
        swap.Enable();
        swap.performed += ScrollSelected;
    }

    void OnDisable()
    {
        // input system boilerplate
        interact.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(ThirdPersonInteractCenter.position, ThirdPersonInteractionRadius);
    }
}
