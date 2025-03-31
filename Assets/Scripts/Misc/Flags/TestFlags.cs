using UnityEngine;

public class TestFlags : MonoBehaviour
{
    public Flags list;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            if(Input.GetKey(KeyCode.LeftShift)){
                list.SetFlag("OtherTestFlag", true);
                return;
            }
            list.SetFlag("TestFlag", true);
        }

        if(Input.GetKeyDown(KeyCode.F)){
            if(Input.GetKey(KeyCode.LeftShift)){
                list.SetFlag("OtherTestFlag", false);
            }
            list.SetFlag("TestFlag", false);
        }

        if(Input.GetKeyDown(KeyCode.B)){
            if(Input.GetKey(KeyCode.LeftShift)){
                list.SetFlag("OtherTestFlag");
            }
            list.SetFlag("TestFlag");
        }

        
    }
}
