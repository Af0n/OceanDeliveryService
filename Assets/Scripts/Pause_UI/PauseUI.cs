using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [Tooltip("How long the menu remembers the selected window.")]
    public float SelectionMemoryTime;
    public int DefaultPanel;
    public int OptionsIndex;

    [Header("Unity Setup")]
    public Transform PauseMenu;
    public Transform ScreenPanels;

    public InputSystem_Actions actions;

    private InputAction pause;

    private bool isPaused;
    private int activeMenu;

    private void Awake()
    {
        // input system boilerplate
        actions = new InputSystem_Actions();

        isPaused = false;
        // defaults to inventory screen
        SetActiveMenu(2);
    }

    private IEnumerator ResetActiveWindow()
    {
        yield return new WaitForSeconds(SelectionMemoryTime);
        SetActiveMenu(DefaultPanel);
    }

    public void Pause(){
        if(activeMenu == OptionsIndex){

        }
        isPaused = !isPaused;

        PauseMenu.gameObject.SetActive(isPaused);

        if (isPaused)
        {
            StopCoroutine(nameof(ResetActiveWindow));
            return;
        }

        // starts coroutine to reset screen if not paused
        StartCoroutine(nameof(ResetActiveWindow));
    }

    public void DoPause(InputAction.CallbackContext context)
    {
        Pause();
    }

    private void OnEnable()
    {
        // input system boilerplate
        pause = actions.UI.Pause;
        pause.Enable();
        pause.performed += DoPause;
    }

    private void OnDisable()
    {
        // input system boilerplate
        pause.Disable();
    }

    public void SetActiveMenu(int index)
    {
        index = Mathf.Clamp(index, 0, ScreenPanels.childCount - 1);

        // disable currently active menu
        ScreenPanels.GetChild(activeMenu).gameObject.SetActive(false);

        // change active menu
        activeMenu = index;
        ScreenPanels.GetChild(activeMenu).gameObject.SetActive(true);

        Debug.Log("Setting active menu to " + activeMenu);
    }
}
