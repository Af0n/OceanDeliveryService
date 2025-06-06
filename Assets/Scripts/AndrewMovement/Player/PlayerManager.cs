using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("How long the player takes to auto-respawn")]
    public float RespawnTime;
    public Transform RespawnPoint;
    public float RespawnDistCheck;
    [Tooltip("Temporary Exclude Layers for CharacterController")]
    public LayerMask ExcludeLayersTemp;
    public bool IsThirdPerson;

    public AndrewMovement movement;
    public AndrewLook look;
    public PlayerInteract interaction;
    public WaterDeath waterDeath;
    public CharacterController controller;
    public PlayerUpgradeManager upgradeManager;
    public PauseUI pauseUI;
    public Image Reticle;
    public bool IsOnBoat;
    public bool IsInDeliveryZone;
    public bool IsUnderwater;
    public float onSurfaceDepth = -1f;
    public GameObject playerFloater;
    public GameObject firstPersonPlayerFloater;
    public GameObject firstPersonVisual;
    public float inventoryVisibility = 1f;
    public bool HasThirdPersonInteractable, HasMultipleTPI;

    [Header("Unity Set Up")]
    public Transform ThirdPersonDisplay;
    public UpdateThirdPersonInteractUI TPIUpdater;
    public CinemachineCamera cinemachineCamera;
    public Transform Cam;
    public float firstPersonCameraHeight = 1.7f;
    public Transform BodyVisual;

    private void Awake()
    {
        movement = GetComponent<AndrewMovement>();
        look = GetComponent<AndrewLook>();
        interaction = GetComponent<PlayerInteract>();
        controller = GetComponent<CharacterController>();
        waterDeath = GetComponent<WaterDeath>();
        upgradeManager = GetComponent<PlayerUpgradeManager>();
        
        playerFloater.SetActive(false);
        firstPersonPlayerFloater.SetActive(false);
        
    }

    private void Start()
    {
        PlayerFloater.OnSurfacedPlayer += ToggleSurface;
        pauseUI = GetComponentInChildren<PauseUI>();

        if (PlayerPrefs.HasKey("IsThirdPerson"))
        {
            IsThirdPerson = PlayerPrefs.GetInt("IsThirdPerson") == 1;
        }
        ThirdPersonDisplay.gameObject.SetActive(IsThirdPerson);

        ToggleThirdPerson(IsThirdPerson);
    }

    private void OnEnable()
    {
        Water.UnderWaterTrigger.OnUnderWaterStateChange += HandleUnderWaterState;
        Water.UnderWaterTrigger.OnUnderWaterSurfaceChange += HandleUnderWaterSurface;
    }

    private void OnDisable()
    {
        Water.UnderWaterTrigger.OnUnderWaterStateChange -= HandleUnderWaterState;
        Water.UnderWaterTrigger.OnUnderWaterSurfaceChange -= HandleUnderWaterSurface;
    }

    private void HandleUnderWaterState(bool isUnderwater)
    {
        IsUnderwater = isUnderwater;

        if (isUnderwater)
        {
            // Only start drowning if the head is submerged too
            if (!movement.isSwimming)
            {
                waterDeath.StartDrowning();
            }
        }
        else
        {
            // If the body is out of the water, stop drowning and floating
            waterDeath.StopDrowning();
            movement.isFloating = false;
            movement.isSwimming = false;
            playerFloater.SetActive(false);
            firstPersonPlayerFloater.SetActive(false);
            //OnEmerge?.Invoke();
            movement.audioManager.Emerge();
        }
    }

    private void HandleUnderWaterSurface(bool headSubmerged)
    {
        if (headSubmerged)
        {
            // If the head goes under, start drowning & disable floating
            Debug.Log("Player head is submerged!");
            movement.isSwimming = true;
            movement.isFloating = false;
            playerFloater.SetActive(false);
            firstPersonPlayerFloater.SetActive(false);
            waterDeath.StartDrowning();
        }
        else
        {
            // If head resurfaces but body is still underwater, stop drowning & enable floating
            if (IsUnderwater)
            {
                Debug.Log("Player head is above water, but body is submerged!");
                movement.isSwimming = false;
                movement.isFloating = true;
                playerFloater.SetActive(true);
                firstPersonPlayerFloater.SetActive(true);
                waterDeath.StopDrowning();
            }
        }
    }

    public void ToggleThirdPerson(bool b)
    {
        IsThirdPerson = b;
        PlayerPrefs.SetInt("IsThirdPerson", IsThirdPerson ? 1 : 0);
        BodyVisual.gameObject.SetActive(IsThirdPerson);

        if (IsThirdPerson)
        {
            firstPersonVisual.SetActive(false);
            SetLook(true);
            cinemachineCamera.enabled = true;
            Reticle.enabled = false;
            interaction.StartThirdPersonCheck();
            ThirdPersonDisplay.gameObject.SetActive(true);

            look.ThirdPersonBody(); // <-- ADD THIS LINE

            return;
        }

        firstPersonVisual.SetActive(true);
        SetLook(true);
        cinemachineCamera.enabled = false;
        Reticle.enabled = true;
        interaction.StopThirdPersonCheck();
        Cam.SetLocalPositionAndRotation(new Vector3(0f, firstPersonCameraHeight, 0f), Quaternion.identity);
        ThirdPersonDisplay.gameObject.SetActive(true);
    }
    public void SetInDeliveryZone(bool b){
        IsInDeliveryZone = b;
    }

    public void SetMovement(bool b)
    {
        movement.enabled = b;
    }

    public void SetLook(bool b)
    {
        look.enabled = b;
        if(IsThirdPerson){
            cinemachineCamera.enabled = b;
        }
    }

    public void SetMoveLook(bool b)
    {
        SetMovement(b);
        SetLook(b);
    }

    public void SetInteraction(bool b)
    {
        interaction.enabled = b;
        ThirdPersonDisplay.gameObject.SetActive(b);
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
            case "DeliveryZone":
                Debug.Log("Entered Delivery Zone");
                if(IsInDeliveryZone) return;
                IsInDeliveryZone = true;
                pauseUI.Pause();
                pauseUI.SetActiveMenu(2);
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
            case "DeliveryZone":
                Debug.Log("Exited Delivery Zone");
                IsInDeliveryZone = false;
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

        // wait for next frame
        yield return 0;

        StartCoroutine(Teleport(RespawnPoint.position));
    }

    public void DoTeleport(Vector3 pos){
        StartCoroutine(Teleport(pos));
    }

    public IEnumerator Teleport(Vector3 teleport){
        controller.excludeLayers = ExcludeLayersTemp;
        controller.Move(teleport - transform.position);
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

    void OnDestroy()
    {
        PlayerFloater.OnSurfacedPlayer -= ToggleSurface;
    }
}
