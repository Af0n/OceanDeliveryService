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

    public InputSystem_Actions actions;

    private InputAction pause;

    private bool isPaused;
    private int activeMenu;

    public int systemIndex;
    public int optionsIndex;

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
