using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Searcher;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("Water Resistance Upgrades")]
    public float waterResistanceUpgrade;
    public List<float> waterResistance = new List<float>();
    private int currentWaterIndex = 0;
    
    [Header("Depth Resistance Upgrades")]
    public float depthUpgrade;
    public List<float> depthResistance = new List<float>();
    private int currentDepthIndex = 0;

    [Header("Swim Ability Upgrades")]
    public bool swimAbilityUpgrade;

    [Header("Inventory Capacity Upgrades")]
    public int inventoryCapacityUpgrade;
    public List<int> inventoryCapacity = new List<int>();
    private int currentInventoryCapacityIndex = 0;
    
    [Header("Swimming Speeds Upgrades")]
    public float swimSpeedUpgrade;
    public List<float> swimSpeed = new List<float>();
    private int currentSwimSpeedIndex = 0;

    [Header("Eye Light Power Upgrades")]
    public float eyeLightUpgrade;
    public List<float> eyeLight = new List<float>();
    private int currentEyeLightIndex = 0;

    public void GiveWaterResistanceUpgrade()
    {
        if (currentWaterIndex < waterResistance.Count)
        {
            waterResistanceUpgrade = waterResistance[currentWaterIndex];
            currentWaterIndex++;
        }
    }

    public void GiveDepthResistanceUpgrade()
    {
        if (currentDepthIndex < depthResistance.Count)
        {
            depthUpgrade = depthResistance[currentDepthIndex];
            currentDepthIndex++;   
        }
    }
    
    public void GiveSwimAbilityUpgrade()
    {
        swimAbilityUpgrade = true;
    }
    
    public void GiveInventoryUpgrade()
    {
        if (currentInventoryCapacityIndex < inventoryCapacity.Count)
        {
            inventoryCapacityUpgrade = inventoryCapacity[currentInventoryCapacityIndex];
            currentInventoryCapacityIndex++;
        }
    }

    public void GiveSwimSpeedUpgrade()
    {
        if (currentSwimSpeedIndex < swimSpeed.Count)
        {
            swimSpeedUpgrade = swimSpeed[currentSwimSpeedIndex];
            currentSwimSpeedIndex++;
        }
    }

    public void GiveEyeLightUpgrade()
    {
        if (currentEyeLightIndex < eyeLight.Count)
        {
            eyeLightUpgrade = eyeLight[currentEyeLightIndex];
            currentEyeLightIndex++;
        }
    }
}
