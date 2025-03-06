using TMPro;
using UnityEngine;

public class RECON_Respawn : Interactable
{
    
    [Header("Colors")]
    public Gradient LockedGrad;
    public Gradient UnlockedGrad;
    public Gradient RespawnGrad;
    public Gradient OppLockedGrad;
    public Gradient OppUnlockedGrad;
    public Gradient OppRespawnGrad;
    [Header("Text Displays")]
    [TextArea]
    public string LockedMessage;
    [TextArea]
    public string UnlockedMessage;
    [TextArea]
    public string RespawnMessage;
    [Header("Unity Set Up")]
    public ParticleSystem Particles;
    public ParticleSystem OppParticles;
    public TextMeshProUGUI TextBox;
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
            SetParticleGradient(LockedGrad, OppLockedGrad);
            TextBox.text = LockedMessage;
            return;
        }

        if(isRespawn){
            SetParticleGradient(RespawnGrad, OppRespawnGrad);
            TextBox.text = RespawnMessage;
            return;
        }

        SetParticleGradient(UnlockedGrad, OppUnlockedGrad);
        TextBox.text = UnlockedMessage;
    }

    private void SetParticleGradient(Gradient g, Gradient og){
        ParticleSystem.MainModule ma = Particles.main;
        ma.startColor = g;

        ma = OppParticles.main;
        ma.startColor = og;
    }

    public override void Interact()
    {
        isLocked = false;
        OnSetRespawn();

        isRespawn = true;
        SetParticleGradient(RespawnGrad, OppRespawnGrad);
        TextBox.text = RespawnMessage;

        Debug.Log("Setting Spawnpoint");

        GameObject.FindWithTag("Player").GetComponent<PlayerManager>().RespawnPoint = transform.GetChild(0);
    }

    public void ResetBool(){
        isRespawn = false;

        if(isLocked){
            TextBox.text = LockedMessage;
            return;
        }

        SetParticleGradient(UnlockedGrad,OppUnlockedGrad);
        TextBox.text = UnlockedMessage;
    }
}
