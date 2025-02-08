using UnityEngine;

public class WindManager : MonoBehaviour
{
    [Header("Testing")]
    public bool Test;
    public Vector3 TestWind;

    private Vector3 wind;

    private void Awake() {
        wind = new Vector3(0,0,0);
    }

    private void Update() {
        if(Test){
            SetWind(TestWind);
        }
    }

    public void SetWind(Vector3 wind){
        this.wind = wind;
    }

    public void SetWind(Vector3 direction, float speed){
        SetWind(direction * speed);
    }
}
