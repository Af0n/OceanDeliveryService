using NUnit.Framework.Internal;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public Transform WindVisual;
    [Header("Testing")]
    public bool Test;
    public Vector3 TestWind;

    private Vector3 windDir;
    private static WindManager instance;

    private void Awake() {
        windDir = new Vector3(0,0,0);

        // singleton
        if(instance != null){
            return;
        }
        instance = this;
    }

    private void Update() {
        if(Test){
            SetWind(TestWind);
        }
    }

    public static WindManager GetInstance(){
        return instance;
    }

    public void SetWind(Vector3 wind){
        windDir = wind;
    }

    public void SetWind(Vector3 direction, float speed){
        SetWind(direction * speed);
    }
}
