using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music Clips")]
    public AudioSource landMusic;
    public AudioSource waterMusic;
    
    [Header("Player Clips")]
    public AudioSource walking;
    public AudioSource boatWalking;
    public AudioSource floating;
    public AudioSource swimming;
    public AudioSource vertical;
    public AudioSource jumping;
    public AudioSource landing;
    [Header("Player and Vehicle Clips")]
    public AudioSource emerge;
    public AudioSource submerge;
    [Header("Vehicle Effects")]
    public AudioSource boatMovement;
    public AudioSource anchoring;
    public AudioSource shiftingVehicle;

    public void PlayMusic(bool isLand)
    {
        if (isLand)
        {
            if (!landMusic.isPlaying)
            {
                waterMusic.Stop();
                landMusic.Play();
            }
        }
        else
        {
            if (!waterMusic.isPlaying)
            {
                landMusic.Stop();
                waterMusic.Play();
            }
        }
    }

    public void StartWalking(bool isOnBoat)
    {
        if (isOnBoat)
        {
            if (!boatWalking.isPlaying)
            {
                walking.Stop();
                boatWalking.Play();
                floating.Stop();
                swimming.Stop();
            }
        }
        else
        {
            if (!walking.isPlaying)
            {
                boatWalking.Stop();
                walking.Play();
                floating.Stop();
                swimming.Stop();
            }
        }
    }

    public void StopWalking(bool isOnBoat)
    {
        if (isOnBoat)
        {
            boatWalking.Stop();
            walking.Stop();
            floating.Stop();
            swimming.Stop();
        }
        else
        {
            boatWalking.Stop();
            walking.Stop();
            floating.Stop();
            swimming.Stop();
        }
    }
    
    public void StartSwimming(bool isFloating)
    {
        if (isFloating)
        {
            if (!floating.isPlaying)
            {
                swimming.Stop();
                floating.Play();
                boatWalking.Stop();
                walking.Stop();
            }
        }
        else
        {
            if (!swimming.isPlaying)
            {
                floating.Stop();
                swimming.Play();
                boatWalking.Stop();
                walking.Stop();
            }
        }
    }

    public void StopSwimming(bool isFloating)
    {
        if (isFloating)
        {
            floating.Stop();
            swimming.Stop();
            boatWalking.Stop();
            walking.Stop();
        }
        else
        {
            floating.Stop();
            swimming.Stop();
            boatWalking.Stop();
            walking.Stop();
        }
    }

    public void Emerge()
    {
        if(!emerge.isPlaying)
        {
            emerge.Play();
        }
    }

    public void Submerge()
    {
        if (!submerge.isPlaying)
        {
            submerge.Play();
        }
    }

    public void Vertical(bool On)
    {
        if (On)
        {
            if (!vertical.isPlaying)
            {
                vertical.Play();
            }
        }
        else
        {
            if (vertical.isPlaying)
            {
                vertical.Stop();
            }
        }
    }

    public void Jump()
    {
        jumping.Play();
    }

    public void Landing()
    {
        landing.Play();
    }

    public void BoatMovement(bool On)
    {
        if(On)
        {
            if (!boatMovement.isPlaying)
            {
                boatMovement.Play();
            }
        }
        else
        {
            boatMovement.Stop();
        }
    }

    public void Anchor()
    {
        anchoring.Play();
    }

    public void SwitchVehicles()
    {
        shiftingVehicle.Play();
    }
    
    
}
