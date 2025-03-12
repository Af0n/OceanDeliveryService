using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("How long the player takes to auto-respawn")]
    public float RespawnTime;
    public Transform RespawnPoint;
    public float RespawnDistCheck;
    [Tooltip("Temporary Exclude Layers for CharacterController")]
    public LayerMask ExcludeLayersTemp;

    private AndrewMovement movement;
    private AndrewLook look;
    private PlayerInteract interaction;
    private WaterDeath waterDeath;
    private CharacterController controller;
    private PlayerUpgradeManager upgradeManager;
    
    public bool IsOnBoat;
    public bool IsUnderwater;
    public float onSurfaceDepth = -1f;
    public GameObject playerFloater;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
        interaction = GetComponent<PlayerInteract>();
        controller = GetComponent<CharacterController>();
        waterDeath = GetComponent<WaterDeath>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();
    }

    private void FixedUpdate()
    {
        if (playerFloater.transform.position.y > onSurfaceDepth && playerFloater.transform.position.y < 0)
        {
            if(upgradeManager.swimAbilityUpgrade)
            {
                movement.isSwimming = false;
                IsUnderwater = false;
                if (!movement.isFloating)
                {
                    movement.audioManager.Emerge();
                }
                movement.isFloating = true;
                playerFloater.SetActive(true);
            }
        } 
        else if (playerFloater.transform.position.y < onSurfaceDepth)
        {
            if(upgradeManager.swimAbilityUpgrade)
            {
                movement.isSwimming = true;
                IsUnderwater = true;
                movement.isFloating = false;
                playerFloater.SetActive(false);
            }
        }
        else if (playerFloater.transform.position.y > 0)
        {
            movement.isSwimming = false;
            IsUnderwater = false;
            movement.isFloating = false;
            playerFloater.SetActive(false);
        }

        if(IsUnderwater){
            waterDeath.StartDrowning();
        }else{
            waterDeath.StopDrowning();
        }
    }

    public void SetMovement(bool b)
    {
        movement.enabled = b;
    }

    public void SetLook(bool b)
    {
        look.enabled = b;
    }

    public void SetMoveLook(bool b)
    {
        SetMovement(b);
        SetLook(b);
    }

    public void SetInteraction(bool b)
    {
        interaction.enabled = b;
    }

    public void SetAll(bool b)
    {
        SetMoveLook(b);
        SetInteraction(b);
    }

    public void Move(Vector3 vec)
    {
        controller.Move(vec);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "BoatTrigger":
                IsOnBoat = true;
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "BoatTrigger":
                IsOnBoat = false;
                break;
        }
    }

    public void Die()
    {
        Debug.Log("Player has Died!");
        SetAll(false);
        StartCoroutine(nameof(RespawnTimer));
    }

    public void Die(string cause)
    {
        Debug.Log($"Player has died from {cause}!");
        SetAll(false);
        StartCoroutine(nameof(RespawnTimer));
    }

    public IEnumerator Respawn()
    {
        Debug.Log("Respawning player at respawn point...");
        SetAll(true);

        controller.excludeLayers = ExcludeLayersTemp;
        controller.Move(RespawnPoint.position - transform.position);
        transform.rotation = RespawnPoint.rotation;

        // wait for a frame
        yield return 0;

        controller.excludeLayers = 0;
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(RespawnTime);
        StartCoroutine(nameof(Respawn));
    }

    public void ToggleSurface(float surfaced)
    {
        // onSurfaceDepth = surfaced;
    }

    void Start()
    {
        PlayerFloater.OnSurfacedPlayer += ToggleSurface;
    }

    void OnDestroy()
    {
        PlayerFloater.OnSurfacedPlayer -= ToggleSurface;
    }
}
