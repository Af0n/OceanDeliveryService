using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [Tooltip("How long the menu remembers the selected window.")]
    public float SelectionMemoryTime;
    public int DefaultPanel;

    [Header("Unity Setup")]
    public Transform PauseMenu;
    public Transform ScreenPanels;
    public PlayerManager playerMan;

    private InputSystem_Actions actions;
    private InputAction pause;

    private bool isPaused;
    private int activeMenu;

    public int systemIndex;
    public int optionsIndex;

    public InventorySystem inventory;

    private void Awake()
    {
        // input system boilerplate
        actions = new InputSystem_Actions();

        isPaused = false;

        // dynamically grabbing panels with unique functionality
        optionsIndex = ScreenPanels.Find("Options").GetSiblingIndex();
        systemIndex = ScreenPanels.Find("System").GetSiblingIndex();

        // defaults to inventory screen
        SetActiveMenu(DefaultPanel);
    }

    private IEnumerator ResetActiveWindow()
    {
        yield return new WaitForSeconds(SelectionMemoryTime);
        SetActiveMenu(DefaultPanel);
    }

    public void Pause()
    {
        // travels back to System if on Options
        if (activeMenu == optionsIndex)
        {
            SetActiveMenu(systemIndex);
            return;
        }

        // actually pause/unpause happens here

        // check to make sure no items are being moved
        inventory.CheckIfMoving();

        isPaused = !isPaused;
        PauseMenu.gameObject.SetActive(isPaused);

        if (isPaused)
        {
            StopCoroutine(nameof(ResetActiveWindow));
            playerMan.SetLook(false);
            playerMan.SetMovement(false);
            Cursor.lockState = CursorLockMode.None;

            // to bring up inventory 
            if(activeMenu == 2) {
                inventory.DisplayInventory(true);
                if(playerMan.IsInDeliveryZone) {
                    inventory.DZPanel.SetActive(true);
                }
            }
            if(activeMenu == 1){
                
            }

            return;
        }

        // starts coroutine to reset screen if not paused
        StartCoroutine(nameof(ResetActiveWindow));
        playerMan.SetAll(true);
        Cursor.lockState = CursorLockMode.Locked;
        inventory.DZPanel.SetActive(false);
        inventory.DisplayInventory(false);
    }

    public void DoPause(InputAction.CallbackContext context)
    {
        Pause();
    }

    private void OnEnable()
    {
        inventory.OnPackageDelivered += HandlePackageDelivered;
        // input system boilerplate
        pause = actions.UI.Pause;
        pause.Enable();
        pause.performed += DoPause;
    }

    private void OnDisable()
    {
        inventory.OnPackageDelivered -= HandlePackageDelivered;
        // input system boilerplate
        pause.Disable();
    }
    private void HandlePackageDelivered(){
        playerMan.SetInDeliveryZone(false);
        // inventory.DZPanel.SetActive(false);
        Pause();
    }

    public void SetActiveMenu(int index)
    {
        index = Mathf.Clamp(index, 0, ScreenPanels.childCount - 1);

        // check to make sure no items are being moved
        inventory.CheckIfMoving();

        // disable currently active menu
        ScreenPanels.GetChild(activeMenu).gameObject.SetActive(false);

        // change active menu
        activeMenu = index;
        ScreenPanels.GetChild(activeMenu).gameObject.SetActive(true);

        Debug.Log("Setting active menu to " + activeMenu);

        // to bring up inventory 
        if(activeMenu == 2 && isPaused) {
            inventory.DisplayInventory(true);
        }
        else {
            inventory.DisplayInventory(false);
        }
    }
}
