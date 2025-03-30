using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCam : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public int Manager;
    public void ChangeCamera()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }
    public void ManagerCamera(){
        if(Manager == 0){
            Cam2();
            Manager=1;
        }
        else{
            Cam1();
            Manager=0;
        }
    }
    void Cam1()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
    }
    void Cam2()
    {
        cam1.SetActive(false);
        cam2.SetActive(true);
    }
}
