using System.Collections;
using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    private PlayerManager playerMan;
    private bool isDrowning;

    [Tooltip("How long it takes for our robot to drown")]
    public float DrownTime;

    private void Awake() {
        playerMan = GetComponent<PlayerManager>();
        isDrowning = false;
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
        StopAllCoroutines();
    }

    public IEnumerator DrownTimer(){
        yield return new WaitForSeconds(DrownTime);
        playerMan.Die("drowning");
        StopDrowning();
    }
}
