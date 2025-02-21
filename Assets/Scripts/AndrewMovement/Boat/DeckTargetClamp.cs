using UnityEngine;

public class DeckTargetClamp : MonoBehaviour
{
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;

    private Vector3 pos;

    private void Awake()
    {
        pos = Vector3.zero;
    }

    private void Update()
    {
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.z = Mathf.Clamp(pos.z, zMin, zMax);
    }
}
