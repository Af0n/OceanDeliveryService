using UnityEngine;

public class DeckTargetClamp : MonoBehaviour
{
    public Transform deckTarget;
    public Transform origin;

    public static DeckTargetClamp instance;

    public float YRot;

    private void Awake() {
        instance = this;
    }

    void Update()
    {
        SetYRotRelative(YRot);
    }

    public void SetYRotRelative(float rot){
        float use = origin.eulerAngles.y + rot;
        deckTarget.rotation = Quaternion.Euler(0f, use, 0f);
    }

    public void SetYRot(float rot){
        YRot = rot;
    }

    public void AddYRot(float rot){
        YRot += rot;
    }

    public void SetPos(Vector3 pos){
        deckTarget.position = pos;
    }
}
