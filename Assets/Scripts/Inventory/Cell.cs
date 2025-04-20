using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private bool cellAvailable;
    private Transform root;

    void Awake()
    {
        cellAvailable = true;
        root = null;
    }

    public void SetAvailable(bool value)
    {
        cellAvailable = value;
    }

    public bool GetAvailable()
    {
        return cellAvailable;
    }

    public void SetRoot(Transform t)
    {
        root = t;
    }

    public Transform GetRoot()
    {
        return root;
    }
}
