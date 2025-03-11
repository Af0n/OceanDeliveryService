using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool cellAvailable;

    void Start()
    {
        cellAvailable = true;
    }

    public void SetAvailable(bool value)
    {
        cellAvailable = value;
    }

    public bool GetAvailable()
    {
        return cellAvailable;
    }
}
