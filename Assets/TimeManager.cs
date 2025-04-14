using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;
 
    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;
    
    [SerializeField] private Transform celestialPivot;
    [SerializeField] private Transform sun;
    [SerializeField] private Transform moon;
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;
 
    [SerializeField] private Light globalLight;
    
    //how many seconds it takes to transition.
    public int transitionTime = 60; 
 
    private int minutes;
 
    public int Minutes
    { get => minutes;
        set { minutes = value; OnMinutesChange(value); }
    }

    private int lastTransitionHour = -1;
    public int hours = 5;
 
    public int Hours
    { get => hours;
        set { hours = value; OnHoursChange(value); } 
    }
 
    private int days;
 
    public int Days
    { get => days;
        set => days = value;
    }
 
    private float tempSecond;

    public void Start()
    {
        SetupInitialSkybox();
        //in case our hour at start up is on a transition hour, we begin the transition.
        OnHoursChange(hours);
    }

    public void Update()
    {
        tempSecond += Time.deltaTime;
 
        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }
        
        RotateCelestials();
        
    }
 
    private void OnMinutesChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);
        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }
 
    private void OnHoursChange(int value)
    {
        if (value == lastTransitionHour) return;
        lastTransitionHour = value;
        
        //From Night to Dawn
        if (value == 5)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, transitionTime));
            //StartCoroutine(LerpLight(graddientNightToSunrise, transitionTime));
        }
        //From Dawn to Day
        else if (value == 10)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, transitionTime));
            //StartCoroutine(LerpLight(graddientSunriseToDay, transitionTime));
        }
        //From Day to Dusk
        else if (value == 17)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, transitionTime));
            //StartCoroutine(LerpLight(graddientDayToSunset, transitionTime));
        }
        //From Dusk to Night
        else if (value == 21)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, transitionTime));
            //StartCoroutine(LerpLight(graddientSunsetToNight, transitionTime));
        }
    }
 
    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
 
    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
    
    private void SetupInitialSkybox()
    {
        //Just an extra layer of protection to make sure we are on the right skybox even on transition hours
        if (hours == 5)
        {
            // Starting transition from Night to Dawn, so we use the Night texture.
            RenderSettings.skybox.SetTexture("_Texture1", skyboxNight);
        }
        else if (hours == 10)
        {
            // Starting transition from Dawn to Day, so we use the Sunrise texture.
            RenderSettings.skybox.SetTexture("_Texture1", skyboxSunrise);
        }
        else if (hours == 17)
        {
            // Starting transition from Day to Dusk, so we use the Day texture.
            RenderSettings.skybox.SetTexture("_Texture1", skyboxDay);
        }
        else if (hours == 21)
        {
            // Starting transition from Dusk to Night, so we use the Sunset texture.
            RenderSettings.skybox.SetTexture("_Texture1", skyboxSunset);
        }
        else
        {
            // The in between hours. Set texture and blend to 0 so we have the right skybox at the right time.
            if (hours > 5 && hours < 10)
            {
                RenderSettings.skybox.SetTexture("_Texture1", skyboxSunrise);
            }
            else if (hours > 10 && hours < 17)
            {
                RenderSettings.skybox.SetTexture("_Texture1", skyboxDay);
            }
            else if (hours > 17 && hours < 21)
            {
                RenderSettings.skybox.SetTexture("_Texture1", skyboxSunset);
            }
            else // For hours between 21 and 5
            {
                RenderSettings.skybox.SetTexture("_Texture1", skyboxNight);
            }
        }
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }

    public void ForceCheckCurrentTime()
    {
        OnHoursChange(hours);
    }
    
    private void RotateCelestials()
    {
        // Calculate how far we are through the day
        float normalizedTime = (Hours * 60f + Minutes) / 1440f; // 1440 total minutes in a day

        // Convert to a 360-degree rotation
        float rotationAngle = normalizedTime * 360;

        // Apply rotation to the pivot (rotating both sun and moon)
        celestialPivot.rotation = Quaternion.Euler(rotationAngle, 0f, 0f);

        // Adjust sun and moon lighting dynamically
        sunLight.intensity = Mathf.Clamp01(Mathf.Cos((normalizedTime -0.5f) * Mathf.PI * 2));
        moonLight.intensity = Mathf.Clamp01(Mathf.Cos((normalizedTime) * Mathf.PI * 2));
        
        sunLight.transform.rotation = Quaternion.LookRotation(celestialPivot.position - sun.position);
        moonLight.transform.rotation = Quaternion.LookRotation(celestialPivot.position - moon.position);
    }

}
