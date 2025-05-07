using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject CreditScreen;
    public GameObject GameOverScreen;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnterCredits()
    {
        CreditScreen.SetActive(true);
        GameOverScreen.SetActive(false);
    }

    public void ExitCredits()
    {
        CreditScreen.SetActive(false);
        GameOverScreen.SetActive(true);
    }
}
