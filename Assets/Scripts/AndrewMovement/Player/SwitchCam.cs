using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public int ManagerCam;
    private PlayerManager manager;
    private void Awake() {
        manager = GetComponent<PlayerManager>();
    }
    // private void Update(){
    //     if(manager.IsInDeliveryZone){
    //         ChangeCamera();
    //         return;
    //     }
    // }
    public void ChangeCamera()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }
    public void ManagerCamera(){
        if(ManagerCam == 0){
            Cam2();
            ManagerCam=1;
        }
        else{
            Cam1();
            ManagerCam=0;
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
