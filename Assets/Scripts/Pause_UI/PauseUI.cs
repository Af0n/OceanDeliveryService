using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [Tooltip("How long the menu remembers the selected window.")]
    public float SelectionMemoryTime;
    public int DefaultPanel;

    [Space]
    public Transform PauseMenu;
    public Transform ScreenPanels;
    public Transform MapPanel;
    public Transform DirPanel;
    public Transform InvPanel;
    public Transform InfPanel;
    public Transform SysPanel;

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

    private void Pause(InputAction.CallbackContext context)
    {
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

    private void OnEnable()
    {
        // input system boilerplate
        pause = actions.UI.Pause;
        pause.Enable();
        pause.performed += Pause;
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
