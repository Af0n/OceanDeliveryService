using UnityEngine;

public class TestFlags : MonoBehaviour
{
    public Flags list;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            list.SetFlag("TestFlag", true);
        }

        if(Input.GetKeyDown(KeyCode.F)){
            list.SetFlag("TestFlag", false);
        }

        if(Input.GetKeyDown(KeyCode.B)){
            list.SetFlag("TestFlag");
        }
    }
}
