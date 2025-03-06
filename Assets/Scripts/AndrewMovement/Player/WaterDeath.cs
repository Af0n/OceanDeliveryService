using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaterDeath : MonoBehaviour
{
    [Tooltip("How long it takes for our robot to drown")]
    public float DrownTime;
    [Header("Unity Set Up")]
    public Image WaterVisual;

    private PlayerManager playerMan;
    private bool isDrowning;

    private float drownTimer;
    public float PercentDrowned{
        get{ return drownTimer / DrownTime; }
    }

    private void Awake() {
        playerMan = GetComponent<PlayerManager>();
        isDrowning = false;
        drownTimer = 0;
    }

    public void StartDrowning(){
        if(isDrowning){
            return;
        }

        isDrowning = true;

        StartCoroutine(nameof(DrownTimer));
    }

    public void StopDrowning(){
        isDrowning = false;
        drownTimer = 0;
        WaterVisual.fillAmount = PercentDrowned;
        StopAllCoroutines();
    }

    public IEnumerator DrownTimer(){
        while(PercentDrowned<1){
            drownTimer += Time.deltaTime;
            WaterVisual.fillAmount = PercentDrowned;
            // waits for next frame
            yield return 0;
        }
        playerMan.Die("drowning");
        StopDrowning();
    }
}
