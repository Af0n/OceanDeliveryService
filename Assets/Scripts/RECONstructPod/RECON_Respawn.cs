using UnityEngine;

public class RECON_Respawn : Interactable
{
    
    [Header("Colors")]
    public Gradient Locked;
    public Gradient Unlocked;
    public Gradient Respawn;
    [Header("Unity Set Up")]
    public ParticleSystem particles;
    public ParticleSystem oppParticles;
    public bool StartLocked;

    public delegate void SetRespawnEvent();
    public static SetRespawnEvent OnSetRespawn;

    private bool isRespawn;
    private bool isLocked;

    void Awake()
    {
        OnSetRespawn += ResetBool;

        isLocked = StartLocked;
    }

    void Start()
    {
        if(isLocked){
            SetParticleGradient(Locked);
            return;
        }

        if(isRespawn){
            SetParticleGradient(Respawn);
            return;
        }

        SetParticleGradient(Unlocked);
    }

    private void SetParticleGradient(Gradient g){
        ParticleSystem.MainModule ma = particles.main;
        ma.startColor = g;

        ma = oppParticles.main;
        ma.startColor = g;
    }

    public override void Interact()
    {
        isLocked = false;
        OnSetRespawn();
        isRespawn = true;
        SetParticleGradient(Respawn);
        Debug.Log("Setting Spawnpoint");
        GameObject.FindWithTag("Player").GetComponent<PlayerManager>().RespawnPoint = transform.GetChild(0);
    }

    public void ResetBool(){
        isRespawn = false;

        if(isLocked){
            return;
        }

        SetParticleGradient(Unlocked);
    }
}
