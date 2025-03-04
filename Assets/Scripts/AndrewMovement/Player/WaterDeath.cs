using System.Collections;
using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    private PlayerManager playerMan;

    [Tooltip("How long it takes for our robot to drown")]
    public float DrownTime;

    private void Awake() {
        playerMan = GetComponent<PlayerManager>();
    }

    void Start()
    {
        StartDrowning();
    }

    public void StartDrowning(){
        StartCoroutine(nameof(DrownTimer));
    }

    public void StopDrowning(){
        StopAllCoroutines();
    }

    public IEnumerator DrownTimer(){
        yield return new WaitForSeconds(DrownTime);
        playerMan.Die("drowning");
    }
}
