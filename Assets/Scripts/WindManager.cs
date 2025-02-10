using UnityEngine;

public class WindManager : MonoBehaviour
{
    public Transform WindVisual;
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
        WindVisual.transform.rotation = Quaternion.Euler(TestWind);
    }

    public void SetWind(Vector3 direction, float speed){
        SetWind(direction * speed);
    }
}
