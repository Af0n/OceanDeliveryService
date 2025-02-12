using UnityEngine;

public class WindManager : MonoBehaviour
{
    /* Working around OnClick limitations
    0 = North
    1 = North-East
    2 = East
    3 = South-East
    4 = South
    5 = South-West
    6 = West
    7 = North-West
    */

    public Transform WindVisual;
    [Header("Testing")]
    public bool Test;
    public Vector3 TestWind;

    [HideInInspector]
    public static WindManager Instance;

    private Vector3 windDir;

    public Vector3 WindDir{
        get { return windDir; }
        set { SetWind(value); } // uses SetWind(Vector3)
    }
    
    private void Awake() {
        windDir = new Vector3(0,0,0);

        // singleton
        if(Instance == null){
            Instance = this;
        }
    }

    private void Update() {
        if(Test){
            SetWind(TestWind);
        }
    }

    public void SetWind(Vector3 wind){
        windDir = wind;

        var vOverL = WindVisual.GetComponent<ParticleSystem>().velocityOverLifetime;
        vOverL.x = windDir.x;
        vOverL.z = windDir.z;
    }

    public void SetWind(Vector3 direction, float speed){
        SetWind(direction * speed);
    }

    public void SetWind(int windCode){
        Vector3 direction = WindCodetoVec3(windCode);
        
        SetWind(direction);
    }

    public Vector3 WindCodetoVec3(int windCode){
        switch(windCode){
            case 0:
                return new Vector3(0f, 0f, 10f);
            case 1:
                return new Vector3(7.071f, 0f, 7.071f);
            case 2:
                return new Vector3(10f, 0f, 0f);;
            case 3:
                return new Vector3(7.071f, 0f, -7.071f);;
            case 4:
                return new Vector3(0f, 0f, -10f);;
            case 5:
                return new Vector3(-7.071f, 0f, -7.071f);;
            case 6:
                return new Vector3(-10f, 0f, 0f);
            case 7:
                return new Vector3(-7.071f, 0f, 7.071f);
        }
        return Vector3.zero;
    }
}
